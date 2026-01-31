using UnityEngine;
[System.Serializable]
public abstract class ABaseEffect
{
   [HideInInspector] public ACharacter owner;


    public abstract void Activate(ACharacter objective);
    public void setOwner(ACharacter owner)
    {
        this.owner = owner;
    }

}

