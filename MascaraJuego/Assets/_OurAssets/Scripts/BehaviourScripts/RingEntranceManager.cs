using UnityEngine;

public class RingEntranceManager : MonoBehaviour
{

    public static RingEntranceManager Instance;
   [SerializeField] Transform[] playerEntrancePoint;
    [SerializeField] Transform[] enemyEntrancePoint;
    private void Awake()
    {
        if (!Instance)
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

        if (character.GetComponent<BaseEnemy>())
        {
            return playerEntrancePoint[Random.Range(0,enemyEntrancePoint.Length)];
        }
        return playerEntrancePoint[Random.Range(0, playerEntrancePoint.Length)];

    }
}