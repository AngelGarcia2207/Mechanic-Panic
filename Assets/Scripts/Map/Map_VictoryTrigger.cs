using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map_VictoryTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision) {
        if (collision.CompareTag("CameraCenter")) {
            GameManager.Instance.LevelComplete();
        }
    }
}
