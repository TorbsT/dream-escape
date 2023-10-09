using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Spikes))]
public class SpikesEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Spikes script = (Spikes)target;
        if (GUILayout.Button("Refresh spikes"))
        {
            script.Refresh();
        }
    }
}
