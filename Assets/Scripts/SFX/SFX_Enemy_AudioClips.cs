using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX_Enemy_AudioClips : MonoBehaviour
{
    [SerializeField] private float minWaitTime = 2f;
    [SerializeField] private float maxWaitTime = 5f;
    [SerializeField] private AudioClip[] defaultAudioClips;
    [SerializeField] private AudioClip[] moveAudioClips;
    [SerializeField] private AudioClip[] chargeAudioClips;
    [SerializeField] private AudioClip[] swayAudioClips;
    [SerializeField] private AudioClip[] aimAudioClips;
    [SerializeField] private AudioClip[] shootAudioClips;
    [SerializeField] private AudioClip[] damageAudioClips;
    [SerializeField] private AudioClip[] deathAudioClips;
    [SerializeField] private AudioClip[] itemDropAudioClips;

    void Start()
    {
        StartCoroutine(DefaultSFXCoroutine());
    }

    private IEnumerator DefaultSFXCoroutine()
    {
        float waitTime = Random.Range(minWaitTime, maxWaitTime);
        yield return new WaitForSeconds(waitTime);

        if (defaultAudioClips != null && defaultAudioClips.Length > 0)
        {
            SFX_Manager.Instance.PlaySpatialRandomSFXClip(defaultAudioClips, transform, 1f);
            StartCoroutine(DefaultSFXCoroutine());
        }
    }

    public void moveAudio()
    {
        if (moveAudioClips != null && moveAudioClips.Length > 0)
        {
            SFX_Manager.Instance.PlaySpatialRandomSFXClip(moveAudioClips, transform, 1f);
        }
    }

    public void chargeAudio()
    {
        if (chargeAudioClips != null && chargeAudioClips.Length > 0)
        {
            SFX_Manager.Instance.PlaySpatialRandomSFXClip(chargeAudioClips, transform, 1f);
        }
    }

    public void swayAudio()
    {
        if (swayAudioClips != null && swayAudioClips.Length > 0)
        {
            SFX_Manager.Instance.PlaySpatialRandomSFXClip(swayAudioClips, transform, 1f);
        }
    }

    public void aimAudio()
    {
        if (aimAudioClips != null && aimAudioClips.Length > 0)
        {
            SFX_Manager.Instance.PlaySpatialRandomSFXClip(aimAudioClips, transform, 1f);
        }
    }

    public void shootAudio()
    {
        if (shootAudioClips != null && shootAudioClips.Length > 0)
        {
            SFX_Manager.Instance.PlaySpatialRandomSFXClip(shootAudioClips, transform, 1f);
        }
    }

    public void damageAudio()
    {
        if (damageAudioClips != null && damageAudioClips.Length > 0)
        {
            SFX_Manager.Instance.PlaySpatialRandomSFXClip(damageAudioClips, transform, 0.7f);
        }
    }

    public void deathAudio()
    {
        if (deathAudioClips != null && deathAudioClips.Length > 0)
        {
            SFX_Manager.Instance.PlaySpatialRandomSFXClip(deathAudioClips, transform, 0.7f);
        }
    }

    public void itemDropAudio()
    {
        if (itemDropAudioClips != null && itemDropAudioClips.Length > 0)
        {
            SFX_Manager.Instance.PlaySpatialRandomSFXClip(itemDropAudioClips, transform, 0.7f);
        }
    }
}