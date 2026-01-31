using UnityEngine;
using UnityEngine.UI;
using PrimeTween;
using TMPro;

public class TextParticle : MonoBehaviour {
    [SerializeField] TMP_Text textMesh;
    [SerializeField] Transform target;
    [SerializeField] float distance = 20f;
    [SerializeField] float duration = 2f;

    Sequence sequence;
    public void PlayAnimation(string text, Color color)
    {
        textMesh.text = text;
        textMesh.color = color;
        
        textMesh.alpha = 0f;
        float halfDuration = duration / 2f;
        Vector3 startPos = target.localPosition;
        Vector3 halfPos = startPos + Vector3.up * distance / 2;
        Vector3 endPos = startPos + Vector3.up * distance;
        
        sequence = Sequence.Create(cycles: 1, cycleMode: Sequence.SequenceCycleMode.Rewind)
            .Group(Tween.LocalPosition(target, halfPos, halfDuration,Ease.InQuad))
            .Group(Tween.Custom(startValue: 0f, endValue: 1f, duration: halfDuration, onValueChange: v => textMesh.alpha = v))
            .Chain(Tween.Custom(startValue: 1f, endValue: 0f, duration: halfDuration, onValueChange: v => textMesh.alpha = v))
            .Group(Tween.LocalPosition(target, endPos, halfDuration,Ease.OutQuad))
            .OnComplete(()=>Destroy(this.gameObject));
    }

    public void CancelAnimation() {
        if(sequence.isAlive)
            sequence.Complete();
    }
}