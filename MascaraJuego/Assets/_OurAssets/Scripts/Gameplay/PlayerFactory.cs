using System.Linq;
using NaughtyAttributes;
using Reflex.Attributes;
using UnityEngine;

public class PlayerFactory : MonoBehaviour
{
    [Inject] RoundScoreTracker scoreTracker;
    [Inject] GameEvents gameEvents;
    
    [SerializeField] BasePlayerCharacter playerPrefab;
    
    [HorizontalLine(color: EColor.Red)]
    [SerializeField] int MaxPlayerCount;
    [SerializeField] CountdownTimer countdownTimer;
    [SerializeField] PlayerSlot[] playerSlots;

    private int PlayersReady => playerSlots.Count(slot => slot.HasPlayer);
    void Awake()
    {
        countdownTimer.ResetCountdown();
        gameEvents.OnRoundStarted += SpawnStartingPlayer;
    }

    void OnDestroy()
    {
        gameEvents.OnRoundStarted -= SpawnStartingPlayer;
    }
    void Update()
    {
        if (!scoreTracker.IsGameActive) return;

        float incrementFactor = scoreTracker.EnemiesInRing >= 4 ? 2 : 1;
        if (countdownTimer.Decrement(Time.deltaTime * incrementFactor)
            && PlayersReady < playerSlots.Length
            && (scoreTracker.PlayersInRing + PlayersReady) < MaxPlayerCount)
        {
            SpawnPlayer();
        }
    }

    void SpawnStartingPlayer()
    {
        float randomTime = Random.Range(1, 2);
        this.InvokeDelayed(randomTime, () => SpawnPlayer());
    }
    
    BasePlayerCharacter SpawnPlayer()
    {
        var slot = GetEmptySlot();
        
        BasePlayerCharacter player = Instantiate(playerPrefab, slot.SpawnSpot.position, Quaternion.identity);
        player.Initialize(slot);
        
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