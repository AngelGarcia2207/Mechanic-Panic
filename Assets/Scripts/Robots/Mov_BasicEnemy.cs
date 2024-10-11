using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
    [SerializeField] private bool usesPlayerDistance = true;
    [SerializeField] private bool retreats = true;
    [SerializeField] private float retreatMultiplier = 1.5f;
    [SerializeField] private float attackPush = 10;

    public GameObject[] players;
    public GameObject playerTarget;

    public Find_Nearest findNearest;



    [Header("Privates")]
    private Rigidbody rb;
    private bool canBePushed = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        findNearest = GetComponent<Find_Nearest>();
        StartCoroutine(FindaAllPlayers());
    }

    void FixedUpdate()
    {
        if(findNearest != null)
        {
            playerTarget = findNearest.FindNearest(this.gameObject, players);
        }
        if (players.Length >= 1)
        {

            Movement(playerTarget.transform.position);
        }

    }


    private void Movement(Vector3 targetPosition)
    {
        Vector3 walkDirection = (targetPosition - transform.position).normalized;
        walkDirection.y = 0;
        bool lowerDistance = Vector3.Distance(transform.position, playerTarget.transform.position) > playerDistance;
        if ((usesPlayerDistance == true && lowerDistance == true) || usesPlayerDistance == false)
        {
            rb.AddForce(walkDirection * groundSpeed);
        }
        else
        {
            if (usesPlayerDistance && retreats)
            {
                rb.AddForce(-walkDirection * groundSpeed * retreatMultiplier);
            }
        }

        rb.velocity = new Vector3(rb.velocity.x / groundSpeedDivisor, rb.velocity.y, rb.velocity.z / groundSpeedDivisor);

        rb.AddForce(new Vector3(0, -extraGravityForce, 0));

        float angle = Mathf.Atan2(walkDirection.x, walkDirection.z) * Mathf.Rad2Deg;
        Quaternion rotacionFinal = Quaternion.Euler(0, angle, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotacionFinal, rotationSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (canBePushed && (other.gameObject.CompareTag("WeaponBase") || other.CompareTag("WeaponComplement")))
        {
            Vector3 pushDirection = (transform.position - playerTarget.transform.position).normalized;
            pushDirection.y = 0;
            rb.AddForce(pushDirection * (rb.mass * attackPush), ForceMode.Impulse);

            canBePushed = false;
            StartCoroutine(CanBePushedAgain());
        }
    }

    IEnumerator CanBePushedAgain()
    {
        yield return new WaitForSeconds(0.4f);
        canBePushed = true;
    }

    IEnumerator FindaAllPlayers()
    {
        while(true)
        {
            yield return new WaitForSeconds(2f);
            players = GameObject.FindGameObjectsWithTag("Player");
        }
    }
}
