using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(IslandGenerator))]
public class IslandGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        IslandGenerator generator = (IslandGenerator)target;
        if (GUILayout.Button("Build Mesh"))
        {
            generator.createAndUpdateMesh();
        }
    }
}
