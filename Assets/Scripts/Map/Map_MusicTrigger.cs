using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map_MusicTrigger : MonoBehaviour
{
    [SerializeField] private AudioClip musicClip;
    [SerializeField] [Range(0f, 1f)] private float volume;
    [SerializeField] private bool isLooping = false;
    private bool hasBeenTriggered = false;

    private void OnTriggerEnter(Collider collision) {
        if (collision.gameObject.CompareTag("Player") && !hasBeenTriggered) {
            SFX_Manager.Instance.PlayMusic(musicClip, volume, isLooping);

            hasBeenTriggered = true;
        }
    }
}
