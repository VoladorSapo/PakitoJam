using UnityEngine;

public class DamageEffect : ABaseEffect
{
   [field:SerializeField] int damage;

 
    public DamageEffect(int damage){ this.damage = damage;}

    public override void Activate(ACharacter objective)
    {
        Debug.Log("DamageEffect");
        objective.getDamaged(damage+owner._baseDamage);
    }
}

