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

    #region Self
    //Fonction qui renvoie la valeur d'energy.
    public int GetSelfPriceEnergy()
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
    public int GetSelfPriceCalm()
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
    public int GetSelfGainEnergy()
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
    public int GetSelfGainCalm()
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
    #endregion

    //Pour récupérer le texte pour la preview des stats.
    public string GetLogsPreview(C_Actor thisActor)
    {
        //Liste de string pour écrire le texte.
        List<string> listLogsPreview = new List<string>();
        string logsPreview = "";

        //Mise en place de 4 var de type string. 1 "SelfPriceEnergy" : 2 "SelfPriceCalm : 3 "SelfGainEnergy" : 4 "SelfGainCalm".
        string SelfPriceEnergy;
        string SelfPriceCalm;
        string SelfGainEnergy;
        string SelfGainCalm;

        //Check si pour le "Self" les variables ne sont pas égale à 0, si c'est le cas alors un system va modifier le text qui v s'afficher.

        #region Price string
        //Pour le prix.
        if (GetSelfPriceEnergy() != 0 || GetSelfPriceCalm() != 0)
        {
            //Si les deux possède un int supérieur à 0.
            if (GetSelfPriceEnergy() != 0 && GetSelfPriceCalm() != 0)
            {
                listLogsPreview.Add(thisActor.name + " va perdre " + GetSelfPriceCalm() + " de calme et " + GetSelfPriceEnergy() + " d'énergie.");
            }
            else if (GetSelfPriceCalm() != 0)
            {
                listLogsPreview.Add(thisActor.name + " va perdre " + GetSelfPriceCalm() + " de calme.");
            }
            else if (GetSelfPriceEnergy() != 0)
            {
                listLogsPreview.Add(thisActor.name + " va perdre " + GetSelfPriceEnergy() + " d'énergie.");
            }
        }
        #endregion

        #region Gain string
        //Pour le prix.
        if (GetSelfGainEnergy() != 0 || GetSelfGainCalm() != 0)
        {
            //Si les deux possède un int supérieur à 0.
            if (GetSelfGainEnergy() != 0 && GetSelfGainCalm() != 0)
            {
                listLogsPreview.Add(thisActor.name + " va gagner " + GetSelfGainCalm() + " de calme et " + GetSelfGainEnergy() + " d'énergie.");
            }
            else if (GetSelfGainCalm() != 0)
            {
                listLogsPreview.Add(thisActor.name + " va gagner " + GetSelfGainCalm() + " de calme.");
            }
            else if (GetSelfGainEnergy() != 0)
            {
                listLogsPreview.Add(thisActor.name + " va gagner " + GetSelfGainEnergy() + " d'énergie.");
            }
        }
        #endregion

        //Prépare le texte de la preview.
        foreach (var thisText in listLogsPreview)
        {
            logsPreview += thisText;
            logsPreview += "\n";
        }

        //Envoie le résultat.
        return logsPreview;
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