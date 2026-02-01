using NaughtyAttributes;
using UnityEngine;

public class RingEntranceManager : MonoBehaviour
{
    public static RingEntranceManager Instance;

 [SerializeField]   Transform[] enemyEntraces;
    [SerializeField] Transform[] playerEntraces;
    [SerializeField, MinMaxSlider(0, 10)] Vector2 retreatRanges;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public Transform getEntrancePoint(ACharacter character)
    {
        if(character.GetComponent<BaseEnemy>() != null)
        {
            return enemyEntraces[Random.Range(0, enemyEntraces.Length)];
        }
        return playerEntraces[Random.Range(0,playerEntraces.Length)];
    }

    internal float getRetreatX(ACharacter character)
    {
        return Random.Range(retreatRanges.x,retreatRanges.y) * (character.GetComponent<BaseEnemy>() ? 1:-1);

    }
}