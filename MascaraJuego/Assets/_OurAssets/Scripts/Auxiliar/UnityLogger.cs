using UnityEngine;

public class UnityLogger : MonoBehaviour
{
    [SerializeField] private bool enabledLogging = true;
    private bool IsEnabled() => enabledLogging && enabled;
    
    public void Log(string message)
    {
        if (IsEnabled()) Debug.Log(message);
    }

    public void LogWarning(string message)
    {
        if (IsEnabled()) Debug.LogWarning(message);
    }

    public void LogError(string message)
    {
        if (IsEnabled()) Debug.LogError(message);
    }
}