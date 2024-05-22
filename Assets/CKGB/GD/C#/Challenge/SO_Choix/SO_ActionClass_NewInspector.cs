using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using static TargetStats;
using static UnityEngine.GraphicsBuffer;

[CreateAssetMenu(fileName = "New Action", menuName = "ScriptableObjects/Challenge/Action_NewInspector", order = 1)]
public class SO_ActionClass_NewInspector : ScriptableObject
{
    #region Data
    #region Texte
    C_Challenge challenge;

    [Header("Text (Button)")]
    public string buttonText;

    [Header("Text (Logs)")]
    public string LogsMakeAction;

    public string logsCantMakeAction;

    public List<string> listLogsAction;
    [HideInInspector] public int logsCursor;
    #endregion

    [Header("Conditions")]
    public AdvancedCondition_NewInspector advancedCondition;

    [Header("Next Action")]
    public SO_ActionClass nextAction;

    [Header("List d'action")]
    public List<Interaction> listInteraction = new List<Interaction>();
    #endregion

    #region Partage de données
    public void SetChallengeData(C_Challenge thisChallenge)
    {
        challenge = thisChallenge;
    }
    #endregion
    
}

#region Conditions avancé
[Serializable]
public class AdvancedCondition_NewInspector
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
public class Interaction_NewInspector
{
    #region Cible
    //Cible qu'on souhaite viser.
    public ETypeTarget whatTarget;
    public enum ETypeTarget { Self, Other };

    //Pour un selection avancé des cibles.
    public bool selectTarget;
    public EType whatTypeTarget;
    public enum EType { None, Actor, Acc };
    public GameObject target;
    #endregion
    
    public ETypeDirectionTarget whatDirectionTarget;
    public enum ETypeDirectionTarget {None, Right, Left, RightAndLeft };
    public int range;

    public List<TargetStats> listTargetStats = new List<TargetStats>();
}

[Serializable]
public class TargetStats_NewInspector
{
    #region Data
    //Cible qu'on souhaite viser.
    public ETypeStatsTarget whatStatsTarget;
    public enum ETypeStatsTarget { Stats, Movement };
    #endregion

    #region Stats
    //Pour les stats
    public ETypeCost whatCost;
    public enum ETypeCost { Price, Gain };

    public ETypeStats whatStats;
    public enum ETypeStats { Energy, Calm };
    #endregion

    #region Movement
    //pour le mouvement
    public ETypeMove whatMove = ETypeMove.None;
    public enum ETypeMove { None, Right, Left, OnTargetCase, SwitchWithActor, SwitchWithAcc };

    //Si c'est une tp ou non
    public bool isTp;

    //Pour echanger de place.
    public C_Actor actorSwitch;
    public C_Accessories accessoriesSwitch;
    #endregion

    public int value;
}
#endregion