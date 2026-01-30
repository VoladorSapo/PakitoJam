using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class AudioEntry
{
    public string Name;
    public AudioResource AudioResource;
}

[CreateAssetMenu(fileName = "AudioGallery", menuName = "Game/AudioGallery")]
public class AudioGallery : ScriptableObject
{
    public GameObject AudioProducerPrefab;
    
    [SerializeField] List<AudioEntry> audioResources;

    private Dictionary<string, AudioResource> audioGalleryDictionary = new();

    public void Initialize()
    {
        audioGalleryDictionary ??= new Dictionary<string, AudioResource>();
        
        audioGalleryDictionary.Clear();
        foreach (AudioEntry audioEntry in audioResources)
        {
            Debug.Log($"Adding {audioEntry.Name} to Audio Gallery");
            audioGalleryDictionary.Add(audioEntry.Name, audioEntry.AudioResource);
        }
    }
    
    public bool TryGetAudioResource(string name, out AudioResource audioResource)
    {
        return audioGalleryDictionary.TryGetValue(name, out audioResource);
    }
}
