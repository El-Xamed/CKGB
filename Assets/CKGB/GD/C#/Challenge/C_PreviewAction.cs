using System;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class C_PreviewAction : MonoBehaviour
{
    //Note personel :
    //Il faut que le challenge au moment ou le joueur survol les actions, inscrit les preview necessaire dans puis le challenge demande � ce script de lancer la preview.
    //3 preview sont possible : Preview texte ; Preview stats ui ; Preview d�placement
    //Il y a aussi des preview pour les autres.
    //La preview � besoin de r�cup�rer les info de l'action.

    public static event Action<SO_ActionClass> onPreview;

    [SerializeField] List<GameObject> listLogsTextPreview = new List<GameObject>();

    //Fonction qui va lancer le setup de preview. CHANGER L'ENTREE CAR ON VEUT RECUP L'ACTOR SELECT PENDANT LA PHASE DU JOUEUR + L'ACTION QUE LE JOUEUR SURVOLE.
    public void ShowPreview(SO_ActionClass thisActionClass, C_Actor thisActor)
    {
        Debug.Log("ShowPreview");

        if (listLogsTextPreview.Count > -1)
        {
            foreach (GameObject thisLogs in listLogsTextPreview)
            {
                Destroy(thisLogs);
            }

            listLogsTextPreview.Clear();
        }

        //Desactive par default les preview de l'actor selectionne.
        thisActor.GetUiStats().ResetUiPreview();

        //Supprime tout les texte dans les logs de preview.
        //FAIRE LE DEV !!!

        //Cr�ation d'une liste d'info qui va etre affich� dans les logs de preview.
        List<string> listTextPreview = new List<string>();

        //Check si dans l'action il y a des info. Si oui, avoir un system qui pr�pare le texte.
        foreach (Interaction thisInteraction in thisActionClass.listInteraction)
        {
            #region Variable
            //Pour le premier texte.
            //Cr�ation du symbol qui va etre plac� au d�but du texte.
            string whatSigne = "";
            string whatValue = "";
            //Cr�ation d'un string pour appliquer la cible.
            string whatTarget = "";

            //Pour le deuxi�me texte.
            //Cr�ation d'un string pour connaitre quelle action va se d�rouler.
            string whatAction = "";
            #endregion

            //Check dans toute la liste.
            foreach (TargetStats thisTargetStats in thisInteraction.listTargetStats)
            {
                //Setup la target.
                whatTarget = SetTarget(thisInteraction.whatTarget);

                //Setup le signe et la valeur du premier texte.
                whatSigne = GetSign(thisActionClass.GetValue(thisInteraction.whatTarget, thisTargetStats.whatStatsTarget));
                //Setup la valeur du premier texte.
                whatValue = GetValue(thisActionClass.GetValue(thisInteraction.whatTarget, thisTargetStats.whatStatsTarget));

                //Setup la description de l'action du deuxi�me texte + la fin du texte.
                whatAction = GetWhatAction(thisTargetStats.whatStatsTarget, thisActionClass.GetValue(thisInteraction.whatTarget, thisTargetStats.whatStatsTarget));

                #region Fonction
                //Fonction qui permet de setup l'action et la fin du deuxi�me texte.
                string GetWhatAction(TargetStats.ETypeStatsTarget whatStatsTarget, int value)
                {
                    string descriptionPreview = "";

                    //Cr�ation d'un string setup la fin de la phrase.
                    string endPreviewText = "";

                    //Check si c'est une stats ou un d�placement.
                    if (whatStatsTarget == TargetStats.ETypeStatsTarget.Stats)
                    {
                        #region Setup string action
                        if (value < 0)
                        {
                            descriptionPreview = " va perdre ";
                        }
                        else if (value > 0)
                        {
                            descriptionPreview = " va gagner ";
                        }
                        #endregion

                        #region Inscription
                        if (thisTargetStats.whatStats == TargetStats.ETypeStats.Calm) //Check si c'est pour le calm.
                        {
                            //Inscrit la preview de calm.
                            onPreview += thisActor.GetUiStats().UiPreviewCalm;
                            Debug.Log("Add UiPreviewCalm");

                            endPreviewText = " de calme.";
                        }
                        else if (thisTargetStats.whatStats == TargetStats.ETypeStats.Energy) //Check si c'est pour l'energie.
                        {
                            //Inscrit la preview de calm.
                            onPreview += thisActor.GetUiStats().UiPreviewEnergy;
                            Debug.Log("Add UiPreviewEnergy");

                            endPreviewText = " d'�nergie.";
                        }
                        #endregion
                    }
                    else if (whatStatsTarget == TargetStats.ETypeStatsTarget.Movement)
                    {
                        #region Setup string Action
                        descriptionPreview = " va se d�placer de ";

                        //Check c'est quoi comme type de mouvement. OPTIMISER LE DEV !!!
                        switch (thisTargetStats.whatMove)
                        {
                            case TargetStats.ETypeMove.Right:
                                //Setup le bon symbol (fleche droite).
                                whatSigne = "<sprite index=[index] tint=9>";
                                //Setup la fin de la phrase.
                                endPreviewText = " vers la droite.";
                                break;
                            case TargetStats.ETypeMove.Left:
                                //Setup le bon symbol (fleche gauche).
                                whatSigne = "<sprite index=[index] tint=8>";
                                //Setup la fin de la phrase.
                                endPreviewText = " vers la gauche.";
                                break;
                            case TargetStats.ETypeMove.OnTargetCase:
                                //Setup le bon symbol (t�l�portation).
                                whatSigne = "<sprite index=[index] tint=10>";

                                //Check si il y a un autre actor.
                                //Si oui alors le symbol sera dif�rrent.
                                //Sinon il ne change pas.
                                break;
                            case TargetStats.ETypeMove.SwitchWithActor:
                                //Setup le bon symbol (�change de place).
                                whatSigne = "<sprite index=[index] tint=1>";
                                break;
                            case TargetStats.ETypeMove.SwitchWithAcc:
                                //Setup le bon symbol (�change de place).
                                whatSigne = "<sprite index=[index] tint=1>";
                                break;
                        }
                        #endregion
                    }

                    //Check si la valeur est n�gative pour retirer le signe.
                    if (value < 0)
                    {
                        value = -value;
                    }

                    descriptionPreview += value + endPreviewText;

                    return descriptionPreview;
                }
                #endregion

                /*ANCIEN DEV !!!
                //Check si c'est des stats ou un Mouvement.
                if (thisTargetStats.whatStatsTarget == TargetStats.ETypeStatsTarget.Stats) //Check si il a des info de stats.
                {
                    if (thisTargetStats.whatStats == TargetStats.ETypeStats.Calm) //Check si c'est pour le calm.
                    {
                        //Inscrit la preview de calm.
                        onPreview += thisActor.GetUiStats().UiPreviewCalm;
                        Debug.Log("Add UiPreviewCalm");

                        endPreviewText = " de calme.";
                    }
                    else if (thisTargetStats.whatStats == TargetStats.ETypeStats.Energy) //Check si c'est pour l'energie.
                    {
                        //Inscrit la preview de calm.
                        onPreview += thisActor.GetUiStats().UiPreviewEnergy;
                        Debug.Log("Add UiPreviewEnergy");

                        endPreviewText = " d'�nergie.";
                    }
                }
                else if (thisTargetStats.whatStatsTarget == TargetStats.ETypeStatsTarget.Movement) //Check si il a des info de movement.
                {
                    //onPreview += GetComponent<C_Challenge>().MovementPreview;

                    //Check c'est quoi comme type de mouvement. OPTIMISER LE DEV !!!
                    switch (thisTargetStats.whatMove)
                    {
                        case TargetStats.ETypeMove.Right:
                            //Setup le bon symbol (fleche droite).
                            whatSigne = "<sprite index=[index] tint=9>";
                            //Setup la fin de la phrase.
                            endPreviewText = " vers la droite.";
                            break;
                        case TargetStats.ETypeMove.Left:
                            //Setup le bon symbol (fleche gauche).
                            whatSigne = "<sprite index=[index] tint=8>";
                            //Setup la fin de la phrase.
                            endPreviewText = " vers la gauche.";
                            break;
                        case TargetStats.ETypeMove.OnTargetCase:
                            //Setup le bon symbol (t�l�portation).
                            whatSigne = "<sprite index=[index] tint=10>";

                            //Check si il y a un autre actor.
                                //Si oui alors le symbol sera dif�rrent.
                                //Sinon il ne change pas.
                            break;
                        case TargetStats.ETypeMove.SwitchWithActor:
                            //Setup le bon symbol (�change de place).
                            whatSigne = "<sprite index=[index] tint=1>";
                            break;
                        case TargetStats.ETypeMove.SwitchWithAcc:
                            //Setup le bon symbol (�change de place).
                            whatSigne = "<sprite index=[index] tint=1>";
                            break;
                    }
                }*/
            }

            #region Final Text
            //Ajoute le premier texte.
            listTextPreview.Add(whatSigne + whatValue + whatTarget);
            //Ajoute le deuxi�me texte.
            listTextPreview.Add("L'utilisateur.ice" + whatAction);
            #endregion
        }

        #region Cr�ation des textes dans l'Ui.
        //Pour toutes les variables stock� dans "listTextPreview", l'afficher dans les logs.
        for (int i = 0; i < listTextPreview.Count / 2; i++)
        {
            #region Raccourcis
            GameObject textPreview = GetComponent<C_Challenge>().GetTextPreviewPrefab();
            Transform transformPreview = GetComponent<C_Challenge>().GetTransformPreview();

            string text1 = listTextPreview[i * 2];
            string text2 = listTextPreview[1 + (i * 2)];
            #endregion

            //Cr�er un nouveau texte dans les logs de preview.
            GameObject newLogs = Instantiate(textPreview, transformPreview);

            //Rename l'objet.
            newLogs.name = "Ui_Logs_Text_Custom_" + i;

            //Setup le text 1.
            newLogs.transform.GetChild(0).GetComponent<TMP_Text>().text = text1;
            //Setup le text 2.
            newLogs.transform.GetChild(1).GetComponent<TMP_Text>().text = text2;

            //Ajout � la liste de la preview.
            listLogsTextPreview.Add(newLogs);

            //Check si les info d�passe pas
            //Si les info d�passe, ouvrir la feuille qui pourra etre ferm� pour un input (BESOIN DE LE SETUP DANS L'INTERFACE !).
        }
        #endregion

        //Check si il y a des info pour "other".
        //Check si il a des info de stats.
        //Check si il a des info de movement.

        //Appelle l'action event pour tous afficher.
        onPreview?.Invoke(thisActionClass);
    }

    #region Fonctions
    //Fonction qui vient setup le texte de la target dans le premier texte.
    string SetTarget(Interaction.ETypeTarget whatTarget)
    {
        if (whatTarget == Interaction.ETypeTarget.Soi)
        {
            //Setup le string de la target "(Soi)".
            return " (Soi)";
        }
        else if (whatTarget == Interaction.ETypeTarget.Other)
        {

        }

        return "X";
    }

    //Fonction quoi permet de setup le signe du premier texte.
    string GetSign(int value)
    {
        if (value < 0)
        {
            return "<sprite index=[index] tint=5>";
        }
        else if (value > 0)
        {
            return "<sprite index=[index] tint=4>";
        }

        return "X";
    }

    string GetValue(int value)
    {
        //Check si la valeur est n�gative pour retirer son signe.
        if (value < 0)
        {
            return (-value).ToString();
        }
        else
        {
            return value.ToString();
        }
    }
    #endregion
}
