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
        SerializedProperty statsTarget = property.FindPropertyRelative("whatStatsTarget");
        SerializedProperty stats = property.FindPropertyRelative("dataStats");
        SerializedProperty move = property.FindPropertyRelative("dataMove");

        //Rect
        float listActionHeight = EditorGUI.GetPropertyHeight(statsTarget, statsTarget.isExpanded);

        float fieldHeight = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

        Rect statsRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        Rect whatStatsRect = new Rect(position.x, position.y + fieldHeight, position.width, EditorGUIUtility.singleLineHeight);
        Rect targetRect = new Rect(position.x, position.y + fieldHeight, position.width, EditorGUIUtility.singleLineHeight);

        //Début du dessin.
        EditorGUI.BeginProperty(position, label, property);

        //Dessin
        ETypeStatsTarget statsEnum = (ETypeStatsTarget)statsTarget.enumValueIndex;

        EditorGUI.PropertyField(statsRect, statsTarget, new GUIContent("Stats / Move"));

        if (statsEnum == ETypeStatsTarget.Stats)
        {
            EditorGUI.PropertyField(whatStatsRect, stats, new GUIContent("what stats ?"));
        }
        
        if (statsEnum == ETypeStatsTarget.Movement)
        {
            EditorGUI.PropertyField(targetRect, move, new GUIContent("Movement"));
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        SerializedProperty statsTarget = property.FindPropertyRelative("whatStatsTarget");
        SerializedProperty stats = property.FindPropertyRelative("dataStats");
        SerializedProperty move = property.FindPropertyRelative("dataMove");

        float statsTargetHeight = EditorGUI.GetPropertyHeight(statsTarget);
        float valueHeight = EditorGUI.GetPropertyHeight(stats);
        float moveHeight = EditorGUI.GetPropertyHeight(move);

        ETypeStatsTarget statsEnum = (ETypeStatsTarget)statsTarget.enumValueIndex;

        if (statsEnum == ETypeStatsTarget.Stats)
        {
            return EditorGUIUtility.singleLineHeight + statsTargetHeight + valueHeight;
        }

        if (statsEnum == ETypeStatsTarget.Movement)
        {
            return EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + moveHeight;
        }

        return EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
    }
}
