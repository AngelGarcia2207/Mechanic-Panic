using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX_Lenador_AudioClips : MonoBehaviour
{
    [SerializeField] private AudioClip[] idleAudioClips;
    [SerializeField] private AudioClip[] moveAudioClips;
    [SerializeField] private AudioClip[] swayAudioClips;
    [SerializeField] private AudioClip[] damageAudioClips;
    [SerializeField] private AudioClip[] deathAudioClips;

    void Update()
    {
        
    }

    public void moveAudio()
    {
        SFX_Manager.Instance.PlaySpatialRandomSFXClip(moveAudioClips, transform, 1f);
    }

    public void swayAudio()
    {
        SFX_Manager.Instance.PlaySpatialRandomSFXClip(swayAudioClips, transform, 1f);
    }

    public void damageAudio()
    {
        SFX_Manager.Instance.PlaySpatialRandomSFXClip(damageAudioClips, transform, 1f);
    }

    public void deathAudio()
    {
        SFX_Manager.Instance.PlaySpatialRandomSFXClip(deathAudioClips, transform, 1f);
    }
}
