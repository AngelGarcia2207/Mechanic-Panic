using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SFX_AudioConfig : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    void Start()
    {
        if (PlayerPrefs.HasKey("masterVolume"))
        {
            loadMasterVolumeSettings();
        }
        else
        {
            SetMasterVolume();
        }

        if (PlayerPrefs.HasKey("musicVolume"))
        {
            loadMusicVolumeSettings();
        }
        else
        {
            SetMusicVolume();
        }

        if (PlayerPrefs.HasKey("sfxVolume"))
        {
            loadSFXVolumeSettings();
        }
        else
        {
            SetSFXVolume();
        }
    }

    public void SetMasterVolume()
    {
        float volume = masterSlider.value;
        audioMixer.SetFloat("Master", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("masterVolume", volume);
    }

    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        audioMixer.SetFloat("Music", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("musicVolume", volume);
    }

    public void SetSFXVolume()
    {
        float volume = sfxSlider.value;
        audioMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("sfxVolume", volume);
    }

    private void loadMasterVolumeSettings()
    {
        float volume = PlayerPrefs.GetFloat("masterVolume");
        masterSlider.value = volume;
        audioMixer.SetFloat("Master", Mathf.Log10(volume) * 20);
    }

    private void loadMusicVolumeSettings()
    {
        float volume = PlayerPrefs.GetFloat("musicVolume");
        musicSlider.value = volume;
        audioMixer.SetFloat("Music", Mathf.Log10(volume) * 20);
    }

    private void loadSFXVolumeSettings()
    {
        float volume = PlayerPrefs.GetFloat("sfxVolume");
        sfxSlider.value = volume;
        audioMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
    }
}
