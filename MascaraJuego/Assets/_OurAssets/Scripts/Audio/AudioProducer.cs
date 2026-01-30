using System;
using System.Collections;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Audio;

public class AudioProducer : MonoBehaviour
{
    public event Action OnReturnedToPool;
    
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioMixerGroup sfxMixer;
    [SerializeField] private AudioMixerGroup musicMixer;
    
    [SerializeField, ReadOnly] private string resourceName;
    [SerializeField, ReadOnly] private bool isPaused;
    [SerializeField, ReadOnly] private bool hasPlayed;
    
    private AudioManager audioManager;
    Coroutine volumeRoutine;
    float targetVolume;
    
    public string PlayingResourceName => resourceName;
    
    public float Volume => audioSource.volume;
    public float Pitch => audioSource.pitch;
    public float SpatialBlend => audioSource.spatialBlend;
    
    public void SetVolume(float volume) => audioSource.volume = volume;
    public void SetPitch(float pitch) => audioSource.pitch = pitch;
    public void SetSpatialBlend(float blend) => audioSource.spatialBlend = blend;

    public void Initialize(AudioManager manager)
    {
        audioManager = manager;
    }
    public void Configure(AudioConfiguration options)
    {
        Stop();
        Reset();

        resourceName = options.AudioResourceName;
        
        audioSource.resource = options.AudioResource;
        audioSource.loop = options.Loop;
        audioSource.volume = options.Volume;
        audioSource.pitch = options.Pitch;
        audioSource.spatialBlend = options.SpatialBlend;
        audioSource.outputAudioMixerGroup = options.IsMusic ? musicMixer : sfxMixer;
        
        transform.position = options.Position;
    }
    
    public bool IsPlaying => audioSource.isPlaying && !isPaused;
    void Reset()
    {
        if (volumeRoutine != null)
        {
            StopCoroutine(volumeRoutine);
            volumeRoutine = null;
        }

        isPaused = false;
        hasPlayed = false;
    }
    public void Play(float fadeInTime = 0f)
    {
        if (!audioSource.isPlaying)
        {
            hasPlayed = true;
            audioSource.Play();
        }
    }
    public void PlayWithFade(float fadeInTime)
    {
        targetVolume = audioSource.volume;
        audioSource.volume = 0f;

        Play();

        StartFade(0f, targetVolume, fadeInTime);
    }

    public void Pause()
    {
        if(isPaused) return;
        
        isPaused = true;
        audioSource.Pause();
    }
    public void PauseWithFade(float fadeTime)
    {
        if (isPaused) return;

        StartFade(audioSource.volume, 0f, fadeTime, Pause);
    }
    public void Resume()
    {
        if(!isPaused) return;
        
        isPaused = false;
        audioSource.UnPause();
    }
    public void ResumeWithFade(float fadeTime)
    {
        if (!isPaused) return;

        Resume();
        StartFade(0f, targetVolume, fadeTime);
    }
    
    public void Stop()
    {
        if(audioSource.isPlaying) audioSource.Stop();
    }
    public void StopWithFade(float fadeOutTime)
    {
        if (!audioSource.isPlaying) return;

        StartFade(audioSource.volume, 0f, fadeOutTime, () =>
        {
            Stop();
            ReturnToPool();
        });
    }
    
    void Update()
    {
        if (!audioSource.isPlaying && !isPaused && hasPlayed)
        {
            ReturnToPool();
        }
    }

    void ReturnToPool()
    {
        Reset();
        audioManager.ReturnAudioProducer(this);
        OnReturnedToPool?.Invoke();
        OnReturnedToPool = null;
    }
    
    void StartFade(float from, float to, float duration, Action onComplete = null)
    {
        if (volumeRoutine != null)
            StopCoroutine(volumeRoutine);

        volumeRoutine = StartCoroutine(FadeVolume(from, to, duration, onComplete));
    }

    IEnumerator FadeVolume(float from, float to, float duration, Action onComplete = null)
    {
        if (duration <= 0f)
        {
            audioSource.volume = to;
            onComplete?.Invoke();
            yield break;
        }

        float t = 0f;
        audioSource.volume = from;

        while (t < duration)
        {
            t += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(from, to, t / duration);
            yield return null;
        }

        audioSource.volume = to;
        onComplete?.Invoke();
    }
}