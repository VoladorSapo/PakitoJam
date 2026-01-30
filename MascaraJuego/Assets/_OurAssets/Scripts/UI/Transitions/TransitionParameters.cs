using System;
using UnityEngine;

public enum TransitionType
{
    None = -1,
    Fade = 0,
}

[System.Serializable]
public class TransitionParameters
{
    
    public TransitionType TransitionType;
    public bool ClosingDirection;
    public float Duration;
    
    public Color Color;
    
    public float EndWaitTime;
    public Action OnCompleteAction;
    
    //Extras
    public float FloatValue1 = 0f;

    public static TransitionParameters GetTransitionParameters => new TransitionParameters();
    private TransitionParameters()
    {
        TransitionType = TransitionType.Fade;
        ClosingDirection = true;
        Duration = 1f;
        Color = Color.white;
        
        EndWaitTime = 0;
        OnCompleteAction = null;
    }
    
    public TransitionParameters CloneWithOppositeDirection()
    {
        TransitionParameters parameters = GetTransitionParameters;
        parameters.ClosingDirection = !ClosingDirection;
        return parameters;
    }
}