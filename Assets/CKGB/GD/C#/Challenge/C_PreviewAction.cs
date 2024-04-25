using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static C_Challenge;
using static UnityEngine.GraphicsBuffer;

public class C_PreviewAction : MonoBehaviour
{
    //Note personel :
    //Il faut que le challenge au moment ou le joueur survol les actions, inscrit les preview necessaire dans puis le challenge demande à ce script de lancer la preview.
    //3 preview sont possible : Preview texte ; Preview stats ui ; Preview déplacement
    //Il y a aussi des preview pour les autres.
    //La preview à besoin de récupérer les info de l'action.

    public static event Action onPreview;

    //Fonction qui va lancer le setup de preview. CHANGER L'ENTREE CAR ON VEUT RECUP L'ACTOR SELECT PENDANT LA PHASE DU JOUEUR + L'ACTION QUE LE JOUEUR SURVOLE.
    void SetupPreview(ActorResolution thisActorResolution)
    {

        SO_ActionClass thisActionClass = thisActorResolution.button.GetActionClass();

        C_Actor thisActor = thisActorResolution.actor;

        //Check si la liste n'est pas vide
        if (thisActionClass.newListInteractions.Count != 0)
        {
            //Pour "Self".
            SetupPreview(Interaction_NewInspector.ETypeTarget.Self, thisActor);

            //Pour "Other".
            SetupPreview(Interaction_NewInspector.ETypeTarget.Other, thisActor);
        }

        void SetupPreview(Interaction_NewInspector.ETypeTarget target, C_Actor thisActor)
        {
            foreach (Interaction_NewInspector thisInteraction in thisActionClass.newListInteractions)
            {
                //Check si c'est égale à "actorTarget".
                if (thisInteraction.whatTarget == target)
                {
                    //Applique à l'actor SEULEMENT LES STATS les stats.
                    foreach (TargetStats_NewInspector thisTargetStats in thisInteraction.listTargetStats)
                    {
                        //Check si c'est des stats ou un Mouvement.
                        if (thisTargetStats.whatStatsTarget == TargetStats_NewInspector.ETypeStatsTarget.Stats)
                        {
                            int value;

                            if (thisTargetStats.dataStats.whatCost == Stats_NewInspector.ETypeCost.Price)
                            {
                                //Retourne une valeur négative.
                                value = -thisTargetStats.dataStats.value;
                            }
                            else
                            {
                                //Retourne une valeur positive.
                                value = thisTargetStats.dataStats.value;
                            }

                            //Envoie une preivew des stats. INUTILE !!!
                            //thisActor.GetComponent<C_Actor>().SetCurrentStats(value, thisTargetStats.dataStats.whatStats);

                            //Inscrit la preview de texte + ui. Avec les info de preview.
                        }
                        else if (thisTargetStats.whatStatsTarget == TargetStats_NewInspector.ETypeStatsTarget.Movement)
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

    void TextPreview()
    {

    }
    void UiPreview()
    {

    }
    void MovePreview()
    {

    }

    public void ShowPreview()
    {
        onPreview?.Invoke();
    }
}
