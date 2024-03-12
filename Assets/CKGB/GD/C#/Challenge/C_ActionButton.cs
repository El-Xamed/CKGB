using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class C_ActionButton : MonoBehaviour
{
    [SerializeField]
    SO_ActionClass actionClass;

    #region DataPreview
    //Liste de string pour écrire le texte.
    List<string> listLogsPreview = new List<string>();
    #endregion

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

    C_Actor GetSwitchActor(Interaction.ETypeTarget actorTarget)
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

    C_Accessories GetSwitchAcc(Interaction.ETypeTarget actorTarget)
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

    //Check si il y a des acteurs dans la range.
    bool CheckPositionOther(C_Actor thisActor, int position, List<C_Case> listCase, C_Actor target)
    {
        if (thisActor.GetPosition() + position >= listCase.Count - 1)
        {
            if (0 + position == target.GetPosition() && target != thisActor)
            {
                Debug.Log(target.name + " à été trouvé ! à la position: " + target.GetPosition());
                return true;
            }
        }
        else if (thisActor.GetPosition() + position <= 0)
        {
            if (0 + position == target.GetPosition() && target != thisActor)
            {
                Debug.Log(target.name + " à été trouvé ! à la position: " + target.GetPosition());
                return true;
            }
        }
        else if (thisActor.GetPosition() + position == target.GetPosition() && target != thisActor)
        {
            Debug.Log(target.name + " à été trouvé ! à la position: " + target.GetPosition());
            return true;
        }

        return false;
    }

    #region Preview
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
    public string GetLogsPreview(List<C_Actor> team, C_Actor thisActor, List<C_Case> plateau)
    {
        listLogsPreview = new List<string>();
        string logsPreview = "";

        //Créer la liste pour "self"
        GetLogsPreviewTarget(Interaction.ETypeTarget.Self, team, thisActor);

        //Créer la liste pour "other"
        if (CheckOtherInAction())
        {
            GetOtherLogsPreview(team, thisActor, GetRange(), plateau);
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

    void GetLogsPreviewTarget(Interaction.ETypeTarget target, List<C_Actor> otherActor, C_Actor thisActor)
    {
        //Check si pour le "target" les variables ne sont pas égale à 0, si c'est le cas alors un system va modifier le text qui va s'afficher.
        #region Price string
        //Pour le prix.
        if (GetStats(target, TargetStats.ETypeStatsTarget.Price, Stats.ETypeStats.Energy) != 0 || GetStats(target, TargetStats.ETypeStatsTarget.Price, Stats.ETypeStats.Calm) != 0)
        {
            //Si les deux possède un int supérieur à 0.
            if (GetStats(target, TargetStats.ETypeStatsTarget.Price, Stats.ETypeStats.Energy) != 0 && GetStats(target, TargetStats.ETypeStatsTarget.Price, Stats.ETypeStats.Calm) != 0)
            {
                listLogsPreview.Add(thisActor.name + " va perdre " + GetColorText(GetStats(target, TargetStats.ETypeStatsTarget.Price, Stats.ETypeStats.Calm).ToString(), Color.blue) + " de calme et " + GetColorText(GetStats(target, TargetStats.ETypeStatsTarget.Price, Stats.ETypeStats.Energy).ToString(), Color.yellow) + " d'énergie.");
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
                                listLogsPreview.Add(GetColorText(thisActor.name, Color.cyan) + " va se déplacer de " + GetColorText(myMove.nbMove.ToString(), Color.green) + GetDirectionOfMovement() + ".");
                                Debug.Log(GetColorText(thisActor.name, Color.cyan) + " va se déplacer de " + GetColorText(myMove.nbMove.ToString(), Color.green) + GetDirectionOfMovement());
                            }
                        }
                    }

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
                    int notBusyByActor = 0;
                    
                    foreach (C_Actor thisOtherActor in otherActor)
                    {
                        if (newPosition == thisOtherActor.GetPosition())
                        {
                            if (thisOtherActor == thisActor)
                            {
                                listLogsPreview.Add(GetColorText(thisActor.name, Color.cyan) + " ne va pas se déplacer à la case " + GetColorText(myMove.nbMove.ToString(), Color.green) + " car " + GetColorText(thisActor.name, Color.cyan) + " est déjà dessus.");
                                Debug.Log(GetColorText(thisActor.name, Color.cyan) + " ne va pas se déplacer à la case " + GetColorText(myMove.nbMove.ToString(), Color.green) + " car " + GetColorText(thisActor.name, Color.green) + " est déjà dessus.");
                            }
                            else
                            {
                                listLogsPreview.Add(GetColorText(thisActor.name, Color.cyan) + " va échanger de place avec " + GetColorText(thisOtherActor.name, Color.green));
                                Debug.Log(GetColorText(thisActor.name, Color.cyan) + " va échanger de place avec " + GetColorText(thisOtherActor.name, Color.green));
                            }
                        }
                        else
                        {
                            notBusyByActor++;

                            if (notBusyByActor == otherActor.Count)
                            {
                                listLogsPreview.Add(GetColorText(thisActor.name, Color.cyan) + " va se déplacer sur la case " + GetColorText(myMove.nbMove.ToString(), Color.green));
                                Debug.Log(GetColorText(thisActor.name, Color.cyan) + " va se déplacer sur la case " + GetColorText(myMove.nbMove.ToString(), Color.green));
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
                                    listLogsPreview.Add(GetColorText(thisActor.name, Color.cyan) + " va échanger sa place avec " + GetColorText(GetSwitchActor(target).name, Color.cyan) + ".");
                                    Debug.Log(GetColorText(thisActor.name, Color.cyan) + " va échanger sa place avec " + GetColorText(GetSwitchActor(target).name, Color.cyan) + ".");
                                }
                                else { Debug.LogWarning(thisStats.move.actor); }
                            }
                            else if (thisStats.move.whatMove == Move.ETypeMove.SwitchWithAcc)
                            {
                                if (thisStats.move.accessories != null)
                                {
                                    listLogsPreview.Add(GetColorText(thisActor.name, Color.cyan) + " va échanger sa place avec " + GetColorText(GetSwitchAcc(target).name, Color.cyan) + ".");
                                    Debug.Log(GetColorText(thisActor.name, Color.cyan) + " va échanger sa place avec " + GetColorText(GetSwitchAcc(target).name, Color.cyan) + ".");
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
                        if (CheckPositionOther(thisActor, i, plateau, thisOtherActor))
                        {
                            GetLogsPreviewTarget(Interaction.ETypeTarget.Other, otherActor, thisOtherActor);
                        }
                        
                        Debug.Log("Direction Range = droite.");
                        break;
                    case Interaction.ETypeDirectionTarget.Left:
                        //Calcul vers la gauche.
                        if (CheckPositionOther(thisActor, -i, plateau, thisOtherActor))
                        {
                            GetLogsPreviewTarget(Interaction.ETypeTarget.Other, otherActor, thisOtherActor);
                        }
                        Debug.Log("Direction Range = Gauche.");
                        break;
                    case Interaction.ETypeDirectionTarget.RightAndLeft:
                        //Calcul vers la droite + gauche.
                        if (CheckPositionOther(thisActor, i, plateau, thisOtherActor))
                        {
                            GetLogsPreviewTarget(Interaction.ETypeTarget.Other, otherActor, thisOtherActor);
                        }
                        else if (CheckPositionOther(thisActor, -i, plateau, thisOtherActor))
                        {
                            GetLogsPreviewTarget(Interaction.ETypeTarget.Other, otherActor, thisOtherActor);
                        }
                        Debug.Log("Direction Range = droite + gauche.");
                        break;
                }
            }
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
    #endregion

    #region Résolution
    public void UseAction(C_Actor thisActor, List<C_Case> plateau, List<C_Actor> myTeam)
    {
        Debug.Log("Use this actionClass : " + actionClass.buttonText);

        //Applique les conséquences de stats peut importe si c'est réusi ou non.
        //Créer la liste pour "self"
        SetStatsTarget(Interaction.ETypeTarget.Self, myTeam, thisActor);

        //Créer la liste pour "other"
        if (CheckOtherInAction())
        {
            SetStatsOther(myTeam, thisActor, GetRange(), plateau);
        }

        //Check dans les data de cette action si la condition est bonne.
        if (actionClass.CanUse(thisActor))
        {

        }
        else
        {
            //Renvoie un petit indice de pourquoi l'action n'a pas fonctionné.

            return;
        }
    }

    void SetStatsTarget(Interaction.ETypeTarget target, List<C_Actor> otherActor, C_Actor thisActor)
    {
        //Check si pour le "target" les variables ne sont pas égale à 0, si c'est le cas alors un system va modifier le text qui va s'afficher.
        #region Price string
        //Pour le prix.
        if (GetStats(target, TargetStats.ETypeStatsTarget.Price, Stats.ETypeStats.Energy) != 0 || GetStats(target, TargetStats.ETypeStatsTarget.Price, Stats.ETypeStats.Calm) != 0)
        {
            //Si les deux possède un int supérieur à 0.
            if (GetStats(target, TargetStats.ETypeStatsTarget.Price, Stats.ETypeStats.Energy) != 0 && GetStats(target, TargetStats.ETypeStatsTarget.Price, Stats.ETypeStats.Calm) != 0)
            {
                listLogsPreview.Add(thisActor.name + " va perdre " + GetColorText(GetStats(target, TargetStats.ETypeStatsTarget.Price, Stats.ETypeStats.Calm).ToString(), Color.blue) + " de calme et " + GetColorText(GetStats(target, TargetStats.ETypeStatsTarget.Price, Stats.ETypeStats.Energy).ToString(), Color.yellow) + " d'énergie.");
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
                                listLogsPreview.Add(GetColorText(thisActor.name, Color.cyan) + " va se déplacer de " + GetColorText(myMove.nbMove.ToString(), Color.green) + GetDirectionOfMovement() + ".");
                                Debug.Log(GetColorText(thisActor.name, Color.cyan) + " va se déplacer de " + GetColorText(myMove.nbMove.ToString(), Color.green) + GetDirectionOfMovement());
                            }
                        }
                    }

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
                    int notBusyByActor = 0;

                    foreach (C_Actor thisOtherActor in otherActor)
                    {
                        if (newPosition == thisOtherActor.GetPosition())
                        {
                            if (thisOtherActor == thisActor)
                            {
                                listLogsPreview.Add(GetColorText(thisActor.name, Color.cyan) + " ne va pas se déplacer à la case " + GetColorText(myMove.nbMove.ToString(), Color.green) + " car " + GetColorText(thisActor.name, Color.cyan) + " est déjà dessus.");
                                Debug.Log(GetColorText(thisActor.name, Color.cyan) + " ne va pas se déplacer à la case " + GetColorText(myMove.nbMove.ToString(), Color.green) + " car " + GetColorText(thisActor.name, Color.green) + " est déjà dessus.");
                            }
                            else
                            {
                                listLogsPreview.Add(GetColorText(thisActor.name, Color.cyan) + " va échanger de place avec " + GetColorText(thisOtherActor.name, Color.green));
                                Debug.Log(GetColorText(thisActor.name, Color.cyan) + " va échanger de place avec " + GetColorText(thisOtherActor.name, Color.green));
                            }
                        }
                        else
                        {
                            notBusyByActor++;

                            if (notBusyByActor == otherActor.Count)
                            {
                                listLogsPreview.Add(GetColorText(thisActor.name, Color.cyan) + " va se déplacer sur la case " + GetColorText(myMove.nbMove.ToString(), Color.green));
                                Debug.Log(GetColorText(thisActor.name, Color.cyan) + " va se déplacer sur la case " + GetColorText(myMove.nbMove.ToString(), Color.green));
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
                                    listLogsPreview.Add(GetColorText(thisActor.name, Color.cyan) + " va échanger sa place avec " + GetColorText(GetSwitchActor(target).name, Color.cyan) + ".");
                                    Debug.Log(GetColorText(thisActor.name, Color.cyan) + " va échanger sa place avec " + GetColorText(GetSwitchActor(target).name, Color.cyan) + ".");
                                }
                                else { Debug.LogWarning(thisStats.move.actor); }
                            }
                            else if (thisStats.move.whatMove == Move.ETypeMove.SwitchWithAcc)
                            {
                                if (thisStats.move.accessories != null)
                                {
                                    listLogsPreview.Add(GetColorText(thisActor.name, Color.cyan) + " va échanger sa place avec " + GetColorText(GetSwitchAcc(target).name, Color.cyan) + ".");
                                    Debug.Log(GetColorText(thisActor.name, Color.cyan) + " va échanger sa place avec " + GetColorText(GetSwitchAcc(target).name, Color.cyan) + ".");
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
    void SetStatsOther(List<C_Actor> otherActor, C_Actor thisActor, int range, List<C_Case> plateau)
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
                        if (CheckPositionOther(thisActor, i, plateau, thisOtherActor))
                        {
                            SetStatsTarget(Interaction.ETypeTarget.Other, otherActor, thisActor);
                        }
                        Debug.Log("Direction Range = droite.");
                        break;
                    case Interaction.ETypeDirectionTarget.Left:
                        //Calcul vers la gauche.
                        if (CheckPositionOther(thisActor, -i, plateau, thisOtherActor))
                        {

                        }
                        Debug.Log("Direction Range = Gauche.");
                        break;
                    case Interaction.ETypeDirectionTarget.RightAndLeft:
                        //Calcul vers la droite + gauche.
                        if (CheckPositionOther(thisActor, i, plateau, thisOtherActor))
                        {

                        }
                        if (CheckPositionOther(thisActor, -i, plateau, thisOtherActor))
                        {

                        }
                        Debug.Log("Direction Range = droite + gauche.");
                        break;
                }
            }
        }
    }
    #endregion

    public void SetActionClass(SO_ActionClass thisActionClass)
    {
        actionClass = thisActionClass;
    }

    public SO_ActionClass GetActionClass() { return actionClass; }
}
