using UnityEngine;
using BehaviourAPI.UnityToolkit.GUIDesigner.Runtime;
using NaughtyAttributes;
using BehaviourAPI.Core;
using System;

public class CharacterAssetBehaviourRunner : AssetBehaviourRunner
{
    public ACharacter character {  get; private set; }
    [field: SerializeField] public ACharacter objective;
    public bool actionFinished {  get; private set; }
  [field:SerializeField]  public LayerMask EnemyLayerMask { get; private set; }

    [SerializeField] float walkStopDistance;

    public void setObjective(ACharacter character)
    {
        if (objective != null && !objective.Equals(character))
        {
            objective.unSubscribeToDieEvent(objectiveDie);
        }
        this.objective = character;
        if (objective != null)
        {
            objective.subscribeToDieEvent(objectiveDie);
        }

    }
    public void objectiveDie(ACharacter character)
    {
        if (objective.Equals(character))
        {
            objective.unSubscribeToDieEvent(objectiveDie);
            objective = null;
        }
    }
    public bool objectiveOnAttackDistance()
    {
        if(objective == null)
            return false;
        return Vector3.SqrMagnitude(transform.position - objective.transform.position) <= character._currentMask.getAttackDistance();
    }
    public bool transformOnWalkDistance(Transform objTransform)
    {
        return Vector3.SqrMagnitude(transform.position - objTransform.transform.position) <= walkStopDistance;
    }
    protected override void OnStarted()
    {
        character = GetComponent<ACharacter>();
        base.OnStarted();
    }

    public Status forceFail()
    {
        return Status.Failure;
    }
  public  void startAction()
    {
        actionFinished = false;
    }
    public void endCurrentAction()
    {
        actionFinished = true;
    }
}
