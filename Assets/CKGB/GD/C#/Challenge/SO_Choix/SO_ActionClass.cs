using System;
using System.Collections.Generic;
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

    [Header("Conditions")]
    public AdvancedCondition advancedCondition;

    [Header("List d'action")]
    public List<Interaction> listInteraction = new List<Interaction>();
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
}

#region Conditions avancé
[Serializable]
public class AdvancedCondition
{
    public bool advancedCondition = false;

    public bool needAcc = false; 
    public C_Accessories whatAcc;

    public bool canMakeByOneActor = false;
    public C_Actor whatActor;
}
#endregion

#region Interaction
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

    public List<TargetStats> listTargetStats = new List<TargetStats>();
}

[Serializable]
public class TargetStats
{
    #region Stats
    //Cible qu'on souhaite viser.
    public ETypeStatsTarget whatStatsTarget;
    public enum ETypeStatsTarget { Price, Gain, Movement };
    #endregion

    public List<Stats> listStats = new List<Stats>();
    public Move move;
}

[Serializable]
public class Stats
{
    public ETypeStats whatStats;
    public enum ETypeStats { Energy, Calm };

    public int value;
}

[Serializable]
public class Move
{
    public ETypeMove whatMove;
    public enum ETypeMove { None, Right, Left, OnTargetCase, SwitchWithActor, SwitchWithAcc };

    public bool isTp;

    public int nbMove;

    //Pour echanger de place.
    public C_Actor actor;
    public C_Accessories accessories;
}
#endregion