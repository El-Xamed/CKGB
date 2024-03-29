using System;
using System.Collections.Generic;
using UnityEngine;
using static TargetStats;

[CreateAssetMenu(fileName = "New Action", menuName = "ScriptableObjects/Challenge/Action", order = 1)]
public class SO_ActionClass : ScriptableObject
{
    #region Data
    #region Texte
    [Header("Text (Button)")]
    public string buttonText;

    [Header("Text (Logs)")]
    public string LogsMakeAction;
    #endregion

    [Header("Conditions")]
    public AdvancedCondition advancedCondition;

    [Header("Next Action")]
    public SO_ActionClass nextAction;

    [Header("List d'action")]
    public List<Interaction> listInteraction = new List<Interaction>();
    #endregion

    #region R�cup�ration de stats
    //Fonction qui renvoie la valeur d'energy.
    public int GetStats(Interaction.ETypeTarget actorTarget, TargetStats.ETypeStatsTarget targetStats, Stats.ETypeStats statsTarget)
    {
        //Pour toutes les liste d'action.
        foreach (Interaction thisInteraction in listInteraction)
        {
            //Check si sont enum est �gale � "actorTarget".
            if (thisInteraction.whatTarget == actorTarget)
            {
                //Pour toutes les list de stats.
                foreach (TargetStats thisTargetStats in thisInteraction.listTargetStats)
                {
                    //Check si sont enum est �gale � "statsTarget".
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

        //Debug.Log("ATTENTION : Cette action ne poss�de pas de prix " + statsTarget + " ! La valeur renvoy� sera de 0.");

        return 0;
    }

    public int GetRange()
    {
        //Pour toutes les liste d'action.
        foreach (Interaction thisInteraction in listInteraction)
        {
            //Check si sont enum est �gale � "Other".
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
            //Check si sont enum est �gale � "Other".
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
            //Check si sont enum est �gale � "Other".
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
            //Check si sont enum est �gale � "Other".
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
            //Check si sont enum est �gale � "Other".
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
            //Check si sont enum est �gale � "Other".
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
            //Check si sont enum est �gale � "actorTarget".
            if (thisInteraction.whatTarget == actorTarget)
            {
                //Pour toutes les list de stats.
                foreach (TargetStats thisTargetStats in thisInteraction.listTargetStats)
                {
                    //Check si sont enum est �gale � "statsTarget".
                    if (thisTargetStats.whatStatsTarget == TargetStats.ETypeStatsTarget.Movement)
                    {
                        return thisTargetStats.move.actor;
                    }
                }
            }
        }

        Debug.Log("ATTENTION : Cette action ne poss�de de switch avec un autre actor !");

        return null;
    }

    public C_Accessories GetSwitchAcc(Interaction.ETypeTarget actorTarget)
    {
        //Pour toutes les liste d'action.
        foreach (Interaction thisInteraction in listInteraction)
        {
            //Check si sont enum est �gale � "actorTarget".
            if (thisInteraction.whatTarget == actorTarget)
            {
                //Pour toutes les list de stats.
                foreach (TargetStats thisTargetStats in thisInteraction.listTargetStats)
                {
                    //Check si sont enum est �gale � "statsTarget".
                    if (thisTargetStats.whatStatsTarget == TargetStats.ETypeStatsTarget.Movement)
                    {
                        return thisTargetStats.move.accessories;
                    }
                }
            }
        }

        Debug.Log("ATTENTION : Cette action ne poss�de de switch avec un autre actor !");

        return null;
    }

    //Fonction qui renvoie le parametre de mouvement.
    public Interaction.ETypeDirectionTarget GetTypeDirectionRange()
    {
        //Pour toutes les liste d'action.
        foreach (Interaction thisInteraction in listInteraction)
        {
            //Check si sont enum est �gale � "other".
            if (thisInteraction.whatTarget == Interaction.ETypeTarget.Other)
            {
                return thisInteraction.whatDirectionTarget;
            }
        }

        Debug.Log("ATTENTION : Cette action ne poss�de pas de gain de calme ! La valeur renvoy� sera de 0.");

        return 0;
    }

    //Fonction qui renvoie le nombre de mouvement pour d�placer "other".
    public int GetMovement(Interaction.ETypeTarget actorTarget)
    {
        //Pour toutes les liste d'action.
        foreach (Interaction thisInteraction in listInteraction)
        {
            //Check si sont enum est �gale � "actorTarget".
            if (thisInteraction.whatTarget == actorTarget)
            {
                //Pour toutes les list de stats.
                foreach (TargetStats thisStats in thisInteraction.listTargetStats)
                {
                    //Check si sont enum est �gale � "Movement".
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

        Debug.Log("ATTENTION : Cette action ne poss�de pas de gain de calme ! La valeur renvoy� sera de 0.");

        return 0;
    }
    #endregion

    //Check si il y a des acteurs dans la range.
    public bool CheckPositionOther(C_Actor thisActor, int position, List<C_Case> listCase, C_Actor target)
    {
        if (thisActor.GetPosition() + position >= listCase.Count - 1)
        {
            if (0 + position == target.GetPosition() && target != thisActor)
            {
                Debug.Log(target.name + " � �t� trouv� ! � la position: " + target.GetPosition());
                return true;
            }
        }
        else if (thisActor.GetPosition() + position <= 0)
        {
            if (0 + position == target.GetPosition() && target != thisActor)
            {
                Debug.Log(target.name + " � �t� trouv� ! � la position: " + target.GetPosition());
                return true;
            }
        }
        else if (thisActor.GetPosition() + position == target.GetPosition() && target != thisActor)
        {
            Debug.Log(target.name + " � �t� trouv� ! � la position: " + target.GetPosition());
            return true;
        }

        return false;
    }

    //Check si dans la config de cette action, des param�tre "other" existe.
    public bool CheckOtherInAction()
    {
        //Pour toutes les liste d'action.
        foreach (Interaction thisInteraction in listInteraction)
        {
            //Check si sont enum est �gale � "Other".
            if (thisInteraction.whatTarget == Interaction.ETypeTarget.Other)
            {
                Debug.Log("D'autres actor peuvent etre impact� !");
                return true;
            }
        }

        Debug.Log("Aucun autre actor peuvent etre impact� !");
        return false;
    }

    #region R�solution
    public void UseAction(C_Actor thisActor, List<C_Case> plateau, List<C_Actor> myTeam)
    {
        Debug.Log("Use this actionClass : " + buttonText);

        //Check dans les data de cette action si la condition est bonne.
        if (CanUse(thisActor))
        {
            //Applique les cons�quences de stats peut importe si c'est r�usi ou non.
            //Cr�er la liste pour "self"
            SetStatsTarget(Interaction.ETypeTarget.Self, myTeam, thisActor, plateau);

            //Cr�er la liste pour "other"
            if (CheckOtherInAction())
            {
                SetStatsOther(myTeam, thisActor, GetRange(), plateau);
            }
        }
        else
        {
            //Renvoie un petit indice de pourquoi l'action n'a pas fonctionn�.
            //A VOIR PLUS TARD.
            return;
        }
    }

    //v�rifie la condition si l'action fonctionne.
    public bool CanUse(C_Actor thisActor)
    {
        //Check si les codition bonus sont activ�.
        if (advancedCondition.advancedCondition)
        {
            //Check si l'action doit etre fait par un actor en particulier + Si "whatActor" n'est pas null + si "whatActor" est �gal � "thisActor".
            if (advancedCondition.canMakeByOneActor && advancedCondition.whatActor && advancedCondition.whatActor != thisActor)
            {
                return false;
            }

            //Check si l'action doit etre fait par un acc en particulier + Si "whatAcc" n'est pas null + si "whatAcc" est �gal � "thisActor".
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

    public void SetStatsTarget(Interaction.ETypeTarget target, List<C_Actor> otherActor, C_Pion thisActor, List<C_Case> plateau)
    {
        //Check si pour le "target" les variables ne sont pas �gale � 0, si c'est le cas alors un system va modifier le text qui va s'afficher.
        #region Price string
        //Pour le prix.
        //Check si il poss�de le component "C_Actor". A RETIRER PLUS TARD CAR C'EST DU PUTAIN DE BRICOLAGE DE MES COUILLES CAR  J'AI PAS LE TEMPS !
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
        //Check si il poss�de le component "C_Actor". A RETIRER PLUS TARD CAR C'EST DU PUTAIN DE BRICOLAGE DE MES COUILLES CAR  J'AI PAS LE TEMPS !
        if (thisActor.GetComponent<C_Actor>())
        {
            if (GetStats(target, TargetStats.ETypeStatsTarget.Gain, Stats.ETypeStats.Energy) != 0 || GetStats(target, TargetStats.ETypeStatsTarget.Gain, Stats.ETypeStats.Calm) != 0)
            {
                thisActor.GetComponent<C_Actor>().SetCurrentStatsGain(GetStats(target, TargetStats.ETypeStatsTarget.Gain, Stats.ETypeStats.Calm), GetStats(target, TargetStats.ETypeStatsTarget.Gain, Stats.ETypeStats.Energy));
            }
        }
        #endregion

        #region Movement
        //Pour le mouvement.
        if (GetMovement(target) != 0)
        {
            //Pour toutes les liste d'action.
            foreach (Interaction thisInteraction in listInteraction)
            {
                //Check si sont enum est �gale � "target".
                if (thisInteraction.whatTarget == target)
                {
                    //Pour toutes les list de stats.
                    foreach (TargetStats thisStats in thisInteraction.listTargetStats)
                    {
                        //Check si sont enum est �gale � "Movement".
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
                        NormalMoveActor(myMove.nbMove, plateau);
                        break;
                    case Move.ETypeMove.Left:
                        NormalMoveActor(-myMove.nbMove, plateau);
                        break;
                    case Move.ETypeMove.OnTargetCase:
                        TargetMoveActor(myMove.nbMove, plateau);
                        break;
                }

                // A VOIR POUR SIMPLIFIER LE CODE. IMPORTANT CAR IL Y A DU BRICOLAGE.
                void NormalMoveActor(int newPosition, List<C_Case> plateau)
                {
                    //Check si la valeur reste dans le plateau.
                    for (int i = 0; i < newPosition; i++)
                    {
                        //Check si un autre membre de l'�quipe occupe deja a place. A voir si je le garde.
                        foreach (C_Actor thisOtherActor in otherActor)
                        {
                            //Si dans la list de l'�quipe c'est pas �gale � l'actor qui joue.
                            if (thisActor != thisOtherActor)
                            {
                                //Detection de si le perso est au bord (� droite).
                                if (thisActor.GetPosition() + i > plateau.Count - 1)
                                {
                                    //D�tection de si il y a un autres actor.
                                    if (0 + i == thisOtherActor.GetPosition())
                                    {
                                        //Check si c'est une TP (donc un swtich) ou un d�placement normal (pousse le personnage).
                                        if (myMove.isTp)
                                        {
                                            thisOtherActor.MoveActor(plateau, thisActor.GetPosition());
                                            Debug.Log(TextUtils.GetColorText(thisActor.name, Color.cyan) + " a �chang� sa place avec " + TextUtils.GetColorText(thisOtherActor.name, Color.green) + ".");
                                        }
                                        else
                                        {
                                            thisOtherActor.MoveActor(plateau, thisOtherActor.GetPosition() + (int)Mathf.Sign(newPosition));
                                            Debug.Log(TextUtils.GetColorText(thisActor.name, Color.cyan) + " a prit la place de " + TextUtils.GetColorText(thisOtherActor.name, Color.green) + " et sera d�placer " + GetDirectionOfMovement());
                                        }
                                    }
                                }
                                else
                                {
                                    //D�tection de si il y a un autres actor.
                                    if (thisActor.GetPosition() + newPosition == thisOtherActor.GetPosition())
                                    {
                                        //Check si c'est une TP (donc un swtich) ou un d�placement normal (pousse le personnage).
                                        if (myMove.isTp)
                                        {
                                            thisOtherActor.MoveActor(plateau, thisActor.GetPosition());
                                            Debug.Log(TextUtils.GetColorText(thisActor.name, Color.cyan) + " a �chang� sa place avec " + TextUtils.GetColorText(thisOtherActor.name, Color.green) + ".");
                                        }
                                        else
                                        {
                                            thisOtherActor.MoveActor(plateau, thisOtherActor.GetPosition() + (int)Mathf.Sign(newPosition));
                                            Debug.Log(TextUtils.GetColorText(thisActor.name, Color.cyan) + " a prit la place de " + TextUtils.GetColorText(thisOtherActor.name, Color.green) + " et sera d�placer " + GetDirectionOfMovement());
                                        }
                                    }
                                }
                            }
                        }
                    }

                    //Check si la valeur reste dans le plateau. A VOIR POUR GARDER CETTE PARTIS SI C'EST PAS POSSIBLE.
                    for (int i = 0; i > newPosition; i--)
                    {
                        //Detection de si le perso est au bord (� gauche).
                        if (thisActor.GetPosition() - i < 0)
                        {
                            //Si il y a d'autre actor, check si ces dernier vont etre d�plac�.
                            foreach (C_Actor thisOtherActor in otherActor)
                            {
                                //D�tection de si il y a un autres actor.
                                if (plateau.Count - 1 - i == thisOtherActor.GetPosition())
                                {
                                    //Check si c'est une TP (donc un switch) ou un d�placement normal (pousse le personnage).
                                    if (myMove.isTp)
                                    {
                                        thisOtherActor.MoveActor(plateau, thisActor.GetPosition());
                                        Debug.Log(TextUtils.GetColorText(thisActor.name, Color.cyan) + " a �chang� sa place avec " + TextUtils.GetColorText(thisOtherActor.name, Color.green) + ".");
                                    }
                                    else
                                    {
                                        thisOtherActor.MoveActor(plateau, thisOtherActor.GetPosition() + (int)Mathf.Sign(newPosition));
                                        Debug.Log(TextUtils.GetColorText(thisActor.name, Color.cyan) + " a prit la place de " + TextUtils.GetColorText(thisOtherActor.name, Color.green) + " et sera d�placer " + GetDirectionOfMovement());
                                    }
                                }
                            }
                        }
                        else
                        {
                            //Si il y a d'autre actor, check si ces dernier vont etre d�plac�.
                            foreach (C_Actor thisOtherActor in otherActor)
                            {
                                if (thisActor != thisOtherActor)
                                {
                                    //D�tection de si il y a un autres actor.
                                    if (thisActor.GetPosition() + i == thisOtherActor.GetPosition())
                                    {
                                        //Check si c'est une TP (donc un switch) ou un d�placement normal (pousse le personnage).
                                        if (myMove.isTp)
                                        {
                                            thisOtherActor.MoveActor(plateau, thisActor.GetPosition());
                                            Debug.Log(TextUtils.GetColorText(thisActor.name, Color.cyan) + " a �chang� sa place avec " + TextUtils.GetColorText(thisOtherActor.name, Color.green) + ".");
                                        }
                                        else
                                        {
                                            thisOtherActor.MoveActor(plateau, thisOtherActor.GetPosition() + (int)Mathf.Sign(newPosition));
                                            Debug.Log(TextUtils.GetColorText(thisActor.name, Color.cyan) + " a prit la place de " + TextUtils.GetColorText(thisOtherActor.name, Color.green) + " et sera d�placer " + GetDirectionOfMovement());
                                        }
                                    }
                                }
                            }
                        }
                    }

                    //D�place l'actor.
                    thisActor.MoveActor(plateau, thisActor.GetPosition() + newPosition);

                    string GetDirectionOfMovement()
                    {
                        if (newPosition < 0)
                        {
                            return " � gauche.";
                        }
                        else if (newPosition > 0)
                        {
                            return " � droite.";
                        }

                        return "Direction Inconu.";
                    }
                }

                void TargetMoveActor(int newPosition, List<C_Case> plateau)
                {
                    foreach (C_Actor thisOtherActor in otherActor)
                    {
                        //D�tection de si il y a un autres actor.
                        if (thisActor.GetPosition() + newPosition == thisOtherActor.GetPosition())
                        {
                            //Check si c'est une TP (donc un swtich) ou un d�placement normal (pousse le personnage).
                            if (myMove.isTp)
                            {
                                thisOtherActor.MoveActor(plateau, thisActor.GetPosition());
                                Debug.Log(TextUtils.GetColorText(thisActor.name, Color.cyan) + " a �chang� sa place avec " + TextUtils.GetColorText(thisOtherActor.name, Color.green) + ".");
                            }
                            else
                            {
                                thisOtherActor.MoveActor(plateau, newPosition + (int)Mathf.Sign(newPosition));
                                Debug.Log(TextUtils.GetColorText(thisActor.name, Color.cyan) + " a prit la place de " + TextUtils.GetColorText(thisOtherActor.name, Color.green) + " et sera d�placer " + GetDirectionOfMovement());
                            }
                        }
                    }

                    thisActor.MoveActor(plateau, newPosition);

                    string GetDirectionOfMovement()
                    {
                        if (newPosition < 0)
                        {
                            return " � gauche.";
                        }
                        else if (newPosition > 0)
                        {
                            return " � droite.";
                        }

                        return "Direction Inconu.";
                    }
                }
            }
        }
        else
        {
            //Pour toutes les liste d'action.
            foreach (Interaction thisInteraction in listInteraction)
            {
                //Check si sont enum est �gale � "Self".
                if (thisInteraction.whatTarget == target)
                {
                    //Pour toutes les list de stats.
                    foreach (TargetStats thisStats in thisInteraction.listTargetStats)
                    {
                        //Check si sont enum est �gale � "Movement".
                        if (thisStats.whatStatsTarget == TargetStats.ETypeStatsTarget.Movement)
                        {
                            if (thisStats.move.whatMove == Move.ETypeMove.SwitchWithActor)
                            {
                                if (thisStats.move.actor != null)
                                {
                                    thisStats.move.actor.MoveActor(plateau, thisActor.GetPosition());
                                    Debug.Log(TextUtils.GetColorText(thisActor.name, Color.cyan) + " va �changer sa place avec " + TextUtils.GetColorText(GetSwitchActor(target).name, Color.cyan) + ".");
                                }
                                else { Debug.LogWarning(thisStats.move.actor); }
                            }
                            else if (thisStats.move.whatMove == Move.ETypeMove.SwitchWithAcc)
                            {
                                if (thisStats.move.accessories != null)
                                {
                                    thisStats.move.accessories.MoveActor(plateau, thisActor.GetPosition());
                                    Debug.Log(TextUtils.GetColorText(thisActor.name, Color.cyan) + " va �changer sa place avec " + TextUtils.GetColorText(GetSwitchAcc(target).name, Color.cyan) + ".");
                                }
                                else { Debug.LogWarning(thisStats.move.accessories); }
                            }
                        }
                    }
                }
            }

            thisActor.MoveActor(plateau, thisActor.GetPosition());
        }
        #endregion
    }

    //Affiche une preview sur les autres actor.
    void SetStatsOther(List<C_Actor> otherActor, C_Actor thisActor, int range, List<C_Case> plateau)
    {
        if (!GetIfTargetOrNot())
        {
            //Boucle avec le range.
            for (int i = 1; i < range; i++)
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
                                SetStatsTarget(Interaction.ETypeTarget.Other, otherActor, thisOtherActor, plateau);
                            }
                            Debug.Log("Direction Range = droite.");
                            break;
                        case Interaction.ETypeDirectionTarget.Left:
                            //Calcul vers la gauche.
                            if (CheckPositionOther(thisActor, -i, plateau, thisOtherActor))
                            {
                                SetStatsTarget(Interaction.ETypeTarget.Other, otherActor, thisOtherActor, plateau);
                            }
                            Debug.Log("Direction Range = Gauche.");
                            break;
                        case Interaction.ETypeDirectionTarget.RightAndLeft:
                            //Calcul vers la droite + gauche.
                            if (CheckPositionOther(thisActor, i, plateau, thisOtherActor))
                            {
                                SetStatsTarget(Interaction.ETypeTarget.Other, otherActor, thisOtherActor, plateau);
                            }
                            if (CheckPositionOther(thisActor, -i, plateau, thisOtherActor))
                            {
                                SetStatsTarget(Interaction.ETypeTarget.Other, otherActor, thisOtherActor, plateau);
                            }
                            Debug.Log("Direction Range = droite + gauche.");
                            break;
                    }
                }
            }
        }
        else
        {
            if (GetTarget().GetComponent<C_Actor>())
            {
                SetStatsTarget(Interaction.ETypeTarget.Other, otherActor, GetTarget().GetComponent<C_Actor>(), plateau);
            }
            else if (GetTarget().GetComponent<C_Accessories>())
            {
                SetTarget(GameObject.Find(GetTarget().GetComponent<C_Accessories>().GetDataAcc().name));

                SetStatsTarget(Interaction.ETypeTarget.Other, otherActor, GetTarget().GetComponent<C_Accessories>(), plateau);
            }
        }
    }
    #endregion
}

#region Conditions avanc�
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

    //Pour un selection avanc� des cibles.
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