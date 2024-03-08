using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;
using static Stats;

[CustomPropertyDrawer(typeof(Stats))]
public class StatsDrawer : PropertyDrawer
{

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        //
        SerializedProperty whatStats = property.FindPropertyRelative("whatStats");
        SerializedProperty value = property.FindPropertyRelative("value");

        //Rect
        float fieldHeight = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

        Rect whatStatsRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);

        Rect statsRect = new Rect(position.x, position.y + fieldHeight, position.width, EditorGUIUtility.singleLineHeight);

        //Début du dessin.
        EditorGUI.BeginProperty(position, label, property);

        //Dessin
        EditorGUI.PropertyField(whatStatsRect, whatStats, new GUIContent("What Price ?"));

        ETypeStats statsTarget = (ETypeStats)whatStats.enumValueIndex;

        if (statsTarget == ETypeStats.Calm || statsTarget == ETypeStats.Energy)
        {
            EditorGUI.PropertyField(statsRect, value, new GUIContent("Value"));
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        SerializedProperty whatStats = property.FindPropertyRelative("whatStats");

        float statsHeight = EditorGUI.GetPropertyHeight(whatStats);

        ETypeStats statsTarget = (ETypeStats)whatStats.enumValueIndex;

        if (statsTarget == ETypeStats.Calm || statsTarget == ETypeStats.Energy)
        {
            return EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + statsHeight;
        }

        return EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
    }
}
