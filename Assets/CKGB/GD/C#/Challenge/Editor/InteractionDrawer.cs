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
        SerializedProperty price = property.FindPropertyRelative("listPrice");
        SerializedProperty gain = property.FindPropertyRelative("listGain");
        SerializedProperty move = property.FindPropertyRelative("listMove");
        SerializedProperty range = property.FindPropertyRelative("range");


        float priceheight = EditorGUI.GetPropertyHeight(price, price.isExpanded);

        float fieldHeight = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

        Rect priceRect = new Rect(position.x, position.y + fieldHeight, position.width, priceheight);

        Rect rangeRect = new Rect(position.x, position.y + fieldHeight + priceheight, position.width, EditorGUIUtility.singleLineHeight);

        //Début du dessin.
        EditorGUI.BeginProperty(position, label, property);

        //Dessin
        EditorGUI.PropertyField(priceRect, price, new GUIContent("Price"));
        EditorGUI.PropertyField(position, what, new GUIContent("Target"));

        ETypeTarget target = (ETypeTarget)what.enumValueIndex;

        if (target == ETypeTarget.Self)
        {
            
        }
        else if (target == ETypeTarget.Other)
        {
            EditorGUI.PropertyField(rangeRect, range, new GUIContent("Range"));

        }


        EditorGUI.EndProperty();
    }

    

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        SerializedProperty price = property.FindPropertyRelative("listPrice");
        SerializedProperty gain = property.FindPropertyRelative("listGain");
        SerializedProperty move = property.FindPropertyRelative("listMove");

        float priceheight = EditorGUI.GetPropertyHeight(price, price.isExpanded);

        return EditorGUIUtility.singleLineHeight * 2 + EditorGUIUtility.standardVerticalSpacing + priceheight;
    }
}
