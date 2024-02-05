using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class C_destination : MonoBehaviour
{
    #region variables
    [Header("WayPoints")]
    public C_destination up, left, right, down;
    public Transform[] upPath, leftPath, rightPath, downPath;

    [TextAreaAttribute]
    public string leveltext;
    
    



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
    string scenename;
   
    //Variable de scene
    #endregion
   public string sceneGet()
    {
        return scenename;
    }
}
