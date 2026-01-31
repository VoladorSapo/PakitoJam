public class BaseEnemy : ACharacter
{
    public override void Die()
    {
        Destroy(this.gameObject);
    }
}

