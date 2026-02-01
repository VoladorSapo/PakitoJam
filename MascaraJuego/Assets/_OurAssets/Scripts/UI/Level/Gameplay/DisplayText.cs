using NaughtyAttributes;
using PrimeTween;
using Reflex.Attributes;
using TMPro;
using UnityEngine;

public class DisplayText : MonoBehaviour
{
    [Inject] GameEvents gameEvents;
    [Inject] GameSettings gameSettings;

    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] TextMeshProUGUI textMesh;
    
    [SerializeField, MinMaxSlider(1, 3)] Vector2 randomScaleRange = new Vector2(0.8f, 1.2f);
    [SerializeField, MinMaxSlider(-75, 75)] Vector2 randomRotationRange = new Vector2(-25f, 25f);
    
    Sequence sequence;
    void Awake()
    {
        gameEvents.OnDisplayTextCalled += ProcessRequest;
    }

    void OnDestroy()
    {
        gameEvents.OnDisplayTextCalled -= ProcessRequest;
    }
    void ProcessRequest(string text)
    {
        if (sequence.isAlive)
            sequence.Complete();

        textMesh.text = text;
        
        float randomZ = Random.Range(randomRotationRange.x, randomRotationRange.y);
        transform.localRotation = Quaternion.Euler(0f, 0f, randomZ);
        
        float randomScale = Random.Range(randomScaleRange.x, randomScaleRange.y);
        transform.localScale = new Vector3(randomScale, randomScale, 1f);
        
        sequence = Sequence.Create(cycles: 1).Chain(FadeIn()).Chain(FadeOut());

        Tween.ShakeLocalPosition(transform, strength: new Vector3(3, 3), duration: 1, frequency: 20);
    }


    Tween FadeIn()
    {
        return Tween.Custom(
            startValue: 0f,
            endValue: 1f,
            duration: 1,
            onValueChange: value => canvasGroup.alpha = value
        );
    }
    Tween FadeOut()
    {
        return Tween.Custom(
            startValue: 1f,
            endValue: 0f,
            duration: 1,
            onValueChange: value => canvasGroup.alpha = value
        );
    }
}
