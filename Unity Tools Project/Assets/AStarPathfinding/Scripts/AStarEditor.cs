using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AStarGrid))]
public class AStarEditor : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        AStarGrid gridScript = (AStarGrid)target;
        if(GUILayout.Button("Generate Grid"))
        {
            gridScript.CreateGrid();
        }

    }

}
