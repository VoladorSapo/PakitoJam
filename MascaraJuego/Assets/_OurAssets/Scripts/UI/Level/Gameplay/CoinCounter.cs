using PrimeTween;
using Reflex.Attributes;
using TMPro;
using UnityEngine;

public class CoinCounter : MonoBehaviour
{
    [Inject] GameEvents gameEvents;
    [SerializeField] TextMeshProUGUI coinCounter;

    Sequence pulseSequence;
    void Awake()
    {
        gameEvents.OnTotalCoinsCollected += UpdateCoinText;
    }

    void OnDestroy()
    {
        gameEvents.OnTotalCoinsCollected -= UpdateCoinText;
    }

    void UpdateCoinText(int totalAmount)
    {
        coinCounter.text = totalAmount.ToString();
        Pulse();
    }

    void Pulse()
    {
        Transform coinCounterTransform = coinCounter.transform;
        if(pulseSequence.isAlive) pulseSequence.Complete();
        
        pulseSequence = Sequence.Create(cycles: 1, CycleMode.Rewind)
            .Chain(Tween.Scale(coinCounterTransform, coinCounterTransform.localScale.x + 0.1f, 0.25f, Ease.OutBounce))
            .Chain(Tween.Scale(coinCounterTransform, 1f, 0.25f, Ease.OutBounce));
    }
}