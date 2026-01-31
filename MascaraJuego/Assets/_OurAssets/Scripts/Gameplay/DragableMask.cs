using Reflex.Attributes;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragableMask :MonoBehaviour, IDragHandler,IDropHandler
{
    [Inject] RoundScoreTracker scoreTracker;
    [Inject] GameEvents gameEvents;
    [Inject] SingletonLocator singletonLocator;
    [Inject] PrefabLocator prefabLocator;
    
    [SerializeField] PowerMaskStats maskStats;
    [SerializeField] Canvas canvas;
    [SerializeField] bool alwaysHit = false;
    [SerializeField] private Image cooldownImage;
    [SerializeField] CountdownTimer cooldownTimer;
    
    BasePlayerCharacter currentHoverPlayer;
    Vector3 startPosition;
    GlobalCamera globalCamera;
    RectTransform rectTransform;

    Image image;
    
    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        startPosition = transform.localPosition;
        globalCamera = singletonLocator.GlobalCamera;
        cooldownTimer.CountdownTime = maskStats.cooldown;
        image = GetComponent<Image>();
        image.sprite = maskStats.sprite;
    }

    void Update()
    {
        UpdateCooldown();
    }
    public void OnDrag(PointerEventData eventData)
    {
        if(!cooldownTimer.HasCounterFinished()) return;
        
        var screenPoint = Input.mousePosition;
        screenPoint.z = 10.0f;
        transform.position = globalCamera.UICamera.ScreenToWorldPoint(screenPoint);
        RaycastHit2D hit = Physics2D.Raycast(globalCamera.UICamera.ScreenToWorldPoint(eventData.position), Vector2.zero );
        if (hit)
        {
            BasePlayerCharacter character = hit.collider.GetComponent<BasePlayerCharacter>();
            if (character != null)
            {
                if (currentHoverPlayer == null || !currentHoverPlayer.Equals(character))
                {
                    if (currentHoverPlayer != null)
                    {
                        currentHoverPlayer.stopHover();
                    }
                    currentHoverPlayer = character;
                    currentHoverPlayer.startHover();
                }
            }
        }
        else if (currentHoverPlayer != null)
        {
            currentHoverPlayer.stopHover();
            currentHoverPlayer = null;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        RaycastHit2D hit = Physics2D.Raycast(globalCamera.UICamera.ScreenToWorldPoint(eventData.position), Vector2.zero);
        if (hit)
        {
            BasePlayerCharacter character = hit.collider.GetComponent<BasePlayerCharacter>();
            ProcessMaskHit(character);
        }
        if (alwaysHit)
        {
            ProcessMaskHit(null);
        }
        transform.localPosition = startPosition;
    }

    void ProcessMaskHit(BasePlayerCharacter character)
    {
        if (scoreTracker.HasEnoughGoldToRemove(maskStats.Price))
        {
            gameEvents.NotifyCoinSpent(maskStats.Price);
            character?.setMask(maskStats);
            ResetCooldown();

            Vector2 pos = GlobalCanvasPos(rectTransform, canvas);
            var textParticle = Instantiate(prefabLocator.TextParticlePrefab).GetComponent<TextParticle>();
            textParticle.transform.SetParent(canvas.transform, worldPositionStays: false);
            textParticle.transform.localPosition = GlobalCanvasPos(rectTransform, canvas);
            textParticle.PlayAnimation($"-{maskStats.Price}", new Color(0.5f, 0.1f, 0.1f, 1));
        }

    }
    void ResetCooldown()
    {
        cooldownTimer.ResetCountdown();
        cooldownImage.gameObject.SetActive(true);
        cooldownImage.fillAmount = 0;
    }
    void UpdateCooldown()
    {
        if(cooldownTimer.HasCounterFinished()) return;
        
        if (cooldownTimer.Decrement(Time.deltaTime))
        {
            cooldownImage.gameObject.SetActive(false);
        }
        else
        {
            cooldownImage.fillAmount = 1f - cooldownTimer.NormalizedRemainingTime;
        }
    }
    
    Vector2 GlobalCanvasPos(RectTransform rect, Canvas canvas) {
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, rect.position);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            screenPoint,
            canvas.worldCamera,
            out Vector2 localPoint
        );
        return localPoint;
    }

}

public enum MaskTypes
{
    CombatMask,
    FreezeMask,
    SpeedMask,
    RegularEnemy,
    AreaMask,
    MiniEnemy,
    HeavyEnemy
}
