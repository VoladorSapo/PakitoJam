using PrimeTween;
using Reflex.Attributes;
using TMPro;
using UnityEngine;

public class AwakeFadingObject : MonoBehaviour
{
    [Inject] GameEvents gameEvents;
    [SerializeField] Animator animator;
    [SerializeField] TextMeshProUGUI textMesh;
    [SerializeField] private string[] possibleMessages;
    void Awake()
    {
        gameEvents.OnRoundAwakened += PlayAnimation;
    }

    void OnDestroy()
    {
        gameEvents.OnRoundAwakened -= PlayAnimation;
    }

    void PlayAnimation()
    {
        textMesh.text = possibleMessages[Random.Range(0, possibleMessages.Length)];
        
        animator.enabled = true;
        animator.Play("Pulse");
        
        Tween.ShakeLocalPosition(transform, strength: new Vector3(3, 3), duration: 2, frequency: 20);
    }

    public void OnPulsingAnimationEnd()
    {
        animator.enabled = false;
        gameEvents.NotifyRoundStart();
        gameEvents.OnRoundAwakened -= PlayAnimation;
    }
}
