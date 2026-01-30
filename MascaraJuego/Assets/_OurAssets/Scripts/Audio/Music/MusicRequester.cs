using NaughtyAttributes;
using Reflex.Attributes;
using UnityEngine;

public class MusicRequester : MonoBehaviour
{
    [SerializeField] bool playOnAwake;
    
    [HorizontalLine(color: EColor.Green)]
    [SerializeField] string musicName;
    [SerializeField] int playOnTrack;
    [SerializeField] float crossFadeDuration;
    
    [Inject] MusicService musicService;
    void Awake()
    {
        if (playOnAwake) RequestMusic();
    }
    public void RequestMusic() => musicService.PlayMusic(musicName, playOnTrack, crossFadeDuration);
}
