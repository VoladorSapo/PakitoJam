using NaughtyAttributes;
using PrimeTween;
using Reflex.Core;
using UnityEngine;

public class RootInstaller : MonoBehaviour, IInstaller
{
    [HorizontalLine(color: EColor.Orange)]
    [SerializeField, Expandable] AudioGallery audioGallery;
    [SerializeField] private int musicTracks = 5;
    
    [HorizontalLine(color: EColor.Orange)]
    [SerializeField, Expandable] GameSettings gameSettings;
    [SerializeField, Expandable] SingletonLocator singletonLocator;
    [SerializeField, Expandable] PrefabLocator prefabLocator;
    
    public void InstallBindings(ContainerBuilder builder)
    {
        PrimeTweenConfig.warnEndValueEqualsCurrent = false;
        PrimeTweenConfig.warnTweenOnDisabledTarget = false;
        
        var audioManager = new AudioManager(audioGallery);
        var musicService = new MusicService(audioManager, musicTracks);
        
        builder.RegisterValue(audioManager);
        builder.RegisterValue(gameSettings);
        builder.RegisterValue(new GameEvents());
        builder.RegisterValue(new UIEvents());
        builder.RegisterValue(singletonLocator);
        builder.RegisterValue(prefabLocator);
        
        //Servicios
        builder.RegisterValue(new SceneService());
        builder.RegisterValue(new PauseService());
        builder.RegisterValue(musicService);
    }
}
