public class BaseEnemy : ACharacter
{
    public override void Die()
    {
        base.Die();
        Destroy(this.gameObject);
    }
}

