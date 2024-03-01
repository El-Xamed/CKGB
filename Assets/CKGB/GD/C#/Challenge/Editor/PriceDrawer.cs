using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;
using static Price;

[CustomPropertyDrawer(typeof(Price))]
public class PriceDrawer : PropertyDrawer
{

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        //
        SerializedProperty whatPrice = property.FindPropertyRelative("whatPrice");
        SerializedProperty price = property.FindPropertyRelative("price");

        //Rect
        float fieldHeight = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

        Rect whatPriceRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);

        Rect priceRect = new Rect(position.x, position.y + fieldHeight, position.width, EditorGUIUtility.singleLineHeight);

        //Début du dessin.
        EditorGUI.BeginProperty(position, label, property);

        //Dessin
        EditorGUI.PropertyField(whatPriceRect, whatPrice, new GUIContent("What Price ?"));

        ETypePrice priceTarget = (ETypePrice)whatPrice.enumValueIndex;

        if (priceTarget == ETypePrice.Calm || priceTarget == ETypePrice.Energy)
        {
            EditorGUI.PropertyField(priceRect, price, new GUIContent("Price"));
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        SerializedProperty whatPrice = property.FindPropertyRelative("whatPrice");

        float priceHeight = EditorGUI.GetPropertyHeight(whatPrice);

        ETypePrice priceTarget = (ETypePrice)whatPrice.enumValueIndex;

        if (priceTarget == ETypePrice.Calm || priceTarget == ETypePrice.Energy)
        {
            return EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + priceHeight;
        }

        return EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
    }
}
