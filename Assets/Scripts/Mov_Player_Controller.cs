using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[System.Serializable]
public class WeaponEvent : UnityEvent<Obj_Weapon> {}

public class Mov_Player_Controller : MonoBehaviour
{
    [SerializeField] private CharacterController player;
    private PlayerInput playerInput;
    private float movementX;
    private float movementZ;
    private Vector3 directionInput;
    [SerializeField] private Vector3 velocity;
    [SerializeField] private Vector3 aceleration;
    [SerializeField] private float mass;
    [SerializeField] [Range(0f, 1f)] private float airFriction;

    [SerializeField] private bool stunned = false;

    [SerializeField] private float speed;
    [SerializeField] private float gravity;
    [SerializeField] private float jumpForce;
    [SerializeField] private float floorRaycastDistance;

    // Esto lo moveré a otro script en el futuro //
    [SerializeField] private Animator weaponAnimator;
    [SerializeField] private Animator spriteAnimator;
    [SerializeField] private Obj_Weapon playerWeapon;
    [SerializeField] private ParticleSystem weaponTrail;
    public WeaponEvent pickUp;
    // // // // // // // // // // // // // // // //


    void Start()
    {
        player = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
    }

    void Update()
    {
        if (!stunned) {
            Vector2 rawDirection = playerInput.actions["Direction"].ReadValue<Vector2>();
            movementX = rawDirection.x;
            movementZ = rawDirection.y;
        }
        else {
            movementX = 0;
            movementZ = 0;
        }

        directionInput = Vector3.ClampMagnitude(new Vector3(movementX, 0, movementZ), 1);

        applyAcceleration();

        velocity += directionInput * speed;
        player.transform.LookAt(player.transform.position + new Vector3(movementX, 0, 0));

        specialInputs();

        player.Move(velocity * Time.deltaTime);

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

        if(movementX != 0 || movementZ != 0)
        {
            spriteAnimator.SetBool("IsMoving", true);
        }
        else
        {
            spriteAnimator.SetBool("IsMoving", false);
        }
        // // // // // // // // // // // // // // // //
    }

    private void specialInputs()
    {
        if ((player.isGrounded || raycastFloor()) && playerInput.actions["Jump"].triggered && !stunned)
        {
            aceleration.y = jumpForce;
            velocity.y = aceleration.y;
            spriteAnimator.SetBool("IsJumping", true);
        }
    }

    private void applyAcceleration()
    {
        // Reducir la aceleración horiontal hasta que ya no sea significativa
        if ((new Vector2(aceleration.x + velocity.x, aceleration.z + velocity.z)).magnitude > speed / 2)
        {
            aceleration.x *= (1 - airFriction * Time.deltaTime);
            aceleration.z *= (1 - airFriction * Time.deltaTime);
        }
        else
        {
            aceleration.x = 0;
            aceleration.z = 0;
        }

        // Gravedad
        if (player.isGrounded && aceleration.y <= -gravity * 0.1f)
        {
            aceleration.y = -gravity * 0.1f;
            spriteAnimator.SetBool("IsJumping", false);
        }
        else
        {
            aceleration.y -= gravity * Time.deltaTime;
        }

        velocity = aceleration;
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
        aceleration = knockback / mass;
    }

    public void applyStun(float stunDuration)
    {
        stunned = true;
        StartCoroutine(stunDelay(stunDuration));
    }

    IEnumerator stunDelay(float stunDuration)
    {
        yield return new WaitForSeconds(stunDuration);
        stunned = false;
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
}
