using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX_Lenador_AudioClips : MonoBehaviour
{
    [SerializeField] private AudioClip[] moveAudioClips;
    [SerializeField] private AudioClip[] swayAudioClips;
    [SerializeField] private AudioClip[] damageAudioClips;
    [SerializeField] private AudioClip[] deathAudioClips;

    public void moveAudio()
    {
        SFX_Manager.Instance.PlayRandomSFXClip(moveAudioClips, transform, 0.7f);
    }

    public void swayAudio()
    {
        SFX_Manager.Instance.PlayRandomSFXClip(swayAudioClips, transform, 0.7f);
    }

    public void damageAudio()
    {
        SFX_Manager.Instance.PlayRandomSFXClip(damageAudioClips, transform, 0.7f);
    }

    public void deathAudio()
    {
        SFX_Manager.Instance.PlayRandomSFXClip(deathAudioClips, transform, 0.7f);
    }
}
