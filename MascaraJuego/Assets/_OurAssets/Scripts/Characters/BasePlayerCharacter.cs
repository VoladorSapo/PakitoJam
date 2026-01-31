using System;
using System.Collections;
using UnityEngine;

public class BasePlayerCharacter : ACharacter
{
    private PlayerSlot spawnSlot;
    [SerializeField] SpriteRenderer spriteRenderer;

    public void Initialize(PlayerSlot spawnSlot)
    {
        spriteRenderer.material = new Material(spriteRenderer.material);
        this.spawnSlot = spawnSlot;
        this.spawnSlot.HasPlayer = true;
        StartCoroutine(Slide(transform, transform.position, spawnSlot.transform.position, 0.5f));
    }
    public void EnterRing()
    {
        spawnSlot.HasPlayer = false;
        spawnSlot = null;
    }
    public override void Die()
    {
        base.Die();
        Destroy(this.gameObject);
    }
    
    IEnumerator Slide(Transform obj, Vector3 a, Vector3 b, float duration) {
        float t = 0f;
        while (t < duration) {
            t += Time.deltaTime;
            obj.position = Vector3.Lerp(a, b, t / duration);
            yield return null;
        }
        obj.position = b;
    }

    public override void startHover()
    {
        Debug.Log("StartHover");
        spriteRenderer.material.SetInt("_UseOutline", 1);
    }

    public override void stopHover()
    {
        Debug.Log("StopHover");
        spriteRenderer.material.SetInt("_UseOutline", 0);
    }

    public override void setMask(PowerMaskStats Mask)
    {
        base.setMask(Mask);
        spriteRenderer.material.SetInt("_UseOutline", 0);
    }

}

