using System;
using Reflex.Attributes;
using UnityEngine;

public class SaveClient : MonoBehaviour
{
    [Inject] SingletonLocator singletonLocator;
    [Inject] GameSettings settings;
    
    [SerializeField] bool loadOnAwake = true;
    void Awake()
    {
        var instance = singletonLocator.SaveClient;
        if (instance == null)
        {
            singletonLocator.SaveClient = this;
            DontDestroyOnLoad(gameObject.transform.root.gameObject);
            
            settings.Load(!loadOnAwake);
        }
        else
        {
            Destroy(gameObject.transform.root.gameObject);
        }
    }

    private void Start()
    {
        if (singletonLocator.SaveClient == this)
            settings.Load(!loadOnAwake);
    }

    void OnDestroy()
    {
        if (singletonLocator.SaveClient == this)
        {
            Save();
            singletonLocator.SaveClient = null;
        }
    }
    
    public void Save()
    {
        settings.Save();
    }
    public void Load()
    {
        settings.Load(false);
    }
}