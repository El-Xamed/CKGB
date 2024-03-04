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

    [Header("Conditions")]
    public AdvancedCondition advancedCondition;

    [Header("List d'action")]
    public List<Interaction> listInteraction;
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

    //Fonction qui renvoie la valeur d'energy.
    public int GetEnergy()
    {
        //Pour toutes les liste d'action.
        foreach (var thisInteraction in listInteraction)
        {
            //Check si sont enum est égale à "Self".
            if (thisInteraction.whatTarget == Interaction.ETypeTarget.Self)
            {
                //Pour toutes les list de stats.
                foreach (var thisStats in thisInteraction.listStats)
                {
                    //Check si sont enum est égale à "Price".
                    if (thisStats.whatStatsTarget == Stats.ETypeStatsTarget.Price)
                    {
                        //Pour toutes les list de prix.
                        foreach (var thisPrice in thisStats.listPrice)
                        {
                            //Check si sont enum est égale à "Energy".
                            if (thisPrice.whatPrice == Price.ETypePrice.Energy)
                            {
                                //Renvoie le prix de cette energy.
                                return thisPrice.price;
                            }
                        }
                    }
                }
            }
        }

        Debug.Log("ATTENTION : Cette action ne possède pas de prix d'énergie ! La valeur renvoyé sera de 0.");

        return 0;
    }

    //Fonction qui renvoie la valeur de calm.
    public int GetCalm()
    {
        //Pour toutes les liste d'action.
        foreach (var thisInteraction in listInteraction)
        {
            //Check si sont enum est égale à "Self".
            if (thisInteraction.whatTarget == Interaction.ETypeTarget.Self)
            {
                //Pour toutes les list de stats.
                foreach (var thisStats in thisInteraction.listStats)
                {
                    //Check si sont enum est égale à "Price".
                    if (thisStats.whatStatsTarget == Stats.ETypeStatsTarget.Price)
                    {
                        //Pour toutes les list de prix.
                        foreach (var thisPrice in thisStats.listPrice)
                        {
                            //Check si sont enum est égale à "Energy".
                            if (thisPrice.whatPrice == Price.ETypePrice.Calm)
                            {
                                //Renvoie le prix de cette energy.
                                return thisPrice.price;
                            }
                        }
                    }
                }
            }
        }

        Debug.Log("ATTENTION : Cette action ne possède pas de prix d'énergie ! La valeur renvoyé sera de 0.");

        return 0;
    }

    //Fonction qui renvoie la valeur d'energy.
    public int GetGainEnergy()
    {
        //Pour toutes les liste d'action.
        foreach (var thisInteraction in listInteraction)
        {
            //Check si sont enum est égale à "Self".
            if (thisInteraction.whatTarget == Interaction.ETypeTarget.Self)
            {
                //Pour toutes les list de stats.
                foreach (var thisStats in thisInteraction.listStats)
                {
                    //Check si sont enum est égale à "Gain".
                    if (thisStats.whatStatsTarget == Stats.ETypeStatsTarget.Gain)
                    {
                        //Pour toutes les list de gain.
                        foreach (var thisGain in thisStats.listGain)
                        {
                            //Check si sont enum est égale à "Energy".
                            if (thisGain.whatGain == Gain.ETypeGain.Energy)
                            {
                                //Renvoie le prix de cette energy.
                                return thisGain.gain;
                            }
                        }
                    }
                }
            }
        }

        Debug.Log("ATTENTION : Cette action ne possède pas de prix d'énergie ! La valeur renvoyé sera de 0.");

        return 0;
    }

    //Fonction qui renvoie la valeur de calm.
    public int GetGainCalm()
    {
        //Pour toutes les liste d'action.
        foreach (var thisInteraction in listInteraction)
        {
            //Check si sont enum est égale à "Self".
            if (thisInteraction.whatTarget == Interaction.ETypeTarget.Self)
            {
                //Pour toutes les list de stats.
                foreach (var thisStats in thisInteraction.listStats)
                {
                    //Check si sont enum est égale à "Price".
                    if (thisStats.whatStatsTarget == Stats.ETypeStatsTarget.Gain)
                    {
                        //Pour toutes les list de prix.
                        foreach (var thisGain in thisStats.listGain)
                        {
                            //Check si sont enum est égale à "Energy".
                            if (thisGain.whatGain == Gain.ETypeGain.Calm)
                            {
                                //Renvoie le prix de cette energy.
                                return thisGain.gain;
                            }
                        }
                    }
                }
            }
        }

        Debug.Log("ATTENTION : Cette action ne possède pas de prix d'énergie ! La valeur renvoyé sera de 0.");

        return 0;
    }

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
    public Move move;
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
#endregion