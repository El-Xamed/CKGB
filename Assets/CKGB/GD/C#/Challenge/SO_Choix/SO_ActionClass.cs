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
    public string LogsMakeAction;
    public string logsCantMakeAction;

    public List<string> listLogsAction;
    public List<string> listLogsCantMakeAction;

    [HideInInspector] public List<string> listCurrentLogs;
    [HideInInspector] public string currentLogs;

    [HideInInspector] public int logsCursor;
    #endregion

    [Header("Conditions")]
    public AdvancedCondition advancedCondition;

    [Header("Next Action")]
    public SO_ActionClass nextAction;

    [Header("List d'action")]
    public List<Interaction> listInteraction = new List<Interaction>();
    #endregion

    #region Récupération de stats
    //Fonction qui renvoie la valeur d'un des stats. RETIRER "TARGET STATS" CAR IL EST INUTILE.
    public int GetValue(Interaction.ETypeTarget actorTarget, TargetStats.ETypeStatsTarget targetStats)
    {
        //Pour toutes les liste d'action.
        foreach (Interaction thisInteraction in listInteraction)
        {
            //Check si sont enum est égale à "actorTarget".
            if (thisInteraction.whatTarget == actorTarget)
            {
                //Pour toutes les list de TargetStats.
                foreach (TargetStats thisTargetStats in thisInteraction.listTargetStats)
                {
                    if (targetStats == TargetStats.ETypeStatsTarget.Stats)
                    {
                        if (thisTargetStats.whatCost == TargetStats.ETypeCost.Price)
                        {
                            return -thisTargetStats.value;
                        }
                        else if (thisTargetStats.whatCost == TargetStats.ETypeCost.Gain)
                        {
                            return thisTargetStats.value;
                        }
                    }
                    else if (targetStats == TargetStats.ETypeStatsTarget.Movement)
                    {
                        return thisTargetStats.value;
                    }
                }
            }
        }

        return 0;
    }

    public TargetStats.ETypeMove GetWhatMove(Interaction.ETypeTarget actorTarget)
    {
        //Pour toutes les liste d'action.
        foreach (Interaction thisInteraction in listInteraction)
        {
            //Check si sont enum est égale à "actorTarget".
            if (thisInteraction.whatTarget == actorTarget)
            {
                //Pour toutes les list de TargetStats.
                foreach (TargetStats thisTargetStats in thisInteraction.listTargetStats)
                {
                    return thisTargetStats.whatMove;
                }
            }
        }

        return 0;
    }

    public bool GetIsTp(Interaction.ETypeTarget actorTarget)
    {
        //Pour toutes les liste d'action.
        foreach (Interaction thisInteraction in listInteraction)
        {
            //Check si sont enum est égale à "actorTarget".
            if (thisInteraction.whatTarget == actorTarget)
            {
                //Pour toutes les list de TargetStats.
                foreach (TargetStats thisTargetStats in thisInteraction.listTargetStats)
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
    #endregion

    public bool GetIfSwitchOrNot()
    {
        //Pour toutes les liste d'action.
        foreach (Interaction thisInteraction in listInteraction)
        {
            //Check si sont enum est égale à "Other".
            if (thisInteraction.whatTarget == Interaction.ETypeTarget.Soi)
            {
                foreach (TargetStats thisTargetStats in thisInteraction.listTargetStats)
                {
                    if (thisTargetStats.whatStatsTarget == TargetStats.ETypeStatsTarget.Movement)
                    {
                        if (thisTargetStats.whatMove == TargetStats.ETypeMove.SwitchWithAcc)
                        {
                            //thisTargetStats. = GameObject.Find(GetSwitchGameObject().GetDataAcc().name).GetComponent<C_Accessories>();
                            return true;
                        }
                    }
                }
            }
        }

        return false;
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
                        return thisTargetStats.accessoriesSwitch;
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
        if (listCurrentLogs.Count == 0 && logsCursor == 0)
        {
            //Check si c'est pas null
            if (listCurrentLogs.Count != 0)
            {
                logsCursor++;

                currentLogs = listCurrentLogs[0];

                //Retourne le premier element de la liste.
                return currentLogs;
            }
            else
            {
                Debug.LogWarning("La liste est vide");
                return null;
            }
        }
        if (logsCursor > listCurrentLogs.Count - 1)
        {
            //Retourne une valeur null.
            return null;
        }
        else
        {
            currentLogs = listCurrentLogs[logsCursor];

            logsCursor++;

            //Retourne un element ciblé avant d'augmenter le curseur.
            return currentLogs;
        }
    }

    public void SetListLogs(bool canMakeAction)
    {
        if (canMakeAction)
        {
            listCurrentLogs = listLogsAction;
        }
        else
        {
            listCurrentLogs = listLogsCantMakeAction;
        }
    }

    public void ResetLogs()
    {
        logsCursor = 0;
    }

    public void SetStatsTarget(Interaction.ETypeTarget target, C_Pion thisActor)
    {
        //Avoir à la place une detection de l'enum du si c'est un prix (-x) ou un gain (+x). Necessaire pour l'outil de preview.
        if (thisActor.GetComponent<C_Actor>())
        {
            //Check si la liste n'est pas vide
            if (listInteraction.Count != 0)
            {
                //Fait toute la liste des cible.
                foreach (Interaction thisInteraction in listInteraction)
                {
                    //Check si c'est égale à "actorTarget".
                    if (thisInteraction.whatTarget == target)
                    {
                        //Applique à l'actor SEULEMENT LES STATS les stats.
                        foreach (TargetStats thisTargetStats in thisInteraction.listTargetStats)
                        {
                            if (thisTargetStats.whatStatsTarget != TargetStats.ETypeStatsTarget.Movement)
                            {
                                int value;

                                if (thisTargetStats.whatCost == TargetStats.ETypeCost.Price)
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
}

#region Conditions avancé
[Serializable]
public class AdvancedCondition
{
    public bool advancedCondition = false;

    public bool needTwoActor;

    public bool needAcc = false; 
    public C_Accessories whatAcc;

    public bool canMakeByOneActor = false;
    public SO_Character whatActor;
}
#endregion

#region Interaction
[Serializable]
public class Interaction
{
    #region Cible
    //Cible qu'on souhaite viser.
    public ETypeTarget whatTarget;
    public enum ETypeTarget { Soi, Other };

    //Pour un selection avancé des cibles.
    public bool selectTarget;
    public EType whatTypeTarget;
    public enum EType { None, Actor, Acc };
    public GameObject target;
    #endregion

    public ETypeDirectionTarget whatDirectionTarget;
    public enum ETypeDirectionTarget { None, Right, Left, RightAndLeft };
    public int range;

    public List<TargetStats> listTargetStats = new List<TargetStats>();
}

[Serializable]
public class TargetStats
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