using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

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

    [Header("List d'action")]
    public List<Interaction> listInteraction;
    int nbInteraction;
    #endregion

    #region Fonctions
    public void UseAction(C_Actor thisActor, List<C_Case> listCase, List<C_Actor> myTeam)
    {
        Debug.Log("Use this actionClass : " + buttonText);
    }

    //vérifie la condition si l'action fonctionne
    public bool CanUse(C_Actor thisActor)
    {
        currentLogs = LogsCantMakeAction;
        return false;
    }

    //pour changer de mini étape
    void UpdateMiniActionClass()
    {

    }

    public void SetNbInteraction(int newInt)
    {
        if (newInt == 0)
        {
            nbInteraction = 0;
            return;
        }

        nbInteraction += newInt;
    }
    #endregion

    public string GetLogsChallenge()
    {
        return currentLogs;
    }

    public int GetNbInteraction()
    {
        return nbInteraction;
    }

    public List<Interaction> GetInteraction() { return listInteraction; }
}

[Serializable]
public class Interaction
{
    #region Cible
    //Cible qu'on souhaite viser.
    //[Header("Target")]
    public ETypeTarget whatTarget;
    public enum ETypeTarget { Self, Other };
    #endregion

    public List<Stats> listStats;
}

[Serializable]
public class Stats
{
    #region Stats
    //Cible qu'on souhaite viser.
    //[Header("Target")]
    public ETypeStatsTarget whatStatsTarget;
    public enum ETypeStatsTarget { Price, Gain, Movement };
    #endregion

    public List<Price> listPrice;
    public List<Gain> listGain;
    public List<Move> listMovement;

    [Serializable]
    public class Price
    {
        [SerializeField] ETypePrice whatPrice;
        [Serializable] enum ETypePrice { None, Energy, Calm };

        [SerializeField] int price;
    }

    [Serializable]
    public class Gain
    {
        [SerializeField] ETypePrice whatGain;
        [Serializable] enum ETypePrice { None, Energy, Calm };

        [SerializeField] int gain;
    }

    [Serializable]
    public class Move
    {
        public ETypeInteraction whatMove;
        public enum ETypeInteraction { None, Right, Left, Switch };

        public int move;

        //Pour echanger de place.
        C_Actor actor;
        C_Accessories accessories;
    }
}