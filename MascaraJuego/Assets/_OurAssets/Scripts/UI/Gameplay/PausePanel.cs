using NaughtyAttributes;
using PrimeTween;
using UnityEngine;

[RequireComponent(typeof(PauseServiceClient))]
public class PausePanel : MonoBehaviour, IPausable
{
    [HorizontalLine(color: EColor.White)]
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] float fadeDuration = 1;
    
    Tween currentTween;

    void Awake()
    {
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
    }
    public void Pause()
    {
        FadeIn();
    }

    public void Resume()
    {
        FadeOut();
    }
    
    void FadeIn() {
        if(currentTween.isAlive) currentTween.Stop();
        
        currentTween = Tween.Custom(
            startValue: 0f,
            endValue: 1f,
            duration: fadeDuration,
            onValueChange: value => canvasGroup.alpha = value
        ).OnComplete(() => canvasGroup.interactable = true);
    }
    
    void FadeOut() {
        if(currentTween.isAlive) currentTween.Stop();
        
        canvasGroup.interactable = false;
        currentTween = Tween.Custom(
            startValue: 1f,
            endValue: 0f,
            duration: fadeDuration,
            onValueChange: value => canvasGroup.alpha = value
        );
    }
}
