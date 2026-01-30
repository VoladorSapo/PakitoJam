using System;
using System.Collections;
using UnityEngine;

public static class GameObjectExtensions
{
    public static T GetOrAddComponent<T>(this GameObject go) where T : Component => go.GetComponent<T>() ?? go.AddComponent<T>();
    
    public static void InvokeDelayed(this MonoBehaviour mb, float seconds, Action action)
    {
        mb.StartCoroutine(InvokeRoutine(seconds, action));
    }
    private static IEnumerator InvokeRoutine(float seconds, Action action)
    {
        yield return new WaitForSeconds(seconds);
        action?.Invoke();
    }

}