using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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

        //Création d'une liste d'info qui va etre affiché dans les logs de preview.
        List<string> listTextPreview = new List<string>();

        //Check si dans l'action il y a des info. Si oui, avoir un sytem qui prépar le texte.
        foreach (Interaction thisInteraction in thisActionClass.listInteraction)
        {
            //Check si il y a des info pour "self".
            if (thisInteraction.whatTarget == Interaction.ETypeTarget.Self)
            {
                //Check dans toute la liste.
                foreach (TargetStats thisTargetStats in thisInteraction.listTargetStats)
                {
                    //Check si c'est des stats ou un Mouvement.
                    if (thisTargetStats.whatStatsTarget == TargetStats.ETypeStatsTarget.Stats) //Check si il a des info de stats.
                    {
                        //Check si c'est pour le calm.
                        if (thisTargetStats.whatStats == TargetStats.ETypeStats.Calm)
                        {
                            //Inscrit la preview de calm.
                            onPreview += thisActor.GetUiStats().UiPreviewCalm;
                            Debug.Log("Add UiPreviewCalm");

                            //Ajoute du texte dans la liste de "listTextPreview".
                            //Version avec le nom de l'actor.
                            //listTextPreview.Add(thisActor.name + GetIfPriceOrGain(thisActionClass.GetValue(Interaction.ETypeTarget.Self, TargetStats.ETypeStatsTarget.Stats)) + " de calme.");
                            //Version Sans le nom du perso.
                            listTextPreview.Add("L'utilisateur.ice" + GetIfPriceOrGain(thisActionClass.GetValue(Interaction.ETypeTarget.Self, TargetStats.ETypeStatsTarget.Stats)) + " de calme.");
                        }

                        //Check si c'est pour l'energie.
                        if (thisTargetStats.whatStats == TargetStats.ETypeStats.Energy)
                        {
                            //Inscrit la preview de calm.
                            onPreview += thisActor.GetUiStats().UiPreviewEnergy;
                            Debug.Log("Add UiPreviewEnergy");

                            //Ajoute du texte dans la liste de "listTextPreview".
                            listTextPreview.Add("L'utilisateur.ice" + GetIfPriceOrGain(thisActionClass.GetValue(Interaction.ETypeTarget.Self, TargetStats.ETypeStatsTarget.Stats)) + " d'energie.");
                        }
                    }
                    else if (thisTargetStats.whatStatsTarget == TargetStats.ETypeStatsTarget.Movement) //Check si il a des info de movement.
                    {
                        //onPreview += GetComponent<C_Challenge>().MovementPreview;
                    }
                }
            }

            if (thisInteraction.whatTarget == Interaction.ETypeTarget.Other)
            {
                //Cherche les autre perso à proximité.

            }
        }

        //Pour toutes les variables stocké dans "listTextPreview", l'afficher dans les logs.
        for (int i = 0; i < listTextPreview.Count; i++)
        {
            #region Raccourcis
            GameObject textPreview = GetComponent<C_Challenge>().GetTextPreviewPrefab();
            Transform transformPreview = GetComponent<C_Challenge>().GetTransformPreview();
            #endregion

            //Créer un nouveau texte dans les logs de preview.
            GameObject newLogs = Instantiate(textPreview, transformPreview);

            newLogs.name = "Ui_Logs_Text_Custom_" + i;

            newLogs.transform.GetChild(1).GetComponent<TMP_Text>().text = listTextPreview[0];

            listLogsTextPreview.Add(newLogs);

            //Check si les info dépasse pas
            //Si les info dépasse, ouvrir la feuille qui pourra etre fermé pour un input (BESOIN DE LE SETUP DANS L'INTERFACE !).
        }

        /*Meme chose mais avec une boucle foreach.
        foreach (string thisTextPreview in listTextPreview)
        {
            #region Raccourcis
            GameObject textPreview = GetComponent<C_Challenge>().GetTextPreviewPrefab();
            Transform transformPreview = GetComponent<C_Challenge>().GetTransformPreview();
            #endregion

            //Créer un nouveau texte dans les logs de preview.
            GameObject newLogs = Instantiate(textPreview, transformPreview);

            newLogs.name = "Ui_Logs_Text_Custom_X";

            //Check si les info dépasse pas
            //Si les info dépasse, ouvrir la feuille qui pourra etre fermé pour un input (BESOIN DE LE SETUP DANS L'INTERFACE !).
        }*/




        //Check si il y a des info pour "other".
        //Check si il a des info de stats.
        //Check si il a des info de movement.

        //Appelle l'action event pour tous afficher.
        onPreview?.Invoke(thisActionClass);





        /* A SUPP
        //Pour "Self". FAIRE EN SORTE QUE CA LE FAIT POUR LES 2.
        SetupPreview(Interaction.ETypeTarget.Self, thisActor);

        //Pour "Other".
        //SetupPreview(Interaction.ETypeTarget.Other, thisActor);

        onPreview?.Invoke(thisActionClass);

        void SetupPreview(Interaction.ETypeTarget target, C_Actor thisActor)
        {
            //Reset les preview des stats.
            thisActor.GetUiStats().ResetUiPreview();

            foreach (Interaction thisInteraction in thisActionClass.listInteraction)
            {
                //Check si c'est égale à "actorTarget".
                if (thisInteraction.whatTarget == target)
                {
                    //Envoie l'action au script de stats.
                    thisActor.GetUiStats().CheckUiPreview(thisActionClass, target);

                    GetComponent<C_Challenge>().CheckPreview(thisActionClass, target);
                }
            }
        }*/
    }

    string GetIfPriceOrGain(int value)
    {
        string newString = "";

        if (value < 0)
        {
            newString += " va perdre ";
        }
        else if (value > 0)
        {
            newString += " va gagner ";
        }

        return newString += value;
    }
}
