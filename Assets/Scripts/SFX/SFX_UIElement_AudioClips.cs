using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX_UIElement_AudioClips : MonoBehaviour
{
    [SerializeField] private AudioClip onEnableAudioClip;
    [SerializeField] private AudioClip onDisableAudioClip;
    [SerializeField] private AudioClip selectAudioClip;
    [SerializeField] private AudioClip hoverAudioClip;
    
    void OnEnable()
    {
        if (onEnableAudioClip != null)
        {
            SFX_Manager.Instance.PlaySFXClip(onEnableAudioClip, transform, 1f);
        }   
    }

    void OnDisable()
    {
        if (onDisableAudioClip != null)
        {
            SFX_Manager.Instance.PlaySFXClip(onDisableAudioClip, transform, 1f);
        }
    }

    public void SelectAudio()
    {
        if (selectAudioClip != null)
        {
            SFX_Manager.Instance.PlaySFXClip(selectAudioClip, transform, 1f);
        }
    }

    public void HoverAudio()
    {
        if (hoverAudioClip != null)
        {
            SFX_Manager.Instance.PlaySFXClip(hoverAudioClip, transform, 1f);
        }
    }
}