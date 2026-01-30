using UnityEngine;
using UnityEngine.Audio;

public sealed class AudioConfiguration
{
    public AudioResource AudioResource { get; }
    public string AudioResourceName { get; }
    public bool Loop { get; }
    public bool IsMusic { get; }
    public float Volume { get; }
    public float Pitch { get; }
    public float SpatialBlend { get; }
    public int MaxSimultaneousPlays { get; }
    public Vector3 Position { get;}

    private AudioConfiguration(string audioResourceName, AudioResource audioResource, bool loop, bool isMusic, float volume, float pitch, float spatialBlend, int maxSimultaneousPlays, Vector3 position)
    {
        AudioResourceName = audioResourceName;
        AudioResource = audioResource;
        Loop = loop;
        IsMusic = isMusic;
        Volume = volume;
        Pitch = pitch;
        SpatialBlend = spatialBlend;
        MaxSimultaneousPlays = maxSimultaneousPlays;
        Position = position;
    }

    public AudioConfiguration Clone()
    {
        return new AudioConfiguration(
            AudioResourceName,
            AudioResource,
            Loop,
            IsMusic,
            Volume,
            Pitch,
            SpatialBlend,
            MaxSimultaneousPlays,
            Position
            );
    }

    public sealed class Builder
    {
        private string resourceName;
        private AudioResource audioResource;
        private bool loop;
        private bool isMusic;
        private float volume = 1f;
        private float pitch = 1f;
        private float spatialBlend = 0f;
        private int maxSimultaneousPlays = 10;
        private Vector3 position;

        private readonly AudioManager audioManager;
        public Builder(AudioManager manager)
        {
            audioManager = manager;
        }

        public Builder WithResource(string name)
        {
            this.resourceName = name;
            
            if (audioManager.TryGetAudioResource(name, out var resource))
                audioResource = resource;
            
            return this;
        }

        public Builder Looping(bool loop = true)
        {
            this.loop = loop;
            return this;
        }
        
        public Builder AsMusic(bool asMusic)
        {
            isMusic = asMusic;
            return this;
        }

        public Builder WithVolume(float volume)
        {
            this.volume = Mathf.Clamp01(volume);
            return this;
        }

        public Builder WithPitch(float pitch)
        {
            this.pitch = Mathf.Clamp(pitch, 0, 3f);
            return this;
        }

        public Builder As2D()
        {
            spatialBlend = 0f;
            return this;
        }

        public Builder As3D()
        {
            spatialBlend = 1f;
            return this;
        }
        

        public Builder WithSpatialBlend(float blend)
        {
            spatialBlend = Mathf.Clamp01(blend);
            return this;
        }

        public Builder OnlyOne()
        {
            this.maxSimultaneousPlays = 1;
            return this;
        }
        public Builder WithMaxSimultaneousPlays(int maxSimultaneousPlays)
        {
            this.maxSimultaneousPlays = Mathf.Max(1, maxSimultaneousPlays);
            return this;
        }
        
        public Builder AtPosition(Vector3 position)
        {
            this.position = position;
            return this;
        }
        public AudioProducer PlayAudio(float fadeInTime = 0f) {
            var configuration = BuildConfiguration();
            return audioManager.PlayAudio(configuration, fadeInTime);
        }
        public AudioConfiguration BuildConfiguration()
        {
            if (string.IsNullOrEmpty(resourceName))
            {
                Debug.LogError("Audio must have a name.");
                return null;
            }

            if (audioResource == null)
            {
                Debug.LogError($"Audio resource '{resourceName}' not found.");
                return null;
            }
            
            return new AudioConfiguration(resourceName, audioResource, loop, isMusic, volume, pitch, spatialBlend, maxSimultaneousPlays, position);
        }
        public AudioConfiguration BuildConfigurationAndReset()
        {
            var configuration = BuildConfiguration();
            Reset();
            return configuration;
        }
        void Reset()
        {
            resourceName = null;
            audioResource = null;
            loop = false;
            isMusic = false;
            volume = 1f;
            pitch = 1f;
            spatialBlend = 0f;
            maxSimultaneousPlays = 10;
            position = Vector3.zero;
        }
    }
}