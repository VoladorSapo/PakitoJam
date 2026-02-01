using UnityEngine;
using UnityEngine.UI;
using PrimeTween;
using TMPro;

public enum TextParticleType
{
    Ease,
    Pow
}
public class TextParticle : MonoBehaviour {
    [SerializeField] TMP_Text textMesh;
    [SerializeField] Transform target;
    [SerializeField] float distance = 20f;
    [SerializeField] float duration = 2f;

    [SerializeField] TextParticleType type;
    Sequence sequence;
    public void PlayAnimation(string text, Color color)
    {
        textMesh.text = text;
        textMesh.color = color;
        textMesh.alpha = 0f;

        sequence = type switch
        {
            TextParticleType.Ease => EaseSequence(),
            TextParticleType.Pow => PowSequence(),
            _ => EaseSequence()
        };
    }

    Sequence EaseSequence()
    {
        float halfDuration = duration / 2f;
        Vector3 startPos = target.localPosition;
        Vector3 halfPos = startPos + Vector3.up * distance / 2;
        Vector3 endPos = startPos + Vector3.up * distance;
        
        return Sequence.Create(cycles: 1, cycleMode: Sequence.SequenceCycleMode.Rewind)
            .Group(Tween.LocalPosition(target, halfPos, halfDuration,Ease.InQuad))
            .Group(Tween.Custom(startValue: 0f, endValue: 1f, duration: halfDuration, onValueChange: v => textMesh.alpha = v))
            .Chain(Tween.Custom(startValue: 1f, endValue: 0f, duration: halfDuration, onValueChange: v => textMesh.alpha = v))
            .Group(Tween.LocalPosition(target, endPos, halfDuration,Ease.OutQuad))
            .OnComplete(()=>Destroy(this.gameObject));
    }

    Sequence PowSequence()
    {
        float thirdDuration = duration / 3f;
        Vector3 startPos = target.localPosition;
        Vector3 endPos = startPos + Vector3.up * distance / 2;

        transform.localScale = Vector3.one * 0.01f;
        return Sequence.Create(cycles: 1, cycleMode: Sequence.SequenceCycleMode.Rewind)
            .Group(Tween.Scale(transform, 0.15f, thirdDuration * 0.75f, Ease.OutBounce))
            .Group(Tween.Custom(startValue: 0f, endValue: 1f, duration: thirdDuration, onValueChange: v => textMesh.alpha = v))
            .Chain(Tween.Scale(transform, 0.1f, thirdDuration, Ease.InCubic))
            .Chain(Tween.Custom(startValue: 1f, endValue: 0f, duration: thirdDuration, onValueChange: v => textMesh.alpha = v))
            .OnComplete(()=>Destroy(this.gameObject));
    }
    
    

    public void CancelAnimation() {
        if(sequence.isAlive)
            sequence.Complete();
    }
}