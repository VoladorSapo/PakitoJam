using Reflex.Attributes;
using UnityEngine;

public class AwakeFadingObject : MonoBehaviour
{
    [Inject] GameEvents gameEvents;
    [SerializeField] Animator animator;

    void Awake()
    {
        gameEvents.OnRoundAwakened += PlayAnimation;
    }

    void PlayAnimation()
    {
        animator.enabled = true;
        animator.Play("Pulse");
    }

    public void OnPulsingAnimationEnd()
    {
        animator.enabled = false;
        gameEvents.OnRoundAwakened -= PlayAnimation;
    }
}
