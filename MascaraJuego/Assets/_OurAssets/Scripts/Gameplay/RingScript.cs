using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class RingScript : MonoBehaviour
{

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
    }

    void Update()
    {
        if (enemyList.Count > 0 && playerList.Count == 0)
        {
            Debug.Log("AQUI PIERDES EN 3 SEGUNDOS");
        }
    }
}
