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
        SerializedProperty isTp = property.FindPropertyRelative("isTp");
        SerializedProperty nbMove = property.FindPropertyRelative("nbMove");
        SerializedProperty actor = property.FindPropertyRelative("actor");
        SerializedProperty acc = property.FindPropertyRelative("accessories");

        //Rect
        float fieldHeight = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

        float isTpHeight = EditorGUI.GetPropertyHeight(isTp);

        Rect statsRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);

        Rect nbMoveRect = new Rect(position.x, position.y + isTpHeight + fieldHeight, position.width, EditorGUIUtility.singleLineHeight);

        Rect switchRect = new Rect(position.x, position.y + fieldHeight, position.width, EditorGUIUtility.singleLineHeight);

        Rect isTpRect = new Rect(position.x, position.y + fieldHeight, position.width, EditorGUIUtility.singleLineHeight);

        //Début du dessin.
        EditorGUI.BeginProperty(position, label, property);

        //Dessin
        EditorGUI.PropertyField(statsRect, move, new GUIContent("What Move ?"));

        ETypeMove moveTarget = (ETypeMove)move.enumValueIndex;

        if (moveTarget == ETypeMove.Right || moveTarget == ETypeMove.Left)
        {
            EditorGUI.PropertyField(isTpRect, isTp, new GUIContent("Téléporte l'acteur ?"));

            EditorGUI.PropertyField(nbMoveRect, nbMove, new GUIContent("Nombre of Move"));
        }
        else if (moveTarget == ETypeMove.OnTargetCase)
        {
            nbMoveRect = new Rect(position.x, position.y + fieldHeight, position.width, EditorGUIUtility.singleLineHeight);

            EditorGUI.PropertyField(nbMoveRect, nbMove, new GUIContent("What target case ?"));
        }
        else if (moveTarget == ETypeMove.SwitchWithActor)
        {
            EditorGUI.PropertyField(switchRect, actor, new GUIContent("With what Actor ?"));
        }
        else if (moveTarget == ETypeMove.SwitchWithAcc)
        {
            EditorGUI.PropertyField(switchRect, acc, new GUIContent("With what Accessorie ?"));
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        SerializedProperty whatMove = property.FindPropertyRelative("whatMove");
        SerializedProperty isTp = property.FindPropertyRelative("isTp");

        float moveHeight = EditorGUI.GetPropertyHeight(whatMove);
        float isTpHeight = EditorGUI.GetPropertyHeight(isTp);

        ETypeMove moveTarget = (ETypeMove)whatMove.enumValueIndex;

        if (moveTarget == ETypeMove.Right || moveTarget == ETypeMove.Left)
        {
            return EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + moveHeight + isTpHeight;
        }

        return EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + moveHeight;
    }
}
