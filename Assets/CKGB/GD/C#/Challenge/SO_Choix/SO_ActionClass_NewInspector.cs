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
    public List<Interaction_NewInspector> listInteraction = new List<Interaction_NewInspector>();
    #endregion

    #region Résolution
    public string GetListLogs()
    {
        if (listLogsAction.Count == 0 && logsCursor == 0)
        {
            logsCursor++;

            return LogsMakeAction;
        }
        if (logsCursor >= listLogsAction.Count)
        {
            return null;
        }
        else
        {
            string logs = listLogsAction[logsCursor];

            logsCursor++;

            return logs;
        }
    }


    public void SetStatsTarget(Interaction_NewInspector.ETypeTarget target, C_Pion thisActor)
    {
        //Avoir à la place une detection de l'enum du si c'est un prix (-x) ou un gain (+x). Necessaire pour l'outil de preview.
        //New version
        if (thisActor.GetComponent<C_Actor>())
        {
            //Check si la liste n'est pas vide
            if (listInteraction.Count != 0)
            {
                //Fait toute la liste des cible.
                foreach (Interaction_NewInspector thisInteraction in listInteraction)
                {
                    //Check si c'est égale à "actorTarget".
                    if (thisInteraction.whatTarget == target)
                    {
                        //Applique à l'actor SEULEMENT LES STATS les stats.
                        foreach (TargetStats_NewInspector thisTargetStats in thisInteraction.listTargetStats)
                        {
                            int value;

                            if (thisTargetStats.dataStats.whatCost == Stats_NewInspector.ETypeCost.Price)
                            {
                                //Retourne une valeur négative.
                                value = -thisTargetStats.dataStats.value;
                            }
                            else
                            {
                                //Retourne une valeur positive.
                                value = thisTargetStats.dataStats.value;
                            }

                            //Envoie les nouvelles information a l'actor.
                            thisActor.GetComponent<C_Actor>().SetCurrentStats(value, thisTargetStats.dataStats.whatStats);
                        }
                    }
                }
            }
        }
    }
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

    public List<TargetStats_NewInspector> listTargetStats = new List<TargetStats_NewInspector>();
}

[Serializable]
public class TargetStats_NewInspector
{
    #region Data
    //Cible qu'on souhaite viser.
    public ETypeStatsTarget whatStatsTarget;
    public enum ETypeStatsTarget { Stats, Movement };
    #endregion

    //Pour les stats.
    public Stats_NewInspector dataStats;

    //Pour le mouvement.
    public Move_NewInspector dataMove;
}

[Serializable]
public class Stats_NewInspector
{
    //Pour les stats
    public ETypeCost whatCost;
    public enum ETypeCost { Price, Gain };

    public ETypeStats whatStats;
    public enum ETypeStats { Energy, Calm };

    public int value;
}

[Serializable]
public class Move_NewInspector
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