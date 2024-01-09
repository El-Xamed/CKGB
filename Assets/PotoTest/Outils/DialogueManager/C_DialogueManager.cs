using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_DialogueManager : MonoBehaviour
{
    [SerializeField] List<GameObject> actor;

    [SerializeField] bool startBegin = false;

    [SerializeField] GameObject spriteRightDialogue;

    [SerializeField] GameObject spriteLeftDialogue;

    [SerializeField] int currentDialogue;

    [SerializeField] List <SO_Dialogue> dialogue;

    public static C_DialogueManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        if (startBegin)
        {
            LetsTalk(currentDialogue);
        }

    }

    //Avancer dans le dialogue.
    void Continue()
    {
        currentDialogue++;

        LetsTalk(currentDialogue);
    }

    //Revenir en arrière dans le dialogue.
    void Back()
    {
        currentDialogue--;

        LetsTalk(currentDialogue);
    }

    //Fonction pour afficher les dialogue.
    void LetsTalk(int currentTalk)
    {
        if (dialogue[currentDialogue].right)
        {
            dialogue[currentDialogue].Speak(spriteRightDialogue);
        }
        else if (dialogue[currentDialogue].right == false)
        {
            dialogue[currentDialogue].Speak(spriteLeftDialogue);
        }
        
    }


    //Pour que les SO_Dialogue puissent récupérer l'acteur pour faire spawn le text au bon endroit.
    public GameObject GetActor(int currentActor)
    {
        return actor[currentActor];
    }
}
