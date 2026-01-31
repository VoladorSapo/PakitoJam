using UnityEngine;

public class BaseEnemy : ACharacter
{
    GameEvents gameEvents;
    PrefabLocator prefabLocator;
    
    public float SpawnWeight;

    public void Initialize(GameEvents gameEvents, PrefabLocator prefabLocator)
    {
        this.gameEvents = gameEvents;
        this.prefabLocator = prefabLocator;
        startGame();
    }
    public override void Die()
    {
        base.Die();
        gameEvents.NotifyEnemyDeath();
        Destroy(this.gameObject);
        
        var textParticle = Instantiate(prefabLocator.OnomatopoeiaParticlePrefab).GetComponent<TextParticle>();
        textParticle.transform.position = this.transform.position;

        Color color1 = new Color(0.9f, 0.9f, 0.9f);
        Color color2 = new Color(0.7f, 0.7f, 0.7f);
        Color finalColor = AuxiliaryMethods.GetRandomColor(color1, color2);
        textParticle.PlayAnimation(AuxiliaryMethods.GetRandomLetter().ToString(), finalColor);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            Die();
        }
    }
}

