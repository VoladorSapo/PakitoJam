using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "Singleton Locator", menuName = "Game/Singleton Locator")]
public class SingletonLocator : ScriptableObject
{
    [System.NonSerialized] public TransitionController TransitionController;
    [System.NonSerialized] public GlobalCamera GlobalCamera; 
    [System.NonSerialized] public SaveClient SaveClient;
}