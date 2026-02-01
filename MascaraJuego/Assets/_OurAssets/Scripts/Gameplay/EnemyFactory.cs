using NaughtyAttributes;
using Reflex.Attributes;
using UnityEngine;

public class EnemyFactory : MonoBehaviour
{
    [Inject] RoundScoreTracker scoreTracker;
    [Inject] GameEvents gameEvents;
    [Inject] PrefabLocator prefabLocator;
    [Inject] AudioManager audioManager;

    [HorizontalLine(color: EColor.Red)]
    [SerializeField] BaseEnemy[] enemyPrefabs;
    [SerializeField] Transform spawnPoint;
    
    [HorizontalLine(color: EColor.Red)]
    [SerializeField] CountdownTimer countdownTimer;
    
    [HorizontalLine(color: EColor.Red)]
    [SerializeField] AnimationCurve spawnIntervalCurve;
    [SerializeField] AnimationCurve maxEnemyQualityCurve;
    [SerializeField] private AnimationCurve maxEnemyInRingCurve;
    [SerializeField] private AnimationCurve maxMultipleEnemySpawnCurve;
    [SerializeField] private AnimationCurve playersWaitingInRingFactorCurve;
    private int maxEnemyQuality = 3;
    private int maxEnemyQuantity;
    private float maxMultipleSpawnFactor = 2;
    private int playersWaitingInRingFactor = 3;

    void Awake()
    {
        gameEvents.OnDifficultyIncreased += UpdateSpawnData;
        UpdateSpawnData(0);
        countdownTimer.ResetCountdown();
    }
    void OnDestroy()
    {
        gameEvents.OnDifficultyIncreased -= UpdateSpawnData;
    }
    
    
    void UpdateSpawnData(int difficulty)
    {
        float spawnInterval = spawnIntervalCurve.Evaluate(difficulty);
        countdownTimer.SetCountdownTime(spawnInterval);
        
        maxEnemyQuality = Mathf.FloorToInt(maxEnemyQualityCurve.Evaluate(difficulty));
        maxEnemyQuantity = Mathf.FloorToInt(maxEnemyInRingCurve.Evaluate(difficulty));
        maxMultipleSpawnFactor = Mathf.RoundToInt(maxMultipleEnemySpawnCurve.Evaluate(difficulty));
        playersWaitingInRingFactor = (int)playersWaitingInRingFactorCurve.Evaluate(difficulty);
    }

    void Update()
    {
        if (!scoreTracker.IsGameActive || !scoreTracker.EnteredOnceInRing) return;

        int incrementFactor = scoreTracker.PlayersInRing >= playersWaitingInRingFactor ? 2 : 1;
        if (countdownTimer.Decrement(Time.deltaTime * incrementFactor) && scoreTracker.EnemiesInRing - scoreTracker.PlayersInRing < maxEnemyQuantity)
        {
            BaseEnemy prefab = GetWeightedRandomEnemy(maxEnemyQuality);
            
            int enemiesToSpawn = prefab.SpawnCount + Mathf.RoundToInt(UnityEngine.Random.Range(0, maxMultipleSpawnFactor));
            enemiesToSpawn *= incrementFactor;
            
            for (int i = 0; i < enemiesToSpawn; i++)
            {
                if ((scoreTracker.EnemiesInRing + i + 1) >= maxEnemyQuantity) break;
                SpawnEnemy(prefab);
            }
        }
    }
    
    BaseEnemy SpawnEnemy(BaseEnemy prefab) {
        
        Vector2 dir = Random.insideUnitCircle.normalized;
        float dist = Random.Range(1f, 2f);
        Vector3 finalVector = dir * dist;
        
        BaseEnemy enemy = Instantiate(prefab, spawnPoint.position + finalVector, Quaternion.identity);
        enemy.Initialize(gameEvents, prefabLocator, audioManager);
        
        return enemy;
    }
    
    BaseEnemy GetWeightedRandomEnemy(int maxIndex) {
        Debug.LogWarning(maxIndex);
        maxIndex = Mathf.Clamp(maxIndex, 0, enemyPrefabs.Length - 1);
        
        float totalWeight = 0f;
        for (int i = 0; i <= maxIndex; i++) {
            totalWeight += enemyPrefabs[i].SpawnWeight;
        }
        
        float randomValue = Random.value * totalWeight;

        float cumulative = 0f;
        for (int i = 0; i <= maxIndex; i++) {
            cumulative += enemyPrefabs[i].SpawnWeight;
            if (randomValue <= cumulative) {
                return enemyPrefabs[i];
            }
        }
        
        return enemyPrefabs[0];
    }

}

