using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Choix", menuName = "ScriptableObjects/Choix", order = 1)]
public class SO_ActionClass : ScriptableObject
{

    public Proto_SO_Character actor;
    public int coutEnergy;
    public int coutCalm;

    public bool moveRight;
    public bool moveLeft;
    public int caseToGo;

    public int leftRange;
    public int rightRange;
    C_Case[] caseTarget;

    public SO_ActionClass nextMiniStep;
    [SerializeField] Button actionButton;
    [SerializeField] string buttonText;

    

    private void Awake()
    {
        //actionButton.GetComponentInChildren<Text>().text = buttonText;
    }
    //v�rifie la condition si l'action fonctionne
    void FunctioningOrNot()
    {

    }

    //v�rifie la pr�sence d'un accessoire
    void IsOnAccessory()
    {

    }
    //pour changer de mini �tape
    void UpdateActionClass()
    {

    }
}
