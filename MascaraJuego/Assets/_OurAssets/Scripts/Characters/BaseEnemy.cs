public class BaseEnemy : ACharacter
{
    public float SpawnWeight;
    public override void Die()
    {
        base.Die();
        Destroy(this.gameObject);
    }
}

