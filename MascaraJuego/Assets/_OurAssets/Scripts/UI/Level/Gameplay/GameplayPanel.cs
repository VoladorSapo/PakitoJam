using NaughtyAttributes;
using PrimeTween;
using Reflex.Attributes;
using UnityEngine;

public class GameplayPanel : MonoBehaviour
{
    [Inject] GameEvents gameEvents;
    
    [HorizontalLine(color: EColor.White)]
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] float appearDuration;
    
    void Awake()
    {
        canvasGroup = gameObject.GetOrAddComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        
        gameEvents.OnRoundStarted += FadeIn;
    }

    void OnDestroy()
    {
        gameEvents.OnRoundStarted -= FadeIn;
    }
    
    void FadeIn()
    {
        Tween.Custom(
            startValue: 0f,
            endValue: 1f,
            duration: appearDuration,
            onValueChange: value => canvasGroup.alpha = value
        ).OnComplete(() =>
        {
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        });
    }
}
