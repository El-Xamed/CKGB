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
    [SerializeField] List<Interaction> listInteraction;
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
    #endregion

    public string GetLogsChallenge()
    {
        return currentLogs;
    }

    public List<Interaction> GetInteraction() { return listInteraction; }
}

[Serializable]
public class Interaction
{
    #region Cible
    //Cible qu'on souhaite viser.
    [Header ("Target")]
    public ETypeTarget whatTarget;
    public enum ETypeTarget { None, Self, Other };
    #endregion
    public string test;
    #region Stats
    //Pour impacter les stats de la cible.
    [Header ("Stats")]
    public List<Price> listPrice;
    public List<Gain> listGain;
    #endregion

    #region Movement
    //Pour déplacer la cible.
    [Header ("Movement")]
    public List<Move> listMove;
    #endregion


    public class Price
    {
        [SerializeField] ETypePrice whatPrice;
        [Serializable] enum ETypePrice { None, Energy, Calm };

        [SerializeField] int price;
    }

    public class Gain
    {
        [SerializeField] ETypePrice whatGain;
        [Serializable] enum ETypePrice { None, Energy, Calm };

        [SerializeField] int gain;
    }

    public class Move
    {
        public ETypeInteraction whatMove;
        public enum ETypeInteraction { None, Right, Left , Switch};

        public int move;

        //Pour echanger de place.
        C_Actor actor;
        C_Accessories accessories;
    }

}
