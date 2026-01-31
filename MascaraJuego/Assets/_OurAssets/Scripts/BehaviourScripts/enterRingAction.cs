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
        character._currentMask.Walk(objectiveTransform.position);

    }
}
