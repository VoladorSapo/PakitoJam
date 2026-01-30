using System;
using System.Collections;
using UnityEngine;

public class SquaresTransition : TransitionBase
{
    #if UNITY_EDITOR
    [ContextMenu("Preview 0%")] private void Preview0() => MaterialInstance.SetFloat(CoverValue, CoverValueRange.x);
    [ContextMenu("Preview 50%")] private void Preview50() => MaterialInstance.SetFloat(CoverValue, Mathf.Lerp(CoverValueRange.x, CoverValueRange.y, 0.5f));
    [ContextMenu("Preview 100%")] private void Preview100() => MaterialInstance.SetFloat(CoverValue, CoverValueRange.y);
    #endif

    [SerializeField] private Vector2 CoverValueRange;
    private static readonly int CoverValue  = Shader.PropertyToID("_TimeValue");
    private static readonly int BorderColor = Shader.PropertyToID("_BorderColor");
    private static readonly int BorderWidth = Shader.PropertyToID("_BorderSize");
    private static readonly int ClosingDirection = Shader.PropertyToID("_ClosingDirection");
    public override void MakeTransition(TransitionParameters parameters)
    {
        if(DoingTransition) return;
        
        Debug.Log("Transitioning");
        DoingTransition = true;
        StartCoroutine(IEHandleTransition(parameters));
    }
    protected override IEnumerator IEHandleTransition(TransitionParameters transitionData)
    {
        
        MaterialInstance.SetColor(BorderColor, transitionData.Color);
        
        MaterialInstance.SetFloat(ClosingDirection, transitionData.ClosingDirection ? 1 : 0);
        
        float fromValue = CoverValueRange.x;
        float toValue   = CoverValueRange.y;

        if (!transitionData.ClosingDirection)
            (fromValue, toValue) = (toValue, fromValue);

        yield return AnimateSquares(transitionData.Duration, fromValue,toValue);
        yield return new WaitForSecondsRealtime(transitionData.EndWaitTime);
        
        DoingTransition = false;
        transitionData.OnCompleteAction?.Invoke();
    }
    private IEnumerator AnimateSquares(float duration, float fromValue, float toValue)
    {
        if (duration <= 0f)
        {
            MaterialInstance.SetFloat(CoverValue, toValue);
            yield break;
        }
        
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = Mathf.Clamp01(time / duration);

            MaterialInstance.SetFloat(CoverValue, Mathf.Lerp(fromValue, toValue, t));
            yield return null;
        }
        
        MaterialInstance.SetFloat(CoverValue, toValue);
    }

    private void Update()
    {
        Vector2 offset = MaterialInstance.GetTextureOffset("_MainTex");
        offset.x += Time.deltaTime * 0.5f;
        MaterialInstance.SetTextureOffset("_MainTex", offset);
    }
}