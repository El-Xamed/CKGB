using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using UnityEngine;
using static TargetStats;

[CreateAssetMenu(fileName = "New Action", menuName = "ScriptableObjects/Challenge/Action", order = 1)]
public class SO_ActionClass : ScriptableObject
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
    public AdvancedCondition advancedCondition;

    [Header("Next Action")]
    public SO_ActionClass nextAction;

    [Header("List d'action")]
    [HideInInspector] public List<Interaction> listInteraction = new List<Interaction>();
    public List<Interaction_NewInspector> newListInteractions;
    #endregion

    #region Récupération de stats
    //Fonction qui renvoie la valeur d'un des stats.
    public int GetValue(Interaction_NewInspector.ETypeTarget actorTarget, TargetStats_NewInspector.ETypeStatsTarget targetStats)
    {
        //Pour toutes les liste d'action.
        foreach (Interaction_NewInspector thisInteraction in newListInteractions)
        {
            //Check si sont enum est égale à "actorTarget".
            if (thisInteraction.whatTarget == actorTarget)
            {
                //Pour toutes les list de TargetStats.
                foreach (TargetStats_NewInspector thisTargetStats in thisInteraction.listTargetStats)
                {
                    //Check si sont enum est égale à "statsTarget".
                    if (thisTargetStats.whatStatsTarget == targetStats)
                    {
                        if (thisTargetStats.whatCost == TargetStats_NewInspector.ETypeCost.Price)
                        {
                            return -thisTargetStats.value;
                        }
                        else if (thisTargetStats.whatCost == TargetStats_NewInspector.ETypeCost.Gain)
                        {
                            return thisTargetStats.value;
                        }

                    }
                }
            }
        }

        return 0;
    }

    public TargetStats_NewInspector.ETypeMove GetWhatMove(Interaction_NewInspector.ETypeTarget actorTarget)
    {
        //Pour toutes les liste d'action.
        foreach (Interaction_NewInspector thisInteraction in newListInteractions)
        {
            //Check si sont enum est égale à "actorTarget".
            if (thisInteraction.whatTarget == actorTarget)
            {
                //Pour toutes les list de TargetStats.
                foreach (TargetStats_NewInspector thisTargetStats in thisInteraction.listTargetStats)
                {
                    return thisTargetStats.whatMove;
                }
            }
        }

        return 0;
    }

    public bool GetIsTp(Interaction_NewInspector.ETypeTarget actorTarget)
    {
        //Pour toutes les liste d'action.
        foreach (Interaction_NewInspector thisInteraction in newListInteractions)
        {
            //Check si sont enum est égale à "actorTarget".
            if (thisInteraction.whatTarget == actorTarget)
            {
                //Pour toutes les list de TargetStats.
                foreach (TargetStats_NewInspector thisTargetStats in thisInteraction.listTargetStats)
                {
                    return thisTargetStats.isTp;
                }
            }
        }

        return false;
    }

    #region Other
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

    //Fonction qui renvoie le parametre de mouvement.
    public Interaction_NewInspector.ETypeDirectionTarget GetTypeDirectionRange()
    {
        //Pour toutes les liste d'action.
        foreach (Interaction_NewInspector thisInteraction in newListInteractions)
        {
            //Check si sont enum est égale à "other".
            if (thisInteraction.whatTarget == Interaction_NewInspector.ETypeTarget.Other)
            {
                return thisInteraction.whatDirectionTarget;
            }
        }

        Debug.Log("ATTENTION : Cette action ne possède pas de gain de calme ! La valeur renvoyé sera de 0.");

        return 0;
    }
    #endregion

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

        /*Retravaille la condition de si l'actor possède l'energie pour faire l'action.
        if (thisActor.GetcurrentEnergy() < GetValue(Interaction_NewInspector.ETypeTarget.Self, TargetStats_NewInspector.ETypeStatsTarget.Stats) && GetValue(Interaction.ETypeTarget.Self, TargetStats.ETypeStatsTarget.Price, Stats.ETypeStats.Energy) != 0)
        {
            return false;
        }

        return true;*/

        return true;
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
                foreach (Interaction_NewInspector thisInteraction in newListInteractions)
                {
                    //Check si c'est égale à "actorTarget".
                    if (thisInteraction.whatTarget == target)
                    {
                        //Applique à l'actor SEULEMENT LES STATS les stats.
                        foreach (TargetStats_NewInspector thisTargetStats in thisInteraction.listTargetStats)
                        {
                            if (thisTargetStats.whatStatsTarget != TargetStats_NewInspector.ETypeStatsTarget.Movement)
                            {
                                int value;

                                if (thisTargetStats.whatCost == TargetStats_NewInspector.ETypeCost.Price)
                                {
                                    //Retourne une valeur négative.
                                    value = -thisTargetStats.value;
                                }
                                else
                                {
                                    //Retourne une valeur positive.
                                    value = thisTargetStats.value;
                                }

                                //Envoie les nouvelles information a l'actor.
                                thisActor.GetComponent<C_Actor>().SetCurrentStats(value, thisTargetStats.whatStats);
                            }
                        }
                    }
                }
            }
        }
    }
    #endregion

    public void Convert()
    {
        //Création d'un nouvelle liste qui va remplacer l'ancien interface.
        newListInteractions = new List<Interaction_NewInspector>();

        //Pour toutes les interaction dans l'ancien interface.
        foreach (Interaction item in listInteraction)
        {
            Interaction_NewInspector newInteraction_NewInspector = new Interaction_NewInspector();

            //Convertisseur pour la liste 1.

            #region Target
            //Pour Convertir la Target.
            switch (item.whatTarget)
            {
                case Interaction.ETypeTarget.Self:
                    newInteraction_NewInspector.whatTarget = Interaction_NewInspector.ETypeTarget.Self;
                    break;
                case Interaction.ETypeTarget.Other:
                    newInteraction_NewInspector.whatTarget = Interaction_NewInspector.ETypeTarget.Other;
                    break;
            }
            #endregion

            newInteraction_NewInspector.selectTarget = item.selectTarget;

            #region whatTypeTarget
            //Pour Convertir le type de cible.
            switch (item.whatTypeTarget)
            {
                case Interaction.EType.Actor:
                    newInteraction_NewInspector.whatTypeTarget = Interaction_NewInspector.EType.Actor;
                    break;
                case Interaction.EType.Acc:
                    newInteraction_NewInspector.whatTypeTarget = Interaction_NewInspector.EType.Acc;
                    break;
            }
            #endregion

            newInteraction_NewInspector.target = item.target;

            #region whatDirectionTarget
            //Pour Convertir la direction.
            switch (item.whatDirectionTarget)
            {
                case Interaction.ETypeDirectionTarget.Right:
                    newInteraction_NewInspector.whatDirectionTarget = Interaction_NewInspector.ETypeDirectionTarget.Right;
                    break;
                case Interaction.ETypeDirectionTarget.Left:
                    newInteraction_NewInspector.whatDirectionTarget = Interaction_NewInspector.ETypeDirectionTarget.Left;
                    break;
                case Interaction.ETypeDirectionTarget.RightAndLeft:
                    newInteraction_NewInspector.whatDirectionTarget = Interaction_NewInspector.ETypeDirectionTarget.RightAndLeft;
                    break;
            }
            #endregion

            newInteraction_NewInspector.range = item.range;


            //Check dans l'ancienne version la liste de stats.
            foreach (TargetStats thisListTargetStats in item.listTargetStats)
            {
                //On récupère si c'est un prix ou un gain
                //Check si c'est pas un prix.
                if (thisListTargetStats.whatStatsTarget == ETypeStatsTarget.Price)
                {
                    //Regarde dans la liste si la TargetStats est égale au prix pour ajouter dans la nouvelle version.
                    foreach (Stats thisStats in thisListTargetStats.listStats)
                    {
                        if (thisStats.whatStats == Stats.ETypeStats.Energy)
                        {
                            ConvertForTargetStats(TargetStats_NewInspector.ETypeStatsTarget.Stats, TargetStats_NewInspector.ETypeCost.Price, TargetStats_NewInspector.ETypeStats.Energy, thisStats.value);
                        }
                        else if (thisStats.whatStats == Stats.ETypeStats.Calm)
                        {
                            ConvertForTargetStats(TargetStats_NewInspector.ETypeStatsTarget.Stats, TargetStats_NewInspector.ETypeCost.Price, TargetStats_NewInspector.ETypeStats.Calm, thisStats.value);
                        }
                    }
                }
                else if (thisListTargetStats.whatStatsTarget == ETypeStatsTarget.Gain) //Check si c'est pas un gain.
                {
                    //Regarde dans la liste si la TargetStats est égale au prix pour ajouter dans la nouvelle version.
                    foreach (Stats thisStats in thisListTargetStats.listStats)
                    {
                        if (thisStats.whatStats == Stats.ETypeStats.Energy)
                        {
                            ConvertForTargetStats(TargetStats_NewInspector.ETypeStatsTarget.Stats, TargetStats_NewInspector.ETypeCost.Gain, TargetStats_NewInspector.ETypeStats.Energy, thisStats.value);
                        }
                        else if (thisStats.whatStats == Stats.ETypeStats.Calm)
                        {
                            ConvertForTargetStats(TargetStats_NewInspector.ETypeStatsTarget.Stats, TargetStats_NewInspector.ETypeCost.Gain, TargetStats_NewInspector.ETypeStats.Calm, thisStats.value);
                        }
                    }
                }
                else if (thisListTargetStats.whatStatsTarget == ETypeStatsTarget.Movement) //Si c'est un mouvement alors il donne les info de mouvement.
                {
                    //Création d'une nouvelle "TargetStats_NewInspector" avec les info de price/gain + energy/calm à entrer.
                    TargetStats_NewInspector newListTargetStats = new TargetStats_NewInspector();

                    //Récupère l'info que c'est une stats.
                    newListTargetStats.whatStatsTarget = TargetStats_NewInspector.ETypeStatsTarget.Movement;

                    #region whatMove
                    //Pour Convertir la direction.
                    switch (thisListTargetStats.move.whatMove)
                    {
                        case Move.ETypeMove.Right:
                            newListTargetStats.whatMove = TargetStats_NewInspector.ETypeMove.Right;
                            break;
                        case Move.ETypeMove.Left:
                            newListTargetStats.whatMove = TargetStats_NewInspector.ETypeMove.Left;
                            break;
                        case Move.ETypeMove.OnTargetCase:
                            newListTargetStats.whatMove = TargetStats_NewInspector.ETypeMove.OnTargetCase;
                            break;
                        case Move.ETypeMove.SwitchWithActor:
                            newListTargetStats.whatMove = TargetStats_NewInspector.ETypeMove.SwitchWithActor;
                            break;
                        case Move.ETypeMove.SwitchWithAcc:
                            newListTargetStats.whatMove = TargetStats_NewInspector.ETypeMove.SwitchWithAcc;
                            break;
                    }
                    #endregion

                    //Récupère l'info sur quelle stats ça va se jouer.
                    newListTargetStats.isTp = thisListTargetStats.move.isTp;
                    newListTargetStats.value = thisListTargetStats.move.nbMove;
                    newListTargetStats.actorSwitch = thisListTargetStats.move.actor;
                    newListTargetStats.accessoriesSwitch = thisListTargetStats.move.accessories;

                    //Ajoute "newListTargetStats" dans la nouvelle version.
                    newInteraction_NewInspector.listTargetStats.Add(newListTargetStats);
                }

                void ConvertForTargetStats(TargetStats_NewInspector.ETypeStatsTarget whatTypeStats, TargetStats_NewInspector.ETypeCost whatCost, TargetStats_NewInspector.ETypeStats whatStats, int valeur)
                {
                    //Création d'une nouvelle "TargetStats_NewInspector" avec les info de price/gain + energy/calm à entrer.
                    TargetStats_NewInspector newListTargetStats = new TargetStats_NewInspector();
                    //Récupère l'info que c'est une stats.
                    newListTargetStats.whatStatsTarget = whatTypeStats;

                    //Si c'est une stats lance alors le script pour setup les stats
                    if (whatTypeStats == TargetStats_NewInspector.ETypeStatsTarget.Stats)
                    {
                        //Création d'une nouvelle "Stats_NewInspector" avec les info de price/gain + energy/calm à entrer pour ajouter dans le nouveau "TargetStats_NewInspector" pour pas qu'il soit null.
                        TargetStats_NewInspector newStats_NewInspector = new TargetStats_NewInspector();
                        //Récupère l'info que c'est un prix.
                        newStats_NewInspector.whatCost = whatCost;
                        //Récupère l'info sur quelle stats ça va se jouer.
                        newStats_NewInspector.whatStats = whatStats;

                        //Récupère l'info de la valeur.
                        newStats_NewInspector.value = valeur;

                        //Ajouter "newStats_NewInspector" dans "TargetStats_NewInspector".
                        newListTargetStats = newStats_NewInspector;
                        //Ajoute "newListTargetStats" dans la nouvelle version.
                        newInteraction_NewInspector.listTargetStats.Add(newListTargetStats);
                    }
                }
            }

            //newInteraction_NewInspector.listTargetStats = newListTargetStats;

            //Ajoute à la nouvelle liste d'interaction.
            newListInteractions.Add(newInteraction_NewInspector);
        }
    }

    #region Partage de données
    public void SetChallengeData(C_Challenge thisChallenge)
    {
        challenge = thisChallenge;
    }
    #endregion
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