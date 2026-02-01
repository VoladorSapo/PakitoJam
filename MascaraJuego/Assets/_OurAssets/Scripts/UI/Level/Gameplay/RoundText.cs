using PrimeTween;
using Reflex.Attributes;
using TMPro;
using UnityEngine;

public class RoundText : MonoBehaviour
{
    [Inject] GameEvents gameEvents;
    [SerializeField] TextMeshProUGUI textMesh;
    Sequence pulseSequence;
    void Awake()
    {
        gameEvents.OnDifficultyIncreased += UpdateRoundText;
    }
    
    void UpdateRoundText(int difficulty)
    {
        difficulty++;
        textMesh.text = $"ROUND\n{difficulty.ToString()}";
        Pulse();
    }

    void Pulse()
    {
        Transform coinCounterTransform = this.transform;
        if(pulseSequence.isAlive) pulseSequence.Complete();
        
        pulseSequence = Sequence.Create(cycles: 1, CycleMode.Rewind)
            .Chain(Tween.Scale(coinCounterTransform, coinCounterTransform.localScale.x + 0.1f, 0.25f, Ease.OutBounce))
            .Chain(Tween.Scale(coinCounterTransform, 1f, 0.25f, Ease.OutBounce));
        
        Tween.ShakeLocalPosition(transform, strength: new Vector3(3, 3), duration: 1, frequency: 20);
    }
}