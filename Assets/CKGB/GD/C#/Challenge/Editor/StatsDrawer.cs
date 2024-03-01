using Ink;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static Stats;

[CustomPropertyDrawer(typeof(Stats))]
public class StatsDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        //Récupération des info.
        SerializedProperty stats = property.FindPropertyRelative("whatStatsTarget");
        SerializedProperty price = property.FindPropertyRelative("listPrice");
        SerializedProperty gain = property.FindPropertyRelative("listGain");
        SerializedProperty move = property.FindPropertyRelative("move");

        //Rect
        float listPriceHeight = EditorGUI.GetPropertyHeight(stats, stats.isExpanded);
        float listGainHeight = EditorGUI.GetPropertyHeight(price, price.isExpanded);

        float fieldHeight = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

        Rect statsRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);

        Rect targetRect = new Rect(position.x, position.y + fieldHeight, position.width, EditorGUIUtility.singleLineHeight);

        //Début du dessin.
        EditorGUI.BeginProperty(position, label, property);

        //Dessin
        EditorGUI.PropertyField(statsRect, stats, new GUIContent("Stats Target"));

        ETypeStatsTarget statsTarget = (ETypeStatsTarget)stats.enumValueIndex;

        if (statsTarget == ETypeStatsTarget.Price)
        {
            EditorGUI.PropertyField(targetRect, price, new GUIContent("Price"));
        }
        else if (statsTarget == ETypeStatsTarget.Gain)
        {
            EditorGUI.PropertyField(targetRect, gain, new GUIContent("Gain"));
        }
        else if (statsTarget == ETypeStatsTarget.Movement)
        {
            EditorGUI.PropertyField(targetRect, move, new GUIContent("Movement"));
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        SerializedProperty statsTarget = property.FindPropertyRelative("whatStatsTarget");
        SerializedProperty price = property.FindPropertyRelative("listPrice");
        SerializedProperty gain = property.FindPropertyRelative("listGain");
        SerializedProperty move = property.FindPropertyRelative("move");

        float priceHeight = EditorGUI.GetPropertyHeight(price, price.isExpanded);
        float gainHeight = EditorGUI.GetPropertyHeight(gain, gain.isExpanded);
        float moveHeight = EditorGUI.GetPropertyHeight(move);

        ETypeStatsTarget stats = (ETypeStatsTarget)statsTarget.enumValueIndex;

        if (stats == ETypeStatsTarget.Price)
        {
            return EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + priceHeight;
        }
        else if (stats == ETypeStatsTarget.Gain)
        {
            return EditorGUIUtility.singleLineHeight  + EditorGUIUtility.standardVerticalSpacing + gainHeight;
        }
        else if (stats == ETypeStatsTarget.Movement)
        {
            return EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + moveHeight;
        }

        return EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
    }
}
