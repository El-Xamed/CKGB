using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//[CustomEditor(typeof(SO_ActionClass))]
public class Editor_SO_ActionClass : Editor
{
    /*
    public override void OnInspectorGUI()
    {
        SO_ActionClass script = (SO_ActionClass)target;

        foreach (Interaction actionClass in script.GetInteraction())
        {
            if (actionClass.whatTarget == Interaction.ETypeTarget.Self)
            {
                
                actionClass.listPrice = EditorGUILayout.TextField("Test string", script.testString);
                script.testInt = EditorGUILayout.IntField("Test int", script.testInt);
            }
        }
        script.GetInteraction()[0].whatTarget = (Interaction.ETypeTarget)EditorGUILayout.EnumPopup("My type", script.GetInteraction()[0].whatTarget);

        if (script.GetInteraction()[0].whatTarget == Interaction.ETypeTarget.Self)
        {
            script.GetInteraction()[0].listPrice = EditorGUILayout.TextField("Test string", script.testString);
            script.testInt = EditorGUILayout.IntField("Test int", script.testInt);
        }
    }
    */

    public override void OnInspectorGUI()
    {
        //Pour afficher la liste.
        EditorGUILayout.PropertyField(serializedObject.FindProperty("listInteraction"), new GUIContent("Interaction"));

        serializedObject.ApplyModifiedProperties();
    }
}