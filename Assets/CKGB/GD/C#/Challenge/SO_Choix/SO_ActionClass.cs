using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Action", menuName = "ScriptableObjects/Challenge/Action", order = 1)]
public class SO_ActionClass : ScriptableObject
{
    //Test
    [Header("Text")]
    public string buttonText;

    [Header("Stats Prix")]
    public int coutEnergy;
    public int coutCalm;

    [Header("Stats Gain")]
    public int gainEnergy;
    public int gainCalm;

    [Header("Mouvement")]
    public bool moveRight;
    public bool moveLeft;
    public int caseToGo;

    [Header("Range")]
    public int leftRange;
    public int rightRange;
    C_Case[] caseTarget;

    [Header("Sous action")]
    public SO_ActionClass nextMiniStep;
    
    //vérifie la condition si l'action fonctionne
    void FunctioningOrNot()
    {

    }
    //vérifie la présence d'un accessoire
    void IsOnAccessory()
    {

    }
    //pour changer de mini étape
    void UpdateMiniActionClass()
    {

    }
}
