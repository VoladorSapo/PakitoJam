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
                character._currentMask.Idle();

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

        if (character is BasePlayerCharacter characterBase)
        {
            characterBase.EnterRing();
        }
        character._currentMask.Walk(objectiveTransform.position);

    }
}
public class WaitCoolDownAction : UnityAction
{
    float time;
    CharacterAssetBehaviourRunner characterBehaviour;
    ACharacter character;
    public override Status Update()
    {
        time += Time.deltaTime;
        if (character is BasePlayerCharacter characterBase)
        {
            Debug.Log($"Time:{time} {character._currentMask.getAttackCooldown()}");
        }
        if(time>= character._currentMask.getAttackCooldown())
        {
            return Status.Success;
        }
        return Status.Running;
    }
    public override void Start()
    {
        base.Start();
        time = 0;
        characterBehaviour = context.GameObject.GetComponent<CharacterAssetBehaviourRunner>();
        character = characterBehaviour.character;
    }
    }