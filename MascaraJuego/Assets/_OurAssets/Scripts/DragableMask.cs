using UnityEngine;
using UnityEngine.EventSystems;
using BehaviourAPI;
using BehaviourAPI.UnityToolkit.GUIDesigner.Framework;
using BehaviourAPI.UnityToolkit.GUIDesigner.Runtime;
using NaughtyAttributes;
using BehaviourAPI.UnityToolkit;
using BehaviourAPI.Core;
using Unity.VisualScripting;
using static UnityEngine.GraphicsBuffer;
public class DragableMask :MonoBehaviour, IDragHandler,IDropHandler
{
    BasePlayerCharacter currentHoverPlayer;
    Vector3 startPosition;
 [SerializeField] PowerMaskStats maskStats;
  
    public void OnDrag(PointerEventData eventData)
    {
       this.transform.position = eventData.position;
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(eventData.position), Vector2.zero);
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
        if (currentHoverPlayer != null)
        {
            currentHoverPlayer.stopHover();
            currentHoverPlayer = null;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(eventData.position), Vector2.zero);
        if (hit)
        {
            BasePlayerCharacter character = hit.collider.GetComponent<BasePlayerCharacter>();
            if (character != null)
            {
                character.setMask(maskStats);
            }
        }
        transform.position = startPosition;
    }
    private void Start()
    {
        startPosition = transform.position;
    }
}

public enum MaskTypes
{
    CombatMask,
    FreezeMask,
    SpeedMask
}
