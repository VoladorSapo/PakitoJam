using BehaviourAPI.UnityToolkit;
using BehaviourAPI.Core;
using System.Collections.Generic;
using UnityEngine;

public class AttackAction : UnityAction
{
    CharacterAssetBehaviourRunner characterBehaviour;
    public override Status Update()
    {
        if (characterBehaviour.actionFinished)
        {
            return Status.Success;
        }
        return Status.Running;
    }
    public override void Start()
    {
        base.Start();
        characterBehaviour = context.GameObject.GetComponent<CharacterAssetBehaviourRunner>();
        characterBehaviour.startAction();
        characterBehaviour.character._currentMask.Attack();
    }
}
