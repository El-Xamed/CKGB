using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class C_PreviewAction : MonoBehaviour
{
    //Note personel :
    //Il faut que le challenge au moment ou le joueur survol les actions, inscrit les preview necessaire dans puis le challenge demande à ce script de lancer la preview.
    //3 preview sont possible : Preview texte ; Preview stats ui ; Preview déplacement
    //Il y a aussi des preview pour les autres.
    //La preview à besoin de récupérer les info de l'action.

    public static event Action onPreview;

    //Fonction qui va lancer le setup de preview.
    void SetupPreview(SO_ActionClass_NewInspector thisActionClass)
    {
        /*
        //Check si la liste n'est pas vide
        if (listInteraction.Count != 0)
        {
            //Fait toute la liste des cible.
            foreach (Interaction_NewInspector thisInteraction in thisActionClass.listInteraction)
            {
                //Check si c'est égale à "actorTarget".
                if (thisInteraction.whatTarget == target)
                {
                    //Applique à l'actor SEULEMENT LES STATS les stats.
                    foreach (TargetStats_NewInspector thisTargetStats in thisInteraction.listTargetStats)
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

                        //Envoie les nouvelles information a l'actor.
                        thisActor.GetComponent<C_Actor>().SetCurrentStats(value, thisTargetStats.dataStats.whatStats);
                    }
                }
            }
        }*/


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
