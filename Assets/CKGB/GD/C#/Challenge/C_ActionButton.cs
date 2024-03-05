using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_ActionButton : MonoBehaviour
{
    [SerializeField]
    SO_ActionClass actionClass;
    List<Interaction> listInteraction;

    private void Awake()
    {
        listInteraction = actionClass.listInteraction;
    }

    #region Self
    //Fonction qui renvoie la valeur d'energy. POUR UNE RAISON INCONNU IL RECUPERE LES INFO DU SO_ACTIONCLASS PRECEDANT A VOIR POURQUOI. NOTE EN PLUS : QUAND JE SOUHAITE SEECTIONNER UNE ACTION, POUR UNE RAISON INCONNU IL RECUPERE TOUT LES LISTES DE STATS QUI SONT STOCKE DANS PLUSIEURS SO_ACTIONCLASS.
    //TESTER EN AJOUTANT UNE PRESCISION : LE BOUTON SELECTIONNER PAR L'EVENT SYSTEM. OU AVEC THIS.
    public int GetSelfPriceEnergy()
    {
        int price = 0;

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
                                Debug.Log(thisPrice.price);
                                //Renvoie le prix de cette energy.
                                //return thisPrice.price;
                                price = thisPrice.price;
                            }
                        }
                    }
                }
            }
            Debug.Log("Test1");
        }

        //Debug.Log("ATTENTION : Cette action ne possède pas de prix d'énergie ! La valeur renvoyé sera de 0.");
        Debug.Log(price);

        return price;
    }

    //Fonction qui renvoie la valeur de calm.
    public int GetSelfPriceCalm()
    {
        int price = 0;

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
                                Debug.Log(thisPrice.price);
                                //Renvoie le prix de cette energy.
                                //return thisPrice.price;
                                price = thisPrice.price;
                            }
                        }
                    }
                }
            }
        }

        //Debug.Log("ATTENTION : Cette action ne possède pas de prix de calme ! La valeur renvoyé sera de 0.");
        Debug.Log(price);

        return price;
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

        Debug.Log("ATTENTION : Cette action ne possède pas de gain d'énergie ! La valeur renvoyé sera de 0.");

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

        Debug.Log("ATTENTION : Cette action ne possède pas de gain de calme ! La valeur renvoyé sera de 0.");

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
        //Pour le gain.
        /*
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
        }*/
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


    public void SetActionClass(SO_ActionClass thisActionClass)
    {
        actionClass = thisActionClass;
    }

    public SO_ActionClass GetActionClass() { return actionClass; }
}
