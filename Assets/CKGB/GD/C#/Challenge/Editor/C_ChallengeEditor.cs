using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(C_Challenge))]
public class C_ChallengeEditor : Editor
{
    [SerializeField]
    int newPosition;
    public override void OnInspectorGUI()
    {
        C_Challenge challengeScript = (C_Challenge)target;

        EditorGUILayout.IntField("New position", newPosition);

        if(GUILayout.Button("Update position for all"))
        {
            challengeScript.UpdateAllPosition();
        }
    }
}
