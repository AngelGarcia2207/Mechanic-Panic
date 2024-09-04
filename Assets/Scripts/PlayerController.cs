using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class WeaponEvent : UnityEvent<Weapon> {}

public class PlayerController : MonoBehaviour
{
    [SerializeField] private CharacterController player;
    private float movementX;
    private float movementZ;
    private Vector3 playerInput;
    private Vector3 playerDirection;
    [SerializeField] private bool immobilized = false;

    [SerializeField] private float speed;
    [SerializeField] private float gravity;
    [SerializeField] private float fallingSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float floorRaycastDistance;

    [SerializeField] private Weapon playerWeapon;
    public WeaponEvent pickUp;


    void Start()
    {
        player = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (!immobilized) {
            movementX = Input.GetAxis("Horizontal");
            movementZ = Input.GetAxis("Vertical");
        }
        else {
            movementX = 0;
            movementZ = 0;
        }

        playerInput = Vector3.ClampMagnitude(new Vector3(movementX, 0, movementZ), 1);
        playerDirection = playerInput * speed;
        player.transform.LookAt(player.transform.position + playerDirection);

        applyGravity();

        specialInputs();

        player.Move(playerDirection * Time.deltaTime);

        if(Input.GetKeyDown(KeyCode.E))
        {
            pickUp.Invoke(playerWeapon);
        }
    }

    void specialInputs() {
        if ((player.isGrounded || raycastFloor()) && Input.GetButtonDown("Jump") && !immobilized) {
            fallingSpeed = jumpForce;
            playerDirection.y = fallingSpeed;
        }
    }

    void applyGravity() {
        if (player.isGrounded) {
            fallingSpeed = -gravity * 0.1f;
            playerDirection.y = fallingSpeed;
        }
        else {
            fallingSpeed -= gravity * Time.deltaTime;
            playerDirection.y = fallingSpeed;
        }
    }

    bool raycastFloor() {
        Vector3 origin = transform.position;
        RaycastHit hit;
        Vector3 direction = -transform.up;

        if (Physics.Raycast(origin, direction, out hit, floorRaycastDistance)) {
            return true;
        }
        else {
            return false;
        }
    }
}