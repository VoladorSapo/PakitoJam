using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.UI;
using UnityEngine.TextCore.Text;
[System.Serializable]
public abstract class ABaseEffect
{
   [HideInInspector] public ACharacter owner;
    [HideInInspector] public ACharacter objective;


    public abstract void Activate(ACharacter objective);
    public void setOwner(ACharacter owner)
    {
        this.owner = owner;
    }

    public virtual bool Instant()
    {
        return true;
    }

}

public abstract class ATimedEffect : ABaseEffect
{
    public abstract float getDuration();
    protected float _currentDuration;
 protected   int _identifier;
    
    public ATimedEffect(float duration) : base()
    {

        _currentDuration = duration;
    }
    public abstract void End();
    public ATimedEffect()
    {
        _currentDuration = getDuration();
    }
    public virtual bool Update()
    {
        _currentDuration -= Time.deltaTime;
        Debug.Log($"{_identifier} : {_currentDuration}");
        if (_currentDuration <= 0)
        {
            return true;
        }
        return false;
    }
    
    public override void Activate(ACharacter character)
    {
        objective = character;
        _currentDuration = getDuration();
        _identifier = Random.Range(0, 100);
        Debug.Log($"{_identifier} Activate:  {_currentDuration}");

    }
    public override bool Instant()
    {
        return false;
    }
    public virtual bool Refreshable()
    {
        return true;
    }

    public void Refresh(ACharacter aCharacter)
    {
        Debug.Log($"{_identifier} Refresh:  {_currentDuration}");
        _currentDuration = getDuration();
    }
}

public class FreezeEffect : ATimedEffect
{
    public float freezeDuration;
    public float slowdownMult;
   public FreezeEffect(float freezeDuration, float slowdownMultiplier)
    {
        this.freezeDuration = freezeDuration;
        this.slowdownMult = slowdownMultiplier;

    }
    public override float getDuration()
    {
        return freezeDuration;
    }
    public override void Activate(ACharacter character)
    {
        
        base.Activate(character);
        Debug.Log($"{_identifier}: Freeze");
        character._currentMask.addMultiplier(MultiplierType.Speed, slowdownMult);
        character._currentMask.addAnimMult(slowdownMult);
        character.addVisualFilter(new Color(0.23f, 0.37f, 1, 1));

    }

    public override void End()
    {
        Debug.Log($"{_identifier}: EndFreeze");
        objective._currentMask.removeAnimMult(slowdownMult);

        objective._currentMask.removeMultiplier(MultiplierType.Speed, slowdownMult);
        objective.removeVisualFilter();

    }
}

public class aditiveSpeedEffect : ATimedEffect
{
    public float freezeDuration;
    public float slowdownMult;
    
    public aditiveSpeedEffect(float freezeDuration, float slowdownMultiplier)
    {
        this.freezeDuration = freezeDuration;
        this.slowdownMult = slowdownMultiplier;

    }
    public override float getDuration()
    {
        return freezeDuration;
    }
    public override void Activate(ACharacter character)
    {

        base.Activate(character);
        Debug.Log($"{_identifier}: Freeze");
        character._currentMask.addMultiplier(MultiplierType.Speed, slowdownMult);
        character._currentMask.addAnimMult(slowdownMult);
        //    character.addVisualFilter(new Color(0, 0, 0.6f, 1));

    }

    public override void End()
    {
        Debug.Log($"{_identifier}: EndFreeze");
        objective._currentMask.removeAnimMult(slowdownMult);

        objective._currentMask.removeMultiplier(MultiplierType.Speed, slowdownMult);
    }
}