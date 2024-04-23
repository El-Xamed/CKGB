using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using UnityEngine;
using static TargetStats;

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

    /*
    #region Récupération de stats
    //Fonction qui renvoie la valeur d'energy.
    public int GetStats(Interaction.ETypeTarget actorTarget, TargetStats.ETypeStatsTarget targetStats, Stats.ETypeStats statsTarget)
    {
        //Pour toutes les liste d'action.
        foreach (Interaction thisInteraction in listInteraction)
        {
            //Check si sont enum est égale à "actorTarget".
            if (thisInteraction.whatTarget == actorTarget)
            {
                //Pour toutes les list de stats.
                foreach (TargetStats thisTargetStats in thisInteraction.listTargetStats)
                {
                    //Check si sont enum est égale à "statsTarget".
                    if (thisTargetStats.whatStatsTarget == targetStats)
                    {
                        foreach (Stats thisStats in thisTargetStats.listStats)
                        {
                            if (thisStats.whatStats == statsTarget)
                            {
                                return thisStats.value;
                            }
                        }
                    }
                }
            }
        }

        //Debug.Log("ATTENTION : Cette action ne possède pas de prix " + statsTarget + " ! La valeur renvoyé sera de 0.");

        return 0;
    }

    public Move GetClassMove(Interaction.ETypeTarget target)
    {
        //Pour toutes les liste d'action.
        foreach (Interaction thisInteraction in listInteraction)
        {
            //Check si sont enum est égale à "target".
            if (thisInteraction.whatTarget == target)
            {
                //Pour toutes les list de stats.
                foreach (TargetStats thisStats in thisInteraction.listTargetStats)
                {
                    //Check si sont enum est égale à "Movement".
                    if (thisStats.whatStatsTarget == TargetStats.ETypeStatsTarget.Movement)
                    {
                        //Deplace l'actor.
                        return thisStats.move;
                    }
                }
            }
        }

        return null;
    }

    public int GetRange()
    {
        //Pour toutes les liste d'action.
        foreach (Interaction thisInteraction in listInteraction)
        {
            //Check si sont enum est égale à "Other".
            if (thisInteraction.whatTarget == Interaction.ETypeTarget.Other)
            {
                return thisInteraction.range;
            }
        }

        return 0;
    }

    public bool GetIfTargetOrNot()
    {
        //Pour toutes les liste d'action.
        foreach (Interaction thisInteraction in listInteraction)
        {
            //Check si sont enum est égale à "Other".
            if (thisInteraction.whatTarget == Interaction.ETypeTarget.Other)
            {
                if (thisInteraction.selectTarget)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public bool GetIfSwitchOrNot()
    {
        //Pour toutes les liste d'action.
        foreach (Interaction thisInteraction in listInteraction)
        {
            //Check si sont enum est égale à "Other".
            if (thisInteraction.whatTarget == Interaction.ETypeTarget.Self)
            {
                foreach (TargetStats thisTargetStats in thisInteraction.listTargetStats)
                {
                    if (thisTargetStats.whatStatsTarget == ETypeStatsTarget.Movement)
                    {
                        if (thisTargetStats.move.whatMove == Move.ETypeMove.SwitchWithAcc)
                        {
                            thisTargetStats.move.accessories = GameObject.Find(GetSwitchGameObject().GetDataAcc().name).GetComponent<C_Accessories>();
                            return true;
                        }
                    }
                }
            }
        }

        return false;
    }

    public C_Accessories GetSwitchGameObject()
    {
        //Pour toutes les liste d'action.
        foreach (Interaction thisInteraction in listInteraction)
        {
            //Check si sont enum est égale à "Other".
            if (thisInteraction.whatTarget == Interaction.ETypeTarget.Self)
            {
                foreach (TargetStats thisTargetStats in thisInteraction.listTargetStats)
                {
                    if (thisTargetStats.whatStatsTarget == ETypeStatsTarget.Movement)
                    {
                        if (thisTargetStats.move.whatMove == Move.ETypeMove.SwitchWithAcc)
                        {
                            return thisTargetStats.move.accessories;
                        }
                    }
                }
            }
        }

        return null;
    }

    public GameObject GetTarget()
    {
        //Pour toutes les liste d'action.
        foreach (Interaction thisInteraction in listInteraction)
        {
            //Check si sont enum est égale à "Other".
            if (thisInteraction.whatTarget == Interaction.ETypeTarget.Other)
            {
                if (thisInteraction.selectTarget)
                {
                    return thisInteraction.target;
                }
            }
        }

        return null;
    }

    public void SetTarget(GameObject thisTarget)
    {
        //Pour toutes les liste d'action.
        foreach (Interaction thisInteraction in listInteraction)
        {
            //Check si sont enum est égale à "Other".
            if (thisInteraction.whatTarget == Interaction.ETypeTarget.Other)
            {
                if (thisInteraction.selectTarget)
                {
                    thisInteraction.target = thisTarget;
                }
            }
        }
    }

    public C_Actor GetSwitchActor(Interaction.ETypeTarget actorTarget)
    {
        //Pour toutes les liste d'action.
        foreach (Interaction thisInteraction in listInteraction)
        {
            //Check si sont enum est égale à "actorTarget".
            if (thisInteraction.whatTarget == actorTarget)
            {
                //Pour toutes les list de stats.
                foreach (TargetStats thisTargetStats in thisInteraction.listTargetStats)
                {
                    //Check si sont enum est égale à "statsTarget".
                    if (thisTargetStats.whatStatsTarget == TargetStats.ETypeStatsTarget.Movement)
                    {
                        return thisTargetStats.move.actor;
                    }
                }
            }
        }

        Debug.Log("ATTENTION : Cette action ne possède de switch avec un autre actor !");

        return null;
    }

    public C_Accessories GetSwitchAcc(Interaction.ETypeTarget actorTarget)
    {
        //Pour toutes les liste d'action.
        foreach (Interaction thisInteraction in listInteraction)
        {
            //Check si sont enum est égale à "actorTarget".
            if (thisInteraction.whatTarget == actorTarget)
            {
                //Pour toutes les list de stats.
                foreach (TargetStats thisTargetStats in thisInteraction.listTargetStats)
                {
                    //Check si sont enum est égale à "statsTarget".
                    if (thisTargetStats.whatStatsTarget == TargetStats.ETypeStatsTarget.Movement)
                    {
                        return thisTargetStats.move.accessories;
                    }
                }
            }
        }

        Debug.Log("ATTENTION : Cette action ne possède de switch avec un autre actor !");

        return null;
    }

    //Fonction qui renvoie le parametre de mouvement.
    public Interaction.ETypeDirectionTarget GetTypeDirectionRange()
    {
        //Pour toutes les liste d'action.
        foreach (Interaction thisInteraction in listInteraction)
        {
            //Check si sont enum est égale à "other".
            if (thisInteraction.whatTarget == Interaction.ETypeTarget.Other)
            {
                return thisInteraction.whatDirectionTarget;
            }
        }

        Debug.Log("ATTENTION : Cette action ne possède pas de gain de calme ! La valeur renvoyé sera de 0.");

        return 0;
    }

    //Fonction qui renvoie le nombre de mouvement pour déplacer "other".
    public int GetMovement(Interaction.ETypeTarget actorTarget)
    {
        //Pour toutes les liste d'action.
        foreach (Interaction thisInteraction in listInteraction)
        {
            //Check si sont enum est égale à "actorTarget".
            if (thisInteraction.whatTarget == actorTarget)
            {
                //Pour toutes les list de stats.
                foreach (TargetStats thisStats in thisInteraction.listTargetStats)
                {
                    //Check si sont enum est égale à "Movement".
                    if (thisStats.whatStatsTarget == TargetStats.ETypeStatsTarget.Movement)
                    {
                        if (thisStats.move.whatMove == Move.ETypeMove.Right || thisStats.move.whatMove == Move.ETypeMove.Left || thisStats.move.whatMove == Move.ETypeMove.OnTargetCase)
                        {
                            return thisStats.move.nbMove;
                        }
                    }
                }
            }
        }

        Debug.Log("ATTENTION : Cette action ne possède pas de gain de calme ! La valeur renvoyé sera de 0.");

        return 0;
    }
    #endregion

    //Check si dans la config de cette action, des paramètre "other" existe.
    public bool CheckOtherInAction()
    {
        //Pour toutes les liste d'action.
        foreach (Interaction thisInteraction in listInteraction)
        {
            //Check si sont enum est égale à "Other".
            if (thisInteraction.whatTarget == Interaction.ETypeTarget.Other)
            {
                Debug.Log("D'autres actor peuvent etre impacté !");
                return true;
            }
        }

        Debug.Log("Aucun autre actor peuvent etre impacté !");
        return false;
    }

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

    public void ResetLogs()
    {
        logsCursor = 0;
    }

    //vérifie la condition si l'action fonctionne.
    public bool CanUse(C_Actor thisActor)
    {
        //AJOUTER LA CONDITION DES ACTIONS FAIT A 2.

        //Check si les codition bonus sont activé.
        if (advancedCondition.advancedCondition)
        {
            //Check si l'action doit etre fait par un actor en particulier + Si "whatActor" n'est pas null + si "whatActor" est égal à "thisActor".
            if (advancedCondition.canMakeByOneActor && advancedCondition.whatActor && advancedCondition.whatActor != thisActor)
            {
                return false;
            }

            //Check si l'action doit etre fait par un acc en particulier + Si "whatAcc" n'est pas null + si "whatAcc" est égal à "thisActor".
            if (advancedCondition.needAcc && advancedCondition.needAcc && advancedCondition.whatAcc.GetPosition() != thisActor.GetPosition())
            {
                return false;
            }
        }

        if (thisActor.GetcurrentEnergy() < GetStats(Interaction.ETypeTarget.Self, TargetStats.ETypeStatsTarget.Price, Stats.ETypeStats.Energy) && GetStats(Interaction.ETypeTarget.Self, TargetStats.ETypeStatsTarget.Price, Stats.ETypeStats.Energy) != 0)
        {
            return false;
        }

        return true;
    }

    public void SetStatsTarget(Interaction.ETypeTarget target, C_Pion thisActor)
    {
        //Check si pour le "target" les variables ne sont pas égale à 0, si c'est le cas alors un system va modifier le text qui va s'afficher.
        #region Price string
        //Pour le prix.
        //Check si il possède le component "C_Actor". A RETIRER PLUS TARD CAR C'EST DU PUTAIN DE BRICOLAGE DE MES COUILLES CAR  J'AI PAS LE TEMPS !
        if (thisActor.GetComponent<C_Actor>())
        {
            if (GetStats(target, TargetStats.ETypeStatsTarget.Price, Stats.ETypeStats.Energy) != 0 || GetStats(target, TargetStats.ETypeStatsTarget.Price, Stats.ETypeStats.Calm) != 0)
            {
                thisActor.GetComponent<C_Actor>().SetCurrentStatsPrice(GetStats(target, TargetStats.ETypeStatsTarget.Price, Stats.ETypeStats.Calm), GetStats(target, TargetStats.ETypeStatsTarget.Price, Stats.ETypeStats.Energy));
            }
        }
        #endregion

        #region Gain string
        //Pour le gain.
        //Check si il possède le component "C_Actor". A RETIRER PLUS TARD CAR C'EST DU PUTAIN DE BRICOLAGE DE MES COUILLES CAR  J'AI PAS LE TEMPS !
        if (thisActor.GetComponent<C_Actor>())
        {
            if (GetStats(target, TargetStats.ETypeStatsTarget.Gain, Stats.ETypeStats.Energy) != 0 || GetStats(target, TargetStats.ETypeStatsTarget.Gain, Stats.ETypeStats.Calm) != 0)
            {
                thisActor.GetComponent<C_Actor>().SetCurrentStatsGain(GetStats(target, TargetStats.ETypeStatsTarget.Gain, Stats.ETypeStats.Calm), GetStats(target, TargetStats.ETypeStatsTarget.Gain, Stats.ETypeStats.Energy));
            }
        }
        #endregion
    }
    #endregion

    #region Partage de données
    public void SetChallengeData(C_Challenge thisChallenge)
    {
        challenge = thisChallenge;
    }

    public bool HasMovement(Interaction.ETypeTarget target)
    {
        if (GetMovement(target) != 0)
        {
            return true;
        }

        return false;
    }
    #endregion
    */
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
    #region Stats
    //Cible qu'on souhaite viser.
    public ETypeStatsTarget whatStatsTarget;
    public enum ETypeStatsTarget { Stats, Movement };

    public ETypeStats whatStats;
    public enum ETypeStats { Energy, Calm };

    public int value;
    #endregion

    public Move_NewInspector move;
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