using Reflex.Core;
using UnityEngine;

public class RoundScopeInstaller : MonoBehaviour, IInstaller
{
    [SerializeField] RoundScoreTracker scoreTracker;
    public void InstallBindings(ContainerBuilder builder)
    {
        builder.RegisterValue(scoreTracker);
    }
}
