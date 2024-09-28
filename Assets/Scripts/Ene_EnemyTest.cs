using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Ene_EnemyTest : MonoBehaviour
{
    [SerializeField] private GameObject damageTag;
    [SerializeField] private Vector3 knockbackDirection;
    [SerializeField] private float stunDuration;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) {
            Mov_Player_Controller playerScript = other.GetComponent<Mov_Player_Controller>();

            playerScript.applyKnockBack(knockbackDirection);

            playerScript.applyStun(stunDuration);
        }

        if(other.CompareTag("WeaponBase"))
        {
            Obj_Weapon playerWeapon = other.GetComponent<Obj_Weapon>();
            Debug.Log(playerWeapon.DealDamage());
            GameObject newTag = Instantiate(damageTag, other.gameObject.transform.position, Quaternion.identity);
            newTag.GetComponent<TMP_Text>().text = playerWeapon.DealDamage().ToString();
            Destroy(newTag, 1f);
        }
        else if(other.CompareTag("WeaponComplement"))
        {
            Obj_Weapon playerWeapon = other.transform.parent.GetComponent<Obj_Weapon>();
            Debug.Log(playerWeapon.DealDamage(other.gameObject.transform.GetSiblingIndex()));
            GameObject newTag = Instantiate(damageTag, other.gameObject.transform.position, Quaternion.identity);
            newTag.GetComponent<TMP_Text>().text = playerWeapon.DealDamage(other.gameObject.transform.GetSiblingIndex()).ToString();
            Destroy(newTag, 1f);
        }
    }
}
