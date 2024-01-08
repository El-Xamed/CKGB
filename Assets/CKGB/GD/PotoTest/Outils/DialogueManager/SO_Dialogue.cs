using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "ScriptableObjects/Dialogue", order = 1)]
public class SO_Dialogue : ScriptableObject
{
    //Pour que le dev selectionne sur quel personnage le texte va se créer.
    public int cible;

    //Pour que le dev rentre son text.
    public string text;

    //Pour que le dev sélectionne le sprite (gauche ou droite).
    public bool right;

    //Fonction pour faire fonctionner le dialogue.
    public void Speak(GameObject sprite)
    {
        //Check si les valeurs sont entré.
        if (cible < 0 || cible > C_DialogueManager.instance.GetListActor().Count)
        {
            Debug.Log("Veuiller entrer une cible.");
        }
        if (text == null)
        {
            Debug.Log("Veuiller entrer un dialogue.");
        }

        //
        Instantiate(sprite, C_DialogueManager.instance.GetActor(cible).transform);

        //Applique le changement de text.
        sprite.GetComponentInChildren<TextMeshPro>().text = text;

        //Debug.
        Debug.Log("La cible N°" + cible + "(" + C_DialogueManager.instance.GetActor(cible).name + ")" + " à parlé");
    }
}
