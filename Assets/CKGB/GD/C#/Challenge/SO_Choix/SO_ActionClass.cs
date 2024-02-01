using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Action", menuName = "ScriptableObjects/Challenge/Action", order = 1)]
public class SO_ActionClass : ScriptableObject
{
    #region Data
    #region Texte
    [Header("Text")]
    public string buttonText;
    public string LogsChallenge;
    #endregion

    #region Stats "PRIX"
    [Header("Stats Prix")]
    public int coutEnergy;
    public int coutCalm;
    #endregion

    #region Stats "GAIN"
    [Header("Stats Gain")]
    public int gainEnergy;
    public int gainCalm;
    #endregion

    #region Movement
    [Header("Mouvement")]
    [SerializeField] bool moveRight;
    [SerializeField] bool moveLeft;
    [SerializeField] int caseToGo = -1;
    #endregion

    #region Range
    [Header("Range")]
    [SerializeField] int leftRange;
    [SerializeField] int rightRange;
    #endregion

    #region Sous action
    [Header("Sous action")]
    public SO_ActionClass nextMiniStep;
    #endregion

    #region Acc
    [Header("Acc")]
    [SerializeField] bool needAcc;
    [SerializeField] SO_Accessories acc;
    #endregion
    #endregion

    #region Fonctions
    public void UseAction(C_Actor thisActor, List<C_Case> listCase)
    {
        //Check si cette action peut etre utilis�.
        if (CanUse(thisActor))
        {
            Debug.Log("Use this actionClass.");

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

    //D�place le personnage
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
        if (caseToGo > -1)
        {
            myActor.transform.parent = listCase[caseToGo].transform;
            myActor.SetPosition(caseToGo);
        }
    }

    //v�rifie la condition si l'action fonctionne
    bool CanUse(C_Actor thisActor)
    {
        //VOIR SI IL PEUT LANCER L'ACTION MEME SI L'ACTEUR NE POSSEDE PAS LES STATS EN QUESTIONS OU SI SEUL L'ENERGIE PEUT LANCER L'ACTION.

        //Si il a besoin d'etre sur l'acc.
        if (needAcc)
        {
            if (IsOnAccessory(thisActor))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return true;
        }
    }

    //v�rifie la pr�sence d'un accessoire
    bool IsOnAccessory(C_Actor thisActor)
    {
        //Si a besoin de l'acc + si l'acteur se trouve sur la meme case que l'acc.
        if (thisActor.GetPosition() == acc.currentPosition)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //pour changer de mini �tape
    void UpdateMiniActionClass()
    {

    }
    #endregion
}
