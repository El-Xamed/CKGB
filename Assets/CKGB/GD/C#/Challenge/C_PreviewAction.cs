using System;
using UnityEngine;
using static C_Challenge;

public class C_PreviewAction : MonoBehaviour
{
    //Note personel :
    //Il faut que le challenge au moment ou le joueur survol les actions, inscrit les preview necessaire dans puis le challenge demande � ce script de lancer la preview.
    //3 preview sont possible : Preview texte ; Preview stats ui ; Preview d�placement
    //Il y a aussi des preview pour les autres.
    //La preview � besoin de r�cup�rer les info de l'action.

    public static event Action<SO_ActionClass> onPreview;

    //Fonction qui va lancer le setup de preview. CHANGER L'ENTREE CAR ON VEUT RECUP L'ACTOR SELECT PENDANT LA PHASE DU JOUEUR + L'ACTION QUE LE JOUEUR SURVOLE.
    public void ShowPreview(SO_ActionClass thisActionClass, C_Actor thisActor)
    {
        Debug.Log("D�but de preview !");

        //Check si la liste n'est pas vide
        if (thisActionClass.newListInteractions.Count != 0)
        {
            Debug.Log("La liste n'est pas vide !");

            //Faut qu'il envoie l'actionClass au script qui vont gerer la preview et ensuite c'est eux qui d�cide de s'inscrir ou non.

            //Pour "Self".
            SetupPreview(Interaction_NewInspector.ETypeTarget.Self, thisActor);

            //Pour "Other".
            SetupPreview(Interaction_NewInspector.ETypeTarget.Other, thisActor);
        }

        Debug.Log("Tentative");
        onPreview?.Invoke(thisActionClass);

        void SetupPreview(Interaction_NewInspector.ETypeTarget target, C_Actor thisActor)
        {
            Debug.Log("Preview pour " + thisActor);

            foreach (Interaction_NewInspector thisInteraction in thisActionClass.newListInteractions)
            {
                //Check si c'est �gale � "actorTarget".
                if (thisInteraction.whatTarget == target)
                {
                    //Envoie l'action au script de stats.
                    thisActor.GetUiStats().CheckUiPreview(thisActionClass, target);

                    GetComponent<C_Challenge>().CheckPreview(thisActionClass, target);
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
