using System;
using PrimeTween;
using Reflex.Attributes;
using UnityEngine;

public class RingObject : MonoBehaviour
{
    [Inject] GameEvents gameEvents;

    [SerializeField] private float adjustDuration = 1;
    private void Awake()
    {
        gameEvents.OnRoundStarted += AdjustTransform;
    }

    private void OnDestroy()
    {
        gameEvents.OnRoundStarted -= AdjustTransform;
    }

    void AdjustTransform()
    {
        Tween.Scale(transform, 1, adjustDuration, Ease.InOutQuad);
        Tween.LocalPositionY(transform, 0, adjustDuration, Ease.OutSine);
    }
}
