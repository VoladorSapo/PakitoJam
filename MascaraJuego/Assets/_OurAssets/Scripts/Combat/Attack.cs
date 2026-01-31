using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Attack : MonoBehaviour
{
    [SerializeField] private ACharacter _owner;

    [SerializeField][SerializeReference] public List<ABaseEffect> effects;
    [SerializeField] private HittableCheckTypes _type;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        print("TriggerEnter");
        ACharacter character = collision.GetComponent<ACharacter>();
        if(character!= null)
        {
            if (character.checkHittable(_type,_owner))
            {
                foreach(ABaseEffect effect in effects)
                {
                    character.addEffect(effect);
                }
            }
            else
            {
                print("No Hittable");
            }
        }
    }

    private void Start()
    {
        _owner = GetComponentInParent<ACharacter>();
        foreach (ABaseEffect effect in effects)
        {
            effect.setOwner(_owner);
        }
    }

    public void addDamageEffect(int damage)
    {
        effects.Add(new  DamageEffect(damage));
    }
    public void addFreezeEffect(float duration, float slowMultiplier)
    {
        effects.Add(new FreezeEffect(duration, slowMultiplier));
    }
}

