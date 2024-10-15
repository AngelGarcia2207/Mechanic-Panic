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

    private List<GameObject> playersList = new List<GameObject>();
    private GameObject[] playersArray;
    private GameObject playerTarget;

    private Ene_EnemyTest enemyTest;
    private Find_Nearest findNearest;
    [SerializeField] private Animator enemyAnimator;



    [Header("Privates")]
    private Rigidbody rb;
    private bool canBePushed = true;

    void Start()
    {
        enemyTest = GetComponent<Ene_EnemyTest>();
        rb = GetComponent<Rigidbody>();
        findNearest = GetComponent<Find_Nearest>();
        StartCoroutine(FindaAllPlayers());
    }

    void FixedUpdate()
    {
        if (playersList.Count >= 1)
        {
            if (findNearest != null)
            {
                playerTarget = findNearest.FindNearest(this.gameObject, playersArray);
            }

            if (enemyTest.stunned == false)
            { Movement(playerTarget.transform.position); if (enemyAnimator != null) { enemyAnimator.SetInteger("state", 1); } }
        }
        if(enemyTest.currentHealth <= 0)
        {
            rb.velocity = Vector3.zero;
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
            if(playerTarget != null)
            {
                Vector3 pushDirection = (transform.position - playerTarget.transform.position).normalized;
                pushDirection.y = 0;
                rb.velocity = pushDirection * attackPush;
            }

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
            playersArray = GameObject.FindGameObjectsWithTag("Player");
            playersList.Clear();
            foreach (GameObject player in playersArray)
            {
                playersList.Add(player);
            }
        }
    }
}
