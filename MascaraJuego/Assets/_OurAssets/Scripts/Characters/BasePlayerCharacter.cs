using System;

public class BasePlayerCharacter : ACharacter
{
    public override void Die()
    {
        base.Die();
        Destroy(this.gameObject);
    }

}

