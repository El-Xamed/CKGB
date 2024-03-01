using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static Interaction;

[CustomPropertyDrawer(typeof(Interaction))]
public class InteractionDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        //
        SerializedProperty what = property.FindPropertyRelative("whatTarget");
        SerializedProperty stats = property.FindPropertyRelative("listStats");
        SerializedProperty gain = property.FindPropertyRelative("listGain");
        SerializedProperty move = property.FindPropertyRelative("listMove");


        float statsHeight = EditorGUI.GetPropertyHeight(stats, stats.isExpanded);

        float fieldHeight = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

        Rect targetRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);

        Rect statsRect = new Rect(position.x, position.y + fieldHeight, position.width, statsHeight);

        Rect testRect = new Rect(position.x, position.y + fieldHeight + statsHeight, position.width, EditorGUIUtility.singleLineHeight);

        //Début du dessin.
        EditorGUI.BeginProperty(position, label, property);

        //Dessin
        EditorGUI.PropertyField(targetRect, what, new GUIContent("Target"));
        EditorGUI.PropertyField(statsRect, stats);

        ETypeTarget target = (ETypeTarget)what.enumValueIndex;

        if (target == ETypeTarget.Self)
        {

        }
        else if (target == ETypeTarget.Other)
        {
            //EditorGUI.PropertyField(rangeRect, range, new GUIContent("Range"));
        }


        EditorGUI.EndProperty();
    }



    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        SerializedProperty stats = property.FindPropertyRelative("listStats");

        float priceheight = EditorGUI.GetPropertyHeight(stats, stats.isExpanded);

        return EditorGUIUtility.singleLineHeight * 2 + EditorGUIUtility.standardVerticalSpacing + priceheight;
    }
}
