using UnityEngine;
using NaughtyAttributes;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine.EventSystems;
using System;

public abstract class ACharacter : MonoBehaviour
{
    [field:SerializeField, ReadOnly] public APowerMask _currentMask {  get; private set; }
    [SerializeField] private int _baseLife;
    [SerializeField,ReadOnly] private int _currentMaxLife;
    [SerializeField, ReadOnly] private int _currentLife;
    [field:SerializeField] public int _baseDamage { get; private set; }
 [field:SerializeField]   public bool Dead { get; private set; }


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

    public abstract void Die();
    private void Awake()
    {
     startGame();
    }

    private void startGame()
    {
        _currentMaxLife = _baseLife;
        setLifeToMax();
        Dead = false;
    }
    //Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void setMask(PowerMaskStats Mask)
    {
        if(Mask == null)
        {
            GetComponent<CharacterAssetBehaviourRunner>().enabled = false;
            return;
        }
        if (_currentMask != null &&  _currentMask.type() == Mask.type)
        {
            return;
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
        setMaxLifeModifier(Mask.lifeModifier);
        GetComponent<CharacterAssetBehaviourRunner>().enabled = true;

        print("NEW MASK");
    }

    public void startHover()
    {
        //Efecto 
    }

    public void stopHover()
    {
    }
    public bool checkHittable(HittableCheckTypes checkType, ACharacter attacker)
    {
        switch (checkType)
        {
            case HittableCheckTypes.onlyOtherTeam:
                if(GetComponent<BaseEnemy>() != null)
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
                default:
                return false;
        }
        
    }
}


public enum HittableCheckTypes
{
    onlyOtherTeam,
    onlyMyTeam,
    allCharactersNoMe,
    allCharacters,
    
    
}

public abstract class APowerMask
{
    Animator anim;
   protected ACharacter _character;
    PowerMaskStats powerMaskStat;
    public abstract void Die();

    public abstract void receiveDamage(int damage);

    public virtual void Attack()
    {

    }

    public virtual void setChar(ACharacter character,PowerMaskStats stats)
    {
        _character = character;
        powerMaskStat = stats;
    }
    
    public abstract MaskTypes type();

    internal float getSpeed() => powerMaskStat.speed;
    internal float getDetectRadius() => powerMaskStat.detectRadius;

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
