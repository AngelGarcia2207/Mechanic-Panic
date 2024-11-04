using System.Collections;
using System.Collections.Generic;
using Unity.Netcode.Components;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.LowLevel;

[System.Serializable]
public class WeaponEvent : UnityEvent<Obj_Player_Weapon> {}
[System.Serializable]
public class ArmorEvent : UnityEvent<Obj_Player_Armor> {}

public class Mov_Player_Controller : MonoBehaviour
{
    private PlayerStateMachine SM;
    private float movementX;
    private float movementZ;
    private Vector3 playerMovementInput;
    private Vector3 nextMovement;
    private Vector3 velocity;
    private bool jumpButtonPressed = false;
    private float remainingJumpTime;
    private float scanFrequency = 0.05f;
    private bool invulnerable = false;
    private float invulnerabilityDuration = 1f;
    private bool alive = true;
    
    private GameObject playerCard;

    [SerializeField] private Mov_Player_Properties[] playerProps;
    private Mov_Player_Properties playerProp;
    private int playerIndex = 0;

    // PRIVATE COMPONENTS
    private PlayerInput playerInput;
    private CharacterController charController;

    // ONLINE
    private Onl_Player_Controller onlController;
    [HideInInspector] public Onl_Player_Manager onlManager;
    private bool isOnline = false;
    public int onlineIndex = 0;

    // ONLINE INPUTS
    private bool canOnlPickUp = false, canOnlAttack = false;

    private int currentHealth;

    // Esto lo moveré a otro script en el futuro //
    [SerializeField] private GameObject weapon;
    public Obj_Player_Weapon playerWeaponScript;
    [SerializeField] private Obj_Player_Armor playerArmor;
    [SerializeField] private ParticleSystem weaponTrail;
    public WeaponEvent pickUpWeapon;
    public ArmorEvent pickUpArmor;
    // // // // // // // // // // // // // // // //

    // Extras
    private bool changedPositionStart = false;


    void Start()
    {
        if (GetComponent<Onl_Player_Controller>() != null)
        {
            onlController = GetComponent<Onl_Player_Controller>();
            isOnline = true;
        }

        playerIndex = GameObject.FindGameObjectsWithTag("Player").Length - 1;

        ChangeCharacter();

        // Configuración adicional si es necesario
    

        charController = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();

        transform.position = Map_Display_Boundaries.Instance.transform.position + new Vector3(0, 3, 0);
        currentHealth = playerProp.maxHealth;

        Map_Display_Boundaries.Instance.AddPlayer(this.gameObject);
        GameManager.Instance.AddPlayer(this.gameObject);
    }

    public void ChangeCharacter()
    {
        if (isOnline)
        {
            playerProp = playerProps[onlineIndex];
            GetComponent<NetworkAnimator>().Animator = playerProp.spriteAnimator;
        }
        else
        {
            playerProp = playerProps[playerIndex];
        }

        foreach (Mov_Player_Properties prop in playerProps)
        {
            if (prop == playerProp)
            {
                prop.gameObject.SetActive(true);
                weapon.transform.SetParent(Finder.FindChildRecursive(prop.gameObject.transform, "weaponSpot"));
            }
            else
            {
                prop.gameObject.SetActive(false);
            }
        }

        GameObject SMObject = new GameObject("StateMachine");
        SMObject.transform.parent = this.transform;
        SM = SMObject.AddComponent<PlayerStateMachine>();
        SM.Initialize(playerProp.spriteAnimator);

        // Crear el player card y referenciarlo en el `Onl_Player_Controller`.
        playerCard = UI_PlayerCard_Manager.Instance.CreatePlayerCard(playerCard, playerProp.headSprite, playerProp.name);
        if (isOnline == true && onlController != null)
        {
            onlController.SetPlayerCard(playerCard);
        }

        AddPlayerCardToNetManager(null);
    }

    public void AddPlayerCardToNetManager(Onl_Player_Manager _onlManager)
    {
        if (_onlManager != null)
        { onlManager = _onlManager; }

        if (isOnline && onlManager != null)
        { onlManager.playerCards.Add(playerCard); }
    }

    void Update()
    {
        Vector2 rawDirection = playerInput.actions["Direction"].ReadValue<Vector2>();
        if (isOnline)
        {
            rawDirection = onlController.onlDirection2D;
            jumpButtonPressed = onlController.onlJumpButtonPressed;
        }
        if (rawDirection.x != 0 || rawDirection.y != 0)
        {
            if (charController.isGrounded && SM.AvailableTransition(SM.move))
            {
                SM.ChangeState(SM.move);
                movementX = rawDirection.x;
                movementZ = rawDirection.y;
            }
            else if (SM.AvailableTransition(SM.jumpMove))
            {
                SM.ChangeState(SM.jumpMove);
                movementX = rawDirection.x;
                movementZ = rawDirection.y;
            }
            else if (SM.GetCurrentState() == SM.moveAttack || SM.GetCurrentState() == SM.jumpAttack)
            {
                movementX = rawDirection.x;
                movementZ = rawDirection.y;
            }
            else
            {
                movementX = 0;
                movementZ = 0;
            }
        }
        else if (charController.isGrounded && SM.AvailableTransition(SM.idle))
        {
            SM.ChangeState(SM.idle);
            movementX = 0;
            movementZ = 0;
        }
        else
        {
            movementX = 0;
            movementZ = 0;
        }

        playerMovementInput = Vector3.ClampMagnitude(new Vector3(movementX, 0, movementZ), 1);

        SpecialInputs();

        ApplyAcceleration();

        nextMovement += playerMovementInput * playerProp.speed;
        charController.transform.LookAt(charController.transform.position + new Vector3(movementX, 0, 0));

        charController.Move(nextMovement * Time.deltaTime);

        // Esto debe de ir al final de Update()
        if (changedPositionStart == false && isOnline == false)
        {
            charController.transform.position = GameObject.Find("CameraCenter").transform.position;
            changedPositionStart = true;
        }
    }

    private void SpecialInputs()
    {
        // JUMP
        if (charController.isGrounded && jumpButtonPressed && SM.AvailableTransition(SM.jump))
        {
            SM.ChangeState(SM.jump);
            StartCoroutine(Jump());
        }

        // DODGE
        if (isOnline == false && playerInput.actions["Dodge"].triggered)
        {
            Dodge();
        }
        else if (isOnline == true && onlController.onlDodgePressed == true)
        {
            Dodge();
        }

        // PICK UP
        if(isOnline == false && playerInput.actions["PickUp"].triggered)
        {
            Grab();
        }
        else if (isOnline == true)
        {
            if (onlController.onlPickUpPressed == false)
            { canOnlPickUp = true; }
            else if(onlController.onlPickUpPressed == true && canOnlPickUp == true)
            {
                if (onlController != null) { Grab(); }
                canOnlPickUp = false;
            }
        }

        // ATTACK
        if (isOnline == false && playerInput.actions["Attack"].triggered)
        {
            Attack();
        }
        else if (isOnline == true)
        {
            if (onlController.onlAttackPressed == false)
            { canOnlAttack = true; }
            else if (onlController.onlAttackPressed == true && canOnlAttack == true)
            { Attack(); canOnlAttack = false; }
        }
        
        if (SM.GetCurrentState() == SM.dead && jumpButtonPressed)
        {
            Revive();
        }

        if (playerInput.actions["Pause"].triggered)
        {
            GameManager.Instance.TogglePause();
        }
    }

    private void ApplyAcceleration()
    {
        // Reducir la velocidad horiontal hasta que ya no sea significativa
        if ((new Vector2(velocity.x + nextMovement.x, velocity.z + nextMovement.z)).magnitude > playerProp.speed / 2)
        {
            velocity.x *= (1 - playerProp.airFriction * 3 * Time.deltaTime);
            velocity.z *= (1 - playerProp.airFriction * 3 * Time.deltaTime);
        }
        else
        {
            velocity.x = 0;
            velocity.z = 0;
        }

        // Gravedad
        if (charController.isGrounded && velocity.y <= -playerProp.gravity * 0.1f)
        {
            velocity.y = -playerProp.gravity * 0.1f;
        }
        else
        {
            velocity.y -= playerProp.gravity * Time.deltaTime;
        }

        nextMovement = velocity;
    }

    public void Dodge()
    {
        if (SM.AvailableTransition(SM.dodge))
        {
            SM.ChangeState(SM.dodge, playerProp.dodgeDelay);
            
            velocity.x = playerProp.dodgeSpeed * transform.forward.x;
        }
    }

    public void receiveDamage(int damage)
    {
        if (SM.AvailableTransition(SM.stunned) && !invulnerable)
        {
            StartCoroutine(InvulnerabilityDelay());
            currentHealth -= damage;

            UI_PlayerCard playerCardScript = playerCard.GetComponent<UI_PlayerCard>();
            playerCardScript.UpdateHealthBar(currentHealth, playerProp.maxHealth);

            if (currentHealth <= 0)
            {
                Die();
            }
        }
    }

    public void applyKnockBack(Vector3 knockback)
    {
        if (SM.AvailableTransition(SM.stunned) && !invulnerable)
        {
            velocity = knockback / playerProp.mass;
        }
    }

    public void applyStun(float stunDuration)
    {
        if (SM.AvailableTransition(SM.stunned) && !invulnerable)
        {
            SM.ChangeState(SM.stunned, stunDuration);
        }
    }

    // Esta función será llamada también en Onl_Player_Controller.cs
    public void Grab()
    {
        if (SM.AvailableTransition(SM.grab))
        {
            SM.ChangeState(SM.grab, playerProp.grabDelay);

            pickUpWeapon.Invoke(playerWeaponScript);
            pickUpArmor.Invoke(playerArmor);
        }
    }

    private void Attack()
    {
        if (SM.AvailableTransition(SM.attack) && playerWeaponScript.HasBase())
        {
            SM.ChangeState(SM.attack, playerProp.attackDelay);
            
            playerWeaponScript.gameObject.tag = "WeaponBase";
            for(int i = 2; i < playerWeaponScript.gameObject.transform.childCount; i++)
            {
                playerWeaponScript.gameObject.transform.GetChild(i).gameObject.tag = "WeaponComplement";
            }
            StartCoroutine(SwingCoroutine());
            weaponTrail.Play();
        }
        else if (SM.AvailableTransition(SM.moveAttack) && playerWeaponScript.HasBase())
        {
            SM.ChangeState(SM.moveAttack, playerProp.attackDelay);
            
            playerWeaponScript.gameObject.tag = "WeaponBase";
            for(int i = 2; i < playerWeaponScript.gameObject.transform.childCount; i++)
            {
                playerWeaponScript.gameObject.transform.GetChild(i).gameObject.tag = "WeaponComplement";
            }
            StartCoroutine(SwingCoroutine());
            weaponTrail.Play();
        }
        else if (SM.AvailableTransition(SM.jumpAttack) && playerWeaponScript.HasBase())
        {
            SM.ChangeState(SM.jumpAttack, playerProp.attackDelay);
            
            playerWeaponScript.gameObject.tag = "WeaponBase";
            for(int i = 2; i < playerWeaponScript.gameObject.transform.childCount; i++)
            {
                playerWeaponScript.gameObject.transform.GetChild(i).gameObject.tag = "WeaponComplement";
            }
            StartCoroutine(SwingCoroutine());
            weaponTrail.Play();
        }
    }

    private void OnJump()
    {
        if (isOnline == false)
        { jumpButtonPressed = !jumpButtonPressed; }
    }

    private void Die()
    {
        SM.ChangeState(SM.dead);
        gameObject.tag = "Untagged";
        alive = false;
        GameManager.Instance.checkForAlivePlayers();
        UI_PlayerCard playerCardScript = playerCard.GetComponent<UI_PlayerCard>();
        Map_Display_Boundaries.Instance.RemovePlayer(this.gameObject);
        playerCardScript.ToggleDeadPanel();
    }

    private void Revive()
    {
        if (GameManager.Instance.ConsumeALive())
        {
            currentHealth = playerProp.maxHealth;
            transform.position = Map_Display_Boundaries.Instance.transform.position + new Vector3(0, 3, 0);
            gameObject.tag = "Player";
            SM.ReturnToIdle();

            StartCoroutine(InvulnerabilityDelay());

            Map_Display_Boundaries.Instance.AddPlayer(this.gameObject);
            transform.position = Map_Display_Boundaries.Instance.transform.position;

            UI_PlayerCard playerCardScript = playerCard.GetComponent<UI_PlayerCard>();
            playerCardScript.ToggleDeadPanel();
            playerCardScript.UpdateHealthBar(currentHealth, playerProp.maxHealth);
        }
    }

    public bool GetAliveStatus()
    {
        return alive;
    }

    // Esto lo moveré a otro script en el futuro (Código de Gael)//
    IEnumerator SwingCoroutine()
    {
        yield return new WaitForSeconds(1f);
        playerWeaponScript.gameObject.tag = "Untagged";
        for(int i = 2; i < playerWeaponScript.gameObject.transform.childCount; i++)
        {
            playerWeaponScript.gameObject.transform.GetChild(i).gameObject.tag = "Untagged";
        }
    }
    // // // // // // // // // // // // // // // //

    IEnumerator InvulnerabilityDelay()
    {
        invulnerable = true;
        yield return new WaitForSeconds(invulnerabilityDuration);
        invulnerable = false;
    }

    IEnumerator Jump()
    {
        remainingJumpTime = playerProp.jumpTime;

        while (jumpButtonPressed && remainingJumpTime > 0)
        {
            velocity.y = playerProp.jumpForce / playerProp.mass;

            remainingJumpTime -= scanFrequency;
            yield return new WaitForSeconds(scanFrequency);
        }
    }
}