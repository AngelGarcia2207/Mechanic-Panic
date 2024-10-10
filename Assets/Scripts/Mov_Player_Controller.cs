using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[System.Serializable]
public class WeaponEvent : UnityEvent<Obj_Player_Weapon> {}

public class Mov_Player_Controller : MonoBehaviour
{
    private PlayerStateMachine SM;
    private PlayerInput playerInput;
    private float movementX;
    private float movementZ;
    private Vector3 playerMovementInput;
    private Vector3 nextMovement;
    private Vector3 velocity;
    private bool jumpButtonPressed = false;
    private float remainingJumpTime;
    private float scanFrequency = 0.05f;

    public Animator spriteAnimator;
    [SerializeField] private CharacterController player;
    [SerializeField] private float speed;
    [SerializeField] private float mass;
    [SerializeField] private float gravity;
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpTime;
    [SerializeField] [Range(0f, 1f)] private float airFriction;
    [SerializeField] private float floorRaycastDistance;

    // Esto lo moveré a otro script en el futuro //
    [SerializeField] private Animator weaponAnimator;
    [SerializeField] private Obj_Player_Weapon playerWeapon;
    [SerializeField] private ParticleSystem weaponTrail;
    public WeaponEvent pickUp;
    // // // // // // // // // // // // // // // //


    void Start()
    {
        player = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        SM = new PlayerStateMachine(spriteAnimator);
    }

    void Update()
    {
        Vector2 rawDirection = playerInput.actions["Direction"].ReadValue<Vector2>();
        if (rawDirection.x != 0 || rawDirection.y != 0)
        {
            if ((player.isGrounded || raycastFloor()) && SM.AvailableTransition(SM.move))
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
        else if ((player.isGrounded || raycastFloor()) && SM.AvailableTransition(SM.idle))
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

        specialInputs();

        applyAcceleration();

        nextMovement += playerMovementInput * speed;
        player.transform.LookAt(player.transform.position + new Vector3(movementX, 0, 0));

        player.Move(nextMovement * Time.deltaTime);

        // Esto lo moveré a otro script en el futuro //
        if(playerInput.actions["PickUp"].triggered)
        {
            pickUp.Invoke(playerWeapon);
        }

        if(playerInput.actions["Attack"].triggered)
        {
            playerWeapon.gameObject.tag = "WeaponBase";
            for(int i = 2; i < playerWeapon.gameObject.transform.childCount; i++)
            {
                playerWeapon.gameObject.transform.GetChild(i).gameObject.tag = "WeaponComplement";
            }
            StartCoroutine(SwingCoroutine());
            weaponAnimator.SetTrigger("Swing");
            weaponTrail.Play();
        }
        // // // // // // // // // // // // // // // //
    }

    private void specialInputs()
    {
        if ((player.isGrounded || raycastFloor()) && jumpButtonPressed && SM.AvailableTransition(SM.jump))
        {
            SM.ChangeState(SM.jump);
            StartCoroutine(Jump());
        }
    }

    private void applyAcceleration()
    {
        // Reducir la velocidad horiontal hasta que ya no sea significativa
        if ((new Vector2(velocity.x + nextMovement.x, velocity.z + nextMovement.z)).magnitude > speed / 2)
        {
            velocity.x *= (1 - airFriction * Time.deltaTime);
            velocity.z *= (1 - airFriction * Time.deltaTime);
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

    private bool raycastFloor()
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

    public void applyKnockBack(Vector3 knockback)
    {
        velocity = knockback / mass;
    }

    public void applyStun(float stunDuration)
    {
        if (SM.AvailableTransition(SM.stunned))
        {
            SM.ChangeState(SM.stunned);
            StartCoroutine(stunDelay(stunDuration));
        }
    }

    IEnumerator stunDelay(float stunDuration)
    {
        yield return new WaitForSeconds(stunDuration);
        SM.returnToIdle();
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

    private void OnJump()
    {
        jumpButtonPressed = !jumpButtonPressed;
    }
}