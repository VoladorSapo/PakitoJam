using UnityEngine;
using BehaviourAPI.UnityToolkit.GUIDesigner.Runtime;
using NaughtyAttributes;
using BehaviourAPI.Core;
using System;

public class CharacterAssetBehaviourRunner : AssetBehaviourRunner
{
    public ACharacter character {  get; private set; }
    [field: SerializeField] public ACharacter objective;
    [SerializeField] float attackDistance;

    public bool actionFinished {  get; private set; }
  [field:SerializeField]  public LayerMask EnemyLayerMask { get; private set; }

    public void setObjective(ACharacter character)
    {
        this.character = character;
    }
    public bool objectiveOnAttackDistance()
    {
        return Vector3.SqrMagnitude(transform.position - objective.transform.position) <= attackDistance;
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
