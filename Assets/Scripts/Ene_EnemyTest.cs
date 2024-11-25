using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class Ene_EnemyTest : MonoBehaviour
{
    [SerializeField] private GameObject damageTag;
    [SerializeField] private Animator enemyAnimator, shakeAnimator;
    [SerializeField] private GameObject smokeEffect;
    [SerializeField] private Vector3 knockbackDirection;
    [SerializeField] private float stunDuration;
    [SerializeField] private float shakeSpeed = 100f, shakeForce = 0.1f;
    [SerializeField] private bool showsDamageAnim, showsDeathAnim;
    [SerializeField] private int playerDamage = 10;
    public bool canDealDamage = true;
    [SerializeField] private int maxHealth;
    public int currentHealth;
    [HideInInspector] public bool stunned = false, swayStart = false;
    private Vector3 initialPosition, leftTarget, rightTarget, frontTarget, backTarget;
    private Queue<Vector3> targetsQueue = new();
    private List<Collider> hittingColliders = new();
    [SerializeField] private float dropChance;
    [SerializeField] private List<GameObject> dropList;
    [SerializeField] private SFX_Enemy_AudioClips audioClips;
    public UnityEvent death = new();

    void Start()
    {
        initialPosition = this.transform.position;
        currentHealth = maxHealth;
    }

    void Update()
    {
        if(stunned)  
        {
            //Debug.Log(stunned);
            //SwayAnimation();
        } 
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && canDealDamage == true) {
            //DamagePlayer(other.gameObject);
        }
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Player") && canDealDamage == true)
        {
            DamagePlayer(col.gameObject);
        }
    }

    private void DamagePlayer(GameObject playerObj)
    {
        Mov_Player_Controller playerScript = playerObj.GetComponent<Mov_Player_Controller>();

        playerScript.receiveDamageRaw(playerDamage);

        playerScript.applyKnockBack(knockbackDirection);

        playerScript.applyStun(stunDuration);
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

           //Debug.Log("sc");
            DamageEnemy(playerWeapon.DealDamage(), other);
        }
        else if(other.CompareTag("WeaponComplement"))
        {
            Obj_Player_Weapon playerWeapon = other.transform.parent.GetComponent<Obj_Player_Weapon>();
            //Debug.Log(playerWeapon.DealDamage(other.gameObject.transform.GetSiblingIndex()));
            playerWeapon.ActivateEffects(other.gameObject.transform.GetSiblingIndex(), transform);
            GameObject newTag = Instantiate(damageTag, other.gameObject.transform.position, Quaternion.identity);
            newTag.GetComponent<TMP_Text>().text = playerWeapon.DealDamage(other.gameObject.transform.GetSiblingIndex()).ToString();
            Destroy(newTag, 1f);

            //Debug.Log("sc2");
            DamageEnemy(playerWeapon.DealDamage(), other);
        }
    }

    private void SwayAnimation()
    {
        Debug.Log(swayStart);

        if (swayStart)
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

        /*if(targetsQueue.Count == 0)
        {
            enemyAnimator.enabled = true;
            stunned = false;
            swayStart = false;
            this.transform.position = initialPosition;
            Debug.Log(GetComponent<Mov_BasicEnemy>().stunned);
            GetComponent<Mov_BasicEnemy>().stunned = false;
            Debug.Log(GetComponent<Mov_BasicEnemy>().stunned);
        }*/
    }

    private void DamageEnemy(int damage, Collider hittingCollider)
    {
        if((stunned == false || (stunned && hittingColliders.Contains(hittingCollider) == false)) && currentHealth > 0)
        {
            shakeAnimator.SetInteger("ShakeState", 1);
            if (showsDamageAnim) { enemyAnimator.SetBool("damaged", true); }
            else { enemyAnimator.enabled = false; }
            audioClips.damageAudio();

            GameManager.Instance.increaseLevelScore(damage);

            GenerateTargets();

            currentHealth -= damage;
            if (currentHealth <= 0)
            {
                QuitStun();
                audioClips.deathAudio();
                death.Invoke();
                StartCoroutine(DropItem());
                canDealDamage = false;
                if (showsDeathAnim) { enemyAnimator.SetBool("death", true); }
            }
            StartCoroutine(QuitStunCoroutine(currentHealth));

            GameObject smokeObj = Instantiate(smokeEffect, this.transform.position, Quaternion.identity);
            Destroy(smokeObj, 1f);
            swayStart = true;
            stunned = true;
            hittingColliders.Add(hittingCollider);
        }
    }

    IEnumerator QuitStunCoroutine(int currentHealth)
    {
        yield return new WaitForSeconds(0.5f);
        QuitStun();
        hittingColliders.Clear();
        if(currentHealth <= 0)
        {
            shakeAnimator.SetInteger("ShakeState", 2);
            Destroy(gameObject, 2f);
        }
    }

    private void QuitStun()
    {
        shakeAnimator.SetInteger("ShakeState", 0);
        if (showsDamageAnim) { enemyAnimator.SetBool("damaged", false); }
        else { enemyAnimator.enabled = true; }
        stunned = false;
        swayStart = false;
        //this.transform.position = initialPosition;
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

    IEnumerator DropItem()
    {
        yield return new WaitForSeconds(1f);
        if(Random.value <= dropChance/100)
        {
            GameObject droppedItem = Instantiate(dropList[Random.Range(0, dropList.Count-1)], transform.position, Quaternion.identity);
            droppedItem.transform.localEulerAngles = new Vector3(-90, 0, 0);
            droppedItem.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(10, 20), 10, Random.Range(10, 20)));
            audioClips.itemDropAudio();
        }
    }
}
