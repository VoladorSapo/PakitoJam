using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

public abstract class APowerMask
{
    Animator anim;
    SpriteRenderer spriteRenderer;
   protected ACharacter _character;
  public  PowerMaskStats powerMaskStat {  get; private set; }
    private float speedMultiplier;

    Dictionary<MultiplierType, float> multipliersDict;

    public abstract void Die();

    public abstract void receiveDamage(int damage);

    public virtual void Attack()
    {
        Debug.Log($"{_character.name}: atacar");
        anim.Play("attack", 0,0);
    }

    public virtual void setChar(ACharacter character,PowerMaskStats stats)
    {
        multipliersDict = new Dictionary<MultiplierType, float>();
        _character = character;
        powerMaskStat = stats;
        anim = _character.GetComponentInChildren<Animator>();
       spriteRenderer = _character.GetComponentInChildren<SpriteRenderer>();
        Debug.Log("animatorcontroller");

       
        foreach (var effect in stats.effects)
        {
            effect.setOwner(character);
        }
    }
    public virtual void setAnim()
    {
        anim.runtimeAnimatorController = powerMaskStat.controller;
    }
   public IEnumerator changeAnim()
    {
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        int stateHash = stateInfo.fullPathHash;
        float normalizedTime = stateInfo.normalizedTime % 1f;

        // Swap controller
        anim.runtimeAnimatorController = powerMaskStat.controller;

        // Wait for Animator to initialize
        yield return null;

        // Force animator update (important)
        anim.Update(0f);

        // Restore state
        anim.Play(stateHash, 0, normalizedTime);
    }
    public abstract MaskTypes type();

    internal virtual float getSpeed() =>(_character._baseSpeed+ powerMaskStat.speedModifier) * getMultiplier(MultiplierType.Speed);
    internal float getAttackDistance() => powerMaskStat.attackDistance;

    internal float getDetectRadius() => powerMaskStat.detectRadius;

    public void addMultiplier(MultiplierType multName,float mult )
    {
        float val;
        if (multipliersDict.TryGetValue(multName, out val))
        {
            multipliersDict[multName] = val * mult;
        }
        else
        {
            val = 1;
            multipliersDict.Add(multName, val * mult);
        }
    }
    public void sumToMultiplier(MultiplierType multName, float add)
    {
        float val;
        if (multipliersDict.TryGetValue(multName, out val))
        {
            multipliersDict[multName] = val + add;
        }
        else
        {
            val = 1;
            multipliersDict.Add(multName, val + add);
        }
    }
    public void removeMultiplier(MultiplierType multName,float mult)
    {
        float val;
        if (multipliersDict.TryGetValue(multName, out val))
        {
            multipliersDict[multName] = val / mult;
        }
    }
    public float getMultiplier(MultiplierType multName)
    {
        if (multipliersDict.TryGetValue(multName, out float val)) 
        {
            return val;
        }
        return 1;
    }

    public void resetAnimSpeed()
    {
        anim.speed = 1;
    }
    public void addAnimMult(float mult)
    {
        anim.speed *= mult;
    }
    public void removeAnimMult(float mult)
    {
        anim.speed /= mult;
    }

    internal void Walk(Vector3 position)
    {
        anim.transform.localScale = new Vector3(position.x < _character.transform.position.x ? -1:1, 1, 1);
        anim.Play("walk", 0, 0);
    }
    internal void Idle()
    {
        anim.Play("idle");
    }

    internal virtual float getAttackCooldown() => powerMaskStat.cooldown / getMultiplier(MultiplierType.Speed);

    internal virtual float getInitCooldown() => powerMaskStat.initCooldown / getMultiplier(MultiplierType.Speed);

    internal virtual void NewObjective()
    {

    }
}
