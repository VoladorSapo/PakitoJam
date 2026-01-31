using System;
using System.Collections;
using UnityEngine;

public class BasePlayerCharacter : ACharacter
{
    private PlayerSlot spawnSlot;

    public void Initialize(PlayerSlot spawnSlot)
    {
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


}

