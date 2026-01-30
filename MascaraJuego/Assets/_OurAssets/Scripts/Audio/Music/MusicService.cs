using UnityEngine;

public class MusicService
{
    private readonly AudioManager audioManager;
    private readonly AudioConfiguration.Builder builder;
    
    private readonly AudioProducer[] musicProducers;
    
    public MusicService(AudioManager audioManager, int musicTracksCount)
    {
        this.audioManager = audioManager;
        builder = audioManager.CreateAudioBuilder();
        builder.Looping(true).AsMusic(true).WithMaxSimultaneousPlays(1);
        musicProducers = new AudioProducer[musicTracksCount];
    }
    
    public void PlayMusic(string nextMusicName, int atTrack, float crossFadeDuration = 2)
    {
        var newProducer = builder.WithResource(nextMusicName).PlayAudio(crossFadeDuration);
        if(newProducer == null) return;
        
        var previousProducer = musicProducers[atTrack];
        previousProducer?.StopWithFade(crossFadeDuration);
        
        musicProducers[atTrack] = newProducer;
        newProducer.OnReturnedToPool += () => ReleaseProducer(atTrack, newProducer);
    }

    public void StopMusic(int atTrack, float fadeOutDuration)
    {
        musicProducers[atTrack]?.StopWithFade(fadeOutDuration);
    }

    void ReleaseProducer(int trackIndex, AudioProducer producer)
    {
        if(producer != null && musicProducers[trackIndex] == producer) musicProducers[trackIndex] = null;
    }
    
}
