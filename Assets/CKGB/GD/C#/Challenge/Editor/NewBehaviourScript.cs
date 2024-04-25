using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SO_ActionClass))]
public class NewBehaviourScript : Editor
{
    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Convert"))
        {
            SO_ActionClass item = (SO_ActionClass)target;
            item.Convert();

            EditorUtility.SetDirty(item);

            AssetDatabase.SaveAssets();
        }

        base.OnInspectorGUI();
    }
}
