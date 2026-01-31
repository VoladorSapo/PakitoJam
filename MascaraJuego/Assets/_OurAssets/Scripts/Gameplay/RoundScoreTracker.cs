using NaughtyAttributes;
using Reflex.Attributes;
using UnityEngine;

public class RoundScoreTracker : MonoBehaviour
{
    [Inject] GameEvents gameEvents;
    
    [HorizontalLine(color: EColor.Blue)]
    [SerializeField] int maxCoins;
    [SerializeField] int totalCoins;
    
    [HorizontalLine(color: EColor.Blue)]
    [ReadOnly, SerializeField] int enemiesKilled;
    [SerializeField] int enemiesToIncreaseDifficulty;
    [SerializeField] int currentDifficulty;
    
    [HorizontalLine(color: EColor.Blue)]
    [ReadOnly] public bool IsLosing = false;
    [ReadOnly] public int PlayersInRing = 0;
    [ReadOnly] public bool EnteredOnceInRing = false;
    public bool IsGameActive => hasStarted && !isPaused;
    bool isPaused;
    bool hasStarted;
    void Awake()
    {
        gameEvents.OnRoundStarted += ProcessStart;
        gameEvents.OnRoundEnded += ProcessEnd;
        
        gameEvents.OnCoinsCollected += AddCoins;
        gameEvents.OnCoinsSpent += RemoveCoins;
        gameEvents.OnEnemyKilled += RegisterEnemyDeath;
    }
    void OnDestroy()
    {
        gameEvents.OnRoundStarted -= ProcessStart;
        gameEvents.OnRoundEnded -= ProcessEnd;
        
        gameEvents.OnCoinsCollected -= AddCoins;
        gameEvents.OnCoinsSpent -= RemoveCoins;
        gameEvents.OnEnemyKilled -= RegisterEnemyDeath;
    }
    void Start()
    {
        gameEvents.NotifyTotalCoinsCollected(totalCoins);
    }
    void ProcessStart()
    {
        hasStarted = true;
        gameEvents.InvokeDisplayText("BEGIN!!!");
    }
    void ProcessEnd(bool _)
    {
        hasStarted = false;
    }
    public void Pause()
    {
        isPaused = true;
    }
    public void Resume()
    {
        isPaused = false;
    }
    
    void AddCoins(int coinsAmount)
    {
        totalCoins = Mathf.Min(totalCoins + coinsAmount, maxCoins);
        gameEvents.NotifyTotalCoinsCollected(totalCoins);
    }

    public bool HasEnoughGoldToRemove(int coinsAmount)
    {
        return totalCoins >= coinsAmount;
    }
    void RemoveCoins(int coinsAmount)
    {
        totalCoins = Mathf.Max(totalCoins - coinsAmount, 0);
        gameEvents.NotifyTotalCoinsCollected(totalCoins);
    }

    void RegisterEnemyDeath()
    {
        enemiesKilled++;
        TryIncreasingDifficulty();
    }

    void TryIncreasingDifficulty()
    {
        if (enemiesKilled % enemiesToIncreaseDifficulty == 0)
        {
            currentDifficulty++;
            AddCoins(200);
            gameEvents.NotifyDifficultyIncreased(currentDifficulty);
            gameEvents.InvokeDisplayText("ROUND INCREASED");
        }
    }
}
