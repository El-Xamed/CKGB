using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Composites;

public class C_DialogueManager : MonoBehaviour
{
    #region Variables
    [Header("Les acteurs pr�sent dans la scene.")]
    [Tooltip ("Ins�rer les acteurs de type (GameObject) pour que les bulles de dialogue apparaissent au bon endroit, cela permet au SO_Dialogue de reconnaitre ses {cibles}")]
    [SerializeField] List<GameObject> listActor;

    [Header("Commencer les dialogue d�s le d�but ?")]
    [SerializeField] bool startBegin = false;

    [Header ("Sprite de bulle (droite).")]
    [Tooltip ("Ajouter un (GameObject) qui poss�de un sprite. A voir si dans le futur juste faire glisser le (sprite) en question ici et non un (GameObject).")]
    [SerializeField] GameObject spriteRightDialogue;

    [Header("Sprite de bulle (Gauche).")]
    [Tooltip("Ajouter un (GameObject) qui poss�de un sprite. A voir si dans le futur juste faire glisser le (sprite) en question ici et non un (GameObject).")]
    [SerializeField] GameObject spriteLeftDialogue;

    [Header("Les bulles de discution (SO_Dialogue)")]
    [Tooltip ("Ici seront ajout� les bulles de dialogue (soit SO_Dialogue) qui d�fileront seront la navigation du joueur.")]
    [SerializeField] List<SO_Dialogue> dialogue;

    //Info sup pour le dev.
    [Space]
    [Header("Info sup pour le dev.")]
    [Tooltip("Permet de voir � quelle �tape de la discution nous somme. C'est une info pour le dev.")]
    [SerializeField] int currentDialogue = 0;

    public static C_DialogueManager instance;
    #endregion

    private void Awake()
    {
        #region Singleton
        if (instance == null)
            instance = this;
        #endregion

        //R�cup�ration automatique des personnages dans le tableau.
        //Pour tous les acteurs qui poss�de le componnent "Actor".
        foreach (C_Actor actor in Resources.FindObjectsOfTypeAll(typeof(C_Actor)) as C_Actor[])
        {
            listActor.Add(actor.gameObject);
        }
    }

    private void Start()
    {
        if (startBegin)
        {
            LetsTalk(currentDialogue);
        }
    }

    #region Controller
    //Avancer dans le dialogue.
    public void UpdateDialogue(InputAction.CallbackContext context)
    {
        //1ere it�ration du code.
        /*if (context.ReadValue<float>() == 0)
        {
            return;
        }
        if (context.ReadValue<float>() < 0 && context.performed)
        {
            currentDialogue--;

            LetsTalk(currentDialogue);

            return;
        }
        else if (context.ReadValue<float>() > 0 && context.performed)
        {
            currentDialogue++;

            LetsTalk(currentDialogue);

            return;
        }*/

        //2eme it�ration du code.
        if (context.ReadValue<float>() != 0 && context.performed)
        {
            if (currentDialogue == 0 && context.ReadValue<float>() == -1)
                return;
            if (currentDialogue == dialogue.Count - 1 && context.ReadValue<float>() == 1)
                return;

            LetsTalk(currentDialogue += (int)context.ReadValue<float>());
        }
    }
    #endregion

    #region Mes fonctions
    //Fonction pour afficher les dialogue.
    public void LetsTalk(int currentTalk)
    {
        //D�sactive les autres dialogues.
        foreach (var otherDialogue in dialogue)
        {
            if (otherDialogue.myDialogue != null)
            {
                otherDialogue.myDialogue.SetActive(false);
            }
        }

        //Active le dialogue avec le spite demand�.
        if (dialogue[currentDialogue].right)
        {
            dialogue[currentDialogue].Speak(spriteRightDialogue);
        }
        else if (dialogue[currentDialogue].right == false)
        {
            dialogue[currentDialogue].Speak(spriteLeftDialogue);
        }
    }

    #region Get info for SO_Dialogue
    //Pour que les SO_Dialogue puissent r�cup�rer l'acteur pour faire spawn le text au bon endroit.
    public GameObject GetActor(int currentActor)
    {
        return listActor[currentActor];
    }

    public List<GameObject> GetListActor()
    {
        return listActor;
    }

    public List<SO_Dialogue> GetListDialogue()
    {
        return dialogue;
    }
    #endregion
    #endregion
}
