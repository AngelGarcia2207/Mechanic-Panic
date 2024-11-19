using System.Collections;
using UnityEngine;

public class SFX_Manager : MonoBehaviour
{
    [SerializeField] private AudioSource SFXObject;
    [SerializeField] private AudioSource MusicObject;
    [SerializeField] private float transitionTime = 1.0f;
    private AudioSource currentMusicObject;
    
    public static SFX_Manager Instance { get; private set; }
    

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Update()
    {
        if (Time.timeScale == 0f && currentMusicObject != null) // Pausar la musica cuando se pausa el juego
        {
            currentMusicObject.Pause();
        }
        else if (currentMusicObject != null)
        {
            currentMusicObject.UnPause();
        }
    }

    public void PlaySFXClip(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        AudioSource audioSource = Instantiate(SFXObject, spawnTransform.position, Quaternion.identity);

        audioSource.clip = audioClip;

        audioSource.volume = volume;

        audioSource.Play();

        float clipLength = audioSource.clip.length;

        Destroy(audioSource.gameObject, clipLength);
    }

    public void PlayRandomSFXClip(AudioClip[] audioClips, Transform spawnTransform, float volume)
    {
        AudioSource audioSource = Instantiate(SFXObject, spawnTransform.position, Quaternion.identity);

        audioSource.clip = audioClips[Random.Range(0, audioClips.Length)];

        audioSource.volume = volume;

        audioSource.Play();

        float clipLength = audioSource.clip.length;

        Destroy(audioSource.gameObject, clipLength);
    }

    public void PlayMusic(AudioClip musicClip, float volume, bool isLooping)
    {
        if (currentMusicObject == null)
        {
            currentMusicObject = Instantiate(MusicObject, transform.position, Quaternion.identity, transform);
        }

        if (currentMusicObject.isPlaying)
        {
            StartCoroutine(FadeOutAndIn(musicClip, volume, isLooping));
        }
        else
        {
            currentMusicObject.clip = musicClip;
            currentMusicObject.volume = volume;

            if (isLooping)
            {
                currentMusicObject.loop = true;
            }
            else
            {
                currentMusicObject.loop = false;
            }

            currentMusicObject.Play();
        }
    }

    private IEnumerator FadeOutAndIn(AudioClip newClip, float newVolume, bool isLooping)
    {
        float startVolume = currentMusicObject.volume;
        float timeElapsed = 0;

        while (timeElapsed < transitionTime)
        {
            currentMusicObject.volume = Mathf.Lerp(startVolume, 0, timeElapsed / transitionTime);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        currentMusicObject.volume = 0;
        currentMusicObject.Stop();

        currentMusicObject.clip = newClip;
        currentMusicObject.loop = isLooping;
        currentMusicObject.Play();

        timeElapsed = 0;
        while (timeElapsed < transitionTime)
        {
            currentMusicObject.volume = Mathf.Lerp(0, newVolume, timeElapsed / transitionTime);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        currentMusicObject.volume = newVolume;
    }
}
