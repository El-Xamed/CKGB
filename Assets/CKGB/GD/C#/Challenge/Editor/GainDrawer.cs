using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static Gain;

[CustomPropertyDrawer(typeof(Gain))]
public class GainDrawer : PropertyDrawer
{

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        //
        SerializedProperty whatGain = property.FindPropertyRelative("whatGain");
        SerializedProperty gain = property.FindPropertyRelative("gain");

        //Rect
        float fieldHeight = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

        Rect whatGainRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);

        Rect gainRect = new Rect(position.x, position.y + fieldHeight, position.width, EditorGUIUtility.singleLineHeight);

        //Début du dessin.
        EditorGUI.BeginProperty(position, label, property);

        //Dessin
        EditorGUI.PropertyField(whatGainRect, whatGain, new GUIContent("What Gain ?"));

        ETypeGain gainTarget = (ETypeGain)whatGain.enumValueIndex;

        if (gainTarget == ETypeGain.Calm)
        {
            EditorGUI.PropertyField(gainRect, gain, new GUIContent("Gain"));
        }
        else if (gainTarget == ETypeGain.Energy)
        {
            EditorGUI.PropertyField(gainRect, gain, new GUIContent("Gain"));
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        SerializedProperty whatGain = property.FindPropertyRelative("whatGain");

        float gainHeight = EditorGUI.GetPropertyHeight(whatGain);

        ETypeGain gainTarget = (ETypeGain)whatGain.enumValueIndex;

        if (gainTarget == ETypeGain.Calm || gainTarget == ETypeGain.Energy)
        {
            return EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + gainHeight;
        }

        return EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
    }
}
