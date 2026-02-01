using UnityEngine;

public class BaseEnemy : ACharacter
{
    GameEvents gameEvents;
    PrefabLocator prefabLocator;

    public int SpawnCount = 1;
    public float SpawnWeight;

    RingScript ring;

    public void Initialize(GameEvents gameEvents, PrefabLocator prefabLocator)
    {
        this.gameEvents = gameEvents;
        this.prefabLocator = prefabLocator;
        startGame();
        ring = FindAnyObjectByType<RingScript>();
    }
    public override void Die()
    {
        gameEvents.NotifyEnemyDeath();
        Destroy(this.gameObject);
        gameEvents.NotifyCoinCollection(_currentMask.powerMaskStat.Price);
        var textParticle = Instantiate(prefabLocator.OnomatopoeiaParticlePrefab).GetComponent<TextParticle>();
        textParticle.transform.position = this.transform.position;

        Color color1 = new Color(0.9f, 0.9f, 0.9f);
        Color color2 = new Color(0.7f, 0.7f, 0.7f);
        Color finalColor = AuxiliaryMethods.GetRandomColor(color1, color2);
        textParticle.PlayAnimation(AuxiliaryMethods.GetRandomLetter().ToString(), finalColor);
        
        this.InvokeDelayed(0.5f, base.Die);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            Die();
        }
    }

    public override bool anyOpponent()
    {
        print(ring.getPlayerCount());
       return ring.getPlayerCount() > 0;
    }
}

