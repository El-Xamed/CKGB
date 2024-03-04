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

        #region Action
        //Var
        SerializedProperty what = property.FindPropertyRelative("whatTarget");

        //List d'information pour "Self"
        SerializedProperty stats = property.FindPropertyRelative("listStats");
        //List d'information pour "Other"
        SerializedProperty directionOther = property.FindPropertyRelative("whatDirectionTarget");
        SerializedProperty rangeOther = property.FindPropertyRelative("range");
        #endregion

        #endregion

        #region Rect
        #region Liste d'interaction
        //Calcul de la hauteur de la liste "stats" + si elle est déplié ou non.
        float statsHeight = EditorGUI.GetPropertyHeight(stats, stats.isExpanded);
        //Calcul de la hauteur pour bien séparer les variables dans l'inspector.
        float fieldHeight = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

        //Rect pour placer "Target".
        Rect targetRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        //Rect pour placer la liste des "stats".
        Rect statsRect = new Rect(position.x, position.y + fieldHeight, position.width, statsHeight);

        //Calcul de la hauteur de l'enum "what" + si elle est déplié ou non (pas utile de connaitre si elle est déplié ou non car c'est pas une liste A TESTER POUR CONFIRMER).
        float targetHeight = EditorGUI.GetPropertyHeight(what, what.isExpanded);
        //Calcul de la hauteur de "range" pour bien séparer les variables dans l'inspector.
        float rangeHeight = EditorGUI.GetPropertyHeight(rangeOther);

        //Pour la partie Other.

        Rect directionTargetRect = new Rect(position.x, position.y + fieldHeight, position.width, targetHeight);
        Rect rangeTargetRect = new Rect(position.x, position.y + fieldHeight * 2, position.width, targetHeight);
        //Meme Rect que "statsRect" sauf qu'il est placé plus bas pour laisser apparaitre les var pour setup la partie "other".
        Rect statsOtherRect = new Rect(position.x, position.y + fieldHeight + 10 + targetHeight, position.width, EditorGUIUtility.singleLineHeight);
        #endregion
        #endregion

        #region Dessin
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
            EditorGUI.PropertyField(directionTargetRect, directionOther);

            ETypeDirectionTarget dirTarget = (ETypeDirectionTarget)directionOther.enumValueIndex;

            if (dirTarget != ETypeDirectionTarget.None)
            {
                statsOtherRect = new Rect(position.x, position.y + fieldHeight +10 + targetHeight + rangeHeight, position.width, EditorGUIUtility.singleLineHeight);

                EditorGUI.PropertyField(rangeTargetRect, rangeOther);
            }

            EditorGUI.PropertyField(statsOtherRect, stats);
        }


        EditorGUI.EndProperty();
        #endregion
    }

    //Pour définir la taille de ma list d'interaction
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
            return EditorGUIUtility.singleLineHeight *2 + EditorGUIUtility.standardVerticalSpacing + statsHeight;
        }
        else if (target == ETypeTarget.Other)
        {
            ETypeDirectionTarget dirTarget = (ETypeDirectionTarget)directionOther.enumValueIndex;

            if (dirTarget != ETypeDirectionTarget.None)
            {
                return EditorGUIUtility.singleLineHeight *2 + EditorGUIUtility.standardVerticalSpacing + statsHeight + dirHeight + rangeHeight;
            }

            return EditorGUIUtility.singleLineHeight *2 + EditorGUIUtility.standardVerticalSpacing + statsHeight + dirHeight;
        }

        return EditorGUIUtility.singleLineHeight *2 + EditorGUIUtility.standardVerticalSpacing + statsHeight + rangeHeight;
    }
}
