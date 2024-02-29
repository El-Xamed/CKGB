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
        //
        SerializedProperty stats = property.FindPropertyRelative("whatStatsTarget");
        SerializedProperty price = property.FindPropertyRelative("listPrice");
        SerializedProperty gain = property.FindPropertyRelative("listGain");
        SerializedProperty move = property.FindPropertyRelative("listMovement");


        //Rect
        float statsHeight = EditorGUI.GetPropertyHeight(stats, stats.isExpanded);

        float priceheight = EditorGUIUtility.singleLineHeight * 2 + EditorGUIUtility.standardVerticalSpacing + statsHeight;

        float fieldHeight = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

        Rect statsRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);

        Rect blaRect = new Rect(position.x, position.y + fieldHeight, position.width, EditorGUIUtility.singleLineHeight);

        Rect gainRect = new Rect(position.x, position.y + fieldHeight + priceheight, position.width, EditorGUIUtility.singleLineHeight);


        //Début du dessin.
        EditorGUI.BeginProperty(position, label, property);

        //Dessin
        EditorGUI.PropertyField(statsRect, stats, new GUIContent("Stats Target"));

        ETypeStatsTarget statsTarget = (ETypeStatsTarget)stats.enumValueIndex;

        if (statsTarget == ETypeStatsTarget.Price)
        {
            EditorGUI.PropertyField(gainRect, price, new GUIContent("Price"));
        }
        else if (statsTarget == ETypeStatsTarget.Gain)
        {
            EditorGUI.PropertyField(gainRect, gain, new GUIContent("Gain"));
        }
        else if (statsTarget == ETypeStatsTarget.Movement)
        {
            EditorGUI.PropertyField(gainRect, move, new GUIContent("Movement"));
        }

        EditorGUI.EndProperty();
    }
}
