using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public static class PoolHelper
{
    /// <summary>
    /// Crea un ObjectPool para UnityEngine.Object con activación/desactivación automática.
    /// </summary>
    public static ObjectPool<T> CreatePool<T>(
        Func<T> createFunc,
        int defaultCapacity = 10,
        int maxSize = 50,
        bool collectionCheck = false
    ) where T : UnityEngine.Object
    {
        return new ObjectPool<T>(
            createFunc,
            actionOnGet: item =>
            {
                if (!IsAlive(item)) return;
                
                SetActive(item, true);
            },
            actionOnRelease: item =>
            {
                if (!IsAlive(item)) return;
                
                SetActive(item, false);
            },
            actionOnDestroy: item =>
            {
                if (IsAlive(item))
                    UnityEngine.Object.Destroy(item);
            },
            collectionCheck: collectionCheck,
            defaultCapacity: defaultCapacity,
            maxSize: maxSize
        );
    }

    /// <summary>
    /// UnityEngine.Object puede estar destruido pero seguir siendo != null.
    /// </summary>
    private static bool IsAlive(UnityEngine.Object obj)
    {
        return obj != null; // Unity sobrecarga == para detectar destrucción
    }

    /// <summary>
    /// Activa/desactiva GameObjects o Components. Otros tipos se ignoran.
    /// </summary>
    private static void SetActive(UnityEngine.Object obj, bool active)
    {
        switch (obj)
        {
            case GameObject go:
                go.SetActive(active);
                break;

            case Component comp:
                comp.gameObject.SetActive(active);
                break;

            default: break;
        }
    }
}
