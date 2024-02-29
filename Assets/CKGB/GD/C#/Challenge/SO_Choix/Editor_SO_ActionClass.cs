using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SO_ActionClass))]
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
        SO_ActionClass script = (SO_ActionClass)target;

        serializedObject.Update();

        //Pour afficher la liste.
        EditorGUILayout.PropertyField(serializedObject.FindProperty("listInteraction"), new GUIContent("Interaction"));

        //Pour tous les object dans cette liste, vérifier l'enum pour afficher les parametre avec.
        foreach (SO_ActionClass actionClass in serializedObject.FindProperty("listInteraction"))
        {

        }

        //Pour créer un bouton
        /*if (GUILayout.Button(new GUIContent("+", "duplicate")))
        {
            
        }
        if (GUILayout.Button(new GUIContent("-", "delete")))
        {
            
        }*/

        serializedObject.ApplyModifiedProperties();
    }
}

//[CustomEditor(typeof(SO_ActionClass.Interaction))]
public class Editor_Interaction : Editor
{
    public override void OnInspectorGUI()
    {
        //SO_ActionClass.Interaction script = (SO_ActionClass.Interaction)target;

        serializedObject.Update();

        if (/*script.whatTarget == SO_ActionClass.Interaction.ETypeTarget.Self*/ true)
        {

        }

        //Pour afficher la liste.
        EditorGUILayout.TextField("Test string");

        serializedObject.ApplyModifiedProperties();
    }
}