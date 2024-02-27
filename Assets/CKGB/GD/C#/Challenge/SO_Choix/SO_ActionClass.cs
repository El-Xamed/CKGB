using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Action", menuName = "ScriptableObjects/Challenge/Action", order = 1)]
public class SO_ActionClass : ScriptableObject
{
    #region Data
    #region Texte
    [Header("Text (Button)")]
    public string buttonText;

    [Header("Text (Logs)")]
    public string LogsCanMakeAction;
    public string LogsCantMakeAction;
    string currentLogs;
    #endregion

    #region Stats "PRIX" Self
    [Header("Stats Prix (Self)")]
    public int coutEnergy;
    public int coutCalm;
    #endregion

    #region Stats "GAIN" Self
    [Header("Stats Gain (Self)")]
    public int gainCalm;
    public int gainEnergy;
    #endregion

    #region Stats "PRIX" Other
    [Header("Stats Prix (Self)")]
    public int coutEnergyOther;
    public int coutCalmOther;
    #endregion

    #region Stats "GAIN" Other
    [Header("Stats Gain (Self)")]
    public int gainCalmOther;
    public int gainEnergyOther;
    #endregion

    #region Range
    [Header("Range (Prix/Gain)")]
    [SerializeField] int leftRangeStatsOther;
    [SerializeField] int rightRangeStatsOther;

    [Header("Range (Other Movement)")]
    [SerializeField] int leftRangeOtherMovement;
    [SerializeField] int rightRangeOtherMovement;
    #endregion

    [Header("Movement switch")]
    [SerializeField] bool swithWithAcc;


    #region Movement
    [Header("Mouvement (Self)")]
    [SerializeField] int moveRightSelf;
    [SerializeField] int moveLeftSelf;
    [SerializeField] int caseToGo = -1;

    [Header("Mouvement (Other)")]
    [SerializeField] bool moveOther = false;
    [SerializeField] int moveRightOther;
    [SerializeField] int moveLeftOther;
    #endregion

    #region Sous action
    [Header("Sous action")]
    public SO_ActionClass nextMiniStep;
    #endregion

    #region Acc
    [Header("Acc")]
    [SerializeField] bool needAcc;
    [SerializeField] C_Accessories acc;

    [Header("Actor")]
    [SerializeField] bool needActor;
    [SerializeField] C_Actor actor;
    #endregion
    #endregion

    #region Fonctions
    public void UseAction(C_Actor thisActor, List<C_Case> listCase)
    {
        Debug.Log("Use this actionClass.");

        //Stocker le perso dans le challenge + la case pour la phase de reso.
        Debug.Log(thisActor.name);
    }

    public void UseAction(C_Actor thisActor, List<C_Case> listCase, List<C_Actor> myTeam)
    {
        Debug.Log("Use this actionClass.");

        //Stats
        StatsOtherActor(thisActor, coutCalmOther, coutEnergyOther, gainCalmOther, gainEnergyOther, myTeam);
        //Apllique les stats sur les autres actor.
        StatsSelfActor(thisActor, coutCalm, coutEnergy, gainCalm, gainEnergy);

        if (!swithWithAcc)
        {
            //Movement
            if (moveOther)
            {
                MoveOtherActor(thisActor, listCase, myTeam);
            }
            
            MoveSelfActor(thisActor, moveRightSelf, moveLeftSelf, caseToGo, listCase);
        }
    }

    #region Stats
    void StatsSelfActor(C_Actor thisActor, int coutCalm, int coutEnergy, int gainCalm, int gainEnergy)
    {
        thisActor.TakeDamage(coutCalm, coutEnergy);
        thisActor.TakeHeal(gainCalm, gainEnergy);

        //Check si le joueur est encore en vie.
        thisActor.CheckIsOut();
    }

    void StatsOtherActor(C_Actor myActor, int coutCalm, int coutEnergy, int gainCalm, int gainEnergy, List<C_Actor> myTeam)
    {
        //Check si sur la droite de l'actor, dans la range il peut déplacer les autres actor.
        foreach (var thisActor in myTeam)
        {
            for (var i = 0; i < rightRangeStatsOther; i++)
            {
                if (thisActor.GetPosition() == myActor.GetPosition() + i)
                {
                    StatsSelfActor(thisActor, coutCalm, coutEnergy , gainCalm, gainEnergy);
                }
            }

            for (var i = 0; i < leftRangeStatsOther; i++)
            {
                if (thisActor.GetPosition() == myActor.GetPosition() - i)
                {
                    StatsSelfActor(thisActor, coutCalm, coutEnergy, gainCalm, gainEnergy);
                }
            }
        }
    }
    #endregion

    #region Movement
    //Déplace l'actor
    void MoveSelfActor(C_Actor myActor, int moveRight ,int moveLeft, int moveToGo,List<C_Case> listCase)
    {
        //Déplacement sur la droite.
        //Si il est au bord et que le déplacement va plus loin que la liste.
        if (myActor.GetPosition() + moveRight > listCase.Count - 1 && moveRight != 0)
        {
            myActor.transform.parent = listCase[myActor.GetPosition() - (listCase.Count -1) - moveRight].transform;
            myActor.GetComponent<RectTransform>().localPosition = new Vector3(0, myActor.GetComponent<RectTransform>().localPosition.y, 0);
            myActor.SetPosition(myActor.GetPosition() - (listCase.Count - 1) - moveRight);
        }
        else if (moveRight != 0)
        {
            Debug.Log(myActor.name);
            myActor.transform.parent = listCase[myActor.GetPosition() + moveRight].transform;
            myActor.GetComponent<RectTransform>().localPosition = new Vector3(0, myActor.GetComponent<RectTransform>().localPosition.y, 0);
            myActor.SetPosition(myActor.GetPosition() + moveRight);
        }
        //Déplacement sur la gauche.
        //Si il est au bord et que le déplacement va moins loin que la liste.
        if (myActor.GetPosition() - moveLeftSelf < 0 && moveLeft != 0)
        {
            myActor.transform.parent = listCase[(myActor.GetPosition() - moveLeft) - listCase.Count -1].transform;
            myActor.GetComponent<RectTransform>().localPosition = new Vector3(0, myActor.GetComponent<RectTransform>().localPosition.y, 0);
            myActor.SetPosition((myActor.GetPosition() - moveLeft) - listCase.Count - 1);
        }
        else if (moveLeft != 0)
        {
            myActor.transform.parent = listCase[myActor.GetPosition() - moveLeft].transform;
            myActor.GetComponent<RectTransform>().localPosition = new Vector3(0, myActor.GetComponent<RectTransform>().localPosition.y, 0);
            myActor.SetPosition(myActor.GetPosition() - moveLeft);
        }
        if (moveToGo > -1)
        {
            myActor.transform.parent = listCase[caseToGo].transform;
            myActor.GetComponent<RectTransform>().localPosition = new Vector3(0, myActor.GetComponent<RectTransform>().localPosition.y, 0);
            myActor.SetPosition(moveToGo);
        }
    }

    //Déplace les autres actor.
    void MoveOtherActor(C_Actor myActor, List<C_Case> listCase, List<C_Actor> myTeam)
    {
        //Check si sur la droite de l'actor, dans la range il peut déplacer les autres actor.
        foreach (var thisActor in myTeam)
        {
            for (var i = 0; i < rightRangeOtherMovement; i++)
            {
                if (thisActor.GetPosition() == myActor.GetPosition() + i)
                {
                    MoveSelfActor(thisActor, moveRightOther, 0, -1, listCase);
                }
            }

            for (var i = 0; i < leftRangeOtherMovement; i++)
            {
                if (thisActor.GetPosition() == myActor.GetPosition() - i)
                {
                    MoveSelfActor(thisActor, 0, moveLeftOther, -1, listCase);
                }
            }
        }
    }
    #endregion

    //vérifie la condition si l'action fonctionne
    public bool CanUse(C_Actor thisActor)
    {
        //Si il possède assez d'énergie.
        if (thisActor.GetcurrentEnergy() >= coutEnergy)
        {
            //Update les logs
            currentLogs = LogsCanMakeAction;
            return true;
        }

        //Si il a besoin d'etre sur l'acc.
        if (needAcc && IsOnAccessory(thisActor))
        {
            //Update les logs
            currentLogs = LogsCanMakeAction;
            return true;
        }

        //Si il a besoin d'etre sur l'acc.
        if (needActor && IsOnAccessory(thisActor))
        {
            //Update les logs
            currentLogs = LogsCanMakeAction;
            return true;
        }

        currentLogs = LogsCantMakeAction;
        return false;
    }

    //vérifie la présence d'un accessoire
    bool IsOnAccessory(C_Actor thisActor)
    {
        if (acc != null)
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
        else
        {
            Debug.Log("Pas d'Acc !");
            return false;
        }
    }

    //vérifie si il a besoin d'etre executé par un actor en particulier.
    bool MakeByThisActor(C_Actor thisActor)
    {
        if (actor != null)
        {
            //Si l'action est bien executé par le bon actor.
            if (actor == thisActor)
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
            Debug.Log("Pas d'Acc !");
            return false;
        }
    }

    //pour changer de mini étape
    void UpdateMiniActionClass()
    {

    }
    #endregion

    public string GetLogsChallenge()
    {
        return currentLogs;
    }

    public bool GetSwitchWithAcc()
    {
        return swithWithAcc;
    }
}
