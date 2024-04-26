using System;
using UnityEngine;
using static C_Challenge;

public class C_PreviewAction : MonoBehaviour
{
    //Note personel :
    //Il faut que le challenge au moment ou le joueur survol les actions, inscrit les preview necessaire dans puis le challenge demande à ce script de lancer la preview.
    //3 preview sont possible : Preview texte ; Preview stats ui ; Preview déplacement
    //Il y a aussi des preview pour les autres.
    //La preview à besoin de récupérer les info de l'action.

    public static event Action<SO_ActionClass, C_Actor> onPreview;

    //Fonction qui va lancer le setup de preview. CHANGER L'ENTREE CAR ON VEUT RECUP L'ACTOR SELECT PENDANT LA PHASE DU JOUEUR + L'ACTION QUE LE JOUEUR SURVOLE.
    public void ShowPreview(SO_ActionClass thisActionClass, C_Actor thisActor)
    {
        Debug.Log("Début de preview !");

        //Check si la liste n'est pas vide
        if (thisActionClass.newListInteractions.Count != 0)
        {
            Debug.Log("La liste n'est pas vide !");

            //Pour "Self".
            SetupPreview(Interaction_NewInspector.ETypeTarget.Self, thisActor);

            //Pour "Other".
            SetupPreview(Interaction_NewInspector.ETypeTarget.Other, thisActor);
        }

        Debug.Log("Tentative");
        onPreview?.Invoke(thisActionClass, thisActor);

        void SetupPreview(Interaction_NewInspector.ETypeTarget target, C_Actor thisActor)
        {
            Debug.Log("Preview pour " + thisActor);

            foreach (Interaction_NewInspector thisInteraction in thisActionClass.newListInteractions)
            {
                //Check si c'est égale à "actorTarget".
                if (thisInteraction.whatTarget == target)
                {
                    foreach (TargetStats_NewInspector thisTargetStats in thisInteraction.listTargetStats)
                    {
                        //Check si c'est des stats ou un Mouvement.
                        if (thisTargetStats.whatStatsTarget == TargetStats_NewInspector.ETypeStatsTarget.Stats)
                        {
                            //Inscrit la preview de texte + ui. Avec les info de preview. (C_Challenge)
                            //onPreview += TextPreview;

                            //Inscrit la preview de stats. (C_Stats)
                            onPreview += thisActor.GetUiStats().UiPreview;
                        }
                        else if (thisTargetStats.whatStatsTarget == TargetStats_NewInspector.ETypeStatsTarget.Movement) //Si c'est un mouvement, alors il lance la preview de déplacement.
                        {
                            //Inscrit la preview de mouvement. Avec les info de preview.
                        }
                    }
                }
            }
        }

        //Regarde les info de l'action
        /*
        //Check si il y a des info de stats.
        if (thisActionClass.AffectStats())
        {
            //Inscrit la preview de texte.
            onPreview += TextPreview;

            //Inscrit la preview de stats
            onPreview += UiPreview;
        }

        //Check si il y a des info de mouvement.
        if (thisActionClass.AffectMovement())
        {
            //Inscrit la preview de mouvement.
            onPreview += MovePreview;
        }
        */
        //Lance la preview
    }

    //A placer ailleur.
    void TextPreview()
    {

    }
    
    void MovePreview()
    {

    }
}
