using Reflex.Attributes;
using UnityEngine;

public class GlobalCamera : MonoBehaviour
{
    public Camera GameplayCamera;
    public Camera UICamera;
    
    [Inject] SingletonLocator singletonLocator;
    private void Awake()
    {
        var instance = singletonLocator.GlobalCamera;
        if (instance == null)
        {
            singletonLocator.GlobalCamera = this;
            DontDestroyOnLoad(gameObject.transform.root.gameObject);
        }
        else
        {
            Destroy(gameObject.transform.root.gameObject);
        }
    }
    
    void OnDestroy()
    {
        if (singletonLocator.GlobalCamera == this)
        {
            singletonLocator.GlobalCamera = null;
        }
    }
}
