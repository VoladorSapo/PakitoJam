using System.Collections;
using NaughtyAttributes;
using PrimeTween;
using UnityEngine;

[System.Serializable]
public class Credits
{
    public CanvasGroup canvasGroup;
    [HorizontalLine(color: EColor.White), AllowNesting]
    public float FadeInDelayTime;
    public float FadeInTime;
    public float ShowTime = 7.5f;
    public float FadeOutTime;
}
public class CreditsCanvas : MonoBehaviour
{
    [HorizontalLine(color: EColor.White)]
    [SerializeField] Credits[] credits;
    [SerializeField] Credits finalCredits;
    
    [HorizontalLine(color: EColor.White)]
    [SerializeField] SceneServiceClient sceneServiceClient;
    [SerializeField, Scene] string menuScene;
    
    bool finishedCredits = false;
    Coroutine coroutine;
    Credits currentCredits;
    Tween currentTween;
    void Start()
    {
        foreach (var credit in credits)
        {
            credit.canvasGroup.alpha = 0;
        }
        finalCredits.canvasGroup.alpha = 0;
        
        coroutine = StartCoroutine(IEFadingCredits());
    }

    IEnumerator IEFadingCredits()
    {
        for (int i = 0; i < credits.Length; i++)
        {
            Credits credit = credits[i];
            currentCredits = credit;
            yield return StartCoroutine(IEFadeCredit(credit));
        }

        finishedCredits = true;
        coroutine = null;
        
        yield return StartCoroutine(IEFadeCredit(finalCredits));
        sceneServiceClient.ChangeScene(menuScene);
    }

    IEnumerator IEFadeCredit(Credits credit)
    {
        yield return new WaitForSeconds(credit.FadeInDelayTime);
        
        currentTween = FadeIn(credit.canvasGroup, credit.FadeInTime);
        yield return new WaitForSeconds(credit.FadeInTime + credit.ShowTime);
        
        currentTween = FadeOut(credit.canvasGroup, credit.FadeOutTime);
        yield return new WaitForSeconds(credit.FadeOutTime);
    }
    Tween FadeIn(CanvasGroup canvasGroup, float duration) {
        return Tween.Custom(
            startValue: 0f,
            endValue: 1f,
            duration: duration,
            onValueChange: value => canvasGroup.alpha = value
        ).OnComplete(() => canvasGroup.interactable = true);
    }
    
    Tween FadeOut(CanvasGroup canvasGroup, float duration) {
        return Tween.Custom(
            startValue: 1f,
            endValue: 0f,
            duration: duration,
            onValueChange: value => canvasGroup.alpha = value
        );
    }

    void Update()
    {
        if(finishedCredits) return;
        
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            finishedCredits = true;
            
            if(currentTween.isAlive) currentTween.Complete();
            StopCoroutine(coroutine);
            
            if(currentCredits != null)
                FadeOut(currentCredits.canvasGroup, currentCredits.FadeOutTime);
            
            StartCoroutine(IEFadeCredit(finalCredits));

            float totalTime = finalCredits.FadeInTime + finalCredits.FadeOutTime + finalCredits.FadeInDelayTime + 0.5f;
            this.InvokeDelayed(totalTime, () => sceneServiceClient.ChangeScene(menuScene) );
        }
    }
}
