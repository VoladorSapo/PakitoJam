using System;
using System.Collections.Generic;
using NaughtyAttributes;
using Reflex.Attributes;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class RingScript : MonoBehaviour
{
    [Inject] private RoundScoreTracker roundScoreTracker;
    [Inject] private GameEvents gameEvents;
    
    [ReadOnly, SerializeField] private List<BaseEnemy> enemyList;
    [ReadOnly, SerializeField] private List<BasePlayerCharacter> playerList;
    public void OnTriggerEnter2D(Collider2D other)
    {
        var character = other.GetComponent<ACharacter>();
        if (character == null) return;

        if (character is BaseEnemy enemy && !enemyList.Contains(enemy))
        {
            enemyList.Add(enemy);
        }
        else if (character is BasePlayerCharacter player && !playerList.Contains(player))
        {
            playerList.Add(player);
        }

        UpdatePlayerCount();
        CheckIfLosing();
    }


    public void OnTriggerExit2D(Collider2D other)
    {
        var character = other.GetComponent<ACharacter>();
        if (character == null) return;
        
        if (character is BaseEnemy enemy && enemyList.Contains(enemy))
        {
            enemyList.Remove(enemy);
        }
        else if (character is BasePlayerCharacter player && playerList.Contains(player))
        {
            playerList.Remove(player);
        }

        UpdatePlayerCount();
        CheckIfLosing();
    }

    void CheckIfLosing()
    {
        bool losing = enemyList.Count > 0 && playerList.Count == 0;
        gameEvents.InvokeLosingAlert(losing);
    }

    void UpdatePlayerCount()
    {
        if (!roundScoreTracker.EnteredOnceInRing)
        {
            roundScoreTracker.EnteredOnceInRing = true;
            gameEvents.NotifyFirstPlayerInRing();
            
        }
        
        roundScoreTracker.PlayersInRing = playerList.Count;
        roundScoreTracker.EnemiesInRing = enemyList.Count;
    }
    public int getPlayerCount()
    {
        return playerList.Count;
    }
    public int getEnemyCount()
    {
        return enemyList.Count;
    }

}
