using UnityEngine;

public class RingEntranceManager : MonoBehaviour
{
    public static RingEntranceManager Instance;

 [SerializeField]   Transform[] enemyEntraces;
    [SerializeField] Transform[] playerEntraces;

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
}