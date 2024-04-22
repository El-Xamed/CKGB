using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_ActionButton : MonoBehaviour
{
    [SerializeField]
    GameObject curseur;

    [SerializeField]
    SO_ActionClass actionClass;

    private void Awake()
    {
        curseur.SetActive(false);

        //Check si la condition avancé est activé
        if (GetActionClass().advancedCondition.canMakeByOneActor)
        {
            GetActionClass().advancedCondition.whatActor = GameObject.Find(GetActionClass().advancedCondition.whatActor.name).GetComponent<C_Actor>();
        }
    }

    #region DataPreview
    //Liste de string pour écrire le texte.
    List<string> listLogsPreview = new List<string>();
    #endregion

    #region Preview

    //Pour récupérer le texte pour la preview des stats.
    public string GetLogsPreview(List<C_Actor> team, C_Actor thisActor, List<C_Case> plateau)
    {
        listLogsPreview = new List<string>();
        string logsPreview = "";

        /* BLOCAGE DE CETTE PARTIE DE SCRIPT QUI PERMET D'ECRIRE LES PREVIEW CAR CA VA ETRE REMPLACE PAR UN OUTIL DE PREVIEW.
        //Créer la liste pour "self"
        GetLogsPreviewTarget(Interaction.ETypeTarget.Self, team, thisActor);

        //Créer la liste pour "other"
        if (actionClass.CheckOtherInAction())
        {
            GetOtherLogsPreview(team, thisActor, actionClass.GetRange(), plateau);
        }*/

        //Prépare le texte de la preview.
        foreach (var thisText in listLogsPreview)
        {
            logsPreview += thisText;
            logsPreview += "\n";
        }

        //Envoie le résultat.
        return logsPreview;
    }

    /* A SUPPRIMER CAR UN OUTIL SERA DEV POUR ECRIRE LES PREVIEW.
    void GetLogsPreviewTarget(Interaction.ETypeTarget target, List<C_Actor> otherActor, C_Pion thisActor)
    {
        //Check si pour le "target" les variables ne sont pas égale à 0, si c'est le cas alors un system va modifier le text qui va s'afficher.
        #region Price string
        //Pour le prix.
        if (actionClass.GetStats(target, TargetStats.ETypeStatsTarget.Price, Stats.ETypeStats.Energy) != 0 || actionClass.GetStats(target, TargetStats.ETypeStatsTarget.Price, Stats.ETypeStats.Calm) != 0)
        {
            //Si les deux possède un int supérieur à 0.
            if (actionClass.GetStats(target, TargetStats.ETypeStatsTarget.Price, Stats.ETypeStats.Energy) != 0 && actionClass.GetStats(target, TargetStats.ETypeStatsTarget.Price, Stats.ETypeStats.Calm) != 0)
            {
                listLogsPreview.Add(thisActor.name + " va perdre " + GetColorText(actionClass.GetStats(target, TargetStats.ETypeStatsTarget.Price, Stats.ETypeStats.Calm).ToString(), Color.blue) + " de calme et " + GetColorText(actionClass.GetStats(target, TargetStats.ETypeStatsTarget.Price, Stats.ETypeStats.Energy).ToString(), Color.yellow) + " d'énergie.");
            }
            else if (actionClass.GetStats(target, TargetStats.ETypeStatsTarget.Price, Stats.ETypeStats.Calm) != 0)
            {
                listLogsPreview.Add(thisActor.name + " va perdre " + GetColorText(actionClass.GetStats(target, TargetStats.ETypeStatsTarget.Price, Stats.ETypeStats.Calm).ToString(), Color.blue) + " de calme.");
            }
            else if (actionClass.GetStats(target, TargetStats.ETypeStatsTarget.Price, Stats.ETypeStats.Energy) != 0)
            {
                listLogsPreview.Add(thisActor.name + " va perdre " + GetColorText(actionClass.GetStats(target, TargetStats.ETypeStatsTarget.Price, Stats.ETypeStats.Energy).ToString(), Color.yellow) + " d'énergie.");
            }
        }
        #endregion

        #region Gain string
        //Pour le gain.
        if (actionClass.GetStats(target, TargetStats.ETypeStatsTarget.Gain, Stats.ETypeStats.Energy) != 0 || actionClass.GetStats(target, TargetStats.ETypeStatsTarget.Gain, Stats.ETypeStats.Calm) != 0)
        {
            //Si les deux possède un int supérieur à 0.
            if (actionClass.GetStats(target, TargetStats.ETypeStatsTarget.Gain, Stats.ETypeStats.Energy) != 0 && actionClass.GetStats(target, TargetStats.ETypeStatsTarget.Gain, Stats.ETypeStats.Calm) != 0)
            {
                listLogsPreview.Add(thisActor.name + " va gagner " + GetColorText(actionClass.GetStats(target, TargetStats.ETypeStatsTarget.Gain, Stats.ETypeStats.Calm).ToString(), Color.blue) + " de calme et " + GetColorText(actionClass.GetStats(target, TargetStats.ETypeStatsTarget.Gain, Stats.ETypeStats.Energy).ToString(), Color.yellow) + " d'énergie.");
            }
            else if (actionClass.GetStats(target, TargetStats.ETypeStatsTarget.Gain, Stats.ETypeStats.Calm) != 0)
            {
                listLogsPreview.Add(thisActor.name + " va gagner " + GetColorText(actionClass.GetStats(target, TargetStats.ETypeStatsTarget.Gain, Stats.ETypeStats.Calm).ToString(), Color.blue) + " de calme.");
            }
            else if (actionClass.GetStats(target, TargetStats.ETypeStatsTarget.Gain, Stats.ETypeStats.Energy) != 0)
            {
                listLogsPreview.Add(thisActor.name + " va gagner " + GetColorText(actionClass.GetStats(target, TargetStats.ETypeStatsTarget.Gain, Stats.ETypeStats.Energy).ToString(), Color.yellow) + " d'énergie.");
            }
        }
        #endregion

        #region Movement
        //Pour le mouvement.
        if (actionClass.GetMovement(target) != 0)
        {
            //Pour toutes les liste d'action.
            foreach (Interaction thisInteraction in actionClass.listInteraction)
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
                            MoveActor(thisStats.move);
                            Debug.Log(target + " : " + thisStats.move.whatMove);
                        }
                    }
                }
            }

            void MoveActor(Move myMove)
            {
                switch (myMove.whatMove)
                {
                    //Si "otherActor" est dans la range alors lui aussi on lui affiche les preview mais avec les info pour "other".
                    case Move.ETypeMove.Right:
                        NormalMoveActor(myMove.nbMove);
                        break;
                    case Move.ETypeMove.Left:
                        NormalMoveActor(-myMove.nbMove);
                        break;
                    case Move.ETypeMove.OnTargetCase:
                        TargetMoveActor(myMove.nbMove);
                        break;
                }

                void NormalMoveActor(int newPosition)
                {
                    int notBusyByActor = 0;

                    #region Detection de tous les autres membre de l'équipe.
                    foreach (C_Actor thisOtherActor in otherActor)
                    {
                        //Détection de si il y a un autres actor.
                        if (thisActor.GetPosition() + newPosition == thisOtherActor.GetPosition())
                        {
                            //Check si c'est une TP (donc un swtich) ou un déplacement normal (pousse le personnage).
                            if (myMove.isTp)
                            {
                                listLogsPreview.Add(GetColorText(thisActor.name, Color.cyan) + " va échanger de place avec " + GetColorText(thisOtherActor.name, Color.green) + ".");
                                Debug.Log(GetColorText(thisActor.name, Color.cyan) + " va échanger de place avec " + GetColorText(thisOtherActor.name, Color.green) + ".");
                            }
                            else
                            {
                                listLogsPreview.Add(GetColorText(thisActor.name, Color.cyan) + " va prendre la place de " + GetColorText(thisOtherActor.name, Color.cyan) + " et sera déplacer " + GetDirectionOfMovement());
                                Debug.Log(GetColorText(thisActor.name, Color.cyan) + " va prendre la place de " + GetColorText(thisOtherActor.name, Color.green) + " et sera déplacer " + GetDirectionOfMovement());
                            }
                        }
                        else
                        {
                            notBusyByActor++;

                            if (notBusyByActor == otherActor.Count)
                            {
                                listLogsPreview.Add(GetColorText(thisActor.name, Color.cyan) + " va se déplacer de " + GetColorText(myMove.nbMove.ToString(), Color.green) + GetDirectionOfMovement());
                                Debug.Log(GetColorText(thisActor.name, Color.cyan) + " va se déplacer de " + GetColorText(myMove.nbMove.ToString(), Color.green) + GetDirectionOfMovement());
                            }
                        }
                    }
                    #endregion

                    string GetDirectionOfMovement()
                    {
                        if (newPosition < 0)
                        {
                            return " à gauche.";
                        }
                        else if (newPosition > 0)
                        {
                            return " à droite.";
                        }

                        return "Direction Inconu.";
                    }
                }

                void TargetMoveActor(int newPosition)
                {
                    foreach (C_Actor thisOtherActor in otherActor)
                    {
                        if (newPosition == thisOtherActor.GetPosition())
                        {
                            listLogsPreview.Add(GetColorText(thisActor.name, Color.cyan) + " va échanger de place avec " + GetColorText(thisOtherActor.name, Color.green)+ ".");
                            Debug.Log(GetColorText(thisActor.name, Color.cyan) + " va échanger de place avec " + GetColorText(thisOtherActor.name, Color.green) + ".");
                        }
                        else
                        {
                            listLogsPreview.Add(GetColorText(thisActor.name, Color.cyan) + " va se déplacer sur la case " + GetColorText(newPosition.ToString(), Color.green) + ".");
                            Debug.Log(GetColorText(thisActor.name, Color.cyan) + " va se déplacer sur la case " + GetColorText(newPosition.ToString(), Color.green) + ".");
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
                if (thisInteraction.whatTarget == target)
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
                                {
                                    listLogsPreview.Add(GetColorText(thisActor.name, Color.cyan) + " va échanger sa place avec " + GetColorText(actionClass.GetSwitchActor(target).name, Color.cyan) + ".");
                                    Debug.Log(GetColorText(thisActor.name, Color.cyan) + " va échanger sa place avec " + GetColorText(actionClass.GetSwitchActor(target).name, Color.cyan) + ".");
                                }
                                else { Debug.LogWarning(thisStats.move.actor); }
                            }
                            else if (thisStats.move.whatMove == Move.ETypeMove.SwitchWithAcc)
                            {
                                if (thisStats.move.accessories != null)
                                {
                                    listLogsPreview.Add(GetColorText(thisActor.name, Color.cyan) + " va échanger sa place avec " + GetColorText(actionClass.GetSwitchAcc(target).name, Color.cyan) + ".");
                                    Debug.Log(GetColorText(thisActor.name, Color.cyan) + " va échanger sa place avec " + GetColorText(actionClass.GetSwitchAcc(target).name, Color.cyan) + ".");
                                }
                                else { Debug.LogWarning(thisStats.move.accessories); }
                            }
                        }
                    }
                }
            }
        }
        #endregion
    }

    //Affiche une preview sur les autres actor.
    void GetOtherLogsPreview(List<C_Actor> otherActor, C_Actor thisActor, int range, List<C_Case> plateau)
    {
        //Check si c'est ciblé ou non.
        if (!actionClass.GetIfTargetOrNot())
        {
            //Boucle avec le range.
            for (int i = 1; i < range; i++)
            {
                //Boucle pour check sur tout les actor du challenge.
                foreach (C_Actor thisOtherActor in otherActor)
                {
                    //Check quel direction la range va faire effet.
                    switch (actionClass.GetTypeDirectionRange())
                    {
                        //Si "otherActor" est dans la range alors lui aussi on lui affiche les preview mais avec les info pour "other".
                        case Interaction.ETypeDirectionTarget.Right:
                            //Calcul vers la droite.
                            if (actionClass.CheckPositionOther(thisActor, i, plateau, thisOtherActor))
                            {
                                GetLogsPreviewTarget(Interaction.ETypeTarget.Other, otherActor, thisOtherActor);
                            }

                            Debug.Log("Direction Range = droite.");
                            break;
                        case Interaction.ETypeDirectionTarget.Left:
                            //Calcul vers la gauche.
                            if (actionClass.CheckPositionOther(thisActor, -i, plateau, thisOtherActor))
                            {
                                GetLogsPreviewTarget(Interaction.ETypeTarget.Other, otherActor, thisOtherActor);
                            }
                            Debug.Log("Direction Range = Gauche.");
                            break;
                        case Interaction.ETypeDirectionTarget.RightAndLeft:
                            //Calcul vers la droite + gauche.
                            if (actionClass.CheckPositionOther(thisActor, i, plateau, thisOtherActor))
                            {
                                GetLogsPreviewTarget(Interaction.ETypeTarget.Other, otherActor, thisOtherActor);
                            }
                            else if (actionClass.CheckPositionOther(thisActor, -i, plateau, thisOtherActor))
                            {
                                GetLogsPreviewTarget(Interaction.ETypeTarget.Other, otherActor, thisOtherActor);
                            }
                            Debug.Log("Direction Range = droite + gauche.");
                            break;
                    }
                }
            }
        }
        else if (actionClass.GetIfTargetOrNot())
        {
            if (actionClass.GetTarget().GetComponent<C_Actor>())
            {
                GetLogsPreviewTarget(Interaction.ETypeTarget.Other, otherActor, actionClass.GetTarget().GetComponent<C_Actor>());
            }
            else if(actionClass.GetTarget().GetComponent<C_Accessories>())
            {
                actionClass.SetTarget(GameObject.Find(actionClass.GetTarget().GetComponent<C_Accessories>().GetDataAcc().name));

                GetLogsPreviewTarget(Interaction.ETypeTarget.Other, otherActor, actionClass.GetTarget().GetComponent<C_Accessories>());
            }
        }
    }
    */

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
        if (actionClass.CheckOtherInAction())
        {
            //ActiveOtherPreviewUi(otherActor, thisActor, GetOtherMovement(), this);
        }
    }
    #endregion

    #endregion

    public void HideCurseur()
    {
        curseur.SetActive(false);
    }

    public void ShowCurseur()
    {
        curseur.SetActive(true);
    }

    public void SetActionClass(SO_ActionClass thisActionClass)
    {
        actionClass = thisActionClass;
    }

    public SO_ActionClass GetActionClass() { return actionClass; }
}
