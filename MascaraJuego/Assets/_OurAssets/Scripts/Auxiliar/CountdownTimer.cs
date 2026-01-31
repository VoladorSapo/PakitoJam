using NaughtyAttributes;
using UnityEngine;

[System.Serializable]
public class CountdownTimer
{
    [HorizontalLine(2f, EColor.Green)]
    [SerializeField] float countdownTime;
    [SerializeField] bool autoReset;

    [SerializeField, ReadOnly, AllowNesting] float remainingTime;

    public float RemainingTime => remainingTime;
    public float NormalizedRemainingTime => remainingTime / countdownTime;
    public float CountdownTime
    {
        get => countdownTime;
        set => countdownTime = value;
    }

    public bool AutoReset
    {
        get => autoReset;
        set => autoReset = value;
    }

    public void SetCountdownTime(float time, bool reset = true)
    {
        CountdownTime = time;
        
        if(reset)
            ResetCountdown();
    }
    public void ResetCountdown()
    {
        remainingTime = countdownTime;
    }

    public bool HasCounterFinished() => remainingTime <= 0f;

    public bool Decrement(float deltaTime)
    {
        if (remainingTime <= 0f) return true;
        remainingTime -= deltaTime;

        bool hasFinished = HasCounterFinished();
        if (hasFinished && autoReset)
        {
            ResetCountdown();
        }
        return hasFinished;
    }
}