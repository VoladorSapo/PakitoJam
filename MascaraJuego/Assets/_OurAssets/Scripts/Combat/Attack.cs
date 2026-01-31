using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Attack : MonoBehaviour
{
    [SerializeField] private ACharacter _owner;

    [SerializeField] private HittableCheckTypes _type;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        print("TriggerEnter");
        ACharacter character = collision.GetComponent<ACharacter>();
        if(character!= null)
        {
            if (character.checkHittable(_type,_owner))
            {
                print("Hittable");

                foreach (ABaseEffect effect in _owner._currentMask.powerMaskStat.effects)
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
      
    }

    

 
}

