using System;
using UnityEngine;

public class GameEvents
{
    public event Action OnRoundAwakened;
    public void NotifyRoundAwakened()
    {
        OnRoundAwakened?.Invoke();
    }
    
    public event Action OnRoundStarted;
    public void NotifyRoundStart()
    {
        OnRoundStarted?.Invoke();
    }
    
    public event Action<bool> OnRoundEnded;
    public void NotifyRoundEnd(bool hasWon)
    {
        OnRoundEnded?.Invoke(hasWon);
    }

    public event Action OnResultScreenCalled;
    public void NotifyResultScreenCalled()
    {
        OnResultScreenCalled?.Invoke();
    }
}
