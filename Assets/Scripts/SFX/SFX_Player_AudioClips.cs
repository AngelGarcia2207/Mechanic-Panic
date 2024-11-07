using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX_Player_AudioClips : MonoBehaviour
{
    [SerializeField] private AudioClip[] swingAudioClips;
    [SerializeField] private AudioClip[] damageAudioClips;
    [SerializeField] private AudioSource walkingAudioClip;

    public void swingAudio()
    {
        SFX_Manager.Instance.PlayRandomSFXClip(swingAudioClips, transform, 0.7f);
    }

    public void damageAudio()
    {
        SFX_Manager.Instance.PlayRandomSFXClip(damageAudioClips, transform, 1f);
    }

    public void walkingAudio()
    {
        if (!walkingAudioClip.isPlaying)
        {
            walkingAudioClip.Play(); // Como necesito controlar este audio, no puedo hacerlo a través de SFX_Manager
        }
    }

    public void stopWalkingAudio()
    {
        if (walkingAudioClip.isPlaying)
        {
            walkingAudioClip.Stop(); // Como necesito controlar este audio, no puedo hacerlo a través de SFX_Manager
        }
    }
}
