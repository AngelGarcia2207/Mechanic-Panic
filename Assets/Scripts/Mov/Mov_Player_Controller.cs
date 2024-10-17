using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

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

    [SerializeField] private Sprite headSprite;
    [SerializeField] private string name;
    public Animator spriteAnimator;
    [SerializeField] private Inp_PlayerInstantiator playerInstantiator;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private CharacterController player;

    [SerializeField] private int maxHealth;
    [SerializeField] private int currentHealth;
    [SerializeField] private float speed;
    [SerializeField] private float mass;
    [SerializeField] private float gravity;
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpTime;
    [SerializeField] private float dodgeSpeed;
    [SerializeField] private float dodgeDuration = 1f;
    [SerializeField] [Range(0f, 1f)] private float airFriction;
    [SerializeField] private float floorRaycastDistance;

    // Esto lo moveré a otro script en el futuro //
    [SerializeField] private Animator weaponAnimator;
    [SerializeField] private Obj_Player_Weapon playerWeapon;
    [SerializeField] private Obj_Player_Armor playerArmor;
    [SerializeField] private ParticleSystem weaponTrail;
    public WeaponEvent pickUpWeapon;
    public ArmorEvent pickUpArmor;
    // // // // // // // // // // // // // // // //


    void Start()
    {
        player = GetComponent<CharacterController>();
        SM = new PlayerStateMachine(spriteAnimator);

        playerCard = UI_PlayerCard.Instance.CreatePlayerCard(headSprite, name);
        currentHealth = maxHealth;
    }

    void Update()
    {
        Vector2 rawDirection = playerInput.actions["Direction"].ReadValue<Vector2>();
        jumpButtonPressed = playerInstantiator.jumpButtonPressed;
        if (rawDirection.x != 0 || rawDirection.y != 0)
        {
            if ((player.isGrounded || RaycastFloor()) && SM.AvailableTransition(SM.move))
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
        else if ((player.isGrounded || RaycastFloor()) && SM.AvailableTransition(SM.idle))
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

        nextMovement += playerMovementInput * speed;
        player.transform.LookAt(player.transform.position + new Vector3(movementX, 0, 0));

        player.Move(nextMovement * Time.deltaTime);
    }

    private void SpecialInputs()
    {
        if ((player.isGrounded || RaycastFloor()) && jumpButtonPressed && SM.AvailableTransition(SM.jump))
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
        if ((new Vector2(velocity.x + nextMovement.x, velocity.z + nextMovement.z)).magnitude > speed / 2)
        {
            velocity.x *= (1 - airFriction * 3 * Time.deltaTime);
            velocity.z *= (1 - airFriction * 3 * Time.deltaTime);
        }
        else
        {
            velocity.x = 0;
            velocity.z = 0;
        }

        // Gravedad
        if (player.isGrounded && velocity.y <= -gravity * 0.1f)
        {
            velocity.y = -gravity * 0.1f;
        }
        else
        {
            velocity.y -= gravity * Time.deltaTime;
        }

        nextMovement = velocity;
    }

    private bool RaycastFloor()
    {
        Vector3 origin = transform.position;
        RaycastHit hit;
        Vector3 direction = -transform.up;

        if (Physics.Raycast(origin, direction, out hit, floorRaycastDistance))
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
            
            velocity.x = dodgeSpeed * transform.forward.x;

            StartCoroutine(DodgeDelay());
        }
    }

    public void receiveDamage(int damage)
    {
        if (SM.AvailableTransition(SM.stunned))
        {
            currentHealth -= damage;
            UIManager.Instance.UpdateHealthBar(currentHealth, maxHealth);

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
            velocity = knockback / mass;
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
        yield return new WaitForSeconds(dodgeDuration);
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
        remainingJumpTime = jumpTime;

        while (jumpButtonPressed && remainingJumpTime > 0)
        {
            velocity.y = jumpForce / mass;

            remainingJumpTime -= scanFrequency;
            yield return new WaitForSeconds(scanFrequency);
        }
    }
}