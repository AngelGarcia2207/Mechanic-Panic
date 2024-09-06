using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mov_Player_Controller : MonoBehaviour
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
        player.transform.LookAt(player.transform.position + new Vector3(movementX, 0, 0));

        applyGravity();

        specialInputs();

        player.Move(playerDirection * Time.deltaTime);
    }

    private void specialInputs() {
        if ((player.isGrounded || raycastFloor()) && Input.GetButtonDown("Jump") && !immobilized) {
            fallingSpeed = jumpForce;
            playerDirection.y = fallingSpeed;
        }
    }

    private void applyGravity() {
        if (player.isGrounded) {
            fallingSpeed = -gravity * 0.1f;
            playerDirection.y = fallingSpeed;
        }
        else {
            fallingSpeed -= gravity * Time.deltaTime;
            playerDirection.y = fallingSpeed;
        }
    }

    private bool raycastFloor() {
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
