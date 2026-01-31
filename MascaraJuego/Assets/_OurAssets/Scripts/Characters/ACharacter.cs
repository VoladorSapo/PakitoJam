using UnityEngine;
using NaughtyAttributes;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine.EventSystems;
using System;
using UnityEngine.Events;
using System.Collections.Generic;
using Reflex.Attributes;
using System.Collections;

public abstract class ACharacter : MonoBehaviour
{
    CharacterAssetBehaviourRunner characterBehaviour;

    [SerializeField] PowerMaskStats startingMask;
    [field:SerializeField, ReadOnly] public APowerMask _currentMask {  get; private set; }
    [SerializeField] private int _baseLife;
    [SerializeField,ReadOnly] private int _currentMaxLife;
    [SerializeField, ReadOnly] private int _currentLife;
    [field:SerializeField] public int _baseDamage { get; private set; }
 [field:SerializeField]   public bool Dead { get; private set; }
    [field: SerializeField] public bool onRing { get; private set; }

    UnityEvent<ACharacter> dieEvent;

    List<ATimedEffect> activeEffects;
    public void getDamaged(int damage)
    {
        _currentLife -= damage;
        if (_currentLife < 0)
        {
            Die();
        }
    }
    public void setLifeToMax()
    {
        _currentLife = _currentMaxLife;
    }
    public void healLife(int heal)
    {
        _currentLife = Mathf.Min(_currentMaxLife, _currentLife + heal);
    }
    public void setMaxLifeModifier(int extraLife)
    {
        int prevLife = _currentMaxLife;
        _currentMaxLife = _baseLife + extraLife;
        if(prevLife < _currentMaxLife)
        {
            healLife(_currentMaxLife-prevLife);
        }
        if (_currentLife > _currentMaxLife)
        {
            setLifeToMax();
        }
    }
    public void setLife(int life)
    {
        _currentLife = life;
    }

    public virtual void Die()
    {
        dieEvent.Invoke(this);
    }
    private void Awake()
    {
        characterBehaviour = GetComponent<CharacterAssetBehaviourRunner>();
            activeEffects = new List<ATimedEffect>();

    }

    protected void startGame()
    {
        setOnRing(false);
        _currentMaxLife = _baseLife;
        setLifeToMax();
        Dead = false;
        if (startingMask)
        {
            setMask(startingMask);
        }
    }
    //Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        print("start"+gameObject.name);
      //  gameEvents.OnRoundStarted += startGame;

        dieEvent = new UnityEvent<ACharacter>();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = activeEffects.Count - 1; i >= 0; i--)
        {
            print($"Check{i}");
            var effect = activeEffects[i];
            if (effect.Update())
            {
                effect.End();
                activeEffects.RemoveAt(i);
            }
        }
    }

    public void addEffect(ABaseEffect effect)
    {

        if (!effect.Instant())
        {
            print("Timed");
            ATimedEffect timed = (ATimedEffect)effect;
            if (activeEffects.Contains(timed)){
                print("contains");
                if (timed.Refreshable())
                {
                    timed.Refresh(this);
                }
                return;
            }
            print("no contains");
            effect.Activate(this);
            activeEffects.Add((ATimedEffect)effect);


        }
        else
        {

            effect.Activate(this);

        }
    }
    public virtual void setMask(PowerMaskStats Mask)
    {
        print("setMask");
        if(Mask == null)
        {
            GetComponent<CharacterAssetBehaviourRunner>().enabled = false;
            return;
        }
        if (_currentMask != null &&  _currentMask.type() == Mask.type)
        {
            return;
        }
        bool firstMask = false;
        if (_currentMask == null)
        {
            firstMask = true;
        }
        switch (Mask.type)
        {
            case MaskTypes.CombatMask:
              _currentMask = new CombatMask();
                
                break;
            case MaskTypes.FreezeMask:
                _currentMask = new FreezeMask();

                break;
            case MaskTypes.SpeedMask:
                break;
        }
        _currentMask.setChar(this,Mask);
        if (firstMask)
        {
            _currentMask.setAnim();
        }
        else
        {
            StartCoroutine(_currentMask.changeAnim());
        }
            setMaxLifeModifier(Mask.lifeModifier);
        GetComponent<CharacterAssetBehaviourRunner>().enabled = true;

        print("NEW MASK");
    }

    public virtual void startHover()
    {
        print("HOVER");
        //Efecto 
    }

    public virtual void stopHover()
    {
        //Efecto 
    }
    public bool checkHittable(HittableCheckTypes checkType, ACharacter attacker)
    {
        switch (checkType)
        {
            case HittableCheckTypes.onlyOtherTeam:
                if (GetComponent<BaseEnemy>() != null)
                {
                    return attacker.GetComponent<BaseEnemy>() == null;
                }
                return attacker.GetComponent<BasePlayerCharacter>() == null;


            case HittableCheckTypes.onlyMyTeam:
                if (GetComponent<BaseEnemy>() != null)
                {
                    return attacker.GetComponent<BaseEnemy>() != null;
                }
                return attacker.GetComponent<BasePlayerCharacter>() != null;
            case HittableCheckTypes.allCharactersNoMe:
                return !gameObject.Equals(attacker.gameObject);
            case HittableCheckTypes.allCharacters:
                return true;
            case HittableCheckTypes.onlyObjective:
                ACharacter ojective = attacker.characterBehaviour.objective;
                print(ojective);
                print(this);
                if (ojective == null)
                    return false;
                return ojective.Equals(this);
            default:
                return false;
        }

    }

    public void subscribeToDieEvent(UnityAction<ACharacter> action)
    {
        dieEvent.AddListener(action);

    }
    public void unSubscribeToDieEvent(UnityAction<ACharacter> action)
    {
        dieEvent.RemoveListener(action);

    }

    internal void setOnRing(bool onRing)
    {
        this.onRing = onRing;
    }
}


public enum HittableCheckTypes
{
    onlyObjective,
    onlyOtherTeam,
    onlyMyTeam,
    allCharactersNoMe,
    allCharacters,
    
    
    
}

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

    internal float getSpeed() => powerMaskStat.speed * getMultiplier(MultiplierType.Speed);
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
        anim.Play("walk", 0, 0);
     spriteRenderer.flipX =   position.x < _character.transform.position.x;
       
    }
    internal void Idle()
    {
        anim.Play("idle");
    }

    internal float getAttackCooldown() => powerMaskStat.cooldown / getMultiplier(MultiplierType.Speed);
}
public class CombatMask : APowerMask
{
   

    public override void Die()
    {
        _character.Die();
    }

    public override void receiveDamage(int damage)
    {
        _character.getDamaged(damage);
    }

    public override MaskTypes type()
    {
        return MaskTypes.CombatMask;
    }
 
}

public class FreezeMask : APowerMask
{
 
    public override void Die()
    {
        _character.Die();
    }

    public override void receiveDamage(int damage)
    {
        _character.getDamaged(damage);
    }

    public override MaskTypes type()
    {
        return MaskTypes.FreezeMask;
    }
   
}
public enum MultiplierType
{
    Speed
}