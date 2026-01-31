using UnityEngine;

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
public class SpeedMas : APowerMask
{
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
    FreezeEffect fr =    new FreezeEffect(1, 2);
        fr.setOwner(_character);
        fr.Activate(_character);
    }

    public override MaskTypes type()
    {
        return MaskTypes.SpeedMask;
    }
}
