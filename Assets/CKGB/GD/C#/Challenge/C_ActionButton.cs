using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class C_ActionButton : MonoBehaviour
{
    [SerializeField]
    SO_ActionClass actionClass;

    //Liste de string pour écrire le texte.
    List<string> listLogsPreview = new List<string>();

    #region Stats
    //Fonction qui renvoie la valeur d'energy.
    public int GetStats(Interaction.ETypeTarget actorTarget, TargetStats.ETypeStatsTarget targetStats, Stats.ETypeStats statsTarget)
    {
        //Pour toutes les liste d'action.
        foreach (Interaction thisInteraction in actionClass.listInteraction)
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

        Debug.Log("ATTENTION : Cette action ne possède pas de prix " + statsTarget + " ! La valeur renvoyé sera de 0.");

        return 0;
    }

    int GetRange()
    {
        //Pour toutes les liste d'action.
        foreach (Interaction thisInteraction in actionClass.listInteraction)
        {
            //Check si sont enum est égale à "Other".
            if (thisInteraction.whatTarget == Interaction.ETypeTarget.Other)
            {
                return thisInteraction.range;
            }
        }

        return 0;
    }

    //Fonction qui renvoie le parametre de mouvement.
    Interaction.ETypeDirectionTarget GetTypeDirectionRange()
    {
        //Pour toutes les liste d'action.
        foreach (Interaction thisInteraction in actionClass.listInteraction)
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
    int GetMovement(Interaction.ETypeTarget actorTarget)
    {
        //Pour toutes les liste d'action.
        foreach (Interaction thisInteraction in actionClass.listInteraction)
        {
            //Check si sont enum est égale à "Self".
            if (thisInteraction.whatTarget == actorTarget)
            {
                //Pour toutes les list de stats.
                foreach (TargetStats thisStats in thisInteraction.listTargetStats)
                {
                    //Check si sont enum est égale à "Movement".
                    if (thisStats.whatStatsTarget == TargetStats.ETypeStatsTarget.Movement)
                    {
                        if (thisStats.move.whatMove == Move.ETypeMove.Right || thisStats.move.whatMove == Move.ETypeMove.Left)
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
    public string GetLogsPreview(List<C_Actor> team, C_Actor thisActor)
    {
        listLogsPreview = new List<string>();
        string logsPreview = "";

        //Créer la liste pour "self"
        GetLogsPreviewTarget(Interaction.ETypeTarget.Self, thisActor);

        //
        if (CheckOtherInAction())
        {
            GetOtherLogsPreview(team, thisActor, GetRange());
        }

        //Prépare le texte de la preview.
        foreach (var thisText in listLogsPreview)
        {
            logsPreview += thisText;
            logsPreview += "\n";
        }

        //Envoie le résultat.
        return logsPreview;
    }

    void GetLogsPreviewTarget(Interaction.ETypeTarget target, C_Actor thisActor)
    {
        //Check si pour le "Self" les variables ne sont pas égale à 0, si c'est le cas alors un system va modifier le text qui v s'afficher.
        #region Price string
        //Pour le prix.
        if (GetStats(target, TargetStats.ETypeStatsTarget.Price, Stats.ETypeStats.Energy) != 0 || GetStats(target, TargetStats.ETypeStatsTarget.Price, Stats.ETypeStats.Calm) != 0)
        {
            //Si les deux possède un int supérieur à 0.
            if (GetStats(target, TargetStats.ETypeStatsTarget.Price, Stats.ETypeStats.Energy) != 0 && GetStats(target, TargetStats.ETypeStatsTarget.Price, Stats.ETypeStats.Calm) != 0)
            {
                listLogsPreview.Add(thisActor.name + " va perdre " + GetColorText(GetStats(target, TargetStats.ETypeStatsTarget.Price, Stats.ETypeStats.Energy).ToString(), Color.blue) + " de calme et " + GetColorText(GetStats(target, TargetStats.ETypeStatsTarget.Price, Stats.ETypeStats.Calm).ToString(), Color.yellow) + " d'énergie.");
            }
            else if (GetStats(target, TargetStats.ETypeStatsTarget.Price, Stats.ETypeStats.Calm) != 0)
            {
                listLogsPreview.Add(thisActor.name + " va perdre " + GetColorText(GetStats(target, TargetStats.ETypeStatsTarget.Price, Stats.ETypeStats.Calm).ToString(), Color.blue) + " de calme.");
            }
            else if (GetStats(target, TargetStats.ETypeStatsTarget.Price, Stats.ETypeStats.Energy) != 0)
            {
                listLogsPreview.Add(thisActor.name + " va perdre " + GetColorText(GetStats(target, TargetStats.ETypeStatsTarget.Price, Stats.ETypeStats.Energy).ToString(), Color.yellow) + " d'énergie.");
            }
        }
        #endregion

        #region Gain string
        //Pour le gain.
        if (GetStats(target, TargetStats.ETypeStatsTarget.Gain, Stats.ETypeStats.Energy) != 0 || GetStats(target, TargetStats.ETypeStatsTarget.Gain, Stats.ETypeStats.Calm) != 0)
        {
            //Si les deux possède un int supérieur à 0.
            if (GetStats(target, TargetStats.ETypeStatsTarget.Gain, Stats.ETypeStats.Energy) != 0 && GetStats(target, TargetStats.ETypeStatsTarget.Gain, Stats.ETypeStats.Calm) != 0)
            {
                listLogsPreview.Add(thisActor.name + " va gagner " + GetColorText(GetStats(target, TargetStats.ETypeStatsTarget.Gain, Stats.ETypeStats.Calm).ToString(), Color.blue) + " de calme et " + GetColorText(GetStats(target, TargetStats.ETypeStatsTarget.Gain, Stats.ETypeStats.Energy).ToString(), Color.yellow) + " d'énergie.");
            }
            else if (GetStats(target, TargetStats.ETypeStatsTarget.Gain, Stats.ETypeStats.Calm) != 0)
            {
                listLogsPreview.Add(thisActor.name + " va gagner " + GetColorText(GetStats(target, TargetStats.ETypeStatsTarget.Gain, Stats.ETypeStats.Calm).ToString(), Color.blue) + " de calme.");
            }
            else if (GetStats(target, TargetStats.ETypeStatsTarget.Gain, Stats.ETypeStats.Energy) != 0)
            {
                listLogsPreview.Add(thisActor.name + " va gagner " + GetColorText(GetStats(target, TargetStats.ETypeStatsTarget.Gain, Stats.ETypeStats.Energy).ToString(), Color.yellow) + " d'énergie.");
            }
        }
        #endregion

        #region Movement
        //Pour le mouvement.
        if (GetMovement(target) != 0)
        {
            //Pour toutes les liste d'action.
            foreach (Interaction thisInteraction in actionClass.listInteraction)
            {
                //Check si sont enum est égale à "Self".
                if (thisInteraction.whatTarget == target)
                {
                    //Pour toutes les list de stats.
                    foreach (TargetStats thisStats in thisInteraction.listTargetStats)
                    {
                        //Check si sont enum est égale à "Movement".
                        if (thisStats.whatStatsTarget == TargetStats.ETypeStatsTarget.Movement)
                        {
                            if (thisStats.move.whatMove == Move.ETypeMove.Right)
                            {
                                listLogsPreview.Add(thisActor.name + " va se déplacer de " + GetColorText(thisStats.move.ToString(), Color.green) + " sur la droite.");
                            }
                            else if (thisStats.move.whatMove == Move.ETypeMove.Left)
                            {
                                listLogsPreview.Add(thisActor.name + " va se déplacer de " + GetColorText(thisStats.move.ToString(), Color.green) + " sur la gauche.");
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
                    foreach (TargetStats thisStats in thisInteraction.listTargetStats)
                    {
                        //Check si sont enum est égale à "Movement".
                        if (thisStats.whatStatsTarget == TargetStats.ETypeStatsTarget.Movement)
                        {
                            if (thisStats.move.whatMove == Move.ETypeMove.SwitchWithActor)
                            {
                                if (thisStats.move.actor != null)
                                    listLogsPreview.Add(thisActor.name + " va échanger sa place avec " + GetColorText(thisStats.move.actor.name, Color.green) + ".");

                                else { Debug.LogWarning(thisStats.move.actor); }
                            }
                            else if (thisStats.move.whatMove == Move.ETypeMove.SwitchWithAcc)
                            {
                                if (thisStats.move.accessories != null)
                                    listLogsPreview.Add(thisActor.name + " va échanger sa place avec " + GetColorText(thisStats.move.accessories.name, Color.green) + ".");

                                else { Debug.LogWarning(thisStats.move.accessories); }
                            }
                        }
                    }
                }
            }
        }
        #endregion
    }

    
    //Affiche une preview sur les autres actor. A REPRENDRE LUNDI !!!
    void GetOtherLogsPreview(List<C_Actor> otherActor, C_Actor thisActor, int range)
    {
        //Boucle avec le range.
        for (int i = 0; i < range; i++)
        {
            //Boucle pour check sur tout les actor du challenge.
            foreach (C_Actor thisOtherActor in otherActor)
            {
                //Check quel direction la range va faire effet.
                switch (GetTypeDirectionRange())
                {
                    //Si "otherActor" est dans la range alors lui aussi on lui affiche les preview mais avec les info pour "other".
                    case Interaction.ETypeDirectionTarget.Right:
                        //Calcul vers la droite.
                        CheckPositionOther(i, thisOtherActor);
                        Debug.Log("Direction Range = droite.");
                        break;
                    case Interaction.ETypeDirectionTarget.Left:
                        //Calcul vers la gauche.
                        CheckPositionOther(-i, thisOtherActor);
                        Debug.Log("Direction Range = Gauche.");
                        break;
                    case Interaction.ETypeDirectionTarget.RightAndLeft:
                        //Calcul vers la droite + gauche.
                        CheckPositionOther(i, thisOtherActor);
                        CheckPositionOther(-i, thisOtherActor);
                        Debug.Log("Direction Range = droite + gauche.");
                        break;
                }
            }
        }

        //Check si il est à bout du plateau.
        void CheckPositionOther(int position, C_Actor target)
        {
            if (thisActor.GetPosition() + position >= otherActor.Count - 1)
            {
                if (0 + position == target.GetPosition() && target != thisActor)
                {
                    GetLogsPreviewTarget(Interaction.ETypeTarget.Other, target);
                    Debug.Log(target.name + " à été trouvé ! à la position: " + target.GetPosition());
                }
            }
            else if (thisActor.GetPosition() + position <= 0)
            {
                if (0 + position == target.GetPosition() && target != thisActor)
                {
                    GetLogsPreviewTarget(Interaction.ETypeTarget.Other, target);
                    Debug.Log(target.name + " à été trouvé ! à la position: " + target.GetPosition());
                    
                }
            }
            else if (thisActor.GetPosition() + position == target.GetPosition() && target != thisActor)
            {
                GetLogsPreviewTarget(Interaction.ETypeTarget.Other, target);
                Debug.Log(target.name + " à été trouvé ! à la position: " + target.GetPosition());
            }

            Debug.Log(thisActor.GetPosition() + position);
        }
    }


    #region Affiche preview sur les stats
    //Cache toutes les preview.
    public void HideUiStatsPreview(List<C_Actor> listActor)
    {
        foreach (C_Actor thisActor in listActor)
        {
            thisActor.GetUiStats().DesactivedAllPreview();
        }
    }

    public void ShowUiStatsPreview(List<C_Actor> otherActor, C_Actor thisActor)
    {
        //Affiche une preview sur l'actor lui meme.
        thisActor.GetUiStats().ActiveSelfPreviewUi(thisActor, this);

        //Check si il y a "other".
        if (CheckOtherInAction())
        {
            //ActiveOtherPreviewUi(otherActor, thisActor, GetOtherMovement(), this);
        }
    }
    #endregion



    //Check si dans la config de cette action, des paramètre "other" existe.
    bool CheckOtherInAction()
    {
        //Pour toutes les liste d'action.
        foreach (Interaction thisInteraction in actionClass.listInteraction)
        {
            //Check si sont enum est égale à "Self".
            if (thisInteraction.whatTarget == Interaction.ETypeTarget.Other)
            {
                Debug.Log("D'autres actor peuvent etre impacté !");
                return true;
            }
        }

        Debug.Log("Aucun autre actor peuvent etre impacté !");
        return false;
    }

    public void SetActionClass(SO_ActionClass thisActionClass)
    {
        actionClass = thisActionClass;
    }

    public SO_ActionClass GetActionClass() { return actionClass; }
}
