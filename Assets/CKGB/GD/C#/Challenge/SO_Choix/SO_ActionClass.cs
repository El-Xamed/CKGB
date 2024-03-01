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
    #endregion

    #region Fonctions
    public void UseAction(C_Actor thisActor, List<C_Case> listCase, List<C_Actor> myTeam)
    {
        Debug.Log("Use this actionClass : " + buttonText);
    }

    //v�rifie la condition si l'action fonctionne
    public bool CanUse(C_Actor thisActor)
    {
        currentLogs = LogsCantMakeAction;
        return false;
    }

    //pour changer de mini �tape
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
    public ETypeTarget whatTarget;
    public enum ETypeTarget { Self, Other };
    #endregion

    public ETypeDirectionTarget whatDirectionTarget;
    public enum ETypeDirectionTarget {None, Right, Left, RightAndLeft };
    public int range;

    public List<Stats> listStats;
    public List<StatsOther> listStatsOther;
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
    public Move listMove;
}

[Serializable]
public class StatsOther
{
    #region Stats
    //Cible qu'on souhaite viser.
    public ETypeStatsTarget whatStatsTarget;
    public enum ETypeStatsTarget { Price, Gain, Movement };
    #endregion

    public List<Price> listPrice;
    public List<Gain> listGain;
    public List<Move> listMove;
}

[Serializable]
public class Price
{
    public ETypePrice whatPrice;
    public enum ETypePrice { None, Energy, Calm };

    public int price;
}

[Serializable]
public class Gain
{
    public ETypeGain whatGain;
    public enum ETypeGain { None, Energy, Calm };

    public int gain;
}

[Serializable]
public class Move
{
    public ETypeMove whatMove;
    public enum ETypeMove { None, Right, Left, SwitchWithActor, SwitchWithAcc };

    public int nbMove;

    //Pour echanger de place.
    public C_Actor actor;
    public C_Accessories accessories;
}