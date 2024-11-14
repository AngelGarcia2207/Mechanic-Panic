using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBackColliderChainSaw : MonoBehaviour
{
    private Vector3 knockbackDirection;
    private float stunDuration;

    [SerializeField] private int damage;

    private void OnTriggerEnter(Collider collision) {
        if (collision.gameObject.CompareTag("Player")) {
            Mov_Player_Controller playerScript = collision.GetComponent<Mov_Player_Controller>();

            Vector3 knockbackDirection = GetRandomKnockbackDirection();

            playerScript.applyKnockBack(knockbackDirection);

            playerScript.receiveDamageRaw(damage);

            playerScript.applyStun(stunDuration);
        }
    }
    private Vector3 GetRandomKnockbackDirection()
    {
        // Generar un vector aleatorio en un rango específico
        float randomX = Random.Range(-12f, 13f); 
        float randomY = Random.Range(0f, 5f);  
        float randomZ = Random.Range(-5f, 5f);

        Vector3 randomDirection = new Vector3(randomX, randomY, randomZ); // Normalizar para mantener la dirección

        return randomDirection;
    }

}
