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
    
    bool gameStarted = false;
    void Awake()
    {
        gameEvents.OnDifficultyIncreased += UpdateSpawnData;
        gameEvents.OnRoundStarted += ProcessStart;
        gameEvents.OnRoundEnded += ProcessEnd;

        UpdateSpawnData(0);
        countdownTimer.ResetCountdown();
    }

    void ProcessStart()
    {
        gameStarted = true;
    }
    void ProcessEnd(bool _)
    {
        gameStarted = false;
    }
    
    void UpdateSpawnData(int difficulty)
    {
        float spawnInterval = spawnIntervalCurve.Evaluate(difficulty);
        countdownTimer.SetCountdownTime(spawnInterval);
        
        maxEnemyQuality = Mathf.FloorToInt(maxEnemyQualityCurve.Evaluate(difficulty));
    }

    void Update()
    {
        if (!gameStarted) return;

        if (countdownTimer.Decrement(Time.deltaTime))
        {
            SpawnEnemy();
        }
    }
    
    BaseEnemy SpawnEnemy() {
        BaseEnemy prefab = GetWeightedRandomEnemy(maxEnemyQuality);
        return Instantiate(prefab, spawnPoint.position, Quaternion.identity);
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

