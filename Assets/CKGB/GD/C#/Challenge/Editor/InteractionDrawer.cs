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
        #region Variables
        //Var
        SerializedProperty what = property.FindPropertyRelative("whatTarget");

        //List d'information pour "Self"
        SerializedProperty stats = property.FindPropertyRelative("listStats");
        //List d'information pour "Other"
        SerializedProperty directionOther = property.FindPropertyRelative("whatDirectionTarget");
        SerializedProperty rangenOther = property.FindPropertyRelative("range");
        #endregion

        #region Rect
        //Rect
        float statsHeight = EditorGUI.GetPropertyHeight(stats, stats.isExpanded);

        float fieldHeight = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

        Rect targetRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);

        Rect statsRect = new Rect(position.x, position.y + fieldHeight, position.width, statsHeight);

        //
        Rect statsOtherRect = new Rect(position.x, position.y + fieldHeight, position.width, statsHeight);

        Rect directionTargetRect = new Rect(position.x, position.y + fieldHeight + statsHeight, position.width, EditorGUIUtility.singleLineHeight);
        Rect rangeTargetRect = new Rect(position.x, position.y + fieldHeight * 2 + statsHeight, position.width, EditorGUIUtility.singleLineHeight);
        #endregion

        //Début du dessin.
        EditorGUI.BeginProperty(position, label, property);

        //Dessin
        //Pour placer l'enum "Target".
        EditorGUI.PropertyField(targetRect, what, new GUIContent("Target"));
        

        ETypeTarget target = (ETypeTarget)what.enumValueIndex;

        if (target == ETypeTarget.Self)
        {
            EditorGUI.PropertyField(statsRect, stats);
        }
        else if (target == ETypeTarget.Other)
        {
            EditorGUI.PropertyField(statsRect, stats);
            EditorGUI.PropertyField(directionTargetRect, directionOther);

            ETypeDirectionTarget dirTarget = (ETypeDirectionTarget)directionOther.enumValueIndex;

            if (dirTarget != ETypeDirectionTarget.None)
            {
                EditorGUI.PropertyField(rangeTargetRect, rangenOther);
            }
        }


        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        SerializedProperty what = property.FindPropertyRelative("whatTarget");
        SerializedProperty directionOther = property.FindPropertyRelative("whatDirectionTarget");
        SerializedProperty stats = property.FindPropertyRelative("listStats");
        SerializedProperty range = property.FindPropertyRelative("range");

        float statsHeight = EditorGUI.GetPropertyHeight(stats, stats.isExpanded);
        float dirHeight = EditorGUI.GetPropertyHeight(directionOther);
        float rangeHeight = EditorGUI.GetPropertyHeight(range);

        ETypeTarget target = (ETypeTarget)what.enumValueIndex;

        if (target == ETypeTarget.Self)
        {
            return EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + statsHeight;
        }
        else if (target == ETypeTarget.Other)
        {
            ETypeDirectionTarget dirTarget = (ETypeDirectionTarget)directionOther.enumValueIndex;

            if (dirTarget != ETypeDirectionTarget.None)
            {
                return EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + statsHeight + dirHeight + rangeHeight;
            }

            return EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + statsHeight + dirHeight;
        }

        return EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + statsHeight + rangeHeight;
    }
}
