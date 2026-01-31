using NaughtyAttributes;
using Reflex.Attributes;
using UnityEngine;

public class EnemyFactory : MonoBehaviour
{
    [Inject] RoundScoreTracker scoreTracker;
    [Inject] GameEvents gameEvents;

    [HorizontalLine(color: EColor.Red)]
    [SerializeField] BaseEnemy[] enemyPrefabs;
    [SerializeField] Transform spawnPoint;
    
    [HorizontalLine(color: EColor.Red)]
    [SerializeField] CountdownTimer countdownTimer;
    
    [HorizontalLine(color: EColor.Red)]
    [SerializeField] AnimationCurve spawnIntervalCurve;
    [SerializeField] AnimationCurve maxEnemyQualityCurve;
    private int maxEnemyQuality = 3;

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
    }

    void Update()
    {
        if (!scoreTracker.IsGameActive) return;

        if (countdownTimer.Decrement(Time.deltaTime))
        {
            SpawnEnemy();
        }
    }
    
    BaseEnemy SpawnEnemy() {
        BaseEnemy prefab = GetWeightedRandomEnemy(maxEnemyQuality);
        
        BaseEnemy enemy = Instantiate(prefab, spawnPoint.position, Quaternion.identity);
        enemy.Initialize(gameEvents);
        
        return enemy;
    }
    
    BaseEnemy GetWeightedRandomEnemy(int maxIndex) {
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

