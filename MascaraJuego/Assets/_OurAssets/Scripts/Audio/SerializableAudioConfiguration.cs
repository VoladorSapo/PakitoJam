using System;
using NaughtyAttributes;
using UnityEngine;

[Serializable]
public class SerializableAudioConfiguration
{
    [HorizontalLine(color: EColor.Green)]
    [SerializeField] string audioResourceName;
    [SerializeField] bool loop;
    [SerializeField] bool isMusic;
    [SerializeField] [Range(0,1)] float volume = 1f;
    [SerializeField] [Range(0,3)] float pitch = 1f;
    [SerializeField] [Range(0,1)] float spatialBlend = 0f;
    [SerializeField] int maxSimultaneousPlays = 10;
    
    [HorizontalLine(color: EColor.Green)]
    [SerializeField] bool useTargetPosition;
    [SerializeField, HideIf("useTargetPosition"), AllowNesting] Vector3 position;
    [SerializeField, ShowIf("useTargetPosition"), AllowNesting] Transform target;
    public AudioConfiguration ToRuntime(AudioConfiguration.Builder builder)
    {
        position = useTargetPosition ? target.position : position;
        
        return builder.WithResource(audioResourceName)
            .Looping(loop)
            .AsMusic(isMusic)
            .WithVolume(volume)
            .WithPitch(pitch)
            .WithSpatialBlend(spatialBlend)
            .WithMaxSimultaneousPlays(maxSimultaneousPlays)
            .AtPosition(position)
            .BuildConfiguration();
    }
}