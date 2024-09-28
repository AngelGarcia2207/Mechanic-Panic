using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBackCollider : MonoBehaviour
{
    [SerializeField] private Vector3 knockbackDirection;
    [SerializeField] private float stunDuration;

    private void OnTriggerEnter(Collider collision) {
        if (collision.gameObject.CompareTag("Player")) {
            Mov_Player_Controller playerScript = collision.GetComponent<Mov_Player_Controller>();

            playerScript.applyKnockBack(knockbackDirection);

            playerScript.applyStun(stunDuration);
        }
    }
}
