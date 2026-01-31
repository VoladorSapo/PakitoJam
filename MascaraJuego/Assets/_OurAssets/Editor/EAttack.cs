using System.Diagnostics;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Attack))]
public class EAttack : Editor
{
    public override void OnInspectorGUI()
    {

        base.OnInspectorGUI();
        Attack data = (Attack)target;

        if (GUILayout.Button("Add Damage")) {
            data.addDamageEffect(1);
        }
}

}