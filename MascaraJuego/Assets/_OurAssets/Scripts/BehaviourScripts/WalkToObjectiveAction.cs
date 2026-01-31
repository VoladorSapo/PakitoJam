using UnityEngine;
using BehaviourAPI.UnityToolkit;
using BehaviourAPI.Core;

public class WalkToObjectiveAction : UnityAction
{
    CharacterAssetBehaviourRunner characterBehaviour;
    ACharacter character;
    float speed;
    public override Status Update()
    {
        if(characterBehaviour.objective == null)
        {
            return Status.Failure;
        }
       // Debug.Log(speed);
        context.Transform.position = Vector3.MoveTowards(context.Transform.position, characterBehaviour.objective.transform.position, character._currentMask.getSpeed() * Time.deltaTime);
        if (characterBehaviour.objectiveOnAttackDistance())
        {
            return Status.Success;
        }
        return Status.Running;


    }
    public override void Start()
    {
        base.Start();
        characterBehaviour = context.GameObject.GetComponent<CharacterAssetBehaviourRunner>();
        character = characterBehaviour.character;
    }
}