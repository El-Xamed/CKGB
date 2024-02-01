using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using static SO_Challenge;

public class C_Challenge : MonoBehaviour
{
    #region Mes variables
    #region Input
    bool canSelectAction = true;
    bool canUpdateRes = false;
    #endregion

    #region De base
    GameObject canva;
    GameObject uiCases;
    [SerializeField] GameObject uiEtape;
    [SerializeField] GameObject uiAction;

    [SerializeField] SO_Challenge myChallenge;

    [Tooltip("Team")]
    [SerializeField] List<C_Actor> myTeam;

    [Tooltip("Case")]
    [SerializeField] C_Case myCase;
    [SerializeField] List<C_Case> listCase;
    #endregion

    #region Challenge
    [Space(50)]
    C_Actor currentActor;

    //S�lection d'actions
    [SerializeField] int currentAction;

    //D�finis l'�tape actuel.
    public SO_Etape currentStep;

    //Utilisation d'une class qui regroupe 1 bouton et 1 action.
    [SerializeField] List<SO_ActionClass> listActions;
    #endregion
    
    #region Résolution
    [SerializeField] List<ActorResolution> listRes;
    ActorResolution currentResolution;
    [SerializeField] TMP_Text uiLogs;
    #endregion
    #endregion

    //------------------------------------------------------------------------------------------------------------------------

    #region Input
    //RCOURSCIR CETTE PARTIE DU CODE
    public void SelectRight(InputAction.CallbackContext context)
    {
        if(!context.performed) { return; }

        if (context.performed && canSelectAction)
        {
            currentAction++;
        }
    }

    public void SelectLeft(InputAction.CallbackContext context)
    {
        if (!context.performed) { return; }

        if (context.performed && canSelectAction)
        {
            currentAction--;
        }
    }

    public void SelectAction(InputAction.CallbackContext context)
    {
        if (!context.performed) { return; }

        if (context.performed && canUpdateRes)
        {
            ResolutionTurn();
        }

        //Pour la partie slection des action.
        if (context.performed && canSelectAction)
        {
            UseAction();
        }

        //Utilise l'action
        void UseAction()
        {
            //Création d'une nouvelle class pour ensuite ajouter dans la liste qui va etre utilisé dans la phase de résolution.
            ActorResolution actorResolution = new ActorResolution();
            actorResolution.action = listActions[currentAction];
            actorResolution.actor = currentActor;
            listRes.Add(actorResolution);

            //Si il reste des acteurs à jouer, alors tu passe à l'acteur suivant, sinon tu passe à la phase de "résolution".
            if (myTeam.IndexOf(currentActor) != myTeam.Count - 1)
            {
                //Passe à l'acteur suivant.
                NextActor();
            }
            else
            {
                //Lance la "Phase de résolution".
                ResolutionTurn();
            }
        }
    }
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
        currentStep = myChallenge.listEtape[0];
        currentActor = myTeam[0];
        UpdateUi(currentStep);

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
            //Création d'une case
            C_Case newCase = Instantiate(myCase, uiCases.transform);
            newCase.GetComponentInChildren<TMP_Text>().text = i.ToString();

            //Change la dernière case par un autre sprite.
            if (i == myChallenge.nbCase -1)
            {
                newCase.GetComponent<Image>().sprite = Resources.Load<Sprite>("Barre_Segment_Bout");
            }

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
            else //Poss�de l'info.
            {
                Debug.LogError("ERROR : Aucune informations de trouv� la liste, aucun acteur n'a pu spawn.");
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
                Debug.Log("Aucune informations de trouv� la liste des acc.");
            }
        }
    }

    //D�place ou fait spawn les acteurs.
    public void SpawnActor(List<InitialActorPosition> listPosition)
    {
        foreach (InitialActorPosition position in listPosition)
        {
            C_Actor myActor = Instantiate(position.perso, listCase[position.position].transform);
            myActor.IniChallenge();
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
        //Check si dans la liste de "Resolution" créer dans le dernier round, une bonne action se trouve deans. POSSIBLE QUE CETTE PARTIE CHANGE DANS LE FUTUR.
        //Applique toutes les actions.
        if (listRes == null)
        {
            foreach (var myRes in listRes)
            {
                //Si c'est la bonne réponse.
                if (myRes.action == currentStep.rightAnswer)
                {
                    Debug.Log("Bonne action");
                    stepUpdate();
                }
                else
                {
                    Debug.Log("Mauvaise action");
                }
            }
        }

        //Débloque les commande.
        GetComponent<PlayerInput>().enabled = true;

        //Change l'UI.
        uiAction.SetActive(true);

        //Fait apparaitre les actions.
        SpawnActions();
    }

    //Fait spawn les bouton d'actions
    void SpawnActions()
    {
        if (currentStep.actions != null)
        {
            Debug.Log("Spawn actions");
            for (int i = 0; i < currentStep.actions.Length; i++)
            {
                //Création d'une nouvelle class qui sera ajout� dans une liste.
                listActions.Add(currentStep.actions[i]);

                //Création d'un boutton qui sera en ref dans la class action.
                GameObject myButton = Instantiate(Resources.Load<GameObject>("ActionButton"), uiAction.transform);
                //Update le text + la ref de l'action.
                myButton.GetComponentInChildren<TMP_Text>().text = currentStep.actions[i].buttonText;
            }
        }
        else
        {
            Debug.LogError("Erreur spawn actions");
        }
    }

    //Passe à l'acteur suivant.
    void NextActor()
    {
        currentActor = myTeam[myTeam.IndexOf(currentActor) + 1];
    }

    //Passe à l'étape suivant.
    public void stepUpdate()
    {
        //Check si il reste des étapes.
        if (CheckEtape())
        {
            Debug.Log("next step");

            //Nouvelle étape.
            currentStep = myChallenge.listEtape[myChallenge.listEtape.IndexOf(currentStep) +1];

            //Update l'UI.
            UpdateUi(currentStep);

            //Supprime tous les boutons.
            for (int i = 0; i < uiAction.transform.childCount; i++)
            {
                Destroy(uiAction.transform.GetChild(i).gameObject);
            }

            //Nouvelle liste.
            listActions = new List<SO_ActionClass>();

            //Apparition des boutons.
            SpawnActions();
        }
        else
        {
            //Fin du challenge.
            EndChallenge();

            Debug.Log("Fin du niveau");
        }
    }

    //Pour Update l'UI.
    void UpdateUi(SO_Etape myEtape)
    {
        uiEtape.GetComponentInChildren<TMP_Text>().text = myEtape.énoncé;
    }
    #endregion

    #region  Phase de résolution
    //Création d'une class pour rassembler l'acteur et l'action.
    [Serializable] public class ActorResolution
    {
        public SO_ActionClass action;
        public C_Actor actor;
    }

    private void ResolutionTurn()
    {
        //Bloque les commande. CHANGER CA POUR QU IL PUISSE UPDATE LUI MEME LA PHASE DE RESO.
        //GetComponent<PlayerInput>().enabled = false;

        //Change l'UI.
        uiAction.SetActive(false);

        currentResolution = listRes[listRes.IndexOf(currentResolution)];

        //Applique toutes les actions. 1 par 1. EN CONSTRUCTION
        if (listRes.IndexOf(currentResolution) < listRes.Count -1)
        {

        }




        foreach (var myRes in listRes)
        {
            //Ecrit dans les logs l'action
            uiLogs.text = myRes.action.LogsChallenge;

            //Utilise l'action.
            myRes.action.UseAction(myRes.actor, listCase);
        }
    }

    //Bool pour check si le vhallenge est fini.
    bool CheckEtape()
    {
        if (myChallenge.listEtape.IndexOf(currentStep) != myChallenge.listEtape.Count -1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //Fin du challenge.
    void EndChallenge()
    {
        Debug.Log("Fin du challenge");
    }
    #endregion

    //Pour faire d�placer les accessoires.
    void UpdateAccessories()
    {

    }

    //Applique la cata.
    void ApplyCata()
    {

    }
    #endregion
}
