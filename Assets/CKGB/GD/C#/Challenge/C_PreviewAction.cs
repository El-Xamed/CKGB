using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class C_PreviewAction : MonoBehaviour
{
    public static event Action<SO_ActionClass> onPreview;

    [SerializeField] List<GameObject> listLogsTextPreview = new List<GameObject>();
    [SerializeField] Image previewBarre;

    #region Fonctions
    //Fonction qui va lancer le setup de preview.
    public void ShowPreview(SO_ActionClass thisActionClass, List<C_Actor> myTeam)
    {
        Debug.Log("ShowPreview");

        //D�truit toutes les preview.
        DestroyAllPreview(myTeam);

        //Lance la preview du text.
        PreviewText(thisActionClass);

        //Affiche les preview (stats + plateau).
        GetComponent<C_Challenge>().PreviewPlateau(thisActionClass);

        #region Barre
        //Active l'image de la barre.
        if (!previewBarre.IsActive())
        {
            ActivePreviewBarre(true);
        }
        #endregion

        //Appelle l'action event pour tous afficher.
        onPreview?.Invoke(thisActionClass);
    }

    public void DestroyAllPreview(List<C_Actor> myTeam)
    {
        Debug.Log("Destruction des preview");

        //Desactive toutes les preview des actor.
        foreach (C_Actor thisActor in myTeam)
        {
            thisActor.GetUiStats().ResetUiPreview();
        }

        //Detruit toutes les preview de movement.
        GetComponent<C_Challenge>().DestroyAllMovementPreview();

        if (listLogsTextPreview.Count > -1)
        {
            foreach (GameObject thisLogs in listLogsTextPreview)
            {
                Destroy(thisLogs);
            }

            listLogsTextPreview.Clear();
        }
    }

    public void ActivePreviewBarre(bool value)
    {
        previewBarre.gameObject.SetActive(value);
    }

    void PreviewText(SO_ActionClass thisActionClass)
    {
        //Cr�ation d'une liste d'info qui va etre affich� dans les logs de preview.
        List<string> listTextPreview = new List<string>();

        #region Variable
        //Pour le premier texte.
        //Cr�ation du symbol qui va etre plac� au d�but du texte.
        string whatSigne = "";
        string whatStats = "";
        string whatValue = "";
        //Cr�ation d'un string pour appliquer la cible.
        string whatTarget = "";

        //Pour le deuxi�me texte.
        //Cr�ation d'un string pour connaitre quelle action va se d�rouler.
        string actorTarget = "";
        string descriptionPreview = "";
        #endregion

        //Check si dans l'action il y a des info. Si oui, avoir un system qui pr�pare le texte.
        foreach (Interaction thisInteraction in thisActionClass.listInteraction)
        {
            //Check dans toute la liste.
            foreach (TargetStats thisTargetStats in thisInteraction.listTargetStats)
            {
                //Cr�ation d'un int pour setup la bonne couleur de sprite.
                int color = 0;

                //Setup la valeur du premier texte.
                whatValue = thisTargetStats.value.ToString();

                //Cr�ation d'un string setup la fin de la phrase.
                string endPreviewText = "";

                //Check si c'est une stats ou un d�placement.
                if (thisTargetStats.whatStatsTarget == TargetStats.ETypeStatsTarget.Stats)
                {
                    if (thisTargetStats.whatStats == TargetStats.ETypeStats.Calm) //Check si c'est pour le calm.
                    {
                        whatStats = "<sprite index=[index] tint=46>";

                        color = 31;

                        endPreviewText = " de calme.";
                    }
                    else if (thisTargetStats.whatStats == TargetStats.ETypeStats.Energy) //Check si c'est pour l'energie.
                    {
                        whatStats = "<sprite index=[index] tint=30>";

                        color = 15;

                        endPreviewText = " d'�nergie.";
                    }

                    #region Setup symbole
                    switch (thisTargetStats.whatCost)
                    {
                        case TargetStats.ETypeCost.Gain:
                            //Setup le bon symbol (fleche droite).
                            whatSigne = "<sprite index=[index] tint= " + (4 + color) + ">";

                            descriptionPreview = " gagner ";
                            break;
                        case TargetStats.ETypeCost.Price:
                            //Setup le bon symbol (fleche gauche).
                            whatSigne = "<sprite index=[index] tint= " + (5 + color) + ">";

                            descriptionPreview = " perdre ";
                            break;
                    }
                    #endregion
                }
                else if (thisTargetStats.whatStatsTarget == TargetStats.ETypeStatsTarget.Movement)
                {
                    #region Setup string Action
                    whatStats = "<sprite index=[index] tint=12>";

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
                            descriptionPreview = " va se d�placer sur la case ";
                            whatSigne = "<sprite index=[index] tint=10>";

                            endPreviewText = ".";
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

                #region Target
                //Setup la target.
                if (thisInteraction.whatTarget == Interaction.ETypeTarget.Soi)
                {
                    actorTarget = "L'utilisateur.ice va";

                    //Setup le string de la target "(Soi)".
                    whatTarget = " (Soi)";
                }
                else if (thisInteraction.whatTarget == Interaction.ETypeTarget.Other)
                {
                    actorTarget = "Les autres utilisateur.ice impact�s vont";

                    string other = "";

                    #region Check le mode de direction.
                    switch (thisInteraction.whatDirectionTarget)
                    {
                        case Interaction.ETypeDirectionTarget.Right:
                            other += "<sprite index=[index] tint= " + (9 + color) + ">";
                            for (int i = 0; i < thisInteraction.range; i++)
                            {
                                other += "<sprite index=[index] tint= " + (12 + color) + ">";
                            }
                            break;
                        case Interaction.ETypeDirectionTarget.Left:
                            for (int i = 0; i < thisInteraction.range; i++)
                            {
                                other += "<sprite index=[index] tint= " + (12 + color) + ">";
                            }
                            other += "<sprite index=[index] tint= " + (8 + color) + ">";
                            break;
                        case Interaction.ETypeDirectionTarget.RightAndLeft:
                            for (int i = 0; i < thisInteraction.range; i++)
                            {
                                other += "<sprite index=[index] tint= " + (12 + color) + ">";
                            }
                            other += "<sprite index=[index] tint= " + (8 + color) + ">";
                            other += "<sprite index=[index] tint= " + (9 + color) + ">";
                            for (int i = 0; i < thisInteraction.range; i++)
                            {
                                other += "<sprite index=[index] tint= " + (12 + color) + ">";
                            }
                            break;
                    }
                    #endregion

                    whatTarget = "(" + other + ")";
                }
                #endregion

                descriptionPreview += whatValue + endPreviewText;

                #region Final Text
                //Ajoute le premier texte.
                listTextPreview.Add(whatSigne + whatValue + whatStats + whatTarget);
                //Ajoute le deuxi�me texte.
                listTextPreview.Add(actorTarget + descriptionPreview);
                #endregion
            }
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
    }
    #endregion
}
