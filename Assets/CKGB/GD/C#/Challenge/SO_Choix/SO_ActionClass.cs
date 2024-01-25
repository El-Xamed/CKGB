using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Action", menuName = "ScriptableObjects/Challenge/Action", order = 1)]
public class SO_ActionClass : ScriptableObject
{

    
    public int coutEnergy;
    public int coutCalm;

    public int gainEnergy;
    public int gainCalm;

    public bool moveRight;
    public bool moveLeft;
    public int caseToGo;

    public int leftRange;
    public int rightRange;
    public C_Case[] caseTarget;

    public SO_ActionClass nextMiniStep;
    //Stocker le prefab ailleur. DANS LE C_CHALLENGE.
    [SerializeField] public Button actionButton;
    [SerializeField] string buttonText;
    //INUTILE.
    [SerializeField]Test_challenge TC;

    //INUTILE.
    /*
    private void Awake()
    {
        //actionButton.GetComponentInChildren<Text>().text = buttonText;
        TC = FindObjectOfType<Test_challenge>();
    }*/

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

    //INUTILE.
    /*
    public void IsPossible()
    {
        if(actionButton==TC.currentStep.rightAnswer.actionButton)
        {
            TC.NextStep();
        }
     
    }*/
}
