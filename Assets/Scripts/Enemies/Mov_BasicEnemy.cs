using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using UnityEngine;

public class Mov_BasicEnemy : MonoBehaviour
{
    [Header("Movement and Physics")]
    [SerializeField] private float groundSpeed = 20;
    //[SerializeField] private float jumpForce = 10;
    [SerializeField] private float extraGravityForce = 10;
    [SerializeField] private float groundSpeedDivisor = 1.1f;

    [Header("Player Distance")]
    [SerializeField] private bool usesPlayerDistance = true;
    [SerializeField] private float playerDistance = 1;

    [Header("State Change")]
    [SerializeField] private bool usesStateChangeDistance = true;
    [SerializeField] private float stateChangeDistance = 1;
    private bool isStateFollow = true;

    [Header("Retreat")]
    [SerializeField] private float retreatMultiplier = 1.5f;
    [SerializeField] private bool retreats = true;

    [Header("Attacked By Player")]
    [SerializeField] private float attackPush = 10;

    [Header("Animators")]
    [SerializeField] private Animator enemyAnimator;


    [Header("Rotation Animations")]
    [SerializeField] private float rotationSpeed = 0.1f;
    [SerializeField] private bool canAnimRotationLook = false;
    [SerializeField] private float rotationSpeedThreshold = 3f; // Ajusta este valor seg�n la sensibilidad de rotaci�n
    [SerializeField] private float frontAngleUncertainty = 0.3f; // Grados de incertidumbre en el �ngulo cuando se mueve hacia el frente
    private Quaternion lastRotation;
    private float rotationDifference;



    private List<GameObject> playersList = new List<GameObject>();
    private GameObject[] playersArray;
    private GameObject playerTarget;
    private Ene_EnemyTest enemyTest;
    private Find_Nearest findNearest;

    

    [Header("Privates")]
    private Rigidbody rb;
    private Vector3 walkDirection;
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
            {
                ChangeStates();
                if(isStateFollow == true)
                {
                    Movement(playerTarget.transform.position);
                    Rotation();
                }
            }
        }
        if(enemyTest.currentHealth <= 0)
        {
            rb.velocity = Vector3.zero;
        }

    }


    private void ChangeStates()
    {
        if(usesStateChangeDistance == true)
        {
            if (Vector3.Distance(transform.position, playerTarget.transform.position) > stateChangeDistance)
            {
                if (enemyAnimator != null) { enemyAnimator.SetInteger("state", 0); }
                isStateFollow = false;
                rb.velocity = Vector3.zero;
            }
            else
            {
                if (enemyAnimator != null) { enemyAnimator.SetInteger("state", 1); }
                isStateFollow = true;
            }
        }
    }

    private void Movement(Vector3 targetPosition)
    {
        walkDirection = (targetPosition - transform.position).normalized;
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

    }

    private void Rotation()
    {
        float angle = Mathf.Atan2(walkDirection.x, walkDirection.z) * Mathf.Rad2Deg;
        Quaternion rotacionFinal = Quaternion.Euler(0, angle, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotacionFinal, rotationSpeed);


        // ANIMACI�N DE ROTACI�N
        if(canAnimRotationLook == true)
        {
            // Calcula la diferencia de rotaci�n desde el �ltimo frame
            rotationDifference = Quaternion.Angle(lastRotation, transform.rotation);

            // Determina el estado del material dependiendo de si est� rotando o no
            if (rotationDifference < frontAngleUncertainty)
            {
                // Mirando hac�a el frente
                enemyAnimator.SetInteger("looking", 0);
            }
            else if (Vector3.SignedAngle(transform.forward, walkDirection, Vector3.up) > rotationSpeedThreshold)
            {
                // Mirando a la derecha
                enemyAnimator.SetInteger("looking", 2);
            }
            else if (Vector3.SignedAngle(transform.forward, walkDirection, Vector3.up) < -rotationSpeedThreshold)
            {
                // Mirando a la izquierda
                enemyAnimator.SetInteger("looking", 1);
            }

            // Actualiza la �ltima rotaci�n para el pr�ximo frame
            lastRotation = transform.rotation;
        }
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
