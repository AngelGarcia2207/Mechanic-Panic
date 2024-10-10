using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mov_Enemy : MonoBehaviour
{
    [SerializeField] private float groundSpeed = 10;
    [SerializeField] private float airSpeed = 10;
    [SerializeField] private float rotationSpeed = 0.1f;
    [SerializeField] private float jumpForce = 10;
    [SerializeField] private float extraGravityForce = 10;
    [SerializeField] private float groundSpeedDivisor = 1.1f;
    [SerializeField] private float minWayPointDistance = 1;
    [SerializeField] private float playerDistance = 10;
    [SerializeField] private int playerFollowTime = 100;

    [Header("Privates")]
    public Rigidbody rb;
    public Dijkstra_PathFinding pathFinding;
    public Find_Nearest findNearest;
    public GameObject[] playerArray;
    public GameObject playerTarget;

    public List<Transform> createdPath;
    public int nextWayPoint;
    public int playerFollowTiming;
    public bool resetLock;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        GameObject[] playerList = GameObject.FindGameObjectsWithTag("Player");

        try { pathFinding = GetComponent<Dijkstra_PathFinding>(); }
        catch { pathFinding = null; }
        try { findNearest = GetComponent<Find_Nearest>(); }
        catch { findNearest = null; }
    }

    void FixedUpdate()
    {
        if (pathFinding != null && findNearest != null && playerArray.Length >= 1)
        {
            playerTarget = findNearest.FindNearest(this.gameObject, playerArray); // Buscamos al jugador más cercano

            // Obtenemos la dirección hacia el jugador cercano.
            Vector3 directionToNearest = (playerTarget.transform.position - transform.position).normalized;
            directionToNearest.y = 0; // Evitamos verticalidad en la dirección.


            // Creamos el raycast
            RaycastHit hit;
            if (Physics.Raycast(transform.position, directionToNearest, out hit, Mathf.Infinity))
            {
                if (hit.transform.gameObject == playerTarget.gameObject)
                {
                    playerFollowTiming++;
                }
                else
                {
                    playerFollowTiming = 0;
                }
            }
            // Esta línea de código te permite ver en la escena el raycast que se está creando durante play mode.
            Debug.DrawRay(transform.position, directionToNearest, Color.red);

            if (playerFollowTiming >= playerFollowTime)
            {
                Movement(playerTarget.transform.position, true);
                resetLock = false;
            }
            else
            {
                if (resetLock == false)
                {
                    ResetPath();
                    resetLock = true;
                }
                if (createdPath.Count >= 2 && createdPath[nextWayPoint] != null)
                {
                    Movement(createdPath[nextWayPoint].position, false);
                    CheckIfNearWayPoint();
                }
                else { ResetPath(); }
            }
        }
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

    public void CheckIfNearWayPoint()
    {
        float wayPointDistance = Vector3.Distance(transform.position, createdPath[nextWayPoint].position);
        if (wayPointDistance < minWayPointDistance)
        {
            if (nextWayPoint == 0) { nextWayPoint++; }
            else if (nextWayPoint >= 1) { ResetPath(); }
        }
    }

    public void ResetPath()
    {
        if(pathFinding != null)
        {
            nextWayPoint = 0;
            pathFinding.wayPointFirst = findNearest.FindNearest(this.gameObject, pathFinding.wayPointsObjList).GetComponent<WayPoint>();
            pathFinding.wayPointFinal = findNearest.FindNearest(playerTarget, pathFinding.wayPointsObjList).GetComponent<WayPoint>();
            createdPath = pathFinding.FindShortestPath();
        }
    }
}