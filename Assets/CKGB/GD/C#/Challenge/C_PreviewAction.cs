using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_PreviewAction : MonoBehaviour
{
    //Note personel :
    //Il faut que le challenge au moment ou le joueur survol les actions, inscrit les preview necessaire dans puis le challenge demande � ce script de lancer la preview.
    //3 preview sont possible : Preview texte ; Preview stats ui ; Preview d�placement
    //Il y a aussi des preview pour les autres.
    //La preview � besoin de r�cup�rer les info de l'action.

    public static event Action onPreview;

    //Fonction qui va lancer le setup de preview.
    void SetupPreview(SO_ActionClass thisActionClass)
    {
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
