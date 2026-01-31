using System.Diagnostics;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PowerMaskStats))]
public class EPowerMaskStats : Editor
{
    public override void OnInspectorGUI()
    {

        base.OnInspectorGUI();
        PowerMaskStats data = (PowerMaskStats)target;

        if (GUILayout.Button("Add Damage")) {
            data.addDamageEffect(1);
        }
        if (GUILayout.Button("Add Freeze"))
        {
            data.addFreezeEffect(2,0.7f);
        }
    }

}