using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "ScriptableObjects/Dialogue", order = 1)]
public class SO_Dialogue : ScriptableObject
{
    //Pour que le dev selectionne sur quel personnage le texte va se créer.
    public int cible;

    //Pour que le dev rentre son text.
    public string text;

    //Pour que le dev sélectionne le sprite (gauche ou droite).
    public bool right;

    public void Speak(GameObject sprite)
    {
        if (right)
        {
            Instantiate(sprite, C_DialogueManager.instance.GetActor(cible).transform);
        }
        else if (!right)
        {
            Instantiate(sprite, C_DialogueManager.instance.GetActor(cible).transform);
        }

        //Debug.
        Debug.Log("La cible N°" + cible + "(" + C_DialogueManager.instance.GetActor(cible).name + ")" + "à parlé");
    }
}
