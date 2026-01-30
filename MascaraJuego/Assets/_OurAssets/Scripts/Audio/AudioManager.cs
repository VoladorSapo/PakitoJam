using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

public class AudioManager
{
    private AudioGallery audioGallery;
    private ObjectPool<GameObject> audioProducerPool;
    
    private readonly HashSet<AudioProducer> activeAudioProducers = new();
    
    public int GetPlayingAudioCount() => activeAudioProducers.Count;
    public int GetPlayingAudioCount(string withName) => activeAudioProducers.Count(p => p.PlayingResourceName == withName);
    public AudioManager(AudioGallery audioGallery)
    {
        this.audioGallery = audioGallery;
        audioProducerPool = PoolHelper.CreatePool(createFunc: CreateAudioProducer);
    }
    GameObject CreateAudioProducer()
    {
        var go = Object.Instantiate(audioGallery.AudioProducerPrefab);
        go.name = "Audio Producer";
        Object.DontDestroyOnLoad(go);
        
        var producer = go.GetComponent<AudioProducer>();
        producer.Initialize(this);

        return go;
    }
    public void ReturnAudioProducer(AudioProducer audioProducer)
    {
        activeAudioProducers.Remove(audioProducer);
        
        audioProducerPool.Release(audioProducer.gameObject);
    }
    
    
    public AudioConfiguration.Builder CreateAudioBuilder() => new AudioConfiguration.Builder(this);
    public bool TryGetAudioResource(string resourceName, out AudioResource resource) => audioGallery.TryGetAudioResource(resourceName, out resource);
    public AudioProducer PlayAudio(AudioConfiguration audioConfiguration, float fadeDuration = 0)
    {
        if(audioConfiguration == null) return null;
        if (!CanPlayAudio(audioConfiguration)) return null;
        
        AudioProducer audioProducer = audioProducerPool.Get().GetComponent<AudioProducer>();
        audioProducer.Configure(audioConfiguration);
        
        activeAudioProducers.Add(audioProducer);
        //Debug.LogWarning(activeAudioProducers.Count + " audio producers playing");
        
        if(fadeDuration == 0) audioProducer.Play();
        else audioProducer.PlayWithFade(fadeDuration);
        
        return audioProducer;
    }
    private bool CanPlayAudio(AudioConfiguration configuration)
    {
        int playedAudioResourceNamesCount = GetPlayingAudioCount(configuration.AudioResourceName);
        Debug.Log($"Playing {configuration.AudioResourceName} ({playedAudioResourceNamesCount}/{configuration.MaxSimultaneousPlays})");
            
        return playedAudioResourceNamesCount < configuration.MaxSimultaneousPlays;
    }

    public void StopAll(float fadeDuration = 0)
    {
        foreach (var audioProducer in activeAudioProducers) StopProducer(audioProducer, fadeDuration);
    }
    public void StopAll(string withName, float fadeDuration = 0)
    {
        foreach (var audioProducer in activeAudioProducers.Where(p => p.PlayingResourceName == withName)) StopProducer(audioProducer, fadeDuration);
    }
    void StopProducer(AudioProducer audioProducer, float fadeDuration = 0)
    {
        if(fadeDuration == 0) audioProducer.Stop();
        else audioProducer.StopWithFade(fadeDuration);
    }
    
    public void PauseAll(float fadeDuration = 0)
    {
        foreach (var audioProducer in activeAudioProducers) PauseProducer(audioProducer, fadeDuration);
    }
    public void PauseAll(string withName, float fadeDuration = 0)
    {
        foreach (var audioProducer in activeAudioProducers.Where(p => p.PlayingResourceName == withName)) PauseProducer(audioProducer, fadeDuration);
    }
    void PauseProducer(AudioProducer audioProducer, float fadeDuration = 0)
    {
        if(fadeDuration == 0) audioProducer.Pause();
        else audioProducer.PauseWithFade(fadeDuration);
    }
    
    public void ResumeAll(float fadeDuration = 0)
    {
        foreach (var audioProducer in activeAudioProducers) ResumeProducer(audioProducer, fadeDuration);
    }
    public void ResumeAll(string withName, float fadeDuration = 0)
    {
        foreach (var audioProducer in activeAudioProducers.Where(p => p.PlayingResourceName == withName)) ResumeProducer(audioProducer, fadeDuration);
    }
    void ResumeProducer(AudioProducer audioProducer, float fadeDuration = 0)
    {
        if(fadeDuration == 0) audioProducer.Resume();
        else audioProducer.ResumeWithFade(fadeDuration);
    }
}