using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static Stats_NewInspector;

[CustomPropertyDrawer(typeof(Stats_NewInspector))]
public class StatsDrawer_NewInspector : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        //Récupération des info.
        SerializedProperty whatCost = property.FindPropertyRelative("whatCost");
        SerializedProperty whatStats = property.FindPropertyRelative("whatStats");
        SerializedProperty value = property.FindPropertyRelative("value");

        //Rect
        float fieldHeight = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

        Rect whatCostRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);

        Rect whatStatsRect = new Rect(position.x, position.y + fieldHeight, position.width, EditorGUIUtility.singleLineHeight);

        Rect valueRect = new Rect(position.x, position.y + (fieldHeight *2), position.width, EditorGUIUtility.singleLineHeight);

        //Début du dessin.
        EditorGUI.BeginProperty(position, label, property);

        //Dessin

        //Enum cost
        EditorGUI.PropertyField(whatCostRect, whatCost, new GUIContent("Price / Gain"));

        //Enum type stats
        EditorGUI.PropertyField(whatStatsRect, whatStats, new GUIContent("Energy / Calm"));

        //Value
        EditorGUI.PropertyField(valueRect, value, new GUIContent("Value"));

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        SerializedProperty whatCost = property.FindPropertyRelative("whatCost");
        SerializedProperty whatStats = property.FindPropertyRelative("whatStats");
        SerializedProperty value = property.FindPropertyRelative("value");

        float costHeight = EditorGUI.GetPropertyHeight(whatCost);
        float statsHeight = EditorGUI.GetPropertyHeight(whatStats);

        return EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + costHeight + statsHeight;
    }
}
