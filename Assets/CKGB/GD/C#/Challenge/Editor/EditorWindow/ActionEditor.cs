using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;


public class ActionEditor : EditorWindow
{
    [MenuItem("Eartsup/CKGB/Action Editor")]
    public static void OpenWindow()
    {
        ActionEditor wnd = GetWindow<ActionEditor>();
        wnd.titleContent = new GUIContent("Action Editor");
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // Import UXML
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/CKGB/GD/C#/Challenge/Editor/EditorWindow/ActionEditor.uxml");
        VisualElement labelFromUXML = visualTree.Instantiate();
        root.Add(labelFromUXML);

        // A stylesheet can be added to a VisualElement.
        // The style will be applied to the VisualElement and all of its children.
        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/CKGB/GD/C#/Challenge/Editor/EditorWindow/ActionEditor.uss");
        root.styleSheets.Add(styleSheet);
    }
}