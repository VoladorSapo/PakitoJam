using NaughtyAttributes;
using System;
using System.Collections;
using UnityEngine;

public class BasePlayerCharacter : ACharacter
{
    private PlayerSlot spawnSlot;
    
    [MinMaxSlider(-5, 10)] public Vector2Int HealthRanges;
    [MinMaxSlider(-5, 10)] public Vector2Int SpeedRanges;
    [MinMaxSlider(-5, 10)] public Vector2Int AttackRanges;
    RingScript ring;


    public void Initialize(PlayerSlot spawnSlot)
    {
        _baseDamage = UnityEngine.Random.Range(AttackRanges.x, AttackRanges.y);
        _baseLife  = UnityEngine.Random.Range(HealthRanges.x, HealthRanges.y);
        _baseSpeed = UnityEngine.Random.Range(SpeedRanges.x, SpeedRanges.y);
        
        spriteRenderer.material = new Material(spriteRenderer.material);
        this.spawnSlot = spawnSlot;
        this.spawnSlot.HasPlayer = true;
        StartCoroutine(Slide(transform, transform.position, spawnSlot.transform.position, 0.5f));
        ring = FindAnyObjectByType<RingScript>();

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

    public override bool anyOpponent()
    {
        print(ring.getEnemyCount());
        return ring.getEnemyCount() > 0;
    }
}

