using System;
using Reflex.Attributes;
using UnityEngine;

public class TransitionController : MonoBehaviour
{
    [SerializeField] private TransitionBase[] transitions;
    
    [Inject] SingletonLocator singletonLocator;
    private void Awake()
    {
        var instance = singletonLocator.TransitionController;
        if (instance == null)
        {
            singletonLocator.TransitionController = this;
        }
    }
    
    void OnDestroy()
    {
        if (singletonLocator.TransitionController == this)
        {
            singletonLocator.TransitionController = null;
        }
    }
    public void DoTransition(TransitionParameters parameters)
    {
        if (parameters == null || parameters.TransitionType == TransitionType.None) return;
            
        int index = (int)parameters.TransitionType;
        if(index >= transitions.Length || index < 0) return;
        
        transitions[index].MakeTransition(parameters);
    }
}