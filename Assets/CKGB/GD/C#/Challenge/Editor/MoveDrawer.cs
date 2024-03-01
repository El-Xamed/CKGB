using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static Move;

[CustomPropertyDrawer(typeof(Move))]
public class MoveDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        //Récupération des info.
        SerializedProperty move = property.FindPropertyRelative("whatMove");
        SerializedProperty nbMove = property.FindPropertyRelative("nbMove");
        SerializedProperty actor = property.FindPropertyRelative("actor");
        SerializedProperty acc = property.FindPropertyRelative("accessories");

        //Rect
        float statsHeight = EditorGUI.GetPropertyHeight(move, move.isExpanded);

        float fieldHeight = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

        Rect statsRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);

        Rect targetRect = new Rect(position.x, position.y + fieldHeight + statsHeight, position.width, EditorGUIUtility.singleLineHeight);

        //Début du dessin.
        EditorGUI.BeginProperty(position, label, property);

        //Dessin
        EditorGUI.PropertyField(statsRect, move, new GUIContent("What Move ?"));

        ETypeMove moveTarget = (ETypeMove)move.enumValueIndex;

        if (moveTarget != ETypeMove.None && moveTarget != ETypeMove.SwitchWithActor && moveTarget != ETypeMove.SwitchWithAcc)
        {
            EditorGUI.PropertyField(targetRect, nbMove, new GUIContent("Nombre of Move"));
        }
        else if (moveTarget == ETypeMove.SwitchWithActor)
        {
            EditorGUI.PropertyField(targetRect, actor, new GUIContent("With what Actor ?"));
        }
        else if (moveTarget == ETypeMove.SwitchWithAcc)
        {
            EditorGUI.PropertyField(targetRect, acc, new GUIContent("With what Accessorie ?"));
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        SerializedProperty move = property.FindPropertyRelative("whatMove");

        float moveHeight = EditorGUI.GetPropertyHeight(move, move.isExpanded);

        return EditorGUIUtility.singleLineHeight * 2 + EditorGUIUtility.standardVerticalSpacing + moveHeight;
    }
}
