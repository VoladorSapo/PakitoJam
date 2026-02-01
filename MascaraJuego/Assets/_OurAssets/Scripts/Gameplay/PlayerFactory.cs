using System.Linq;
using NaughtyAttributes;
using Reflex.Attributes;
using UnityEngine;

public class PlayerFactory : MonoBehaviour
{
    [Inject] RoundScoreTracker scoreTracker;
    [Inject] GameEvents gameEvents;
    [Inject] AudioManager audioManager;
    
    [SerializeField] BasePlayerCharacter playerPrefab;
    
    [HorizontalLine(color: EColor.Red)]
    [SerializeField] private AnimationCurve maxPlayerCountCurve;
    [SerializeField] CountdownTimer countdownTimer;
    [SerializeField] PlayerSlot[] playerSlots;

    int maxPlayerCount;
    private int PlayersReady => playerSlots.Count(slot => slot.HasPlayer);
    void Awake()
    {
        countdownTimer.ResetCountdown();
        UpdateSpawnData(0);
        gameEvents.OnRoundStarted += SpawnStartingPlayer;
        gameEvents.OnDifficultyIncreased += UpdateSpawnData;
    }

    void UpdateSpawnData(int difficulty)
    {
        maxPlayerCount = (int)maxPlayerCountCurve.Evaluate(difficulty);
    }
    void OnDestroy()
    {
        gameEvents.OnRoundStarted -= SpawnStartingPlayer;
        gameEvents.OnDifficultyIncreased -= UpdateSpawnData;
    }
    void Update()
    {
        if (!scoreTracker.IsGameActive) return;

        float incrementFactor = scoreTracker.EnemiesInRing >= 4 ? 2 : 1;
        if (countdownTimer.Decrement(Time.deltaTime * incrementFactor)
            && PlayersReady < playerSlots.Length
            && (scoreTracker.PlayersInRing + PlayersReady) < maxPlayerCount)
        {
            SpawnPlayer();
        }
    }

    void SpawnStartingPlayer()
    {
        float randomTime = Random.Range(1, 2);
        this.InvokeDelayed(randomTime, () => SpawnFirstPlayer());
    }
    
    BasePlayerCharacter SpawnPlayer()
    {
        var slot = GetEmptySlot();
        
        BasePlayerCharacter player = Instantiate(playerPrefab, slot.SpawnSpot.position, Quaternion.identity);
        player.Initialize(slot, audioManager);
        
        return player;
    }
    BasePlayerCharacter SpawnFirstPlayer()
    {
        var slot = playerSlots[1];

        BasePlayerCharacter player = Instantiate(playerPrefab, slot.SpawnSpot.position, Quaternion.identity);
        player.Initialize(slot, audioManager);

        return player;
    }

    PlayerSlot GetEmptySlot()
    {
        var emptySlots = playerSlots.Where(slot => !slot.HasPlayer);
        var enumerable = emptySlots.ToArray();
        int randomIndex = Random.Range(0, enumerable.Count());
        return enumerable.ElementAt(randomIndex);
    }
}