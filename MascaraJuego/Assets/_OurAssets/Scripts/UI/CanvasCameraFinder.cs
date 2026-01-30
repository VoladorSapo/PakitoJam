using Reflex.Attributes;
using Reflex.Extensions;
using UnityEngine;

public class CanvasCameraFinder : MonoBehaviour
{
    [SerializeField] bool isUI = false;
    [SerializeField] private float planeDistance = 0.2f;
    
    [SerializeField] RenderMode renderMode;
    [SerializeField] string sortingLayerName = "Default";
    [SerializeField] int sortingOrder = 0;
    
    [Inject] SingletonLocator singletonLocator;
    void Awake()
    {
        singletonLocator ??= gameObject.scene.GetSceneContainer().Resolve<SingletonLocator>();
        GlobalCamera globalCamera = singletonLocator.GlobalCamera;
        Canvas canvas = GetComponent<Canvas>();
        
        canvas.worldCamera = !isUI ? globalCamera.GameplayCamera : globalCamera.UICamera;
        canvas.planeDistance = planeDistance;
        canvas.renderMode = renderMode;
        canvas.sortingLayerName = sortingLayerName;
        canvas.sortingOrder = sortingOrder;
    }
    
}