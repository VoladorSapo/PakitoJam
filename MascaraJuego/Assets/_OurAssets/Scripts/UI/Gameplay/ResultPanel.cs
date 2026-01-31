using NaughtyAttributes;
using UnityEngine;
using PrimeTween;
using Reflex.Attributes;

public class ResultPanel : MonoBehaviour
{
    [Inject] GameEvents gameEvents;
    
    [HorizontalLine(color: EColor.White)]
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] float fadeDuration = 1;
    
    [HorizontalLine(color: EColor.White)]
    [SerializeField] GameObject winPanel;
    [SerializeField] GameObject losePanel;
    void Awake()
    {
        winPanel.SetActive(false);
        losePanel.SetActive(false);
        
        canvasGroup = gameObject.GetOrAddComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
        
        gameEvents.OnRoundEnded += ProcessRoundEnd;
        gameEvents.OnResultScreenCalled += FadeIn;
    }
    void OnDestroy()
    {
        gameEvents.OnRoundEnded -= ProcessRoundEnd;
        gameEvents.OnResultScreenCalled -= FadeIn;
    }

    void ProcessRoundEnd(bool hasWon)
    {
        winPanel.SetActive(hasWon);
        losePanel.SetActive(!hasWon);
    }
    
    void FadeIn() {
        Tween.Custom(
            startValue: 0f,
            endValue: 1f,
            duration: fadeDuration,
            onValueChange: value => canvasGroup.alpha = value
        );
    }
    
}
