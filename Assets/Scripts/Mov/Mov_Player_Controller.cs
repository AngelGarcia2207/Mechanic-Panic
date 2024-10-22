using System.Collections;
using System.Collections.Generic;
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
    private GameObject playerCard;

    [SerializeField] private Mov_Player_Properties[] playerProps;
    private Mov_Player_Properties playerProp;
    private int playerIndex = 0;

    private PlayerInput playerInput;
    private CharacterController charController;


    private int currentHealth;

    // Esto lo moveré a otro script en el futuro //
    [SerializeField] private Animator weaponAnimator;
    public Obj_Player_Weapon playerWeapon;
    [SerializeField] private Obj_Player_Armor playerArmor;
    [SerializeField] private ParticleSystem weaponTrail;
    public WeaponEvent pickUpWeapon;
    public ArmorEvent pickUpArmor;
    // // // // // // // // // // // // // // // //


    void Start()
    {
        playerIndex = GameObject.FindGameObjectsWithTag("Player").Length - 1;
        playerProp = playerProps[playerIndex];
        playerProp.spriteAnimator.gameObject.SetActive(true);

        charController = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        SM = new PlayerStateMachine(playerProp.spriteAnimator);

        playerCard = UI_PlayerCard_Manager.Instance.CreatePlayerCard(playerProp.headSprite, playerProp.name);
        currentHealth = playerProp.maxHealth;
    }

    void Update()
    {
        Vector2 rawDirection = playerInput.actions["Direction"].ReadValue<Vector2>();
        if (rawDirection.x != 0 || rawDirection.y != 0)
        {
            if ((charController.isGrounded || RaycastFloor()) && SM.AvailableTransition(SM.move))
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
            else
            {
                movementX = 0;
                movementZ = 0;
            }
        }
        else if ((charController.isGrounded || RaycastFloor()) && SM.AvailableTransition(SM.idle))
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
    }

    private void SpecialInputs()
    {
        if ((charController.isGrounded || RaycastFloor()) && jumpButtonPressed && SM.AvailableTransition(SM.jump))
        {
            SM.ChangeState(SM.jump);
            StartCoroutine(Jump());
        }

        if (playerInput.actions["Dodge"].triggered)
        {
            Dodge();
        }

        if (playerInput.actions["PickUp"].triggered)
        {
            Grab();
        }

        if (playerInput.actions["Attack"].triggered)
        {
            Attack();
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

    private bool RaycastFloor()
    {
        Vector3 origin = transform.position;
        RaycastHit hit;
        Vector3 direction = -transform.up;

        if (Physics.Raycast(origin, direction, out hit, playerProp.floorRaycastDistance))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Dodge()
    {
        if (SM.AvailableTransition(SM.dodge))
        {
            SM.ChangeState(SM.dodge);
            
            velocity.x = playerProp.dodgeSpeed * transform.forward.x;

            StartCoroutine(DodgeDelay());
        }
    }

    public void receiveDamage(int damage)
    {
        if (SM.AvailableTransition(SM.stunned))
        {
            currentHealth -= damage;

            UI_PlayerCard playerCardScript = playerCard.GetComponent<UI_PlayerCard>();
            playerCardScript.UpdateHealthBar(currentHealth, playerProp.maxHealth);

            if (currentHealth <= 0)
            {
                SM.ChangeState(SM.dead);
            }
        }
    }

    public void applyKnockBack(Vector3 knockback)
    {
        if (SM.AvailableTransition(SM.stunned))
        {
            velocity = knockback / playerProp.mass;
        }
    }

    public void applyStun(float stunDuration)
    {
        if (SM.AvailableTransition(SM.stunned))
        {
            SM.ChangeState(SM.stunned);
            StartCoroutine(StunDelay(stunDuration));
        }
    }

        private void Grab()
    {
        if (SM.AvailableTransition(SM.grab))
        {
            SM.ChangeState(SM.grab);
            StartCoroutine(GrabDelay());

            pickUpWeapon.Invoke(playerWeapon);
            pickUpArmor.Invoke(playerArmor);
        }
    }

    private void Attack()
    {
        if (SM.AvailableTransition(SM.attack) && playerWeapon.HasBase())
        {
            SM.ChangeState(SM.attack);
            StartCoroutine(AttackDelay());
            
            playerWeapon.gameObject.tag = "WeaponBase";
            for(int i = 2; i < playerWeapon.gameObject.transform.childCount; i++)
            {
                playerWeapon.gameObject.transform.GetChild(i).gameObject.tag = "WeaponComplement";
            }
            StartCoroutine(SwingCoroutine());
            weaponAnimator.SetTrigger("Swing");
            weaponTrail.Play();
        }
    }

    private void OnJump()
    {
        jumpButtonPressed = !jumpButtonPressed;
    }


    // Esto lo moveré a otro script en el futuro (Código de Gael)//
    IEnumerator SwingCoroutine()
    {
        yield return new WaitForSeconds(1f);
        playerWeapon.gameObject.tag = "Untagged";
        for(int i = 2; i < playerWeapon.gameObject.transform.childCount; i++)
        {
            playerWeapon.gameObject.transform.GetChild(i).gameObject.tag = "Untagged";
        }
    }
    // // // // // // // // // // // // // // // //

    IEnumerator DodgeDelay()
    {
        yield return new WaitForSeconds(playerProp.dodgeDuration);
        SM.returnToIdle();
    }

    IEnumerator StunDelay(float stunDuration)
    {
        yield return new WaitForSeconds(stunDuration);
        SM.returnToIdle();
    }

    IEnumerator GrabDelay()
    {
        yield return new WaitForSeconds(0.3f);
        SM.returnToIdle();
    }

    IEnumerator AttackDelay()
    {
        yield return new WaitForSeconds(0.5f);
        SM.returnToIdle();
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