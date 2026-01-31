using BehaviourAPI.UnityToolkit;
using BehaviourAPI.Core;
using UnityEngine;

public class enterRingAction : UnityAction
{
    CharacterAssetBehaviourRunner characterBehaviour;
    ACharacter character;
    Transform objectiveTransform;
    int obj = 0;
    public override Status Update()
    {
        if (objectiveTransform == null)
        {
            return Status.Failure;
        }
        context.Transform.position = Vector3.MoveTowards(context.Transform.position, objectiveTransform.GetChild(obj).position, character._currentMask.getSpeed() * Time.deltaTime);
        if (characterBehaviour.transformOnWalkDistance(objectiveTransform.GetChild(obj)))
        {
            obj++;
            if (obj >= objectiveTransform.childCount)
            {
                character.setOnRing(true);
                return Status.Success;
            }
        }
        return Status.Running;
    }
    public override void Start()
    {
        base.Start();
        characterBehaviour = context.GameObject.GetComponent<CharacterAssetBehaviourRunner>();
        character = characterBehaviour.character;
        objectiveTransform = RingEntranceManager.Instance.getEntrancePoint(character);
    }
}
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