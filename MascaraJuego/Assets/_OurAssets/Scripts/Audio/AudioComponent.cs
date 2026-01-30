using System;
using NaughtyAttributes;
using Reflex.Attributes;
using UnityEngine;

public class AudioComponent : MonoBehaviour
{
    [HorizontalLine(color: EColor.Green)]
    [SerializeField, Expandable] AudioGallery audioGallery;
    
    [ShowIf("HasGalleryAssigned"), SerializeField] bool playOnAwake = false;
    [ShowIf("HasGalleryAssigned"), SerializeField] SerializableAudioConfiguration serializedConfiguration;
    [ShowIf("HasGalleryAssigned"), SerializeField] float playFadeTime = 0;
    
    [Inject] AudioManager audioManager;
    private AudioConfiguration configuration;
    private AudioProducer audioProducer;
    private bool HasGalleryAssigned => audioGallery != null;
    private bool HasConfiguration => configuration != null;
    
    public AudioConfiguration GetConfiguration() => configuration;
    public void SetConfiguration(AudioConfiguration configuration) => this.configuration = configuration;
    public AudioConfiguration CloneConfiguration() => configuration.Clone();
    private void Awake()
    {
        if(!HasGalleryAssigned) return;
        
        configuration = serializedConfiguration.ToRuntime(audioManager.CreateAudioBuilder());
        
        if(playOnAwake) PlayAudio();
    }
    
    public void PlayAudio()
    {
        if (!HasConfiguration) return;

        var producer = audioManager.PlayAudio(configuration, playFadeTime);
        SetAudioProducer(producer);
    }

    void SetAudioProducer(AudioProducer newProducer)
    {
        ClearProducer();
        
        if (newProducer == null) return;

        audioProducer = newProducer;
        audioProducer.OnReturnedToPool += ClearProducer;
    }

    void ClearProducer()
    {
        if (audioProducer != null)
        {
            audioProducer.OnReturnedToPool -= ClearProducer;
            audioProducer = null;
        }
    }

}
