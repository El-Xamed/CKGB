using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;



[CreateAssetMenu(fileName = "New Dialogue", menuName = "ScriptableObjects/Dialogue", order = 2)]
public class SO_Dialogue : ScriptableObject
{
    //C�ration d'un enum
    public enum ECibleClass
    {
        KoolKid, Grandma, Clown
    }

    #region Variables
    //R�cup�ration en variable qui apparait dans l'inspector.
    public ECibleClass myTarget;

    //Pour que le dev selectionne sur quel personnage le texte va se cr�er.
    int cible;

    //Pour que le dev rentre son text.
    public string text;

    //Pour que le dev s�lectionne le sprite (gauche ou droite).
    public bool right;

    //Cr�ation de la var si elle existe pas sinon elle se r�active juste, d�sactivant au passage les autres bulles.
    [HideInInspector]
    public GameObject myDialogue = null;
    #endregion

    #region Mes fonctions
    //Fonction pour faire fonctionner le dialogue.
    public void Speak(GameObject sprite)
    {
        #region Check
        //Check si les valeurs sont entr�.
        if (cible < 0 || cible > C_DialogueManager.instance.GetListActor().Count)
        {
            Debug.Log("Veuiller entrer une cible.");
        }
        if (text == null)
        {
            Debug.Log("Veuiller entrer un dialogue.");
        }
        #endregion

        #region Spawn / SetActive le dialogue

        //D�finition de la valeur "cible" par l'enum.
        switch (myTarget)
        {
            case ECibleClass.KoolKid:
                cible = 0;
                break;
            case ECibleClass.Grandma:
                cible = 1;
                break;
            case ECibleClass.Clown:
                cible = 2;
                break;
        }

        if (myDialogue == null)
        {
            //Cr�ation du "GameObject".
            myDialogue = Instantiate(sprite, C_DialogueManager.instance.GetActor(cible).transform);

            Debug.Log("Cr�ation de la bulle.");
        }
        else
        {
            myDialogue.SetActive(true);
        }
        #endregion

        #region Update test
        //V�rification de si le Gameobject poss�de un component "text".
        if (sprite.GetComponentInChildren<TextMeshPro>())
        {
            //Applique le changement de text.
            sprite.GetComponentInChildren<TextMeshPro>().text = text;
        }
        else
        {
            Debug.Log("Warning ! Aucun Component<TextMeshPro> d�tect� !");
        }
        #endregion

        //Debug.
        Debug.Log("La cible N�" + cible + "(" + C_DialogueManager.instance.GetActor(cible).name + ")" + " � parl�");
    }
    #endregion
}
