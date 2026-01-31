using System;
using UnityEngine;

public class GameEvents
{
    #region Round Flow
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
    public event Action<bool> OnRoundPaused;
    public void NotifyRoundPaused(bool isPaused)
    {
        OnRoundPaused?.Invoke(isPaused);
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

    public event Action<string> OnDisplayTextCalled;
    public void InvokeDisplayText(string text)
    {
        OnDisplayTextCalled?.Invoke(text);
    }

    #endregion
    
    #region Gameplay

    public event Action<int> OnCoinsCollected;
    public void NotifyCoinCollection(int coinsAmount)
    {
        OnCoinsCollected?.Invoke(coinsAmount);
    }
    public event Action<int> OnCoinsSpent;
    public void NotifyCoinSpent(int coinsAmount)
    {
        OnCoinsSpent?.Invoke(coinsAmount);
    }
    
    public event Action<int> OnTotalCoinsCollected;
    public void NotifyTotalCoinsCollected(int totalCoinsAmount)
    {
        OnTotalCoinsCollected?.Invoke(totalCoinsAmount);
    }

    public event Action OnEnemyKilled;
    public void NotifyEnemyDeath()
    {
        OnEnemyKilled?.Invoke();
    }
    
    public event Action<int> OnDifficultyIncreased;

    public void NotifyDifficultyIncreased(int currentDifficulty)
    {
        OnDifficultyIncreased?.Invoke(currentDifficulty);
    }

    public event Action<bool> OnLosingAlerted;
    public void InvokeLosingAlert(bool isLosing)
    {
        OnLosingAlerted?.Invoke(isLosing);
    }

    #endregion

}
