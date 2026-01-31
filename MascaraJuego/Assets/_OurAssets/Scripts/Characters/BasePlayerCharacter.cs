using System;

public class BasePlayerCharacter : ACharacter
{
    private PlayerSlot spawnSlot;

    public void Initialize(PlayerSlot spawnSlot)
    {
        this.spawnSlot = spawnSlot;
    }
    public void EnterRing()
    {
        spawnSlot.HasPlayer = false;
        spawnSlot = null;
    }
    public override void Die()
    {
        base.Die();
        Destroy(this.gameObject);
    }

}

