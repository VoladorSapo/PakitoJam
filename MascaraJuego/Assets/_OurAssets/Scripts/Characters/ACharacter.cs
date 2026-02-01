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
using PrimeTween;
using Sequence = PrimeTween.Sequence;

public abstract class ACharacter : MonoBehaviour
{
    CharacterAssetBehaviourRunner characterBehaviour;
    [SerializeField] protected SpriteRenderer spriteRenderer;
    [SerializeField] PowerMaskStats startingMask;
    [field:SerializeField, ReadOnly] public APowerMask _currentMask {  get; private set; }
    [SerializeField] protected int _baseLife;
    [SerializeField, ReadOnly] protected int _currentMaxLife;
    [SerializeField] private int _currentLife;

    Color supposedColor = new Color(1, 1, 1, 1);
    [field:SerializeField] public int _baseDamage { get; protected set; }
    [field: SerializeField] public int _baseSpeed { get; protected set; }

    [field:SerializeField]   public bool Dead { get; private set; }
    [field: SerializeField] public bool onRing { get; private set; }

    UnityEvent<ACharacter> dieEvent;

    List<ATimedEffect> activeEffects;
    Sequence damageTween;

  [SerializeField]  bool damagedCooldown;
    [SerializeField] float damagedCooldownTime =1;

    public void getDamaged(int damage)
    {
        //if (damagedCooldown)
        //    return;

        _currentLife -= damage;
        
        /*Color ogColor = spriteRenderer.color;
        Sequence.Create(cycles: 1)
            .Chain(Tween.ScaleX(transform, 0.9f, 0.15f, Ease.OutBounce))
            .Group(Tween.Custom(startValue: ogColor,
                endValue: new Color(0.6f, 0.1f, 0.1f, 1),
                duration: 0.15f, onValueChange:
                value => spriteRenderer.color = value))
            .Chain(Tween.ScaleX(transform, 1f, 0.15f, Ease.OutBounce))
            .Group(Tween.Custom(startValue: new Color(0.6f, 0.1f, 0.1f, 1),
                endValue: ogColor,
                duration: 0.15f, onValueChange:
                value => spriteRenderer.color = value));*/
        
        Color ogColor = spriteRenderer.color;
        float endAngle = spriteRenderer.flipX ? -15 : 15;
        Vector3 rot = new Vector3(0, 0, endAngle);
        damageTween = Sequence.Create(cycles: 1)
            .Chain(Tween.ScaleX(transform, 0.7f, 0.15f, Ease.OutBounce))
                .Group(Tween.ScaleY(transform, 0.8f, 0.15f, Ease.OutBounce))
                .Group(Tween.LocalRotation(transform, rot, 0.15f, Ease.OutBounce))
                .Group(Tween.Custom(startValue: ogColor,
                    endValue: new Color(0.6f, 0.1f, 0.1f, 1),
                    duration: 0.15f, onValueChange:
                    value => spriteRenderer.color = value))
            .Chain(Tween.ScaleX(transform, 1f, 0.15f, Ease.OutBounce))
                .Group(Tween.ScaleY(transform, 1f, 0.15f, Ease.OutBounce))
                .Group(Tween.LocalRotation(transform, Vector3.zero, 0.15f, Ease.OutBounce))
                .Group(Tween.Custom(startValue: new Color(0.6f, 0.1f, 0.1f, 1),
                    endValue: spriteRenderer.color,
                    duration: 0.15f, onValueChange:
                    value => spriteRenderer.color = value)).OnComplete(()=>spriteRenderer.color = supposedColor);
        
        if (_currentLife < 0)
        {
            Die();
        }
        else
        {
            damagedCooldown = true;
          //  StartCoroutine(damageCooldown());
        }
    }
    //IEnumerator damageCooldown()
    ////{
    ////    for (int i = 0; i < 5; i++)
    ////    {
    ////        yield return new WaitForSecondsRealtime(damagedCooldownTime / 5f);
    ////        spriteRenderer.enabled = !spriteRenderer.enabled;
            

    ////    }
    ////    damagedCooldown = false;
    ////    spriteRenderer.enabled = true;

    //}
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
       
            damageTween.Complete();
        
        dieEvent.Invoke(this);
    }
    private void Awake()
    {
        spriteRenderer=GetComponentInChildren<SpriteRenderer>();
        characterBehaviour = GetComponent<CharacterAssetBehaviourRunner>();
            activeEffects = new List<ATimedEffect>();

    }
    public bool inOnRing() { print(onRing); return onRing; }
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
        bool firstMask = _currentMask == null;
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
            case MaskTypes.RegularEnemy:
                _currentMask = new CombatMask();

                break;
                case MaskTypes.MiniEnemy:
                _currentMask = new CombatMask();
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
    internal void addVisualFilter(Color color)
    {
        spriteRenderer.color= supposedColor = color;
    }

    internal void removeVisualFilter()
    {
        spriteRenderer.color = supposedColor = new Color(1,1,1,1);

    }
    public abstract bool anyOpponent();

}


public enum HittableCheckTypes
{
    onlyObjective,
    onlyOtherTeam,
    onlyMyTeam,
    allCharactersNoMe,
    allCharacters,
    
    
    
}
public enum MultiplierType
{
    Speed
}