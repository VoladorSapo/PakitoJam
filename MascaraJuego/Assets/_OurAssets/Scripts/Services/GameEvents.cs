using System;
using UnityEngine;

public class GameEvents
{
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
}
