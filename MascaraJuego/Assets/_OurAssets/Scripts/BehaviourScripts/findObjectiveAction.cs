using BehaviourAPI.UnityToolkit;
using BehaviourAPI.Core;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class findObjectiveAction : UnityAction
{
    CharacterAssetBehaviourRunner characterBehaviour;
    ACharacter character;
    LayerMask layer;

    public override void Start()
    {
        base.Start();
        characterBehaviour = context.GameObject.GetComponent<CharacterAssetBehaviourRunner>();
        character = characterBehaviour.character;
        float minDist = float.MaxValue;
        Collider2D bestcol = null;
        foreach (Collider2D col in Physics2D.OverlapCircleAll(context.Transform.position, characterBehaviour.character._currentMask.getDetectRadius(),characterBehaviour.EnemyLayerMask))
        {
            Debug.Log(col.gameObject.name);
            if (col.gameObject == context.GameObject)
            {

                continue;
            }
            float dist = Vector3.SqrMagnitude(col.transform.position - characterBehaviour.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                bestcol = col;
            }
        }
        if (bestcol != null)
        {
            characterBehaviour.setObjective(bestcol.GetComponent<ACharacter>());
        }
        else
        {
            characterBehaviour.setObjective(null);
        }
}

    public override Status Update()
    {
        if(characterBehaviour.objective == null)
        {
            return Status.Failure;
        }
        return Status.Success;
    }
}
