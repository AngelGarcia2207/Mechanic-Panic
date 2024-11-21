using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX_Player_AudioClips : MonoBehaviour
{
    [SerializeField] private AudioClip[] swingAudioClips;
    [SerializeField] private AudioClip[] dodgeAudioClips;
    [SerializeField] private AudioClip[] itemPickupAudioClips;
    [SerializeField] private AudioClip[] armorPickupAudioClips;
    [SerializeField] private AudioClip[] damageAudioClips;
    [SerializeField] private AudioClip[] deathAudioClips;
    [SerializeField] private AudioSource walkingAudioClip;

    public void swingAudio()
    {
        SFX_Manager.Instance.PlayRandomSFXClip(swingAudioClips, transform, 0.5f);
    }

    public void dodgeAudio()
    {
        SFX_Manager.Instance.PlayRandomSFXClip(dodgeAudioClips, transform, 0.5f);
    }

    public void itemPickupAudio()
    {
        SFX_Manager.Instance.PlayRandomSFXClip(itemPickupAudioClips, transform, 0.5f);
    }

    public void armorPickupAudio()
    {
        SFX_Manager.Instance.PlayRandomSFXClip(armorPickupAudioClips, transform, 0.5f);
    }

    public void damageAudio()
    {
        SFX_Manager.Instance.PlayRandomSFXClip(damageAudioClips, transform, 0.6f);
    }

    public void deathAudio()
    {
        SFX_Manager.Instance.PlayRandomSFXClip(deathAudioClips, transform, 0.6f);
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