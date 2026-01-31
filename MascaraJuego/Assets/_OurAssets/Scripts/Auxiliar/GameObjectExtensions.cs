using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

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

public static class AuxiliaryMethods
{
    public static char GetRandomLetter()
    {
        int index = Random.Range(0, 26); 
        return (char)('A' + index);
    }
    
    public static Color GetRandomColor(Color a, Color b) {
        float t = Random.value;
        return Color.Lerp(a, b, t);
    }

}