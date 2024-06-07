using System;
using UnityEngine;

public class C_PreviewAction : MonoBehaviour
{
    //Note personel :
    //Il faut que le challenge au moment ou le joueur survol les actions, inscrit les preview necessaire dans puis le challenge demande à ce script de lancer la preview.
    //3 preview sont possible : Preview texte ; Preview stats ui ; Preview déplacement
    //Il y a aussi des preview pour les autres.
    //La preview à besoin de récupérer les info de l'action.

    public static event Action<SO_ActionClass> onPreview;

    //Fonction qui va lancer le setup de preview. CHANGER L'ENTREE CAR ON VEUT RECUP L'ACTOR SELECT PENDANT LA PHASE DU JOUEUR + L'ACTION QUE LE JOUEUR SURVOLE.
    public void ShowPreview(SO_ActionClass thisActionClass, C_Actor thisActor)
    {
        Debug.Log("ShowPreview");

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
        }
    }
}
