using Ink;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static TargetStats_NewInspector;

[CustomPropertyDrawer(typeof(TargetStats_NewInspector))]
public class TargetStatsDrawer_NewInspector : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        //Récupération des info.
        SerializedProperty stats = property.FindPropertyRelative("whatStatsTarget");
        SerializedProperty whatStats = property.FindPropertyRelative("whatStats");
        SerializedProperty value = property.FindPropertyRelative("value");
        SerializedProperty move = property.FindPropertyRelative("move");

        //Rect
        float listActionHeight = EditorGUI.GetPropertyHeight(stats, stats.isExpanded);

        float fieldHeight = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

        Rect statsRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        Rect whatStatsRect = new Rect(position.x, position.y + fieldHeight, position.width, EditorGUIUtility.singleLineHeight);
        Rect valueRect = new Rect(position.x, position.y + fieldHeight*2, position.width, EditorGUIUtility.singleLineHeight);
        Rect targetRect = new Rect(position.x, position.y + fieldHeight, position.width, EditorGUIUtility.singleLineHeight);

        //Début du dessin.
        EditorGUI.BeginProperty(position, label, property);

        //Dessin
        ETypeStatsTarget statsTarget = (ETypeStatsTarget)stats.enumValueIndex;

        EditorGUI.PropertyField(statsRect, stats, new GUIContent("Stats / Move"));

        if (statsTarget == ETypeStatsTarget.Stats)
        {
            EditorGUI.PropertyField(whatStatsRect, whatStats, new GUIContent("what stats ?"));
            EditorGUI.PropertyField(valueRect, value, new GUIContent("Value"));
        }
        
        if (statsTarget == ETypeStatsTarget.Movement)
        {
            EditorGUI.PropertyField(targetRect, move, new GUIContent("Movement"));
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        SerializedProperty statsTarget = property.FindPropertyRelative("whatStatsTarget");
        SerializedProperty whatStats = property.FindPropertyRelative("whatStats");
        SerializedProperty value = property.FindPropertyRelative("value");
        SerializedProperty move = property.FindPropertyRelative("move");

        float statsTargetHeight = EditorGUI.GetPropertyHeight(statsTarget);
        float whatStatsHeight = EditorGUI.GetPropertyHeight(whatStats);
        float valueHeight = EditorGUI.GetPropertyHeight(value);
        float moveHeight = EditorGUI.GetPropertyHeight(move);

        ETypeStatsTarget stats = (ETypeStatsTarget)statsTarget.enumValueIndex;

        if (stats == ETypeStatsTarget.Stats)
        {
            return EditorGUIUtility.singleLineHeight + statsTargetHeight + whatStatsHeight + valueHeight;
        }

        if (stats == ETypeStatsTarget.Movement)
        {
            return EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + moveHeight;
        }

        return EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
    }
}
