using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(Guard))]
public class CreateColorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Generate Color"))
        {
            GenerateColor();
        }
    }
    public void GenerateColor()
    {
        Guard guard = (Guard)target;
        Color aplhaCorected = Random.ColorHSV();
        aplhaCorected.a = 1;
        guard.ColorToID = aplhaCorected;
    }
}
