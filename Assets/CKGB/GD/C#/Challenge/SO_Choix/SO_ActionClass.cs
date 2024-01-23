using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Choix", menuName = "ScriptableObjects/Choix", order = 1)]
public class SO_ActionClass : ScriptableObject
{

    
    public int coutEnergy;
    public int coutCalm;

    public bool moveRight;
    public bool moveLeft;
    public int caseToGo;

    public int leftRange;
    public int rightRange;
    C_Case[] caseTarget;

    public SO_ActionClass nextMiniStep;
    [SerializeField] public Button actionButton;
    [SerializeField] string buttonText;
    [SerializeField]Test_challenge TC;

    

    private void Awake()
    {
        //actionButton.GetComponentInChildren<Text>().text = buttonText;
        TC = FindObjectOfType<Test_challenge>();
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
    void UpdateMiniActionClass()
    {

    }
    public void IsPossible()
    {
        if(actionButton==TC.currentStep.rightAnswer.actionButton)
        {
            TC.NextStep();
        }
     
    }
}
