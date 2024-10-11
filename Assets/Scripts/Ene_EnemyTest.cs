using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Ene_EnemyTest : MonoBehaviour
{
    [SerializeField] private GameObject damageTag;
    [SerializeField] private Animator enemyAnimator;
    [SerializeField] private Vector3 knockbackDirection;
    [SerializeField] private float stunDuration;
    [SerializeField] private float shakeSpeed = 100f, shakeForce = 0.1f;
    private bool stunned = false, swayStart = false;
    private Vector3 initialPosition, leftTarget, rightTarget, frontTarget, backTarget;
    private Queue<Vector3> targetsQueue = new();

    // Start is called before the first frame update
    void Start()
    {
        initialPosition = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(stunned)  
        {
            SwayAnimation();
        } 
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) {
            Mov_Player_Controller playerScript = other.GetComponent<Mov_Player_Controller>();

            playerScript.applyKnockBack(knockbackDirection);

            playerScript.applyStun(stunDuration);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("WeaponBase"))
        {
            Obj_Player_Weapon playerWeapon = other.GetComponent<Obj_Player_Weapon>();
            //Debug.Log(playerWeapon.DealDamage());
            GameObject newTag = Instantiate(damageTag, other.gameObject.transform.position, Quaternion.identity);
            newTag.GetComponent<TMP_Text>().text = playerWeapon.DealDamage().ToString();
            Destroy(newTag, 1f);

            if(stunned == false)
            {
                enemyAnimator.enabled = false;
                stunned = true;
                swayStart = true;
                GenerateTargets();
            }
        }
        else if(other.CompareTag("WeaponComplement"))
        {
            Obj_Player_Weapon playerWeapon = other.transform.parent.GetComponent<Obj_Player_Weapon>();
            //Debug.Log(playerWeapon.DealDamage(other.gameObject.transform.GetSiblingIndex()));
            GameObject newTag = Instantiate(damageTag, other.gameObject.transform.position, Quaternion.identity);
            newTag.GetComponent<TMP_Text>().text = playerWeapon.DealDamage(other.gameObject.transform.GetSiblingIndex()).ToString();
            Destroy(newTag, 1f);

            if(stunned == false)
            {
                enemyAnimator.enabled = false;
                stunned = true;
                swayStart = true;
                GenerateTargets();
            }
        }
    }

    private void SwayAnimation()
    {
        if(swayStart)
        {
            this.transform.position = new Vector3(Mathf.Lerp(this.transform.position.x, targetsQueue.Peek().x, Time.deltaTime * shakeSpeed),
                this.transform.position.y, Mathf.Lerp(this.transform.position.z, targetsQueue.Peek().z, Time.deltaTime * shakeSpeed));
            if(new Vector3(Mathf.Round(this.transform.position.x*1000f), this.transform.position.y, Mathf.Round(this.transform.position.z*1000f))
                == new Vector3(Mathf.Round(targetsQueue.Peek().x*1000f), this.transform.position.y, Mathf.Round(targetsQueue.Peek().z*1000f)))
            {
                swayStart = false;
            }
        }
        else
        {
            this.transform.position = new Vector3(Mathf.Lerp(this.transform.position.x, initialPosition.x, Time.deltaTime * shakeSpeed),
                this.transform.position.y, Mathf.Lerp(this.transform.position.z, initialPosition.z, Time.deltaTime * shakeSpeed));
            if(new Vector3(Mathf.Round(this.transform.position.x*1000f), this.transform.position.y, Mathf.Round(this.transform.position.z*1000f))
                == new Vector3(Mathf.Round(initialPosition.x*1000f), this.transform.position.y, Mathf.Round(initialPosition.z*1000f)))
            {
                targetsQueue.Dequeue();
                swayStart = true;
            }
        }

        if(targetsQueue.Count == 0)
        {
            enemyAnimator.enabled = true;
            stunned = false;
            swayStart = false;
            this.transform.position = initialPosition;
        }
    }

    private void GenerateTargets()
    {
        initialPosition = this.transform.position;

        leftTarget = new Vector3(this.transform.position.x - Random.Range(shakeForce, shakeForce*2f), this.transform.position.y,
            this.transform.position.z + Random.Range(shakeForce*0.5f, shakeForce));
        rightTarget = new Vector3(this.transform.position.x + Random.Range(shakeForce, shakeForce*2f), this.transform.position.y,
            this.transform.position.z - Random.Range(shakeForce*0.5f, shakeForce));
        frontTarget = new Vector3(this.transform.position.x - Random.Range(shakeForce*0.5f, shakeForce), this.transform.position.y,
            this.transform.position.z - Random.Range(shakeForce, shakeForce*2f));
        backTarget = new Vector3(this.transform.position.x + Random.Range(shakeForce*0.5f, shakeForce), this.transform.position.y,
            this.transform.position.z + Random.Range(shakeForce, shakeForce*2f));

        targetsQueue.Enqueue(leftTarget);
        targetsQueue.Enqueue(rightTarget);
        targetsQueue.Enqueue(frontTarget);
        targetsQueue.Enqueue(backTarget);
    }
}
