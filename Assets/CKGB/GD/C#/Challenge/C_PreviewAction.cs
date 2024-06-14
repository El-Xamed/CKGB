using Ink.Runtime;
using System;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class C_PreviewAction : MonoBehaviour
{
    //Note personel :
    //Il faut que le challenge au moment ou le joueur survol les actions, inscrit les preview necessaire dans puis le challenge demande à ce script de lancer la preview.
    //3 preview sont possible : Preview texte ; Preview stats ui ; Preview déplacement
    //Il y a aussi des preview pour les autres.
    //La preview à besoin de récupérer les info de l'action.

    public static event Action<SO_ActionClass> onPreview;

    [SerializeField] List<GameObject> listLogsTextPreview = new List<GameObject>();
    [SerializeField] Image previewBarre;

    //Fonction qui va lancer le setup de preview. CHANGER L'ENTREE CAR ON VEUT RECUP L'ACTOR SELECT PENDANT LA PHASE DU JOUEUR + L'ACTION QUE LE JOUEUR SURVOLE.
    public void ShowPreview(SO_ActionClass thisActionClass, C_Actor thisActor)
    {
        Debug.Log("ShowPreview");

        //Active l'image de la barre.
        if (!previewBarre.IsActive())
        {
            ActivePreviewBarre(true);
        }

        //Détruit toutes les preview.
        DestroyAllPreview();

        //Desactive par default les preview de l'actor selectionne.
        thisActor.GetUiStats().ResetUiPreview();

        //Création d'une liste d'info qui va etre affiché dans les logs de preview.
        List<string> listTextPreview = new List<string>();

        #region Variable
        //Pour le premier texte.
        //Création du symbol qui va etre placé au début du texte.
        string whatSigne = "";
        string whatStats = "";
        string whatValue = "";
        //Création d'un string pour appliquer la cible.
        string whatTarget = "";

        //Pour le deuxième texte.
        //Création d'un string pour connaitre quelle action va se dérouler.
        string actorTarget = "";
        string descriptionPreview = "";
        #endregion

        //Check si dans l'action il y a des info. Si oui, avoir un system qui prépare le texte.
        foreach (Interaction thisInteraction in thisActionClass.listInteraction)
        {
            //Check dans toute la liste.
            foreach (TargetStats thisTargetStats in thisInteraction.listTargetStats)
            {
                #region Target
                //Setup la target.
                if (thisInteraction.whatTarget == Interaction.ETypeTarget.Soi)
                {
                    actorTarget = "L'utilisateur.ice";

                    //Setup le string de la target "(Soi)".
                    whatTarget = " (Soi)";
                }
                else if (thisInteraction.whatTarget == Interaction.ETypeTarget.Other)
                {
                    actorTarget = "";

                    string other = "";

                    //Check le mode de direction.
                    switch (thisInteraction.whatDirectionTarget)
                    {
                        case Interaction.ETypeDirectionTarget.Right:
                            other += "<sprite index=[index] tint=9>";
                            for (int i = 0; i < thisInteraction.range; i++)
                            {
                                other += "<sprite index=[index] tint=12>";
                            }
                            break;
                        case Interaction.ETypeDirectionTarget.Left:
                            for (int i = 0; i < thisInteraction.range; i++)
                            {
                                other += "<sprite index=[index] tint=12>";
                            }
                            other += "<sprite index=[index] tint=8>";
                            break;
                        case Interaction.ETypeDirectionTarget.RightAndLeft:
                            for (int i = 0; i < thisInteraction.range; i++)
                            {
                                other += "<sprite index=[index] tint=12>";
                            }
                            other += "<sprite index=[index] tint=8>";
                            other += "<sprite index=[index] tint=9>";
                            for (int i = 0; i < thisInteraction.range; i++)
                            {
                                other += "<sprite index=[index] tint=12>";
                            }
                            break;
                    }

                    whatTarget = "(" + other + ")";
                }
                #endregion

                //Setup la valeur du premier texte.
                whatValue = GetValue(thisActionClass.GetValue(thisInteraction.whatTarget, thisTargetStats.whatStatsTarget));

                //Création d'un string setup la fin de la phrase.
                string endPreviewText = "";

                //Check si c'est une stats ou un déplacement.
                if (thisTargetStats.whatStatsTarget == TargetStats.ETypeStatsTarget.Stats)
                {
                    #region Setup string action
                    switch (thisTargetStats.whatCost)
                    {
                        case TargetStats.ETypeCost.Gain:
                            //Setup le bon symbol (fleche droite).
                            whatSigne = "<sprite index=[index] tint=4>";

                            descriptionPreview = " va gagner ";
                            break;
                        case TargetStats.ETypeCost.Price:
                            //Setup le bon symbol (fleche gauche).
                            whatSigne = "<sprite index=[index] tint=5>";

                            descriptionPreview = " va perdre ";
                            break;
                    }
                    #endregion

                    #region Inscription
                    if (thisTargetStats.whatStats == TargetStats.ETypeStats.Calm) //Check si c'est pour le calm.
                    {
                        //Inscrit la preview de calm.
                        onPreview += thisActor.GetUiStats().UiPreviewCalm;
                        Debug.Log("Add UiPreviewCalm");

                        whatStats = "<sprite index=[index] tint=16>";

                        endPreviewText = " de calme.";
                    }
                    else if (thisTargetStats.whatStats == TargetStats.ETypeStats.Energy) //Check si c'est pour l'energie.
                    {
                        //Inscrit la preview de calm.
                        onPreview += thisActor.GetUiStats().UiPreviewEnergy;
                        Debug.Log("Add UiPreviewEnergy");

                        whatStats = "<sprite index=[index] tint=15>";

                        endPreviewText = " d'énergie.";
                    }
                    #endregion
                }
                else if (thisTargetStats.whatStatsTarget == TargetStats.ETypeStatsTarget.Movement)
                {
                    Debug.Log("Add Preview Movement");

                    #region Setup string Action
                    whatStats = "<sprite index=[index] tint=12>";

                    descriptionPreview = " va se déplacer de ";

                    onPreview += GetComponent<C_Challenge>().MovementPreview;

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
                            //Setup le bon symbol (téléportation).
                            whatSigne = "<sprite index=[index] tint=10>";

                            //Check si il y a un autre actor.
                            //Si oui alors le symbol sera diférrent.
                            //Sinon il ne change pas.
                            break;
                        case TargetStats.ETypeMove.SwitchWithActor:
                            //Setup le bon symbol (échange de place).
                            whatSigne = "<sprite index=[index] tint=1>";
                            break;
                        case TargetStats.ETypeMove.SwitchWithAcc:
                            //Setup le bon symbol (échange de place).
                            whatSigne = "<sprite index=[index] tint=1>";
                            break;
                    }
                    #endregion
                }

                descriptionPreview += whatValue + endPreviewText;

                #region Final Text
                //Ajoute le premier texte.
                listTextPreview.Add(whatSigne + whatValue + whatStats + whatTarget);
                //Ajoute le deuxième texte.
                listTextPreview.Add(actorTarget + descriptionPreview);
                #endregion
            }
        }

        #region Création des textes dans l'Ui.
        //Pour toutes les variables stocké dans "listTextPreview", l'afficher dans les logs.
        for (int i = 0; i < listTextPreview.Count / 2; i++)
        {
            #region Raccourcis
            GameObject textPreview = GetComponent<C_Challenge>().GetTextPreviewPrefab();
            Transform transformPreview = GetComponent<C_Challenge>().GetTransformPreview();

            string text1 = listTextPreview[i * 2];
            string text2 = listTextPreview[1 + (i * 2)];
            #endregion

            //Créer un nouveau texte dans les logs de preview.
            GameObject newLogs = Instantiate(textPreview, transformPreview);

            //Rename l'objet.
            newLogs.name = "Ui_Logs_Text_Custom_" + i;

            //Setup le text 1.
            newLogs.transform.GetChild(0).GetComponent<TMP_Text>().text = text1;
            //Setup le text 2.
            newLogs.transform.GetChild(1).GetComponent<TMP_Text>().text = text2;

            //Ajout à la liste de la preview.
            listLogsTextPreview.Add(newLogs);

            //Check si les info dépasse pas
            //Si les info dépasse, ouvrir la feuille qui pourra etre fermé pour un input (BESOIN DE LE SETUP DANS L'INTERFACE !).
        }
        #endregion

        //Appelle l'action event pour tous afficher.
        onPreview?.Invoke(thisActionClass);
    }

    #region Fonctions
    public void DestroyAllPreview()
    {
        Debug.Log("Destruction des preview");

        if (listLogsTextPreview.Count > -1)
        {
            foreach (GameObject thisLogs in listLogsTextPreview)
            {
                Destroy(thisLogs);
            }

            listLogsTextPreview.Clear();
        }

        GetComponent<C_Challenge>().DestroyAllMovementPreview();
    }

    public void ActivePreviewBarre(bool value)
    {
        previewBarre.gameObject.SetActive(value);
    }

    string GetValue(int value)
    {
        //Check si la valeur est négative pour retirer son signe.
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
