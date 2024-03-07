using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_ActionButton : MonoBehaviour
{
    [SerializeField]
    SO_ActionClass actionClass;

    #region Self
    #region Stats
    //Fonction qui renvoie la valeur d'energy.
    public int GetSelfPriceEnergy()
    {
        //Pour toutes les liste d'action.
        foreach (Interaction thisInteraction in actionClass.listInteraction)
        {
            //Check si sont enum est égale à "Self".
            if (thisInteraction.whatTarget == Interaction.ETypeTarget.Self)
            {
                //Pour toutes les list de stats.
                foreach (Stats thisStats in thisInteraction.listStats)
                {
                    //Check si sont enum est égale à "Price".
                    if (thisStats.whatStatsTarget == Stats.ETypeStatsTarget.Price)
                    {
                        //Pour toutes les list de prix.
                        foreach (Price thisPrice in thisStats.listPrice)
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
        foreach (Interaction thisInteraction in actionClass.listInteraction)
        {
            //Check si sont enum est égale à "Self".
            if (thisInteraction.whatTarget == Interaction.ETypeTarget.Self)
            {
                //Pour toutes les list de stats.
                foreach (Stats thisStats in thisInteraction.listStats)
                {
                    //Check si sont enum est égale à "Price".
                    if (thisStats.whatStatsTarget == Stats.ETypeStatsTarget.Price)
                    {
                        //Pour toutes les list de prix.
                        foreach (Price thisPrice in thisStats.listPrice)
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

        Debug.Log("ATTENTION : Cette action ne possède pas de prix de calme ! La valeur renvoyé sera de 0.");

        return 0;
    }

    //Fonction qui renvoie la valeur d'energy.
    public int GetSelfGainEnergy()
    {
        //Pour toutes les liste d'action.
        foreach (Interaction thisInteraction in actionClass.listInteraction)
        {
            //Check si sont enum est égale à "Self".
            if (thisInteraction.whatTarget == Interaction.ETypeTarget.Self)
            {
                //Pour toutes les list de stats.
                foreach (Stats thisStats in thisInteraction.listStats)
                {
                    //Check si sont enum est égale à "Gain".
                    if (thisStats.whatStatsTarget == Stats.ETypeStatsTarget.Gain)
                    {
                        //Pour toutes les list de gain.
                        foreach (Gain thisGain in thisStats.listGain)
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
        foreach (Interaction thisInteraction in actionClass.listInteraction)
        {
            //Check si sont enum est égale à "Self".
            if (thisInteraction.whatTarget == Interaction.ETypeTarget.Self)
            {
                //Pour toutes les list de stats.
                foreach (Stats thisStats in thisInteraction.listStats)
                {
                    //Check si sont enum est égale à "Price".
                    if (thisStats.whatStatsTarget == Stats.ETypeStatsTarget.Gain)
                    {
                        //Pour toutes les list de prix.
                        foreach (Gain thisGain in thisStats.listGain)
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

    #region Movement
    //Fonction qui renvoie le parametre de mouvement.
    public int GetSelfMovement()
    {
        //Pour toutes les liste d'action.
        foreach (Interaction thisInteraction in actionClass.listInteraction)
        {
            //Check si sont enum est égale à "Self".
            if (thisInteraction.whatTarget == Interaction.ETypeTarget.Self)
            {
                //Pour toutes les list de stats.
                foreach (Stats thisStats in thisInteraction.listStats)
                {
                    //Check si sont enum est égale à "Movement".
                    if (thisStats.whatStatsTarget == Stats.ETypeStatsTarget.Movement)
                    {
                        
                        if (thisStats.move.whatMove == Move.ETypeMove.Right || thisStats.move.whatMove == Move.ETypeMove.Left)
                        {
                            return thisStats.move.nbMove;
                        }
                        else
                        {
                            return 0;
                        }
                    }
                }
            }
        }

        Debug.Log("ATTENTION : Cette action ne possède pas de gain de calme ! La valeur renvoyé sera de 0.");

        return 0;
    }
    #endregion
    #endregion

    #region System de couleur dans un texte.
    string GetColorToString(Color32 color)
    {
        string hex = color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");
        return hex;
    }

    string GetColorText(string myString, Color32 myColor)
    {
        
        return "<color=#" + GetColorToString(myColor) + ">" + myString + "<" + "/color>";
    }
    #endregion

    //Pour récupérer le texte pour la preview des stats.
    public string GetLogsPreview(C_Actor thisActor)
    {
        //Liste de string pour écrire le texte.
        List<string> listLogsPreview = new List<string>();
        string logsPreview = "";

        //Check si pour le "Self" les variables ne sont pas égale à 0, si c'est le cas alors un system va modifier le text qui v s'afficher.

        #region Price string
        //Pour le prix.
        if (GetSelfPriceEnergy() != 0 || GetSelfPriceCalm() != 0)
        {
            //Si les deux possède un int supérieur à 0.
            if (GetSelfPriceEnergy() != 0 && GetSelfPriceCalm() != 0)
            {
                listLogsPreview.Add(thisActor.name + " va perdre " + GetColorText(GetSelfPriceEnergy().ToString(), Color.blue) + " de calme et " + GetColorText(GetSelfPriceEnergy().ToString(), Color.yellow) + " d'énergie.");
            }
            else if (GetSelfPriceCalm() != 0)
            {
                listLogsPreview.Add(thisActor.name + " va perdre " + GetColorText(GetSelfPriceEnergy().ToString(), Color.blue) + " de calme.");
            }
            else if (GetSelfPriceEnergy() != 0)
            {
                listLogsPreview.Add(thisActor.name + " va perdre " + GetColorText(GetSelfPriceEnergy().ToString(), Color.yellow) + " d'énergie.");
            }
        }
        #endregion

        #region Gain string
        //Pour le gain.
        if (GetSelfGainEnergy() != 0 || GetSelfGainCalm() != 0)
        {
            //Si les deux possède un int supérieur à 0.
            if (GetSelfGainEnergy() != 0 && GetSelfGainCalm() != 0)
            {
                listLogsPreview.Add(thisActor.name + " va gagner " + GetColorText(GetSelfGainCalm().ToString(), Color.blue) + " de calme et " + GetColorText(GetSelfGainEnergy().ToString(), Color.yellow) + " d'énergie.");
            }
            else if (GetSelfGainCalm() != 0)
            {
                listLogsPreview.Add(thisActor.name + " va gagner " + GetColorText(GetSelfGainCalm().ToString(), Color.blue) + " de calme.");
            }
            else if (GetSelfGainEnergy() != 0)
            {
                listLogsPreview.Add(thisActor.name + " va gagner " + GetColorText(GetSelfGainEnergy().ToString(), Color.yellow) + " d'énergie.");
            }
        }
        #endregion

        #region Movement
        //Pour le mouvement.
        if (GetSelfMovement() != 0)
        {
            //Pour toutes les liste d'action.
            foreach (Interaction thisInteraction in actionClass.listInteraction)
            {
                //Check si sont enum est égale à "Self".
                if (thisInteraction.whatTarget == Interaction.ETypeTarget.Self)
                {
                    //Pour toutes les list de stats.
                    foreach (Stats thisStats in thisInteraction.listStats)
                    {
                        //Check si sont enum est égale à "Movement".
                        if (thisStats.whatStatsTarget == Stats.ETypeStatsTarget.Movement)
                        {
                            if (thisStats.move.whatMove == Move.ETypeMove.Right)
                            {
                                listLogsPreview.Add(thisActor.name + " va se déplacer de " + GetColorText(GetSelfMovement().ToString(), Color.green) + " sur la droite.");
                            }
                            else if(thisStats.move.whatMove == Move.ETypeMove.Left)
                            {
                                listLogsPreview.Add(thisActor.name + " va se déplacer de " + GetColorText(GetSelfMovement().ToString(), Color.green) + " sur la gauche.");
                            }
                        }
                    }
                }
            }
        }
        else
        {
            //Pour toutes les liste d'action.
            foreach (Interaction thisInteraction in actionClass.listInteraction)
            {
                //Check si sont enum est égale à "Self".
                if (thisInteraction.whatTarget == Interaction.ETypeTarget.Self)
                {
                    //Pour toutes les list de stats.
                    foreach (Stats thisStats in thisInteraction.listStats)
                    {
                        //Check si sont enum est égale à "Movement".
                        if (thisStats.whatStatsTarget == Stats.ETypeStatsTarget.Movement)
                        {
                            if (thisStats.move.whatMove == Move.ETypeMove.SwitchWithActor)
                            {
                                if (thisStats.move.actor != null)
                                    listLogsPreview.Add(thisActor.name + " va échanger sa place avec " + GetColorText(thisStats.move.actor.name, Color.green) + ".");

                                else { Debug.LogWarning(thisStats.move.actor); }
                            }
                            else if (thisStats.move.whatMove == Move.ETypeMove.SwitchWithAcc)
                            {
                                if(thisStats.move.accessories != null)
                                listLogsPreview.Add(thisActor.name + " va échanger sa place avec " + GetColorText(thisStats.move.accessories.name, Color.green) + ".");

                                else { Debug.LogWarning(thisStats.move.accessories); }
                            }
                        }
                    }
                }
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

    public void SetActionClass(SO_ActionClass thisActionClass)
    {
        actionClass = thisActionClass;
    }

    public SO_ActionClass GetActionClass() { return actionClass; }
}
