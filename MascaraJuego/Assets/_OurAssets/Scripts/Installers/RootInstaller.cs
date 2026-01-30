using NaughtyAttributes;
using Reflex.Core;
using UnityEngine;

public class RootInstaller : MonoBehaviour, IInstaller
{
    [HorizontalLine(color: EColor.Orange)]
    [SerializeField, Expandable] AudioGallery audioGallery;
    
    [HorizontalLine(color: EColor.Orange)]
    [SerializeField, Expandable] GameSettings gameSettings;
    [SerializeField, Expandable] SingletonLocator singletonLocator;
    
    public void InstallBindings(ContainerBuilder builder)
    {
        builder.RegisterValue(new AudioManager(audioGallery));
        builder.RegisterValue(gameSettings);
        builder.RegisterValue(new GameEvents());
        builder.RegisterValue(singletonLocator);
        
        //Servicios
        builder.RegisterValue(new SceneService());
        builder.RegisterValue(new PauseService());
    }
}
