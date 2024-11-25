using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Miner : MonoBehaviour
{
    [SerializeField] private float speed, rotationSpeed, jumpForce, raycastDis, minDistance, attackForce, velDivisor, extraGravity, pushBackDistance, pushBackForce;
    [SerializeField] private float maximumChargingSeconds, maximumSwingSeconds;
    private float currentChargingSeconds, currentSwingSeconds;
    [SerializeField] private Transform target;
    [SerializeField] private Animator anim;
    private int state = 0;
    private Rigidbody rb;
    private Ene_EnemyTest enemyTest;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        try
        { enemyTest = GetComponent<Ene_EnemyTest>(); }
        catch { }
        SetState(0, 0);
        StartCoroutine(SearchNearestPlayer());
    }

    IEnumerator SearchNearestPlayer()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            float minDistance = Mathf.Infinity;
            target = null;
            foreach (GameObject player in players)
            {
                float distance = Vector3.Distance(transform.position, player.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    target = player.transform;
                }
            }
        }
    }

    void FixedUpdate()
    {
        if (target != null)
        {
            Vector3 directionToTarget = (target.position - transform.position).normalized;
            bool isFar = Vector3.Distance(transform.position, target.position) > minDistance;


            if (isFar && state == 0)
            {
                rb.AddForce(transform.forward * speed);
            }
            else if (!isFar && state == 0)
            {
                SetState(1, 0);
            }
            else if (isFar && state != 0)
            {
                SetState(0, 0);
            }
            else if (!isFar && state == 1)
            {
                if (currentChargingSeconds > 0)
                {
                    currentChargingSeconds -= 1f;
                }
                else
                {
                    SetState(2, 0);
                }
            }
            else if (!isFar && state == 2)
            {
                if (currentSwingSeconds > 0)
                {
                    currentSwingSeconds -= 1f;
                }
                else
                {
                    if(Vector3.Distance(transform.position, target.position) < pushBackDistance)
                    { SetState(1, pushBackForce); }
                    else
                    { SetState(0, 0);}
                }
            }

            if(enemyTest != null && enemyTest.stunned)
            {
                SetState(0, 0);
            }

            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, raycastDis))
            {
                Vector2 rbVel2D = new Vector2(rb.velocity.x, rb.velocity.z);
                if (rbVel2D.magnitude < 0.15f && state == 0)
                {
                    rb.velocity = new Vector3(rbVel2D.x, jumpForce, rbVel2D.y);
                }
            }

            float angle = Mathf.Atan2(directionToTarget.x, directionToTarget.z) * Mathf.Rad2Deg;
            Quaternion finalRotation = Quaternion.Euler(0, angle, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, finalRotation, rotationSpeed);
        }

        rb.AddForce(Vector3.down * extraGravity);

        Vector3 rbVel = rb.velocity;
        rb.velocity = new Vector3(rbVel.x / velDivisor, rbVel.y, rbVel.z / velDivisor);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Vector3.down * raycastDis);
    }

    private void SetState(int _state, float _pushBackForce)
    {
        state = _state;
        anim.SetInteger("state", _state);
        rb.AddForce(-transform.forward * _pushBackForce);

        currentChargingSeconds = maximumChargingSeconds * 60f;
        currentSwingSeconds = maximumSwingSeconds * 60f;

        if (state == 2)
        {
            rb.AddForce(transform.forward * attackForce);
            if (enemyTest != null) { enemyTest.canDealDamage = true; }
        }
        else
        {
            if (enemyTest != null) { enemyTest.canDealDamage = false; }
        }
    }
}
