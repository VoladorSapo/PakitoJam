using System.Collections;
using PrimeTween;
using Reflex.Attributes;
using TMPro;
using UnityEngine;

public class FinalCountdown : MonoBehaviour
{
    [Inject] GameEvents gameEvents;
    [Inject] AudioManager audioManager;
    [SerializeField] CanvasGroup canvasGroup;
    
    [SerializeField] Transform textTransform;
    [SerializeField] private TextMeshProUGUI textMesh;
    
    [SerializeField] RectTransform backImage;
    [SerializeField] float rotateSpeed;
    private bool wasLosing;
    private bool hasLost;
    
    Coroutine coroutine;
    private Tween fadeTween;
    AudioConfiguration audioConfiguration;
    void Awake()
    {
        gameEvents.OnLosingAlerted += ProcessAlert;
        canvasGroup.alpha = 0;
        audioConfiguration = audioManager.CreateAudioBuilder().WithResource("ui back").BuildConfiguration();
    }
    void OnDestroy()
    {
        gameEvents.OnLosingAlerted -= ProcessAlert;
        Tween.StopAll();
    }

    void ProcessAlert(bool isLosing)
    {
        if(hasLost) return;
        
        Debug.LogWarning(wasLosing);
        
        if (wasLosing && !isLosing)
        {
            CancelLosing();
        }else if (!wasLosing && isLosing)
        {
            StartLosing();
        }

        
    }

    void StartLosing()
    {
        if(fadeTween.isAlive) fadeTween.Stop();
        textMesh.text = "";
        fadeTween = FadeIn(1);
        wasLosing = true;
        coroutine = StartCoroutine(IEBeginCountdown());
    }

    void CancelLosing()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }
        if(fadeTween.isAlive) fadeTween.Stop();
        
        fadeTween = FadeOut(1);
        wasLosing = false;
        
    }

    IEnumerator IEBeginCountdown()
    {
        yield return new WaitForSeconds(0.25f);
        
        textMesh.text = "3";
        textTransform.localScale = Vector3.one * 1.5f;
        Tween.Scale(textTransform, 1, 1, Ease.InOutQuad);
        audioManager.PlayAudio(audioConfiguration);
        
        yield return new WaitForSeconds(1);
        
        textMesh.text = "2";
        textTransform.localScale = Vector3.one * 1.5f;
        Tween.Scale(textTransform, 1, 1, Ease.InOutQuad);
        audioManager.PlayAudio(audioConfiguration);
        
        yield return new WaitForSeconds(1);
        
        textMesh.text = "1";
        textTransform.localScale = Vector3.one * 1.5f;
        Tween.Scale(textTransform, 1, 1, Ease.InOutQuad);
        audioManager.PlayAudio(audioConfiguration);
        
        yield return new WaitForSeconds(1);
        
        audioManager.PlayAudio(audioConfiguration);
        gameEvents.NotifyRoundEnd(false);
        FadeOut(1);
        hasLost = true;
    }

    void Update()
    {
        backImage.Rotate(0f, 0f, rotateSpeed * Time.deltaTime);
    }
    
    Tween FadeIn(float duration)
    {
        return Tween.Custom(
            startValue: 0f,
            endValue: 0.7f,
            duration: duration,
            onValueChange: value => canvasGroup.alpha = value
        );
    }
    Tween FadeOut(float duration)
    {
        
        return Tween.Custom(
            startValue: 0.7f,
            endValue: 0f,
            duration: duration,
            onValueChange: value => canvasGroup.alpha = value
        );
    }
}
