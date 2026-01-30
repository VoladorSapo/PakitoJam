using System;
using System.Collections.Generic;
using UnityEngine;
using PrimeTween;

public enum TweenType {
    Scale,
    Move,
}

public enum EaseProxy {
    Custom = -1,
    Default = 0,
    Linear = 1,
    InSine, OutSine, InOutSine,
    InQuad, OutQuad, InOutQuad,
    InCubic, OutCubic, InOutCubic,
    InQuart, OutQuart, InOutQuart,
    InQuint, OutQuint, InOutQuint,
    InExpo, OutExpo, InOutExpo,
    InCirc, OutCirc, InOutCirc,
    InElastic, OutElastic, InOutElastic,
    InBack, OutBack, InOutBack,
    InBounce, OutBounce, InOutBounce
}
public class TweenComponent : MonoBehaviour
{
    [SerializeField] private Transform targetTransform;

    private Dictionary<TweenType, Tween> activeTweens = new();

    // Estado original para Transform
    private Vector3 originalLocalPos;
    private Quaternion originalLocalRot;
    private Vector3 originalLocalScale;

    // Estado original para RectTransform
    private RectTransform rect;
    private Vector2 originalAnchoredPos;

    private void Awake()
    {
        rect = targetTransform as RectTransform;

        if (rect != null)
        {
            // UI
            originalAnchoredPos = rect.anchoredPosition;
            originalLocalRot = rect.localRotation;
            originalLocalScale = rect.localScale;
        }
        else
        {
            // Transform normal
            originalLocalPos = targetTransform.localPosition;
            originalLocalRot = targetTransform.localRotation;
            originalLocalScale = targetTransform.localScale;
        }
    }

    public static Ease ToPrimeTweenEase(EaseProxy proxy)
    {
        return (Ease)(sbyte)proxy;
    }

    private void SafeTween(TweenType type, Tween tween)
    {
        if (activeTweens.TryGetValue(type, out var activeTween))
        {
            if (activeTween.isAlive)
                activeTween.Stop();
        }
        activeTweens[type] = tween;
    }

    #region Scale

    public void UniformScaleTo(float targetScale, EaseProxy ease, float duration)
    {
        var tween = Tween.Scale(targetTransform, Vector3.one * targetScale, duration, ease: ToPrimeTweenEase(ease));
        SafeTween(TweenType.Scale, tween);
    }

    public void ScaleTo(Vector3 targetScale, EaseProxy ease, float duration)
    {
        var tween = Tween.Scale(targetTransform, targetScale, duration, ease: ToPrimeTweenEase(ease));
        SafeTween(TweenType.Scale, tween);
    }

    #endregion

    #region Move

    public void LocalMoveTo(Vector3 localTargetPos, EaseProxy ease, float duration)
    {
        Tween tween;

        if (rect != null)
        {
            tween = Tween.UIAnchoredPosition(rect, localTargetPos, duration, ease: ToPrimeTweenEase(ease));
        }
        else
        {
            tween = Tween.LocalPosition(targetTransform, localTargetPos, duration, ease: ToPrimeTweenEase(ease));
        }

        SafeTween(TweenType.Move, tween);
    }

    public void LocalTranslate(Vector3 offset, EaseProxy ease, float duration)
    {
        Tween tween;

        if (rect != null)
        {
            Vector2 target = rect.anchoredPosition + (Vector2)offset;
            tween = Tween.UIAnchoredPosition(rect, target, duration, ease: ToPrimeTweenEase(ease));
        }
        else
        {
            Vector3 target = targetTransform.localPosition + offset;
            tween = Tween.LocalPosition(targetTransform, target, duration, ease: ToPrimeTweenEase(ease));
        }

        SafeTween(TweenType.Move, tween);
    }

    #endregion

    private void OnDisable()
    {
        ResetAnimated(0.01f, EaseProxy.Default);
    }

    public void ResetAnimated(float duration, EaseProxy ease)
    {
        foreach (var kvp in activeTweens)
        {
            if (kvp.Value.isAlive)
                kvp.Value.Stop();
        }
        activeTweens.Clear();

        Tween moveTween;

        if (rect != null)
        {
            moveTween = Tween.UIAnchoredPosition(rect, originalAnchoredPos, duration, ease: ToPrimeTweenEase(ease));
        }
        else
        {
            moveTween = Tween.LocalPosition(targetTransform, originalLocalPos, duration, ease: ToPrimeTweenEase(ease));
        }

        var scaleTween = Tween.Scale(targetTransform, originalLocalScale, duration, ease: ToPrimeTweenEase(ease));

        SafeTween(TweenType.Move, moveTween);
        SafeTween(TweenType.Scale, scaleTween);
    }
}

