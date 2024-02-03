using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography;
using TMPro;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static SO_Challenge;

public class C_Challenge : MonoBehaviour
{
    #region Mes variables
    #region Input
    [Header("Input")]
    [SerializeField] bool canSelectAction = true;
    [SerializeField] bool canUpdateRes = false;
    #endregion

    #region De base
    //Pour connaitre la phasse de jeu.
    public enum PhaseDeJeu { PlayerTrun, ResoTurn, CataTurn}
    [Header("Phase de jeu")]
    [SerializeField] PhaseDeJeu myPhaseDeJeu = PhaseDeJeu.PlayerTrun;

    GameObject canva;
    GameObject uiCases;
    [Header("UI")]
    
    [SerializeField] C_Stats uiStatsPrefab;
    [SerializeField] GameObject uiStats;
    [SerializeField] GameObject uiEtape;
    [SerializeField] GameObject uiAction;
    [SerializeField] GameObject uiGameOver;

    [Header("Data")]
    [SerializeField] SO_Challenge myChallenge;

    [SerializeField] List<C_Actor> myTeam;

    [Tooltip("Case")]
    [SerializeField] C_Case myCase;
    [SerializeField] List<C_Case> listCase;
    #endregion

    #region Interface
    [Header ("UI (Interface)")]
    [SerializeField] GameObject uiInterface;
    [SerializeField] GameObject currentButton;

    public enum Interface { Neutre, Logs, Actions, Traits, Back }

    [SerializeField] Interface currentInterface = Interface.Neutre;
    #endregion

    #region Challenge
    [Space(50)]
    [Header("Challenge")]
    bool canUpdateEtape = false;

    [SerializeField] C_Actor currentActor;

    //S�lection d'actions
    [SerializeField] int currentAction;

    //D�finis l'�tape actuel.
    public SO_Etape currentStep;

    //Utilisation d'une class qui regroupe 1 bouton et 1 action.
    [SerializeField] List<ActionButton> listButton;
    #endregion

    #region Résolution
    [Header("Resolution")]
    [SerializeField] List<ActorResolution> listRes;
    [SerializeField] ActorResolution currentResolution;
    [SerializeField] TMP_Text uiLogs;
    #endregion
    #endregion

    //------------------------------------------------------------------------------------------------------------------------

    #region Input
    public void ResetChallenge(InputAction.CallbackContext context)
    {
        if (!context.performed) { return; }

        if (!context.performed)
        {
            SceneManager.LoadScene("Destination_Test");
        }
    }

    public void SelectButton(InputAction.CallbackContext context)
    {
        if (!context.performed) { return; }

        if (context.performed)
        {
            Vector2 input = context.ReadValue<Vector2>();

            //Pour la navigation dans l'interface "Neutre"
            if (currentInterface == Interface.Neutre)
            {
                if (input.x > 0)
                {
                    GoBack();
                    return;
                }
                if (input.x < 0)
                {
                    GoLogs();
                    return;
                }

                if (input.y < 0)
                {
                    GoAction();
                    return;
                }
                if (input.y > 0)
                {
                    GoTraits();
                    return;
                }
            }

            //Pour selectionner ses actions.
            if (currentInterface == Interface.Actions)
            {
                if (input.x > 0)
                {
                    GoBack();
                    return;
                }
                if (input.y < 0)
                {
                    //Pour la partie slection des action.
                    if (canSelectAction)
                    {
                        UseAction();
                        return;
                    }
                }
            }

            //Pour Update ResoTrun
            if (input.y < 0 && myPhaseDeJeu == PhaseDeJeu.ResoTurn)
            {
                if (listRes.IndexOf(currentResolution) < listRes.Count - 1)
                {
                    //Reféfinis "currentResolution" avec 'index de base + 1.
                    currentResolution = listRes[listRes.IndexOf(currentResolution) + 1];

                    ResolutionTurn();
                }
                else
                {
                    //Lance la phase "Cata".
                    CataTrun();
                    Debug.Log("Toutes les actions on été fait");
                }
            }
        }
    }

    public void SelectAction(InputAction.CallbackContext context)
    {
        if (!context.performed) { return; }

        if (context.performed && currentInterface == Interface.Actions)
        {
            int input = (int)context.ReadValue<float>();

            if (input > 0 && currentAction < listButton.Count - 1)
            {
                currentAction++;
                UpdateActionSelected();
            }
            if (input < 0 && currentAction > 0)
            {
                currentAction--;
                UpdateActionSelected();
            }
        }
    }
    #endregion

    //------------------------------------------------------------------------------------------------------------------------

    private void Awake()
    {
        #region Racourcis
        canva = transform.GetChild(0).gameObject;

        uiCases = canva.transform.GetChild(1).gameObject;

        #endregion
    }

    [Obsolete]
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
        uiGameOver.SetActive(false);

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
            //New actor
            C_Actor myActor = Instantiate(position.perso, listCase[position.position].transform);
            myActor.IniChallenge();

            //New Ui stats
            C_Stats newStats = Instantiate(uiStatsPrefab, uiStats.transform);

            //Add Ui Stats
            myActor.SetUiStats(newStats);

            //Update UI
            myActor.UpdateUiStats();

            myTeam.Add(myActor);
        }
    }

    public void SpawnAcc(List<InitialAccPosition> listPosition)
    {
        foreach (InitialAccPosition position in listPosition)
        {
            C_Accessories myAcc = Instantiate(position.acc, listCase[position.position].transform);
        }
    }

    #endregion

    #region Tour du joueur

    //Utilise l'action
    void UseAction()
    {
        //Création d'une nouvelle class pour ensuite ajouter dans la liste qui va etre utilisé dans la phase de résolution.
        ActorResolution actorResolution = new ActorResolution();
        actorResolution.action = listButton[currentAction].myActionClass;
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
            currentActor = null;
            UpdateActorSelected();
            currentResolution = listRes[0];
            //Update les bool.
            canSelectAction = false;
            canUpdateRes = true;
            ResolutionTurn();
        }
    }




    #region Interface
    //Création d'une interface pour naviguer dans l'ui est les actions qu'on souhaite sélectionner

    //Pour accéder au actions.
    public void GoAction()
    {
        uiInterface.GetComponent<Animator>().SetTrigger("OpenActions");

        uiAction.SetActive(true);

        currentInterface = Interface.Actions;

        canSelectAction = true;
    }

    //Pour accéder au logs.
    public void GoLogs()
    {
        Debug.Log("Pas disponible");
    }

    //Pour accéder au traits.
    public void GoTraits()
    {

    }

    //Pour revenir au temps mort. (Et aussi au autres boutons ?)
    public void GoBack()
    {
        if (currentInterface == Interface.Actions)
        {
            currentInterface = Interface.Neutre;

            uiAction.SetActive(false);

            uiInterface.GetComponent<Animator>().SetTrigger("CloseActions");
        }
    }
    #endregion

    void PlayerTrun()
    {
        //Check si le perso est jouable
        if (!currentActor.GetIsOut())
        {
            Debug.Log("Player turn !");

            uiLogs.text = "";

            //Affiche l'interface.
            uiInterface.SetActive(true);

            //Défini la phase de jeu.
            myPhaseDeJeu = PhaseDeJeu.PlayerTrun;

            currentInterface = Interface.Neutre;

            //Update le contour blanc
            UpdateActorSelected();

            //Vide la listeReso
            listRes = new List<ActorResolution>();

            //Initialise la prochaine cata.
            InitialiseCata();

            //Check si pendant la réso, un acteur a trouvé la bonne reponse. UTILISATION D4UN BOOL QUI SERA DESACTIVE APRES. PERMET DE UPDATE AU BON MOMENT.
            if (canUpdateEtape)
            {
                stepUpdate();

                canUpdateEtape = false;
            }

            //Change l'UI.
            uiAction.SetActive(true);

            //Fait apparaitre les actions.
            SpawnActions();

            //Met a jour le curseur des actions.
            UpdateActionSelected();
        }
        else
        {
            NextActor();
            PlayerTrun();
        }
    }

    

    //VFX des cases visées.
    void InitialiseCata()
    {
        //Supprime toutes les cata
        foreach (var thisCase in listCase)
        {
            thisCase.DestroyVfxCata();
        }

        //Affiche la prochaine cata.
        foreach (var thisCase in myChallenge.listCatastrophy[0].targetCase)
        {
            listCase[thisCase].ShowDangerZone(myChallenge.listCatastrophy[0].vfxCataPrefab);
        }
    }

    [Serializable]
    public class ActionButton
    {
        public GameObject myButton;
        public SO_ActionClass myActionClass;
    }
    
    //Fait spawn les bouton d'actions
    void SpawnActions()
    {
        if (currentStep.actions != null)
        {
            //Supprime les boutons précédent
            if (listButton != null)
            {
                foreach (var myAction in listButton)
                {
                    Destroy(myAction.myButton);
                }
            }

            //Créer une nouvelle liste.
            listButton = new List<ActionButton>();

            //Créer de nouveau boutons
            for (int i = 0; i < currentStep.actions.Length; i++)
            {
                //Nouvelle class.
                ActionButton newActionButton = new ActionButton();

                //Reférence button.
                newActionButton.myButton = Instantiate(Resources.Load<GameObject>("ActionButton"), uiAction.transform);
                newActionButton.myButton.GetComponentInChildren<TMP_Text>().text = currentStep.actions[i].buttonText;

                //Reférence Action.
                newActionButton.myActionClass = currentStep.actions[i];
                listButton.Add(newActionButton);
            }

            uiAction.SetActive(false);
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

        UpdateActorSelected();

        //Check si l'actor en question est ko.
        if (currentActor.GetIsOut())
        {
            NextActor();
        }
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

    void UpdateActionSelected()
    {
        foreach (var myButton in listButton)
        {
            //Feedback du bouton non-selecioné.
            myButton.myButton.transform.GetChild(0).gameObject.SetActive(false);
        }

        listButton[currentAction].myButton.transform.GetChild(0).gameObject.SetActive(true);
    }
    #endregion

    #region  Phase de résolution
    //Création d'une class pour rassembler l'acteur et l'action.
    [Serializable] public class ActorResolution
    {
        public SO_ActionClass action;
        public C_Actor actor;
    }

    void ResolutionTurn()
    {
        Debug.Log("Resolution trun !");

        //Défini la phase de jeu.
        myPhaseDeJeu = PhaseDeJeu.ResoTurn;

        //Cache l'interface.
        uiInterface.SetActive(false);

        //Change l'UI.
        uiAction.SetActive(false);

        //Applique toutes les actions. 1 par 1. EN CONSTRUCTION
        //Si la reso en question n'est pas dernier, alors il peut passer a la reso suivante sinon il lance la cat
        if (currentResolution.action.CanUse(currentResolution.actor))
        {
            //Utilise l'action.
            currentResolution.action.UseAction(currentResolution.actor, listCase);

            //Check si il est sur une case "Dangereuse".
            currentResolution.actor.CheckIsInDanger(myChallenge.listCatastrophy[0]);

            //Ecrit dans les logs le résultat de l'action.
            uiLogs.text = currentResolution.action.GetLogsChallenge();

            //Si c'est la bonne réponse. LE FAIRE DANS L'ACTION DIRECTEMENT
            if (currentResolution.action == currentStep.rightAnswer)
            {
                Debug.Log("Bonne action");
                canUpdateEtape = true;
            }
            else
            {
                Debug.Log("Mauvaise action");
            }
        }
    }
    #endregion

    #region Tour de la Cata
    //Pour lancer la cata.
    void CataTrun()
    {
        Debug.Log("CataTrun");

        //Défini la phase de jeu.
        myPhaseDeJeu = PhaseDeJeu.ResoTurn;

        canUpdateRes = false;

        //Check au début si tous les perso sont "out".
        if (!CheckGameOver())
        {
            //Applique la catastrophe. FONCTIONNE AVEC 1 CATA, A MODIFIER POUR QU'IL UTILISE LES CATA
            ApplyCatastrophy(myChallenge.listCatastrophy[0]);

            //Re-Check si tous les perso sont "out".
            CheckGameOver();

            if (!CheckGameOver())
            {
                //Redéfini le début de la liste.
                currentActor = myTeam[0];

                Invoke("PlayerTrun", 1f);
            }
        }
    }

    //Applique la cata
    void ApplyCatastrophy(SO_Catastrophy thisCata)
    {
        //Pour tous les nombre dans la liste dela cata.
        foreach (var thisCase in thisCata.targetCase)
        {
            //Pour tous les actor.
            foreach (var myActor in myTeam)
            {
                if (thisCase == myActor.GetPosition())
                {
                    myActor.TakeDamage(thisCata.reducStress, thisCata.reducEnergie);



                    myActor.CheckIsOut();
                }
            }
        }

        //Ecrit dans les logs le résultat de l'action.
        uiLogs.text = thisCata.catastrophyLog;

        //Check si le jeu est fini "GameOver".
        CheckGameOver();
    }
    #endregion

    void UpdateActorSelected()
    {
        //Retire sur tout les actor le contours blanc
        foreach (var myActor in myTeam)
        {
            myActor.IsSelected(false);
        }

        if (currentActor != null)
        {
            //Fait apparaitre le contour blanc de l'actor selectionne.
            currentActor.IsSelected(true);
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

    bool CheckGameOver()
    {
        int nbActorOut = 0;

        foreach (var thisActor in myTeam)
        {
            if (thisActor.GetIsOut()) { nbActorOut++; }
        }

        if (nbActorOut == myTeam.Count)
        {
            GameOver();
            return true;
        }

        return false;
    }

    void GameOver()
    {
        Debug.Log("GameOver");
        uiGameOver.SetActive(true);
        return;
    }

    //Fin du challenge.
    void EndChallenge()
    {
        Debug.Log("Fin du challenge");
    }

    //Pour faire d�placer les accessoires.
    void UpdateAccessories()
    {

    }
    #endregion
}
