using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX_MainMenuMusic : MonoBehaviour
{
    [SerializeField] private AudioClip musicClip;
    [SerializeField] [Range(0f, 1f)] private float volume;
    [SerializeField] private bool isLooping = true;

    void Start()
    {
        SFX_Manager.Instance.PlayMusic(musicClip, volume, isLooping);
    }
}
