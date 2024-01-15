using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build.Content;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class C_destination : MonoBehaviour
{
    #region variables
    [Header("WayPoints")]
    public C_destination up, left, right, down;

    [Header("Options")]
    public int levelID = 0;


    [TextArea(1, 2)]

    [Header("type")]
    public bool IsLevel;
    public bool IsCorner;
    public bool Islocked;
    public bool IsDone;

    [Header("Level UI")]
    [SerializeField] Text UiLevelName;

    
    [SerializeField]
    GameObject actor;

    [SerializeField]
    public SceneAsset destination;

    [SerializeField]
    [Tooltip("Permet de setup dans la Worldmap la position initial des acteurs dans la scene.")]
    int[] initialPlayerPosition;
    //Variable de scene
    #endregion
}
