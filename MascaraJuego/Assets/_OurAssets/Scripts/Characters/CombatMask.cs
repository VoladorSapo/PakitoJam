using UnityEngine;
using UnityEngine.Rendering;

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
public class SpeedMask : APowerMask
{
    float currentAcceleration;
    public override void Die()
    {
        _character.Die();
    }

    public override void receiveDamage(int damage)
    {
        _character.getDamaged(damage);
    }
    public override void Attack()
    {
        base.Attack();
        accelerate(0.1f);
    }

    public override MaskTypes type()
    {
        return MaskTypes.SpeedMask;
    }
    void accelerate(float amount)
    {
        currentAcceleration += amount;
    }
    internal override void NewObjective()
    {
        base.NewObjective();
        currentAcceleration = 0;
    }
    internal override float getSpeed()
    {
        return base.getSpeed() + currentAcceleration;
    }
    internal override float getAttackCooldown()
    {
        return base.getAttackCooldown()/currentAcceleration;
    }
}
