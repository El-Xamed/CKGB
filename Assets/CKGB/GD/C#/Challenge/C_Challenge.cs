using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography;
using TMPro;
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
    [SerializeField] bool canSelectAction = false;
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
    [SerializeField] GameObject uiTrait;
    [SerializeField] GameObject uiGoodAction;
    [SerializeField] GameObject uiGameOver;

    [Header("UI (VFX)")]
    [SerializeField] GameObject vfxPlayerTurn;
    [SerializeField] GameObject vfxResoTurn;
    [SerializeField] GameObject vfxCataTurn;



    [Header("Data")]
    [SerializeField] SO_Challenge myChallenge;

    List<C_Actor> myTeam = new List<C_Actor>();
    List<C_Accessories> listAcc = new List<C_Accessories>();

    [Tooltip("Case")]
    [SerializeField] C_Case myCase;
    List<C_Case> listCase = new List<C_Case>();
    #endregion

    #region Interface
    [Header ("UI (Interface)")]
    [SerializeField] GameObject uiInterface;

    public enum Interface { Neutre, Logs, Actions, Traits, Back }

    [SerializeField] Interface currentInterface = Interface.Neutre;
    #endregion

    #region Challenge
    [Space(50)]
    [Header("Challenge")]
    bool canUpdateEtape = false;

    [SerializeField] C_Actor currentActor;

    //S�lection d'actions
    [SerializeField] int currentAction = 0;
    [SerializeField] int currentTrait = 0;

    //D�finis l'�tape actuel.
    public SO_Etape currentStep;

    //Utilisation d'une class qui regroupe 1 bouton et 1 action.
    List<ActionButton> listButton = new List<ActionButton>();

    //Utilisation d'une class qui regroupe 1 bouton et 1 action.
    [SerializeField] List<ActionButton> listButtonTraits = new List<ActionButton>();
    #endregion

    #region Résolution
    [Header("Resolution")]
    List<ActorResolution> listRes = new List<ActorResolution>();
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
                    GoTraits();
                    return;
                }

                if (input.y < 0)
                {
                    GoAction();
                    return;
                }
                if (input.y > 0)
                {
                    GoLogs();
                    return;
                }
            }

            //Pour selectionner ses actions.
            if (currentInterface == Interface.Actions || currentInterface == Interface.Traits)
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

                        //Si le joueur et dans l'interface des choix de traits, alors il update les actions disponible.
                        if (currentInterface == Interface.Traits)
                        {
                            SpawnTraits();
                        }
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
                    UpdateAccessories();
                    //Lance la phase "Cata".
                    Invoke("CataTrun", 0.5f);
                }
            }
        }
    }

    public void SelectAction(InputAction.CallbackContext context)
    {
        if (!context.performed) { return; }

        //FAIRE EN SORT QU'ON EST BESOIN DE UNE VARIABLE QUI GERE CA + QUE TOUTES LES ACTION ET TRAIT SPAWN AU MEME ENDROIT (1 VARIABLE EST NESSESSAIRE POUR NAVIGURE DANS LES 2 TABLEAU (TRAITS / ACTIONS) SEUL LA CONDITION D'AUGMENTER OU NON CETTE VALEUR CHANGERA CAR LA TAILLE DU TABLEAU EN QUESTION NE SERA PAS LE MEME.)
        //pour selectionner les action du challenge
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

        //Pour selectionner les traits de l'actor.
        if (context.performed && currentInterface == Interface.Traits)
        {
            int input = (int)context.ReadValue<float>();

            if (input > 0 && currentTrait < listButtonTraits.Count - 1)
            {
                currentTrait++;
                UpdateActionSelected();
            }
            if (input < 0 && currentTrait > 0)
            {
                currentTrait--;
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
        if (GameManager.instance)
        {
            GameManager.instance.ChangeActionMap("Challenge");
        }
    }

    private void Start()
    {
        #region Initialisation
        //Apparition des cases
        SpawnCases();

        //Place les acteurs sur les cases.
        InitialiseAllPosition();

        //Set l'étape en question.
        currentStep = myChallenge.listEtape[0];
        currentActor = myTeam[0];
        UpdateUi(currentStep);

        //Lance directement le tour du joueur
        Invoke("PlayerTrun", 2f);
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

            listAcc.Add(myAcc);
        }
    }

    #endregion

    #region Tour du joueur

    //Utilise l'action
    void UseAction()
    {
        //Création d'une nouvelle class pour ensuite ajouter dans la liste qui va etre utilisé dans la phase de résolution.
        ActorResolution actorResolution = new ActorResolution();

        //Check si c'est une action du challenge ou alors le trait d'un actor.
        switch (currentInterface)
        {
            case Interface.Actions:
                actorResolution.action = listButton[currentAction].myActionClass;
                break;
            case Interface.Traits:
                actorResolution.action = listButtonTraits[currentTrait].myActionClass;
                break;
        }

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
            ResolutionTurn();
        }
    }

    #region Interface
    //Création d'une interface pour naviguer dans l'ui est les actions qu'on souhaite sélectionner
    //Pour accéder au actions.
    public void GoAction()
    {
        currentAction = 0;

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
        //Replace le curseur à l'emplacement 0.
        currentTrait = 0;

        //Animation.
        uiInterface.GetComponent<Animator>().SetTrigger("OpenTraits");

        //Fait apparaitre la liste de trait.
        uiTrait.SetActive(true);

        //Fait apparaitre les traits du l'actor.
        SpawnTraits();

        //Modifie l'état de navigation.
        currentInterface = Interface.Traits;

        //Peut mmtn selectionner une action.
        canSelectAction = true;
    }

    //Pour revenir au temps mort. Et aussi au autres boutons
    public void GoBack()
    {
        switch (currentInterface)
        {
            case Interface.Actions:
                uiInterface.GetComponent<Animator>().SetTrigger("CloseActions");
                uiAction.SetActive(false);
                canSelectAction = false;
                break;
            case Interface.Traits:
                uiInterface.GetComponent<Animator>().SetTrigger("CloseTraits");
                uiTrait.SetActive(false);
                canSelectAction = false;
                break;
            case Interface.Logs:

                break;
        }

        currentInterface = Interface.Neutre;
    }
    #endregion

    void PlayerTrun()
    {
        //Affiche l'interface.
        uiInterface.SetActive(true);

        uiAction.SetActive(false);
        uiTrait.SetActive(false);

        //Défini la phase de jeu.
        myPhaseDeJeu = PhaseDeJeu.PlayerTrun;

        currentInterface = Interface.Neutre;

        //Joue l'animation (PASSER PAR UNE FONCTION QUI AVEC UN SWITCH LANCE LA BONNE ANIM)
        vfxPlayerTurn.GetComponent<Animator>().enabled = true;

        //Check si le perso est jouable
        if (!currentActor.GetIsOut())
        {
            Debug.Log("Player turn !");

            uiLogs.text = "";

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
            //UpdateActionSelected();
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

        if (myChallenge.listCatastrophy[0].modeAttack == SO_Catastrophy.EModeAttack.Random)
        {
            //Augmente ou réduit le nombre.
            int newInt = UnityEngine.Random.Range(0, listCase.Count -1);

            //Vide la liste.
            myChallenge.listCatastrophy[0].targetCase.Clear();

            //Ajoute la valeur aléatoire.
            myChallenge.listCatastrophy[0].targetCase.Add(newInt);
        }

        //Affiche la prochaine cata.
        foreach (var thisCase in myChallenge.listCatastrophy[0].targetCase)
        {
            listCase[thisCase].ShowDangerZone(myChallenge.listCatastrophy[0].vfxCataPrefab);
        }

        //Ajout des zones de danger des acc
        foreach (var thisAcc in listAcc)
        {
            if (thisAcc.dataAcc.canMakeDamage)
            {
                listCase[thisAcc.currentPosition].ShowDangerZone(myChallenge.listCatastrophy[0].vfxCataPrefab);
            }
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

    void SpawnTraits()
    {
        //Check si la list stocké dans le SO_Character est vide
        if (currentActor.dataActor.listTraits != null)
        {
            //Supprime les boutons précédent
            if (listButtonTraits != null)
            {
                foreach (var myTraits in listButtonTraits)
                {
                    Destroy(myTraits.myButton);
                }
            }

            //Créer une nouvelle liste.
            listButtonTraits = new List<ActionButton>();

            //Créer de nouveau boutons (Traits)
            for (int i = 0; i < currentActor.dataActor.listTraits.Count; i++)
            {
                //Nouvelle class.
                ActionButton newTraitsButton = new ActionButton();

                //Reférence button.
                newTraitsButton.myButton = Instantiate(Resources.Load<GameObject>("ActionButton"), uiTrait.transform);
                newTraitsButton.myButton.GetComponentInChildren<TMP_Text>().text = currentActor.dataActor.listTraits[i].buttonText;

                //Reférence Action.
                newTraitsButton.myActionClass = currentActor.dataActor.listTraits[i];
                listButtonTraits.Add(newTraitsButton);
            }

            uiAction.SetActive(false);
        }
        else
        {
            Debug.LogError("Erreur spawn traits. La liste de trait du perso est vide.");
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

        if (currentInterface == Interface.Actions)
        {
            uiAction.SetActive(true);
            UpdateActionSelected();
        }
        if (currentInterface == Interface.Traits)
        {
            uiTrait.SetActive(true);
            UpdateActionSelected();
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
        switch (currentInterface)
        {
            case Interface.Actions:
                #region Action Challenge
                //Cache tous les boutons du challenge.
                foreach (var myButton in listButton)
                {
                    //Feedback du bouton non-selecioné.
                    myButton.myButton.transform.GetChild(0).gameObject.SetActive(false);
                }

                //Affiche le bouton que le joueur souhaite selectionner dans le challenge.
                listButton[currentAction].myButton.transform.GetChild(0).gameObject.SetActive(true);
                #endregion
                break;
            case Interface.Traits:
                #region Trait Actor
                if (currentInterface == Interface.Traits)
                {
                    //Cache tous les boutons de traits de l'actor.
                    foreach (var myTrait in listButtonTraits)
                    {
                        //Feedback du bouton non-selecioné.
                        myTrait.myButton.transform.GetChild(0).gameObject.SetActive(false);
                    }

                    //Affiche le bouton que le joueur souhaite selectionner dans les traits de l'actor.
                    listButtonTraits[currentTrait].myButton.transform.GetChild(0).gameObject.SetActive(true);
                }
                #endregion
                break;
        }
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

        //Joue l'animation (PASSER PAR UNE FONCTION QUI AVEC UN SWITCH LANCE LA BONNE ANIM)
        vfxResoTurn.GetComponent<Animator>().enabled = true;

        //Applique toutes les actions. 1 par 1. EN CONSTRUCTION
        //Si la reso en question n'est pas dernier, alors il peut passer a la reso suivante sinon il lance la cat
        if (currentResolution.action.CanUse(currentResolution.actor))
        {
            //Utilise l'action.
            currentResolution.action.UseAction(currentResolution.actor, listCase, myTeam);

            //Check si il est sur une case "Dangereuse".
            currentResolution.actor.CheckIsInDanger(myChallenge.listCatastrophy[0]);

            //Ecrit dans les logs le résultat de l'action.
            uiLogs.text = currentResolution.action.GetLogsChallenge();

            //Si c'est la bonne réponse. LE FAIRE DANS L'ACTION DIRECTEMENT
            if (currentResolution.action == currentStep.rightAnswer)
            {
                Debug.Log("Bonne action");

                uiGoodAction.GetComponentInChildren<Image>().sprite = currentResolution.actor.dataActor.challengeSpriteUiGoodAction;

                uiGoodAction.GetComponent<Animator>().SetTrigger("GoodAction");

                canUpdateEtape = true;
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
        myPhaseDeJeu = PhaseDeJeu.CataTurn;

        //Check au début si tous les perso sont "out".
        if (!CheckGameOver())
        {
            //Applique la catastrophe. FONCTIONNE AVEC 1 CATA, A MODIFIER POUR QU'IL UTILISE LES CATA
            ApplyCatastrophy(myChallenge.listCatastrophy[0]);

            //Re-Check si tous les perso sont "out".
            CheckGameOver();

            if (!CheckGameOver())
            {
                //Update les acc
                UpdateAccessories();

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

    #region GameOver
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
    #endregion

    //Fin du challenge.
    void EndChallenge()
    {
        Debug.Log("Fin du challenge");
    }

    //Pour faire d�placer les accessoires.
    void UpdateAccessories()
    {
        ApplyAccDamage(listAcc[0]);
    }

    void ApplyAccDamage(C_Accessories thisAcc)
    {
        //Check si il attaque.
        if (!thisAcc.dataAcc.canMakeDamage) { return; }

        if (thisAcc.dataAcc.typeAttack == SO_Accessories.ETypeAttack.All)
        {
            //Check si la position des actor est sur la meme case que l'acc.
            foreach (var thisActor in myTeam)
            {
                thisActor.TakeDamage(thisAcc.dataAcc.reducStress, thisAcc.dataAcc.reducEnergie);

                thisActor.CheckIsOut();
            }
        }

        //Check si la position des actor est sur la meme case que l'acc.
        foreach (var thisActor in myTeam)
        {
            if (thisActor.GetPosition() == thisAcc.currentPosition)
            {
                thisActor.TakeDamage(thisAcc.dataAcc.reducStress, thisAcc.dataAcc.reducEnergie);

                thisActor.CheckIsOut();
            }
        }

        //Ecrit dans les logs le résultat de l'action.
        uiLogs.text = thisAcc.dataAcc.damageLogs;

        //Check si le jeu est fini "GameOver".
        CheckGameOver();
    }
    #endregion
}
