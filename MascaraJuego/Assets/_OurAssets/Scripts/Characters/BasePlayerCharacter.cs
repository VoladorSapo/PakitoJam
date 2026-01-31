using System;

public class BasePlayerCharacter : ACharacter
{
    public override void Die()
    {
        Destroy(this.gameObject);
    }

}

