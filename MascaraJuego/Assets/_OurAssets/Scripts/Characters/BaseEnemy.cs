public class BaseEnemy : ACharacter
{
    GameEvents gameEvents;
    public float SpawnWeight;

    public void Initialize(GameEvents gameEvents)
    {
        this.gameEvents = gameEvents;
    }
    public override void Die()
    {
        base.Die();
        gameEvents.NotifyEnemyDeath();
        Destroy(this.gameObject);
    }
}

