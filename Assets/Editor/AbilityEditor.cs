using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Ability))]
public class AbilityEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Ability script = (Ability)target;
        if (GUILayout.Button("Use ability"))
        {
            script.UseAbility();
        }
    }
}
