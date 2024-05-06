using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    public GameObject flag;
    public GameObject levelUI;
    public TMP_Text leveltextprovenance;
    public GameObject charactersToShow;

    [Header("Level UI")]
    [SerializeField] Text UiLevelName;

    [SerializeField]
    string scenename;

    [Header("Data")]
    [SerializeField] public SO_TempsMort dataTM;
    [SerializeField] public SO_Challenge dataC;
    //PAS SUR D'EN AVOIR BESOIN.
    [SerializeField] GameObject actor;

    //Variable de scene
    #endregion
    public void Start()
    {
        leveltextprovenance.text = UiLevelName.text;
        levelUI.GetComponent<levelUI>().leveltext.text = "???";
    }
    public string sceneGet()
    {
        return scenename;
    }

    #region Partage de donné
    public SO_TempsMort GetDataTempsMort()
    {
        return dataTM;
    }

    public SO_Challenge GetDataChallenge()
    {
        return dataC;
    }
    #endregion
}
