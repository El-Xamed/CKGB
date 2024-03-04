using Ink;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(AdvancedCondition))]
public class AdvancedDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        #region Variables

        #region Condition avancé
        SerializedProperty advancedCondition = property.FindPropertyRelative("advancedCondition");
        SerializedProperty needAcc = property.FindPropertyRelative("needAcc");
        SerializedProperty needActor = property.FindPropertyRelative("canMakeByOneActor");
        SerializedProperty whatAcc = property.FindPropertyRelative("whatAcc");
        SerializedProperty whatActor = property.FindPropertyRelative("whatActor");

        #endregion
        #endregion


        #region Rect
        //Calcul de la hauteur pour bien séparer les variables dans l'inspector.
        float fieldHeight = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

        //Calcul de la hauteur de "needAcc".
        float needAccHeight = EditorGUI.GetPropertyHeight(needAcc);
        //Calcul de la hauteur de "whatAcc".
        float needActorHeight = EditorGUI.GetPropertyHeight(needActor);
        //Calcul de la hauteur de "whatAcc".
        float whatAccHeight = EditorGUI.GetPropertyHeight(whatAcc);

        //Rect pour placer "AdvancedCondition".
        Rect advancedConditionRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        //Rect pour placer "needAcc".
        Rect needAccRect = new Rect(position.x +50, position.y + fieldHeight, position.width, EditorGUIUtility.singleLineHeight);
        //Rect pour placer "needActor".
        Rect needActorRect = new Rect(position.x +50, position.y + needAccHeight + fieldHeight, position.width, EditorGUIUtility.singleLineHeight);

        #endregion

        #region Dessin
        //Début du dessin.
        EditorGUI.BeginProperty(position, label, property);

        //Dessin
        //Pour placer l'enum "Target".
        EditorGUI.PropertyField(advancedConditionRect, advancedCondition, new GUIContent("Adanced Condition"));

        bool boolAdvancedCondition = advancedCondition.boolValue;

        if (boolAdvancedCondition)
        {
            bool boolneedAcc = needAcc.boolValue;
            bool boolneedActor = needActor.boolValue;
            
            //Check si on souhaite utiliser needAcc ou needActor.
            if (boolneedAcc)
            {
                //Rect pour placer "whatAcc".
                Rect whatAccRect = new Rect(position.x + 75, position.y + needAccHeight + fieldHeight, position.width / 1.5f, EditorGUIUtility.singleLineHeight);
                EditorGUI.PropertyField(whatAccRect, whatAcc);
                needActorRect = new Rect(position.x +50, position.y + whatAccHeight + needAccHeight + fieldHeight, position.width, EditorGUIUtility.singleLineHeight);
            }
            if (boolneedActor)
            {
                Rect whatActorRect;
                if (boolneedAcc)
                {
                    //Rect pour placer "whatActor".
                    whatActorRect = new Rect(position.x + 75, position.y + needAccHeight + whatAccHeight + needActorHeight + fieldHeight, position.width / 1.5f, EditorGUIUtility.singleLineHeight);
                }
                else
                {
                    //Rect pour placer "whatActor".
                    whatActorRect = new Rect(position.x + 75, position.y + needAccHeight + needActorHeight + fieldHeight, position.width / 1.5f, EditorGUIUtility.singleLineHeight);
                }

                EditorGUI.PropertyField(whatActorRect, whatActor);
            }

            //Place mes bool
            EditorGUI.PropertyField(needAccRect, needAcc);
            EditorGUI.PropertyField(needActorRect, needActor);
        }


        EditorGUI.EndProperty();
        #endregion
    }

    //Pour définir la taille de ma list d'interaction
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        SerializedProperty advancedCondition = property.FindPropertyRelative("advancedCondition");
        SerializedProperty needAcc = property.FindPropertyRelative("needAcc");
        SerializedProperty needActor = property.FindPropertyRelative("canMakeByOneActor");
        SerializedProperty whatAcc = property.FindPropertyRelative("whatAcc");
        SerializedProperty whatActor = property.FindPropertyRelative("whatActor");

        //Calcul de la hauteur de "advancedCondition".
        float advancedConditionHeight = EditorGUI.GetPropertyHeight(advancedCondition);
        //Calcul de la hauteur de "needAcc".
        float needAccHeight = EditorGUI.GetPropertyHeight(needAcc);
        //Calcul de la hauteur de "needActor".
        float needActorHeight = EditorGUI.GetPropertyHeight(needActor);
        //Calcul de la hauteur de "whatAcc".
        float whatAccHeight = EditorGUI.GetPropertyHeight(whatAcc);
        //Calcul de la hauteur de "whatActor".
        float whatActorHeight = EditorGUI.GetPropertyHeight(whatActor);

        bool boolAdvancedCondition = advancedCondition.boolValue;

        if (boolAdvancedCondition)
        {
            bool boolneedAcc = needAcc.boolValue;
            bool boolneedActor = needActor.boolValue;

            //Check si on souhaite utiliser needAcc ou needActor.
            if (boolneedAcc || boolneedActor)
            {
                if (boolneedActor && boolneedAcc)
                {
                    return EditorGUIUtility.singleLineHeight * 2 + EditorGUIUtility.standardVerticalSpacing + needAccHeight + needActorHeight + whatAccHeight + whatActorHeight;
                }

                return EditorGUIUtility.singleLineHeight * 2 + EditorGUIUtility.standardVerticalSpacing + needAccHeight + needActorHeight;
            }
            else
            {
                return EditorGUIUtility.singleLineHeight * 2 + EditorGUIUtility.standardVerticalSpacing + advancedConditionHeight;
            }
        }

        return EditorGUIUtility.singleLineHeight * 2 + EditorGUIUtility.standardVerticalSpacing;
    }
}
