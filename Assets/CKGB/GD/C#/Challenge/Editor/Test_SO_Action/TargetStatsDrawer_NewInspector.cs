using Ink;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static TargetStats_NewInspector;

[CustomPropertyDrawer(typeof(TargetStats_NewInspector))]
public class TargetStatsDrawer_NewInspector : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        //Récupération des info.
        SerializedProperty statsTarget = property.FindPropertyRelative("whatStatsTarget");

        #region Stats
        SerializedProperty cost = property.FindPropertyRelative("whatCost");
        SerializedProperty stats = property.FindPropertyRelative("whatStats");
        #endregion

        #region Movement
        SerializedProperty move = property.FindPropertyRelative("whatMove");
        SerializedProperty tp = property.FindPropertyRelative("isTp");
        SerializedProperty actorSwitch = property.FindPropertyRelative("actorSwitch");
        SerializedProperty accSwitch = property.FindPropertyRelative("accessoriesSwitch");
        #endregion

        SerializedProperty value = property.FindPropertyRelative("value");

        float fieldHeight = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

        Rect statsRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        Rect pos2Rect = new Rect(position.x, position.y + fieldHeight, position.width, EditorGUIUtility.singleLineHeight);
        Rect pos3Rect = new Rect(position.x, position.y + (fieldHeight * 2), position.width, EditorGUIUtility.singleLineHeight);
        Rect pos4Rect = new Rect(position.x, position.y + (fieldHeight * 3), position.width, EditorGUIUtility.singleLineHeight);
        Rect pos5Rect = new Rect(position.x, position.y + (fieldHeight * 4), position.width, EditorGUIUtility.singleLineHeight);

        //Début du dessin.
        EditorGUI.BeginProperty(position, label, property);

        //Place en premier l'enum pour connaitre si les info à afficher sont des stats ou un movement.
        EditorGUI.PropertyField(statsRect, statsTarget, new GUIContent("Stats / Move"));

        //Bool
        ETypeStatsTarget statsEnum = (ETypeStatsTarget)statsTarget.enumValueIndex;

        if (statsEnum == ETypeStatsTarget.Stats)
        {
            EditorGUI.PropertyField(pos2Rect, cost, new GUIContent("Price / Gain"));
            EditorGUI.PropertyField(pos3Rect, stats, new GUIContent("Energy / calm"));
            EditorGUI.PropertyField(pos4Rect,value, new GUIContent("Value"));
        }
        
        if (statsEnum == ETypeStatsTarget.Movement)
        {
            EditorGUI.PropertyField(pos2Rect, move, new GUIContent("What movement ?"));

            //Check si c'est un déplacemement normal ou alors un switch
            ETypeMove moveEnum = (ETypeMove)move.enumValueIndex;

            if (moveEnum == ETypeMove.SwitchWithAcc || moveEnum == ETypeMove.SwitchWithActor)
            {
                if (moveEnum == ETypeMove.SwitchWithActor)
                {
                    EditorGUI.PropertyField(pos4Rect, actorSwitch, new GUIContent("With what Actor ?"));
                }
                else if (moveEnum == ETypeMove.SwitchWithAcc)
                {
                    EditorGUI.PropertyField(pos4Rect, accSwitch, new GUIContent("With what Acc ?"));
                }

                EditorGUI.PropertyField(pos5Rect, value, new GUIContent("Value"));
            }
            else
            {
                EditorGUI.PropertyField(pos3Rect, tp, new GUIContent("Is tp ?"));
                EditorGUI.PropertyField(pos4Rect, value, new GUIContent("Value"));
            }
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        //Récupération des info.
        SerializedProperty statsTarget = property.FindPropertyRelative("whatStatsTarget");

        #region Stats
        SerializedProperty cost = property.FindPropertyRelative("whatCost");
        SerializedProperty stats = property.FindPropertyRelative("whatStats");
        #endregion

        #region Movement
        SerializedProperty move = property.FindPropertyRelative("whatMove");
        SerializedProperty tp = property.FindPropertyRelative("isTp");
        SerializedProperty actorSwitch = property.FindPropertyRelative("actorSwitch");
        #endregion

        SerializedProperty value = property.FindPropertyRelative("value");

        float statsTargetHeight = EditorGUI.GetPropertyHeight(statsTarget);

        float costHeight = EditorGUI.GetPropertyHeight(cost);
        float statsHeight = EditorGUI.GetPropertyHeight(stats);

        float moveHeight = EditorGUI.GetPropertyHeight(move);
        float tpHeight = EditorGUI.GetPropertyHeight(tp);
        float actorOrAccSwitchHeight = EditorGUI.GetPropertyHeight(actorSwitch);

        float valueHeight = EditorGUI.GetPropertyHeight(value);

        ETypeStatsTarget statsEnum = (ETypeStatsTarget)statsTarget.enumValueIndex;

        if (statsEnum == ETypeStatsTarget.Stats)
        {
            return EditorGUIUtility.singleLineHeight + statsTargetHeight + costHeight + statsHeight + valueHeight;
        }

        if (statsEnum == ETypeStatsTarget.Movement)
        {
            return EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + statsTargetHeight + moveHeight + tpHeight + actorOrAccSwitchHeight + valueHeight;
        }

        return EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
    }
}
