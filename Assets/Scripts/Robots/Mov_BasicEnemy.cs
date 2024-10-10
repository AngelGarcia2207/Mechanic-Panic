using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mov_BasicEnemy : MonoBehaviour
{
    [SerializeField] private float groundSpeed = 20;
    [SerializeField] private float airSpeed = 10;
    [SerializeField] private float rotationSpeed = 0.1f;
    [SerializeField] private float jumpForce = 10;
    [SerializeField] private float extraGravityForce = 10;
    [SerializeField] private float groundSpeedDivisor = 1.1f;
    [SerializeField] private float playerDistance = 1;

    [SerializeField] private GameObject playerTarget;


    [Header("Privates")]
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Movement(playerTarget.transform.position, true);
    }


    private void Movement(Vector3 targetPosition, bool usesPlayerDistance)
    {
        Vector3 walkDirection = (targetPosition - transform.position).normalized;
        walkDirection.y = 0;
        if ((usesPlayerDistance == true && Vector3.Distance(transform.position, playerTarget.transform.position) > playerDistance) || usesPlayerDistance == false)
        {
            rb.AddForce(walkDirection * groundSpeed);
        }

        rb.velocity = new Vector3(rb.velocity.x / groundSpeedDivisor, rb.velocity.y, rb.velocity.z / groundSpeedDivisor);

        rb.AddForce(new Vector3(0, -extraGravityForce, 0));

        float angle = Mathf.Atan2(walkDirection.x, walkDirection.z) * Mathf.Rad2Deg;
        Quaternion rotacionFinal = Quaternion.Euler(0, angle, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotacionFinal, rotationSpeed);
    }
}
