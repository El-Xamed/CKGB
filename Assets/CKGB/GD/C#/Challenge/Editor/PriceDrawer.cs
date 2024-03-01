using System.Collections;
using System.Collections.Generic;
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
        float statsHeight = EditorGUI.GetPropertyHeight(whatPrice, whatPrice.isExpanded);

        float fieldHeight = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

        Rect whatPriceRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);

        Rect priceRect = new Rect(position.x, position.y + fieldHeight + statsHeight, position.width, EditorGUIUtility.singleLineHeight);

        //Début du dessin.
        EditorGUI.BeginProperty(position, label, property);

        //Dessin
        EditorGUI.PropertyField(whatPriceRect, whatPrice, new GUIContent("What Price ?"));

        ETypePrice priceTarget = (ETypePrice)whatPrice.enumValueIndex;

        if (priceTarget == ETypePrice.Calm)
        {
            EditorGUI.PropertyField(priceRect, price, new GUIContent("Price"));
        }
        else if (priceTarget == ETypePrice.Energy)
        {
            EditorGUI.PropertyField(priceRect, price, new GUIContent("Price"));
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        SerializedProperty price = property.FindPropertyRelative("price");

        float priceheight = EditorGUI.GetPropertyHeight(price, price.isExpanded);

        return EditorGUIUtility.singleLineHeight * 2 + EditorGUIUtility.standardVerticalSpacing + priceheight;
    }
}
