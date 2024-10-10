using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SawmillRotation : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;
    [SerializeField] private GameObject[] logs;

    private PlayerInput playerInput;


    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 direction2D = playerInput.actions["DirectionAxis"].ReadValue<Vector2>();
        transform.Rotate(Vector3.up, direction2D.x * -rotationSpeed);
    }

    private void OnJumpPress()
    {
        if (logs[0].activeSelf == false)
        {
            logs[1].SetActive(false);
            logs[0].SetActive(true);
        }
        else
        {
            logs[0].SetActive(false);
            logs[1].SetActive(true);
        }
    }
}
