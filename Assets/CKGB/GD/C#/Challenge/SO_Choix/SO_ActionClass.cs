using System.Collections.Generic;
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
    [SerializeField] bool moveRight;
    [SerializeField] bool moveLeft;
    [SerializeField] int caseToGo;

    [Header("Range")]
    [SerializeField] int leftRange;
    [SerializeField] int rightRange;

    [Header("Sous action")]
    public SO_ActionClass nextMiniStep;

    [Header("Acc")]
    [SerializeField] bool needAcc;
    [SerializeField] SO_Accessories acc;
    
    public void UseAction(C_Actor thisActor, List<C_Case> listCase)
    {
        //Check si cette action peut etre utilisé.
        if (CanUse())
        {
            Debug.Log("Use this action.");

            //Stats
            thisActor.TakeDamage(coutCalm, coutEnergy);
            thisActor.TakeHeal(gainCalm, gainEnergy);

            //Movement
            MovePlayer(thisActor, listCase);

        }
        else
        {
            Debug.Log("Can't use this action.");
        }
    }

    //Déplace le personnage
    void MovePlayer(C_Actor myActor, List<C_Case> listCase)
    {
        if (moveRight)
        {
            myActor.transform.parent = listCase[myActor.GetPosition() + 1].transform;
            myActor.SetPosition(myActor.GetPosition() + 1);
        }
        if (moveLeft)
        {
            myActor.transform.parent = listCase[myActor.GetPosition() - 1].transform;
            myActor.SetPosition(myActor.GetPosition() - 1);
        }
        if (caseToGo > 0)
        {
            myActor.transform.parent = listCase[caseToGo].transform;
            myActor.SetPosition(caseToGo);
        }
    }

    //vérifie la condition si l'action fonctionne
    bool CanUse()
    {
        if (true)
        {
            return true;
        }
        else
        {
            return false;
        }
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
