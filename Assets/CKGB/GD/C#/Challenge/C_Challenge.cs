using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using static SO_Challenge;

public class C_Challenge : MonoBehaviour
{
    #region Mes variables
    GameObject canva;
    GameObject uiCases;

    [SerializeField] SO_Challenge myChallenge;

    [Tooltip("Team")]
    [SerializeField] List<C_Actor> myTeam;

    [Tooltip("Case")]
    [SerializeField] C_Case myCase;
    [SerializeField] List<C_Case> listCase;

    #region Challenge
    [Space(50)]
    //Tableau de toutes les étapes.
    [SerializeField] SO_Etape[] allSteps;
    //Position des boutons.  A FAIRE SPAWN CORRECTEMENT DANS L'UI.
    [SerializeField] public Transform[] buttonPlacements;

    //Définis l'étape actuel.
    [SerializeField] public SO_Etape currentStep;
    [SerializeField] int currentStepID = 0;

    //Utilisation d'une class qui regroupe 1 bouton et 1 action.
    [SerializeField] List<Action> listActions;
    #endregion

    #endregion

    private void Awake()
    {
        #region Racourcis
        canva = transform.GetChild(0).gameObject;

        uiCases = canva.transform.GetChild(1).gameObject;
        #endregion
    }

    private void Start()
    {
        //Apparition des cases
        SpawnCases();

        //Place les acteurs sur les cases.
        InitialiseAllPosition();

        #region Initialisation
        //Set l'étape en question.
        currentStep = allSteps[0];

        //Lance directement le tour du joueur
        PlayerTrun();
        #endregion
    }

    #region Mes fonctions

    #region Début de partie
    void SpawnCases()
    {
        //Spawn toutes les cases.
        for (int i = 0; i < myChallenge.nbCase; i++)
        {
            C_Case newCase = Instantiate(myCase, uiCases.transform);

            listCase.Add(newCase);
        }
    }

    //Set la position de tous les acteurs sur les cases.
    void InitialiseAllPosition()
    {
        ActorPosition();

        AccPosition();

        //Fonction de spawn "actor".
        void ActorPosition()
        {
            if (myChallenge.listStartPosTeam != null)
            {
                //Fait spawn avec des info random.
                SpawnActor(myChallenge.GetInitialPlayersPosition());
            }
            else //Possède l'info.
            {
                Debug.LogError("ERROR : Aucune informations de trouvé la liste, aucun acteur n'a pu spawn.");
            }
        }

        //Fonction de spawn "Accessories".
        void AccPosition()
        {
            if (myChallenge.listStartPosAcc != null)
            {
                //Fait spawn avec des info random.
                SpawnAcc(myChallenge.GetInitialAccPosition());
            }
            else 
            {
                Debug.Log("Aucune informations de trouvé la liste des acc.");
            }
        }
    }

    //Déplace ou fait spawn les acteurs.
    public void SpawnActor(List<InitialActorPosition> listPosition)
    {
        foreach (InitialActorPosition position in listPosition)
        {
            C_Actor myActor = Instantiate(position.perso, listCase[position.position].transform);
            myTeam.Add(myActor);
        }
    }

    public void SpawnAcc(List<InitialAccPosition> listPosition)
    {
        foreach (InitialAccPosition position in listPosition)
        {
            Instantiate(position.acc, listCase[position.position].transform);
        }
    }
    #endregion

    #region Tour du joueur
    void PlayerTrun()
    {
        //Fait apparaitre les actions.
        SpawnActions();
    }

    //Création d'une class qui permet de lier les boutons avec l'action.
    [Serializable] public class Action
    {
        C_Challenge challenge;
        [SerializeField] SO_ActionClass actionClass;
        Button button;

        public void SetButton(Button myButton)
        {
            button = myButton;
        }

        public void SetAction(SO_ActionClass myAction)
        {
            actionClass = myAction;
        }

        public void SetChallenge(C_Challenge myChallenge)
        {
            challenge = myChallenge;
        }

        public SO_ActionClass GetAction()
        {
            return actionClass;
        }

        public void UseAction()
        {
            if (challenge.currentStep.rightAnswer == actionClass)
            {
                Debug.Log("Bonne action");
                challenge.stepUpdate();
                //Check si il possède une sous action.
            }
            else
            {
                Debug.Log("Mauvaise action");
            }
        }
    }

    //Fait spawn les bouton d'actions
    void SpawnActions()
    {
        if (currentStep.actions != null)
        {
            Debug.Log("Spawn actions");
            for (int i = 0; i < currentStep.actions.Length; i++)
            {
                //Création d'une nouvelle class qui sera ajouté dans une liste.
                Action myAction = new Action();
                listActions.Add(myAction);

                //Création d'un boutton qui sera en ref dans la class action.
                Button myButton = Instantiate(currentStep.actions[0].actionButton, GameObject.Find("UI_Action_Background").transform);
                myButton.onClick.AddListener(listActions[i].UseAction);

                //Change les info de la class.
                listActions[i].SetButton(myButton);
                listActions[i].SetAction(currentStep.actions[i]);
                listActions[i].SetChallenge(this);
            }
        }
        else
        {
            Debug.LogError("Erreur spawn actions");
        }
    }

    //Passe à l'étape suivant.
    public void stepUpdate()
    {
        Debug.Log("next step");
        // Destroy();
        if (currentStepID < allSteps.Length)
        {

            currentStepID++;
            Debug.Log(currentStepID);
            currentStep = allSteps[currentStepID];
            listActions = new List<Action>();
            SpawnActions();
        }
    }
    #endregion

    //Pour faire déplacer les accessoires.
    void UpdateAccessories()
    {

    }

    //Applique la cata.
    void ApplyCata()
    {

    }
    #endregion
}
