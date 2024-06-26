using Febucci.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static SO_Challenge;

public class C_Challenge : MonoBehaviour
{
    #region Mes variables

    #region Tuto
    [Header("Tuto")]
    [SerializeField] Animator tuto;
    bool canMakeTuto = true;
    #endregion

    #region Dialogue
    [Header("Dialogue")]
    [SerializeField] bool onDialogue = false;
    [SerializeField] GameObject black;
    #endregion

    #region Interface
    [Header("Interface")]
    [SerializeField] C_Interface myInterface;
    #endregion

    #region Data Challenge
    [Header("Data Challenge")]
    [SerializeField] SO_Challenge myChallenge;

    [Header("Data 1er Challenge")]
    [SerializeField] SO_TempsMort tm1;
    [SerializeField] SO_Challenge c1;
    #endregion

    #region Phase de jeu
    //Pour connaitre la phasse de jeu.
    public enum PhaseDeJeu { PlayerTrun, ResoTurn, CataTurn, EndGame, GameOver}
    [Header("Phase de jeu")]
    [SerializeField] PhaseDeJeu myPhaseDeJeu = PhaseDeJeu.PlayerTrun;
    bool canUpdate = false;
    bool canGoNext = false;
    #endregion

    #region Plateau
    [SerializeField] GameObject plateauGameObject;

    [Header("Case")]
    [SerializeField] C_Case myCase;
    List<C_Case> plateau = new List<C_Case>();
    List<Image> plateauPreview = new List<Image>();
    #endregion

    #region Logs
    [Header("Logs")]
    [SerializeField] Animator uiLogsAnimator;
    [SerializeField] GameObject uiLogs;
    [SerializeField] GameObject uiLogsTimeline;
    [SerializeField] Transform uiLogsPreview;
    [SerializeField] GameObject uiLogsTextPreviewPrefab;
    #endregion

    List<C_Actor> myTeam = new List<C_Actor>();
    List<C_Accessories> listAcc = new List<C_Accessories>();

    #region Current Data
    C_Actor currentActor;
    ActorResolution currentResolution;
    SO_Etape currentStep;
    SO_Catastrophy currentCata;
    #endregion

    bool canUseCata = false;

    [SerializeField] EventSystem eventSystem;

    #region UI
    [Header("UI")]
    [SerializeField] GameObject background;
    [SerializeField] C_Stats uiStatsPrefab;
    [SerializeField] GameObject uiStats;
    [SerializeField] GameObject uiEtape;
    [SerializeField] GameObject uiGoodAction;
    [SerializeField] GameObject uiVictoire;
    [SerializeField] GameObject uiGameOver;
    [SerializeField] Image uiGameOverImage;

    #endregion

    #region Résolution
    [Header("Resolution")]
    List<ActorResolution> listRes = new List<ActorResolution>();
    #endregion

    #region Vfx
    [Header("UI (VFX)")]
    [SerializeField] Animator vfxStartChallenge;
    [SerializeField] Animator vfxPlayerTurn;
    [SerializeField] Animator vfxResoTurn;
    #endregion

    #region SFX
    [Header("SFX")]
    [SerializeField] string sfxOverButton;
    [SerializeField] string sfxSelectButton;
    [SerializeField] string sfxGoodAction;
    [SerializeField] string sfxWriteText;
    [SerializeField] string sfxNewPhase;
    #endregion

    #region Transition
    [Header("Scene")]
    [SerializeField] string sceneMenu = "S_MainMenu";
    [SerializeField] string sceneTM = "S_TempsLibre";
    [SerializeField] string sceneWM = "S_Worldmap";
    [SerializeField] string sceneCredits = "S_Credits";

    #endregion
    #endregion

    //------------------------------------------------------------------------------------------------------------------------

    #region Input
    public void ResetChallenge(InputAction.CallbackContext context)
    {
        if (!context.performed) { return; }

        if (context.performed)
        {
            foreach (GameObject c in GameManager.instance.GetTeam())
            {
                c.transform.parent = GameManager.instance.transform;
            }
            Debug.Log("Reload");
            SceneManager.LoadScene("S_DestinationTest");
        }
    }
    public void ResetChallengePause()
    {
        
            foreach (GameObject c in GameManager.instance.GetTeam())
            {
                c.transform.parent = GameManager.instance.transform;
            }
            Debug.Log("Reload");
            SceneManager.LoadScene("S_DestinationTest");
        
    }

    //Affiche les information de ce bouton dans la preview.
    public void Naviguate(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            //Active une fonction qui affiche toutes les preview de stats sur les actor.
            //Check si il n'est pas dans les logs.
            if (!myInterface.GetOnLogs())
            {
                WriteStatsPreview();
            }
        } 
    }
    #endregion

    //------------------------------------------------------------------------------------------------------------------------

    private void Awake()
    {
        if (GameManager.instance)
        {
            //Appel la transition.
            GameManager.instance.OpenTransitionFlannel();
            GameManager.instance.TS_flanel.GetComponent<C_TransitionManager>().SetupFirthEvent(LaunchIChallenge);

            //Check si le GameManager possède bien l'info de la Worldmap.
            if (GameManager.instance.GetDataChallenge() != null)
            {
                myChallenge = GameManager.instance.GetDataChallenge();
            }
            else
            {
                Debug.LogWarning("AUCUNE DONNE DU CHALLENGE DETECTE");
            }
        }
        else
        {
            Debug.LogWarning("AUCUN GAMEMANAGER DETECTE");
            StartCoroutine(StartChallenge());
        }

        if (AudioManager.instanceAM && myChallenge.LevelChallenge != "")
        {
            AudioManager.instanceAM.Play(myChallenge.LevelChallenge);
        }

        if (AudioManager.instanceAM && myChallenge.AmbianceChallenge != "")
        {
            AudioManager.instanceAM.Play(myChallenge.AmbianceChallenge);
        }
    }

    void Start()
    {
        //Set le background
        //Check si le background n'est pas vide.
        if (myChallenge.background)
        {
            Instantiate(myChallenge.background, this.transform);
        }
        else
        {
            Debug.LogError("AUCUN BACKGROUND DETECTE !!!");
        }

        if (myChallenge.name != "SO_Tuto")
        {
            black.SetActive(false);
        }

        //Desactive par default les logs timeleine.
        uiLogsTimeline.SetActive(false);
        uiVictoire.SetActive(false);
    }

    public void LaunchIChallenge()
    {
        StartCoroutine(StartChallenge());
    }

    public IEnumerator StartChallenge()
    {
        Debug.Log("Start Challenge !");
        //Apparition des cases
        SpawnCases();

        yield return new WaitForEndOfFrame();

        //Place les acteurs sur les cases.
        InitialiseAllPosition();

        yield return new WaitForEndOfFrame();

        if (GameManager.instance)
        {
            //Cache toute l'Ui pour les dialogue.
            ShowUiChallenge(false);
        }

        //Lance l'intro.
        StartIntroChallenge();
    }

    #region Mes fonctions

    #region Dialogue
    public void ShowUiChallenge(bool active)
    {
        //Pour l'énoncé.
        uiEtape.SetActive(active);

        //Pour l'Ui des actor.
        uiStats.SetActive(active);

        //Pour l'interface.
        myInterface.gameObject.SetActive(active);

        //Pour le plateau.
        foreach (C_Case thisCase in plateau)
        {
            thisCase.gameObject.SetActive(active);
        }

        //Pour activer l'Ui des logs.
        uiLogsAnimator.gameObject.SetActive(active);
        uiLogsAnimator.gameObject.GetComponentInChildren<Image>().enabled = active;
    }

    public void SetCanContinueToYes()
    {
        myInterface.canContinue = true;
        /*if (GameManager.instance.textToWriteIn == GetuiLogs().GetComponentInChildren<TMP_Text>())
        {

        }
        else
        {
            AudioManager.instanceAM.Stop("Dialogue");
        }*/
    }
    public void SetCanContinueToNo()
    {
        myInterface.canContinue = false;
        /*if (GameManager.instance.textToWriteIn == GetuiLogs().GetComponentInChildren<TMP_Text>())
        {

        }
        else
        {
            AudioManager.instanceAM.Play("Dialogue");
        }*/
    }
    #endregion

    #region Début de partie
    void SpawnCases()
    {
        Debug.Log("Spawn Case");
        plateauGameObject.GetComponent<HorizontalLayoutGroup>().spacing = myChallenge.spaceCase;

        //Spawn toutes les cases.
        for (int i = 0; i < myChallenge.nbCase; i++)
        {
            //Création d'une case
            C_Case newCase = Instantiate(myCase, plateauGameObject.transform);

            newCase.AddNumber(i + 1);

            plateau.Add(newCase);
        }
    }

    //Set la position de tous les acteurs sur les cases.
    void InitialiseAllPosition()
    {
        //Force update sur le parent canva. A LE PLACER AVANT LE SPAWN DES PERSO.
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponentInChildren<RectTransform>());

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

        //Fait spawn les acteurs/Acc.
        void SpawnActor(List<InitialActorPosition> listPosition)
        {
            if (GameManager.instance)
            {
                Debug.LogWarning("GameManager détecté ! Spawn des actor en cours...");

                //Récupère les info du GameManager
                foreach (GameObject thisActor in GameManager.instance.GetTeam())
                {
                    foreach (InitialActorPosition position in listPosition)
                    {
                        //Check si dans les info du challenge est dans l'équipe stocké dans le GameManager.
                        if (thisActor.GetComponent<C_Actor>().GetDataActor().name == position.perso.GetComponent<C_Actor>().GetDataActor().name)
                        {
                            #region Pour setup le sprite de Morgan en pyjama.
                            //Check si le challenge en question c'est le tuto.
                            if (thisActor.GetComponent<C_Actor>().GetDataActor().name == "Morgan")
                            {
                                if (myChallenge.name == "SO_Tuto")
                                {
                                    thisActor.GetComponent<C_Actor>().GetDataActor().challengeSprite = Resources.Load<Sprite>("Sprite/Character/Morgan/Morgan_Dodo_Chara_Challenge");
                                    thisActor.GetComponent<C_Actor>().GetDataActor().challengeSpriteOnCata = Resources.Load<Sprite>("Sprite/Character/Morgan/Morgan_Dodo_Cata_Chara_Challenge");

                                    //thisActor.GetComponent<C_Actor>().CheckInDanger();
                                }
                                else
                                {
                                    thisActor.GetComponent<C_Actor>().GetDataActor().challengeSprite = Resources.Load<Sprite>("Sprite/Character/Morgan/Morgan_Chara_Challenge");
                                    thisActor.GetComponent<C_Actor>().GetDataActor().challengeSpriteOnCata = Resources.Load<Sprite>("Sprite/Character/Morgan/Morgan_Cata_Chara_Challenge");
                                }
                            }
                            #endregion

                            //Ini data actor.
                            thisActor.GetComponent<C_Actor>().IniChallenge();

                            //Replace l'actor dans un autre Transform.
                            thisActor.transform.parent = GameObject.Find("BackGround").transform;

                            //Placement sur le plateau.
                            PlacePionOnBoard(thisActor.GetComponent<C_Actor>(), position.position, false);
                            thisActor.GetComponent<C_Actor>().SetInDanger(false);
                            thisActor.transform.localScale = Vector3.one;

                            //New Ui stats
                            C_Stats newStats = Instantiate(uiStatsPrefab, uiStats.transform);

                            //Add Ui Stats
                            thisActor.GetComponent<C_Actor>().SetUiStats(newStats);
                            thisActor.GetComponent<C_Actor>().GetUiStats().InitUiStats(thisActor.GetComponent<C_Actor>());

                            //Update UI
                            thisActor.GetComponent<C_Actor>().UpdateUiStats();

                            Debug.Log(thisActor.GetComponent<C_Actor>().GetDataActor().vfxUiGoodAction);
                            Debug.Log(uiGoodAction.transform);

                            //Mise en place du VFX de bonne action.
                            if (thisActor.GetComponent<C_Actor>().GetDataActor().vfxUiGoodAction != null)
                            {
                                Instantiate(thisActor.GetComponent<C_Actor>().GetDataActor().vfxUiGoodAction.gameObject, uiGoodAction.transform);
                            }
                            else
                            {
                                Debug.LogWarning("Pas de vfx good action de détecté !");
                            }

                            myTeam.Add(thisActor.GetComponent<C_Actor>());
                        }
                        else
                        {
                            //Cache les actor qui ne seront pas présent dans ce challenge.
                            //thisActor.SetActive(false);
                        }
                    }
                }
            }
            else
            {
                Debug.LogWarning("Pas de GameManager détecté ! Spawn de nouvel actor en cours...");

                foreach (InitialActorPosition position in listPosition)
                {
                    //New actor
                    C_Actor thisActor = Instantiate(position.perso, GameObject.Find("BackGround").transform);
                    thisActor.IniChallenge();
                    PlacePionOnBoard(thisActor, position.position, false);
                    thisActor.GetComponent<C_Actor>().CheckInDanger();
                    thisActor.transform.localScale = Vector3.one;

                    //Centrage sur la case et position sur Y.
                    thisActor.transform.localPosition = new Vector3();

                    //New Ui stats
                    C_Stats newStats = Instantiate(uiStatsPrefab, uiStats.transform);

                    //Add Ui Stats
                    thisActor.SetUiStats(newStats);
                    thisActor.GetComponent<C_Actor>().GetUiStats().InitUiStats(thisActor.GetComponent<C_Actor>());

                    //Update UI
                    thisActor.UpdateUiStats();

                    //Mise en place du VFX de bonne action.
                    if (thisActor.GetComponent<C_Actor>().GetDataActor().vfxUiGoodAction != null)
                    {
                        GameObject newVfxGoodAction = Instantiate(thisActor.GetComponent<C_Actor>().GetDataActor().vfxUiGoodAction.gameObject, uiGoodAction.transform);
                        //thisActor.GetComponent<C_Actor>().GetDataActor().vfxUiGoodAction = newVfxGoodAction.GetComponent<Animator>();
                    }
                    else
                    {
                        Debug.LogWarning("Pas de vfx good action de détecté !");
                    }

                    myTeam.Add(thisActor);
                }
            }
        }

        void SpawnAcc(List<InitialAccPosition> listPosition)
        {
            foreach (InitialAccPosition position in listPosition)
            {
                C_Accessories myAcc = Instantiate(position.acc, plateau[position.position].transform);

                //Replace l'actor dans un autre Transform.
                myAcc.transform.parent = GameObject.Find("BackGround").transform;

                PlacePionOnBoard(myAcc, position.position, false);

                listAcc.Add(myAcc);
            }
        }
    }

    public void StartIntroChallenge()
    {
        //Vérifie si il y a un GameManager
        if (GameManager.instance)
        {
            Debug.Log("Lancement de la transition");

            onDialogue = true;

            //Pour renseigner le challenge dans le GameManager.
            GameManager.instance.C = this;

            GameManager.instance.textToWriteIn = uiLogs.GetComponentInChildren<TMP_Text>();

            //Vérifie si il y a du dialogue.
            if (myChallenge.introChallenge)
            {
                //Ajoute les fonction pour permettre la navigation dans les dialogues.
                uiLogs.GetComponentInChildren<TextAnimatorPlayer>().onTextShowed.AddListener(() => SetCanContinueToYes());
                uiLogs.GetComponentInChildren<TextAnimatorPlayer>().onTypewriterStart.AddListener(() => SetCanContinueToNo());

                #region Setup les dialogues dans les challenges
                //Pour attacher les fonction à tous les actor de ce challenge pour les dialogues.
                foreach (C_Actor thisActor in myTeam)
                {
                    thisActor.GetComponent<C_Actor>().txtHautGauche.GetComponent<TextAnimatorPlayer>().onTypewriterStart.AddListener(() => SetCanContinueToNo());
                    thisActor.GetComponent<C_Actor>().txtHautDroite.GetComponent<TextAnimatorPlayer>().onTypewriterStart.AddListener(() => SetCanContinueToNo());
                    thisActor.GetComponent<C_Actor>().txtBasGauche.GetComponent<TextAnimatorPlayer>().onTypewriterStart.AddListener(() => SetCanContinueToNo());
                    thisActor.GetComponent<C_Actor>().txtBasDroite.GetComponent<TextAnimatorPlayer>().onTypewriterStart.AddListener(() => SetCanContinueToNo());

                    thisActor.GetComponent<C_Actor>().txtHautGauche.GetComponent<TextAnimatorPlayer>().onTextShowed.AddListener(() => SetCanContinueToYes());
                    thisActor.GetComponent<C_Actor>().txtHautDroite.GetComponent<TextAnimatorPlayer>().onTextShowed.AddListener(() => SetCanContinueToYes());
                    thisActor.GetComponent<C_Actor>().txtBasGauche.GetComponent<TextAnimatorPlayer>().onTextShowed.AddListener(() => SetCanContinueToYes());
                    thisActor.GetComponent<C_Actor>().txtBasDroite.GetComponent<TextAnimatorPlayer>().onTextShowed.AddListener(() => SetCanContinueToYes());
                }
                #endregion

                GameManager.instance.EnterDialogueMode(myChallenge.introChallenge);
            }
        }
        else
        {
            Debug.Log("Lancement direct du challenge");
            StartChallenge(null);
            ShowUiChallenge(true);
            onDialogue = false;
        }
    }

    public void StartChallenge(string name)
    {
        onDialogue = false;

        //Check si après le dialogue ils sont en danger.
        foreach(C_Case thisCase in plateau)
        {
            thisCase.CheckIsInDanger();
        }

        //Desactive les input.
        GetComponent<PlayerInput>().enabled = false;

        //Change l'état du challenge.
        myPhaseDeJeu = PhaseDeJeu.PlayerTrun;

        //Lance l'animation qui présente le challenge après la transition
        vfxStartChallenge.SetTrigger("start");

        if (GameManager.instance)
        {
            GameManager.instance.ExitDialogueMode();
        }

        //Re-active le fond des logs.

        #region Initialisation

        #region Instance des data challenge.
        //Instancie le challenge
        myChallenge = SO_Challenge.Instantiate(myChallenge);

        for (int i = 0; i < myChallenge.listEtape.Count; i++)
        {
            myChallenge.listEtape[i] = SO_Etape.Instantiate(myChallenge.listEtape[i]);

            myChallenge.listEtape[i].rightAnswer = SO_ActionClass.Instantiate(myChallenge.listEtape[i].rightAnswer);

            for (int j = 0; j < myChallenge.listEtape[i].actions.Count; j++)
            {
                myChallenge.listEtape[i].actions[j] = SO_ActionClass.Instantiate(myChallenge.listEtape[i].actions[j]);

                if (myChallenge.listEtape[i].actions[j].nextAction != null)
                {
                    myChallenge.listEtape[i].actions[j].nextAction = SO_ActionClass.Instantiate(myChallenge.listEtape[i].actions[j].nextAction);
                }
            }

            //Pour ajouter l'action attendre dans toutes les étapes.
            myChallenge.listEtape[i].actions.Add(SO_ActionClass.Instantiate(Resources.Load<SO_ActionClass>("Attendre")));
        }
        #endregion

        //Set l'étape en question.
        currentStep = myChallenge.listEtape[0];

        if (currentStep.useCata)
        {
            currentCata = myChallenge.listCatastrophy[0];
        }

        currentActor = myTeam[0];

        UpdateUi();

        //Lance directement le tour du joueur
        uiGameOver.SetActive(false);
        #endregion
    }
    #endregion

    #region Tour du joueur
    //Fonction pour faire spawn les cata.
    void InitialiseCata()
    {
        //SFX
        if (AudioManager.instanceAM)
        {
            AudioManager.instanceAM.Play("Catastrophe");
        }

        //Supprime toutes les catasur le plateau.
        foreach (var thisCase in plateau)
        {
            thisCase.DestroyVfxCata();
        }

        //Initialise la cata (Random avec 1 valeur).
        if (currentCata.modeAttack == SO_Catastrophy.EModeAttack.Random)
        {
            //Augmente ou réduit le nombre.
            int newInt = UnityEngine.Random.Range(0, plateau.Count);

            //Vide la liste.
            currentCata.targetCase.Clear();

            //Ajoute la valeur aléatoire.
            currentCata.targetCase.Add(newInt);
        }

        //Affiche la prochaine cata.
        foreach (var thisCase in currentCata.targetCase)
        {
            //Check si le VFX est bien renseigné.
            if (currentCata.vfxCataPrefab == null)
            {
                Debug.LogWarning("Il y a pas de cata dans cette cata !");
                return;
            }

            plateau[thisCase].ShowDangerZone(currentCata.vfxCataPrefab);
            Debug.Log("nouvelle Cata sur la case " + thisCase);
        }

        //Check si les actor sont en danger
        foreach (C_Case thisCase in plateau)
        {
            thisCase.CheckIsInDanger();
        }
    }

    //Pour Update l'UI. CHANGER LA FONCTION !!!!
    void UpdateUi()
    {
        uiEtape.GetComponentInChildren<TMP_Text>().text = myChallenge.objectif;
    }

    //Pour faire déplacer le curseur + active la preview.
    public void WriteStatsPreview()
    {
        if (eventSystem.currentSelectedGameObject != null && eventSystem.currentSelectedGameObject.activeSelf)
        {
            #region Curseur
            //Cache les autres curseur.
            foreach (GameObject thisActionButton in myInterface.GetListActionButton())
            {
                thisActionButton.GetComponent<C_ActionButton>().HideCurseur();
            }

            //Chech si il possède un bouton pour la gestion de curseur.
            if (eventSystem.currentSelectedGameObject.GetComponent<C_ActionButton>())
            {
                //Fait apparaitre le curseur.
                eventSystem.currentSelectedGameObject.GetComponent<C_ActionButton>().ShowCurseur(currentActor.GetDataActor().headButton);
            }
            #endregion

            if (eventSystem.currentSelectedGameObject.GetComponent<C_ActionButton>())
            {
                //Affiche la preview.
                GetComponent<C_PreviewAction>().ShowPreview(eventSystem.currentSelectedGameObject.GetComponent<C_ActionButton>().GetActionClass(), myTeam);

                //SFX
                if (AudioManager.instanceAM)
                {
                    AudioManager.instanceAM.Play(sfxOverButton);
                }
            }
        }
    }

    //Fonction qui est stocké dans les button action donné par l'interface + permet de passer à l'acteur suivant ou alors de lancer la phase de résolution.
    public void ConfirmAction(SO_ActionClass thisAction)
    {
        #region Desactive toutes les preview

        //Desactive la preview de l'actor.
        currentActor.GetUiStats().ResetUiPreview();

        //Detruit toutes les preview de movment.
        DestroyAllMovementPreview();

        //Supprime les textes de preview + cache la barre.
        GetComponent<C_PreviewAction>().DestroyAllPreview(myTeam);
        GetComponent<C_PreviewAction>().ActivePreviewBarre(false);
        #endregion

        #region Création d'une nouvelle class de reso
        //Création d'une nouvelle class pour ensuite ajouter dans la liste qui va etre utilisé dans la phase de résolution.
        ActorResolution actorResolution = new ActorResolution();

        //Renseigne l'actor actuel + l'action.
        actorResolution.actor = currentActor;
        actorResolution.action = thisAction;

        //Ajoute à la liste.
        listRes.Add(actorResolution);
        #endregion

        #region Feedback
        //SFX
        if (AudioManager.instanceAM)
        {
            AudioManager.instanceAM.Play(sfxSelectButton);
        }

        //FeedBack sur l'actor.
        currentActor.PlayAnimSelectAction();

        //Ferme l'interface.
        myInterface.GoBack();
        #endregion

        //Remplace tout les sprite des preview de déplacement par une autre image (petite tete des perso)

        //Si il reste des acteurs à jouer, alors tu passe à l'acteur suivant, sinon tu passe à la phase de "résolution".
        if (myTeam.IndexOf(currentActor) != myTeam.Count - 1)
        {
            //Passe à l'acteur suivant.
            NextActor();
        }
        else
        {
            //Pour retirer le contour blanc.
            currentActor = null;
            UpdateActorSelected();

            //Début de la list de la phase de réso.
            currentResolution = listRes[0];

            //Cache les boutons + ferme l'interface.
            //Animation.
            myInterface.ResetTargetButton();
            myInterface.GetComponent<Animator>().SetTrigger("Close");

            //Rend les couleurs sur tous les actor.
            foreach (C_Actor thisActor in myTeam)
            {
                thisActor.GetImageActor().sprite = thisActor.GetDataActor().challengeSprite;
                thisActor.CheckInDanger();
            }

            //Supprime toutes les preview de mouvment.

            #region lance la phase de reso
            myPhaseDeJeu = PhaseDeJeu.ResoTurn;
            vfxResoTurn.SetTrigger("PlayerTurn");
            #endregion

            //Deselct le dernier bouton.
            eventSystem.SetSelectedGameObject(null);

            //Supprime toutes les preview.
            //DestroyAllMovementPreview();
        }
    }

    public void PlayerTurn()
    {
        #region Tuto
        if (canMakeTuto)
        {
            //Check si c'est le premier niveau.
            if (myChallenge.name == "SO_Tuto(Clone)")
            {
                //ETAPE 1.
                if (currentStep.name == "SO_step1tuto(Clone)" || currentStep.name == "SO_step2tuto(Clone)" || currentStep.name == "SO_step3tuto(Clone)")
                {
                    Debug.Log("Lancement du tuto " + myChallenge.listEtape.IndexOf(currentStep) + 1);

                    myInterface.SetCurrentInterface(C_Interface.Interface.Tuto);

                    //Lance l'animation.
                    GetComponentInChildren<C_Tuto>().NextTuto(myChallenge.listEtape.IndexOf(currentStep) + 1);

                    canMakeTuto = false;
                }
            }
            else if (myChallenge.name == "SO_lvl2A(Clone)")
            {
                myInterface.SetCurrentInterface(C_Interface.Interface.Tuto);

                //Lance l'animation.
                GetComponentInChildren<C_Tuto>().NextTuto(4);

                canMakeTuto = false;
            }
        }
        #endregion

        Debug.Log("Player turn !");

        UpdateUi();

        //Vide la listeReso
        listRes = new List<ActorResolution>();

        #region Initialise la prochaine cata.
        if (currentStep.useCata)
        {
            //Check si il n'est pas null.
            if (currentCata == null)
            {
                currentCata = myChallenge.listCatastrophy[0];
            }

            InitialiseCata();

            canUseCata = true;
        }
        else
        {
            canUseCata = false;
        }
        #endregion

        #region Check si le perso est jouable
        if (!currentActor.GetIsOut())
        {
            //Update le contour blanc
            UpdateActorSelected();
        }
        else
        {
            NextActor();
            PlayerTurn();
        }
        #endregion
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
    #endregion

    #region Preview
    public List<C_Actor> GetOtherActor(SO_ActionClass thisActionClass)
    {
        #region Racourci
        int range = thisActionClass.GetRange();

        Interaction.ETypeDirectionTarget direction = thisActionClass.GetTypeDirectionRange();
        #endregion

        //Création d'une liste.
        List<C_Actor> actorHit = new List<C_Actor>();

        //Boucle avec la range.
        for (int i = 0; i < range; i++)
        {
            if (direction != Interaction.ETypeDirectionTarget.None)
            {
                //Boucle pour check sur tout les actor du challenge.
                foreach (C_Actor thisOtherActor in myTeam)
                {
                    //Check si c'est pas égal au currentActor.
                    if (thisOtherActor != currentActor)
                    {
                        //Check quel direction la range va faire effet.
                        switch (direction)
                        {
                            //Si "otherActor" est dans la range alors lui aussi on lui affiche les preview mais avec les info pour "other".
                            case Interaction.ETypeDirectionTarget.Right:
                                //Calcul vers la droite.
                                CheckPositionOther(currentActor, i, thisOtherActor);
                                break;
                            case Interaction.ETypeDirectionTarget.Left:
                                //Calcul vers la gauche.
                                CheckPositionOther(currentActor, -i, thisOtherActor);
                                break;
                            case Interaction.ETypeDirectionTarget.RightAndLeft:
                                //Calcul vers la droite + gauche.
                                CheckPositionOther(currentActor, i, thisOtherActor);
                                CheckPositionOther(currentActor, -i, thisOtherActor);
                                break;
                        }
                    }
                }
            }
            else
            {
                Debug.LogWarning("AUCUNE DIRECTION DE MOUVEMENT EST ENTRE !");
            }
        }

        //Fonction pour check si il y a des acteurs dans la range.
        void CheckPositionOther(C_Actor thisActor, int position, C_Actor target)
        {
            if (thisActor.GetPosition() + position >= plateau.Count - 1)
            {
                if (0 + position == target.GetPosition() && target != thisActor)
                {
                    Debug.Log(target.name + " à été trouvé ! à la position: " + target.GetPosition());

                    actorHit.Add(target);
                }
            }
            else if (thisActor.GetPosition() + position <= 0)
            {
                if (0 + position == target.GetPosition() && target != thisActor)
                {
                    Debug.Log(target.name + " à été trouvé ! à la position: " + target.GetPosition());

                    actorHit.Add(target);
                }
            }
            else if (thisActor.GetPosition() + position == target.GetPosition() && target != thisActor)
            {
                Debug.Log(target.name + " à été trouvé ! à la position: " + target.GetPosition());

                actorHit.Add(target);
            }
        }

        return actorHit;
    }

    public void DestroyAllMovementPreview()
    {
        if (plateauPreview.Count > -1)
        {
            foreach (Image thisMovementPreview in plateauPreview)
            {
                Destroy(thisMovementPreview.gameObject);
            }

            plateauPreview.Clear();
        }
    }

    public void PreviewPlateau(SO_ActionClass thisActionClass)
    {
        //Vérifie si c'est bien un actor est pas un acc.
        if (currentActor.GetComponent<C_Actor>())
        {
            //Check si la liste n'est pas vide
            if (thisActionClass.listInteraction.Count != 0)
            {
                //Fait toute la liste des cible.
                foreach (Interaction thisInteraction in thisActionClass.listInteraction)
                {
                    //Check si c'est égale à "actorTarget".
                    if (thisInteraction.whatTarget == Interaction.ETypeTarget.Soi)
                    {
                        AddPreviewActor(Interaction.ETypeTarget.Soi, thisInteraction, currentActor);
                    }
                    else if (thisInteraction.whatTarget == Interaction.ETypeTarget.Other)
                    {
                        //Créer la liste pour "other"
                        if (thisActionClass.CheckOtherInAction())
                        {
                            //Récupère la liste des actor touché et affiche leur preview.
                            foreach (C_Actor thisOtherActor in GetComponent<C_Challenge>().GetOtherActor(thisActionClass))
                            {
                                AddPreviewActor(Interaction.ETypeTarget.Other, thisInteraction, thisOtherActor);
                            }
                        }
                    }
                }
            }
        }

        void AddPreviewActor(Interaction.ETypeTarget target, Interaction thisInteraction, C_Actor thisActor)
        {
            //Applique à l'actor SEULEMENT LES STATS les stats.
            foreach (TargetStats thisTargetStats in thisInteraction.listTargetStats)
            {
                if (thisTargetStats.whatStatsTarget == TargetStats.ETypeStatsTarget.Stats)
                {
                    if (thisTargetStats.whatStats == TargetStats.ETypeStats.Calm) //Check si c'est pour le calm.
                    {
                        Debug.Log("Add UiPreviewCalm");

                        C_PreviewAction.onPreview += thisActor.GetUiStats().UiPreviewCalm;
                    }
                    else if (thisTargetStats.whatStats == TargetStats.ETypeStats.Energy) //Check si c'est pour l'energie.
                    {
                        Debug.Log("Add UiPreviewEnergy");

                        C_PreviewAction.onPreview += thisActor.GetUiStats().UiPreviewEnergy;
                    }
                }
                else if (thisTargetStats.whatStatsTarget == TargetStats.ETypeStatsTarget.Movement)
                {
                    Debug.Log("Add Preview Movement");

                    //C_PreviewAction.onPreview += thisActor.UiPreviewMovement;

                    MovementPreview(target, thisActor, thisActionClass);
                }
            }
        }
    }

    void MovementPreview(Interaction.ETypeTarget target, C_Actor thisActor, SO_ActionClass thisActionClass)
    {
        #region  Création de data
        //Création de sa position sur le plateau.
        TargetStats.ETypeMove whatMove = thisActionClass.GetWhatMove(target);
        bool isTp = thisActionClass.GetIsTp(target);
        int position = thisActor.GetPosition();
        int nbMove = thisActionClass.GetValue(target, TargetStats.ETypeStatsTarget.Movement);
        #endregion

        PlacePreviewMovement(whatMove, currentActor, position, nbMove, isTp);

        void PlacePreviewMovement(TargetStats.ETypeMove whatMove, C_Actor targetActor, int position, int nbMove, bool isTp)
        {
            #region création d'une Preview.
            //Ajout d'un component d'image dans l'objet.
            Image thisPreview = new GameObject().AddComponent<Image>();
            //Desactive le mask.
            thisPreview.maskable = false;
            //Set le parent de l'image.
            thisPreview.transform.parent = targetActor.GetImageActor().transform;
            //Scale
            thisPreview.gameObject.transform.localScale = Vector3.one;
            //Taille
            thisPreview.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(targetActor.GetComponent<RectTransform>().rect.width, targetActor.GetComponent<RectTransform>().rect.height);
            //Set l'image
            thisPreview.sprite = targetActor.GetDataActor().challengeSprite;
            //Change le nom
            thisPreview.name = targetActor.name + "_Preview";
            //Change la couleur.
            thisPreview.color = new Color32(255, 255, 255, 150);
            //Et ajouté dans la liste des preview de movement.
            plateauPreview.Add(thisPreview);
            #endregion

            #region Check mode de déplacement
            //Check si c'est le mode normal de déplacement ou alors le mode target case.
            if (whatMove == TargetStats.ETypeMove.Right || whatMove == TargetStats.ETypeMove.Left) //Normal move mode.
            {
                //Check si cette valeur doit etre negative ou non pour setup correctement la direction.
                if (whatMove == TargetStats.ETypeMove.Left)
                {
                    nbMove = -nbMove;
                }

                CheckIfNotExceed();
            }
            else //Passe en mode "targetCase". Pour permettre de bien setup le déplacement meme si la valeur est trop élevé par rapport au nombre de case dans la liste.
            {
                nbMove--;

                //Check si le nombre de déplacement est trop élevé par rapport au nombre de case.
                if (nbMove > plateau.Count - 1)
                {
                    Debug.LogWarning("La valeur de déplacement et trop élevé par rapport au nombre de cases sur le plateau la valeur sera donc égale à 0.");

                    nbMove = 0;
                }
            }
            #endregion

            //Check si un autre membre de l'équipe occupe deja a place. A voir pour le déplacer après que l'actor ait bougé.
            foreach (C_Actor thisOtherActor in myTeam)
            {
                //Si dans la liste de l'équipe c'est pas égale à l'actor qui joue.
                if (thisActor != thisOtherActor)
                {
                    //Détection de si il y a un autres actor.
                    if (nbMove == thisOtherActor.GetPosition())
                    {
                        //Check si c'est une Tp ou non.
                        if (isTp)
                        {
                            Debug.Log("J'échange de place !");

                            //Place l'autre actor à la position de notre actor.

                            #region création d'une Preview.
                            //Ajout d'un component d'image dans l'objet.
                            Image thisOtherPreview = new GameObject().AddComponent<Image>();
                            //Set le parent de l'image.
                            thisOtherPreview.transform.parent = targetActor.transform;
                            //Scale
                            thisOtherPreview.gameObject.transform.localScale = Vector3.one;
                            //Taille
                            thisOtherPreview.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(targetActor.GetComponent<RectTransform>().rect.width, targetActor.GetComponent<RectTransform>().rect.height);
                            //Set l'image
                            thisOtherPreview.sprite = targetActor.GetDataActor().challengeSprite;
                            //Change le nom
                            thisOtherPreview.name = targetActor.name + "_Preview";
                            //Change la couleur.
                            thisOtherPreview.color = new Color32(255, 255, 255, 150);
                            //Et ajouté dans la liste des preview de movement.
                            plateauPreview.Add(thisOtherPreview);
                            #endregion

                            thisOtherPreview.transform.position = new Vector3(plateau[currentActor.GetPosition()].transform.position.x, 0, plateau[currentActor.GetPosition()].transform.position.z);
                        }
                        else
                        {
                            Debug.Log("Pousse toi !");

                            //Le spawn de la preview sera décalé de 1 dans la direction du déplacement avec l'image de l'actor visé.

                            PlacePreviewMovement(whatMove, thisOtherActor, thisOtherActor.GetPosition(), 1, isTp);
                        }
                    }
                }
            }

            //Nouvelle position de l'actor visé sur une case visée.
            thisPreview.transform.position = new Vector3(plateau[nbMove].transform.position.x, 0, plateau[nbMove].transform.position.z);

            //Permet de réduire/augmenter la valeur pour placer l'actor sur le plateau.
            void CheckIfNotExceed()
            {
                //Si la valeur ne dépasse pas la plateau alors pas besoin de modification de valeur.
                if (position + nbMove < plateau.Count && position + nbMove > -1)
                {
                    nbMove = position + nbMove;
                    return;
                }

                //Pour placer l'actor de l'autre coté du plateau correctement.
                if (nbMove > 0)
                {
                    //Vers la droite.
                    for (int i = 0; i <= nbMove; i++)
                    {
                        //Detection de si le perso est au bord (à droite).
                        if (position + i > plateau.Count - 1)
                        {
                            //Replace le pion sur la case 0.
                            thisPreview.transform.position = new Vector3(plateau[0].transform.position.x, 0, plateau[0].transform.position.z);
                            position = 0;

                            nbMove -= i;
                        }
                    }
                }
                else if (nbMove < 0)
                {
                    //Vers la gauche.
                    for (int i = 0; i >= nbMove; i--)
                    {
                        if (position + i < 0)
                        {
                            //Replace le pion sur la case sur la case la plus à droite.
                            thisPreview.transform.position = new Vector3(plateau[plateau.Count - 1].transform.position.x, 0, plateau[plateau.Count - 1].transform.position.z);
                            position = plateau.Count - 1;

                            nbMove -= i;
                        }
                    }
                }

                //Puis recheck si le calcul est bon.
                CheckIfNotExceed();
            }
        }
    }

    /*Old Version
    public void MovementPreview(SO_ActionClass thisActionClass)
    {
        Debug.Log("Launch preview movement");
        
        DestroyAllMovementPreview();

        //Regarde dans toutes les actions combien de movement il y a.
        foreach (Interaction thisInteraction in thisActionClass.listInteraction)
        {
            //Check si c'est "self" ou other.
            if (thisInteraction.whatTarget == Interaction.ETypeTarget.Soi)
            {
                //Place la preview par rapport au "self".
                Debug.Log("Launch preview movement self");

                #region  Création de data
                //Création de sa position sur le plateau.
                TargetStats.ETypeMove whatMove = thisActionClass.GetWhatMove(Interaction.ETypeTarget.Soi);
                bool isTp = thisActionClass.GetIsTp(Interaction.ETypeTarget.Soi);
                int position = currentActor.GetPosition();
                int nbMove = thisActionClass.GetValue(Interaction.ETypeTarget.Soi, TargetStats.ETypeStatsTarget.Movement);
                #endregion

                PlacePreviewMovement(whatMove, currentActor, position, nbMove, isTp);
            }
            else if (thisInteraction.whatTarget == Interaction.ETypeTarget.Other)
            {
                //Place une preview de movement pour tous les autres actor.
                Debug.Log("Launch preview movement other");

                foreach (C_Actor thisOtherActor in myTeam)
                {
                    if (thisOtherActor != currentActor)
                    {
                        Debug.Log("C'est good pour :" + thisOtherActor.name);
                        //Boucle avec la range.
                        for (int i = 0; i < thisActionClass.GetRange(); i++)
                        {
                            Debug.Log("Je cherche sur la case " + i);

                            if (thisActionClass.GetTypeDirectionRange() != Interaction.ETypeDirectionTarget.None)
                            {
                                //Check quel direction la range va faire effet.
                                switch (thisActionClass.GetTypeDirectionRange())
                                {
                                    //Si "otherActor" est dans la range alors lui aussi on lui affiche les preview mais avec les info pour "other".
                                    case Interaction.ETypeDirectionTarget.Right:
                                        //Calcul vers la droite.
                                        CheckPositionOther(currentActor, i, thisOtherActor);
                                        Debug.Log("Direction Range = droite.");
                                        break;
                                    case Interaction.ETypeDirectionTarget.Left:
                                        //Calcul vers la gauche.
                                        CheckPositionOther(currentActor, -i, thisOtherActor);
                                        Debug.Log("Direction Range = Gauche.");
                                        break;
                                    case Interaction.ETypeDirectionTarget.RightAndLeft:
                                        //Calcul vers la droite + gauche.
                                        CheckPositionOther(currentActor, i, thisOtherActor);
                                        CheckPositionOther(currentActor, -i, thisOtherActor);
                                        Debug.Log("Direction Range = droite + gauche.");
                                        break;
                                }
                            }
                        }
                    }
                }
            }
        }

        void PlacePreviewMovement(TargetStats.ETypeMove whatMove, C_Actor targetActor, int position, int nbMove, bool isTp)
        {
            #region création d'une Preview.
            //Ajout d'un component d'image dans l'objet.
            Image thisPreview = new GameObject().AddComponent<Image>();
            //Desactive le mask.
            thisPreview.maskable = false;
            //Set le parent de l'image.
            thisPreview.transform.parent = targetActor.GetImageActor().transform;
            //Scale
            thisPreview.gameObject.transform.localScale = Vector3.one;
            //Taille
            thisPreview.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(targetActor.GetComponent<RectTransform>().rect.width, targetActor.GetComponent<RectTransform>().rect.height);
            //Set l'image
            thisPreview.sprite = targetActor.GetDataActor().challengeSprite;
            //Change le nom
            thisPreview.name = targetActor.name + "_Preview";
            //Change la couleur.
            thisPreview.color = new Color32(255, 255, 255, 150);
            //Et ajouté dans la liste des preview de movement.
            plateauPreview.Add(thisPreview);
            #endregion

            #region Check mode de déplacement
            //Check si c'est le mode normal de déplacement ou alors le mode target case.
            if (whatMove == TargetStats.ETypeMove.Right || whatMove == TargetStats.ETypeMove.Left) //Normal move mode.
            {
                //Check si cette valeur doit etre negative ou non pour setup correctement la direction.
                if (whatMove == TargetStats.ETypeMove.Left)
                {
                    nbMove = -nbMove;
                }

                CheckIfNotExceed();
            }
            else //Passe en mode "targetCase". Pour permettre de bien setup le déplacement meme si la valeur est trop élevé par rapport au nombre de case dans la liste.
            {
                nbMove--;

                //Check si le nombre de déplacement est trop élevé par rapport au nombre de case.
                if (nbMove > plateau.Count - 1)
                {
                    Debug.LogWarning("La valeur de déplacement et trop élevé par rapport au nombre de cases sur le plateau la valeur sera donc égale à 0.");

                    nbMove = 0;
                }
            }
            #endregion

            //Check si un autre membre de l'équipe occupe deja a place. A voir pour le déplacer après que l'actor ait bougé.
            foreach (C_Actor thisOtherActor in myTeam)
            {
                //Si dans la liste de l'équipe c'est pas égale à l'actor qui joue.
                if (currentActor != thisOtherActor)
                {
                    //Détection de si il y a un autres actor.
                    if (nbMove == thisOtherActor.GetPosition())
                    {
                        //Check si c'est une Tp ou non.
                        if (isTp)
                        {
                            Debug.Log("J'échange de place !");
                            //Place l'autre actor à la position de notre actor.
                            #region création d'une Preview.
                            //Ajout d'un component d'image dans l'objet.
                            Image thisOtherPreview = new GameObject().AddComponent<Image>();
                            //Set le parent de l'image.
                            thisOtherPreview.transform.parent = targetActor.transform;
                            //Scale
                            thisOtherPreview.gameObject.transform.localScale = Vector3.one;
                            //Taille
                            thisOtherPreview.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(targetActor.GetComponent<RectTransform>().rect.width, targetActor.GetComponent<RectTransform>().rect.height);
                            //Set l'image
                            thisOtherPreview.sprite = targetActor.GetDataActor().challengeSprite;
                            //Change le nom
                            thisOtherPreview.name = targetActor.name + "_Preview";
                            //Change la couleur.
                            thisOtherPreview.color = new Color32(255, 255, 255, 150);
                            //Et ajouté dans la liste des preview de movement.
                            plateauPreview.Add(thisOtherPreview);
                            #endregion

                            thisOtherPreview.transform.position = new Vector3(plateau[currentActor.GetPosition()].transform.position.x, 0, plateau[currentActor.GetPosition()].transform.position.z);
                        }
                        else
                        {
                            Debug.Log("Pousse toi !");
                            //Le spawn de la preview sera décalé de 1 dans la direction du déplacement avec l'image de l'actor visé.
                            PlacePreviewMovement(whatMove, thisOtherActor, thisOtherActor.GetPosition(), 1, isTp);
                        }
                    }
                }
            }

            //Nouvelle position de l'actor visé sur une case visée.
            thisPreview.transform.position = new Vector3(plateau[nbMove].transform.position.x, 0, plateau[nbMove].transform.position.z);

            //Permet de réduire/augmenter la valeur pour placer l'actor sur le plateau.
            void CheckIfNotExceed()
            {
                //Si la valeur ne dépasse pas la plateau alors pas besoin de modification de valeur.
                if (position + nbMove < plateau.Count && position + nbMove > -1)
                {
                    nbMove = position + nbMove;
                    return;
                }

                //Pour placer l'actor de l'autre coté du plateau correctement.
                if (nbMove > 0)
                {
                    //Vers la droite.
                    for (int i = 0; i <= nbMove; i++)
                    {
                        //Detection de si le perso est au bord (à droite).
                        if (position + i > plateau.Count - 1)
                        {
                            //Replace le pion sur la case 0.
                            thisPreview.transform.position = new Vector3(plateau[0].transform.position.x, 0, plateau[0].transform.position.z);
                            position = 0;

                            nbMove -= i;
                        }
                    }
                }
                else if (nbMove < 0)
                {
                    //Vers la gauche.
                    for (int i = 0; i >= nbMove; i--)
                    {
                        if (position + i < 0)
                        {
                            //Replace le pion sur la case sur la case la plus à droite.
                            thisPreview.transform.position = new Vector3(plateau[plateau.Count - 1].transform.position.x, 0, plateau[plateau.Count - 1].transform.position.z);
                            position = plateau.Count - 1;

                            nbMove -= i;
                        }
                    }
                }

                //Puis recheck si le calcul est bon.
                CheckIfNotExceed();
            }
        }

        //Fonction pour check si il y a des acteurs dans la range.
        bool CheckPositionOther(C_Actor thisActor, int newPosition, C_Actor target)
        {
            #region  Création de data
            //Création de sa position sur le plateau.
            TargetStats.ETypeMove whatMove = thisActionClass.GetWhatMove(Interaction.ETypeTarget.Other);
            bool isTp = thisActionClass.GetIsTp(Interaction.ETypeTarget.Other);
            int nbMove = thisActionClass.GetValue(Interaction.ETypeTarget.Other, TargetStats.ETypeStatsTarget.Movement);
            int position = target.GetPosition();
            #endregion

            if (thisActor.GetPosition() + newPosition >= plateau.Count - 1)
            {
                if (0 + position == target.GetPosition() && target != thisActor)
                {
                    Debug.Log(target.name + " à été trouvé ! à la position: " + target.GetPosition());
                    PlacePreviewMovement(whatMove, target, position, nbMove, isTp);
                    return true;
                }
            }
            else if (thisActor.GetPosition() + newPosition <= 0)
            {
                if (0 + position == target.GetPosition() && target != thisActor)
                {
                    Debug.Log(target.name + " à été trouvé ! à la position: " + target.GetPosition());
                    PlacePreviewMovement(whatMove, target, position, nbMove, isTp);
                    return true;
                }
            }
            else if (thisActor.GetPosition() + newPosition == target.GetPosition() && target != thisActor)
            {
                Debug.Log(target.name + " à été trouvé ! à la position: " + target.GetPosition());
                PlacePreviewMovement(whatMove, target, position, nbMove, isTp);
                return true;
            }

            return false;
        }
    }*/
    #endregion

    #region  Phase de résolution
    //Création d'une class pour rassembler l'acteur et l'action.
    [Serializable] public class ActorResolution
    {
        public SO_ActionClass action;
        public C_Actor actor;
    }

    //Fonction appelé par "C_Interface" pour passer à la résolution suivante.
    public void NextResolution()
    {
        //Check si l'element de la liste n'est pas null. Si ce dernier est null il passera à la reso suivante.
        if (!string.IsNullOrEmpty(currentResolution.action.GetListLogs()))
        {
            //SFX
            if (AudioManager.instanceAM)
            {
                AudioManager.instanceAM.Play(sfxWriteText);
            }

            uiLogs.GetComponentInChildren<TMP_Text>().text = currentResolution.action.currentLogs;
        }
        else if (currentStep == null) //Check si la partie est fini.
        {
            //Fin du challenge.
            //GetEventSystem().SetSelectedGameObject(null);
            EndChallenge();

            Debug.Log("Fin du niveau");
        }
        else if (listRes.IndexOf(currentResolution) < listRes.Count - 1) //Check si on n'est pas arr au dernier reso.
        {
            //Check si il reste des étapes.
            if (myChallenge.listEtape.IndexOf(currentStep) != myChallenge.listEtape.Count - 1)
            {
                Debug.Log("Next réso !");

                currentResolution = listRes[listRes.IndexOf(currentResolution) + 1];

                ResolutionTurn();
            }
            else
            {
                //Fin du challenge.
                //GetEventSystem().SetSelectedGameObject(null);
                EndChallenge();

                Debug.Log("Fin du niveau");
            }
        }
        else //Passe à la phase suivante.
        {
            Debug.Log("Fin de la phase de réso !");

            //Check si une cata est présente.
            if (canUseCata)
            {
                //Check si après la phase de réso, tous les perso sont vivant.
                if (!CheckGameOver())
                {
                    myPhaseDeJeu = PhaseDeJeu.CataTurn;

                    //Lance la phase "Cata".
                    CataTurn();
                    ResetCataLogs();
                    listCurrentCataLogs.Add(currentCata.catastrophyLog);
                    uiLogs.GetComponentInChildren<TMP_Text>().text = GetListCataLogs();
                }
            }
            else //Lance la phase du joueur.
            {
                //Redéfini le début de la liste.
                currentActor = myTeam[0];

                //Efface les logs.
                uiLogs.GetComponentInChildren<TMP_Text>().text = "";

                //transition.
                myPhaseDeJeu = PhaseDeJeu.PlayerTrun;
                vfxPlayerTurn.SetTrigger("PlayerTurn");
            }
        }
    }

    public void ResolutionTurn()
    {
        //Supprime toutes les preview de déplacement.

        //Met en noir et blanc tous les actor sauf l'actor qui joue la reso.
        foreach (C_Actor thisActor in myTeam)
        {
            if (thisActor != currentResolution.actor)
            {
                thisActor.SetSpriteChallengeBlackAndWhite();
            }
            else
            {
                thisActor.SetSpriteChallenge();
            }
        }

        //Applique toutes les actions. 1 par 1.
        //Check si l'action peut etre effectue. 
        if (CanUse(currentResolution))
        {
            Debug.Log("Peut faire l'action");

            //Utilise l'action.
            UseAction(currentResolution.action, currentResolution.actor);

            #region Pour le challenge "Tuto" redonne les bon sprite à Morgan.
            if (currentResolution.action.buttonText == "Attraper tes lunettes")
            {
                //Detruit les lunettes.
                Destroy(GameObject.Find("Glasses"));

                //Change le sprite de Morgan.
                foreach (C_Actor thisActor in myTeam)
                {
                    if (thisActor.GetDataActor().name == "Morgan")
                    {
                        thisActor.GetDataActor().challengeSprite = Resources.Load<Sprite>("Sprite/Character/Morgan/Morgan_DodoLunettes_Chara_Challenge");
                        thisActor.GetDataActor().challengeSpriteOnCata = Resources.Load<Sprite>("Sprite/Character/Morgan/Morgan_DodoLunettes_Cata_Chara_Challenge");

                        thisActor.CheckInDanger();
                    }
                }
            }
            #endregion

            #region Check si c'était la bonne action à faire
            //Check si c'est la bonne action.
            if (currentResolution.action.name == currentStep.rightAnswer.name)
            {
                //SFX
                if (AudioManager.instanceAM)
                {
                    AudioManager.instanceAM.Play(sfxGoodAction);
                }

                Debug.Log("Bonne action pour passer à l'étape suivante");

                //Vfx de bonne action.
                GameObject.Find(currentResolution.actor.GetDataActor().vfxUiGoodAction.name + "(Clone)").GetComponent<Animator>().SetTrigger("GoodAction");

                UpdateEtape();
            }
            else //Check si c'est pas une action secondaire.
            {
                foreach (ActorResolution thisReso in listRes)
                {
                    if (thisReso.action.nextAction != null)
                    {
                        for (int i = 0; i < myChallenge.listEtape[myChallenge.listEtape.IndexOf(currentStep)].actions.Count; i++)
                        {
                            if (myChallenge.listEtape[myChallenge.listEtape.IndexOf(currentStep)].actions[i] == thisReso.action)
                            {
                                Debug.Log("Update next action");
                                myChallenge.listEtape[myChallenge.listEtape.IndexOf(currentStep)].actions[i] = thisReso.action.nextAction;
                            }
                        }
                    }
                }
            }
            #endregion

            //Set les logs de la bonne action.
            currentResolution.action.ResetLogs();
            currentResolution.action.SetListLogs(true);
        }
        else
        {
            Debug.Log("Peut pas faire l'action");

            //Set les logs de la mauvaise action.
            currentResolution.action.ResetLogs();
            currentResolution.action.SetListLogs(false);
        }

        #region Logs
        //Ecrit dans les logs le résultat de l'action.
        currentResolution.action.ResetLogs();
        uiLogs.GetComponentInChildren<TMP_Text>().text = currentResolution.action.GetListLogs();
        #endregion
    }

    //vérifie la condition si l'action fonctionne.
    bool CanUse(ActorResolution thisReso)
    {
        #region Raccourcis
        AdvancedCondition advancedCondition = thisReso.action.advancedCondition;
        #endregion

        //Check si l'actor en question possède assez d'energie.
        if (thisReso.actor.GetcurrentEnergy() >= thisReso.action.GetValue(Interaction.ETypeTarget.Soi, TargetStats.ETypeStatsTarget.Stats))
        {
            Debug.Log("Possède assez d'energie");

            #region Condition avancé.
            //Check si les conditions bonus sont activé.
            if (advancedCondition.advancedCondition)
            {
                #region Check si l'action doit etre fait par un actor.
                if (advancedCondition.canMakeByOneActor)
                {
                    //Check si il est pas null.
                    if (advancedCondition.whatActor)
                    {
                        //Check si le bon actor qui utilise l'action.
                        if (advancedCondition.whatActor != thisReso.actor)
                        {
                            Debug.Log("L'action n'est pas fait par la bonne personne");
                            return false;
                        }
                    }
                    else
                    {
                        Debug.LogWarning("whatActor n'est pas setup dans l'action ! Cette condition ne sera pa sdonc prise en charge");
                    }
                }
                #endregion

                #region Check si l'action doit etre fait par un acc.
                if (advancedCondition.needAcc)
                {
                    //Check si il est pas null.
                    if (advancedCondition.whatAcc)
                    {
                        if (advancedCondition.whatAcc.GetPosition() != thisReso.actor.GetPosition())
                        {
                            Debug.Log("N'est pas sur la meme case que l'acc");
                            return false;
                        }
                    }
                    else
                    {
                        Debug.LogWarning("whatAcc n'est pas setup dans l'action ! Cette condition ne sera pa sdonc prise en charge");
                    }
                }
                #endregion

                #region Pour les action à 2.
                //Check si la condition est activé + si la réso est égale au réso actuel (permet de bloquer cette partie pour ne pas lancer en boucle la fonction).
                if (advancedCondition.needTwoActor && thisReso == currentResolution)
                {
                    int nbEchec = 0;

                    //Check si dans toute les réso un autre actor à fait aussi la meme action.
                    foreach (ActorResolution thisResoInList in listRes)
                    {
                        //Pour éviter qu'il se compte lui meme.
                        if (thisResoInList != thisReso)
                        {
                            //Check si dans la reso en cours et égale à une autre reso qui possède la meme action.
                            if (thisResoInList.action == thisReso.action)
                            {
                                Debug.Log(thisResoInList.actor.name + " utilise aussi l'action !");


                                //Check si l'autre actor peut aussi faire l'action.
                                if (CanUse(thisResoInList))
                                {
                                    //Applique seulement les stats. A VOIR PLUS TARD COMMENT LE PLACER AVEC UNE AUTRE POSITION.
                                    thisResoInList.action.SetStatsTarget(Interaction.ETypeTarget.Soi, thisResoInList.actor);

                                    //Retir l'autre actor de la liste de reso qui à fait la meme action pour éviter que l'action se joue 2 fois.
                                    listRes.Remove(thisResoInList);

                                    return true;
                                }
                            }
                            else
                            {
                                nbEchec++;

                                if (nbEchec == listRes.Count - 1)
                                {
                                    Debug.Log("Aucun autre actor ne fait l'action");

                                    return false;
                                }
                            }
                        }
                    }
                }
                #endregion
            }
            #endregion
        }
        else
        {
            return false;
        }

        return true;
    }

    //Utilise l'action.
    void UseAction(SO_ActionClass thisActionClass, C_Actor thisActor)
    {
        Debug.Log("Use this actionClass : " + thisActionClass.buttonText);

        //Applique les conséquences de stats.
        #region Self
        //Créer la liste pour "self"
        thisActionClass.SetStatsTarget(Interaction.ETypeTarget.Soi, thisActor);

        //Check si un mouvement pour "self" existe.
        CheckIfTargetMove(Interaction.ETypeTarget.Soi, thisActor);
        #endregion

        #region Other
        //Créer la liste pour "other"
        if (thisActionClass.CheckOtherInAction())
        {
            //Boucle avec la range.
            for (int i = 0; i < thisActionClass.GetRange(); i++)
            {
                Debug.Log("Je cherche sur la case " + i);

                if (thisActionClass.GetTypeDirectionRange() != Interaction.ETypeDirectionTarget.None)
                {
                    //Boucle pour check sur tout les actor du challenge.
                    foreach (C_Actor thisOtherActor in myTeam)
                    {
                        //Check quel direction la range va faire effet.
                        switch (thisActionClass.GetTypeDirectionRange())
                        {
                            //Si "otherActor" est dans la range alors lui aussi on lui affiche les preview mais avec les info pour "other".
                            case Interaction.ETypeDirectionTarget.Right:
                                //Calcul vers la droite.
                                CheckPositionOther(thisActor, i, thisOtherActor);
                                Debug.Log("Direction Range = droite.");
                                break;
                            case Interaction.ETypeDirectionTarget.Left:
                                //Calcul vers la gauche.
                                CheckPositionOther(thisActor, -i, thisOtherActor);
                                Debug.Log("Direction Range = Gauche.");
                                break;
                            case Interaction.ETypeDirectionTarget.RightAndLeft:
                                //Calcul vers la droite + gauche.
                                CheckPositionOther(thisActor, i, thisOtherActor);
                                CheckPositionOther(thisActor, -i, thisOtherActor);
                                Debug.Log("Direction Range = droite + gauche.");
                                break;
                        }
                    }
                }
                else
                {
                    Debug.LogWarning("AUCUNE DIRECTION DE MOUVEMENT EST ENTRE !");
                }
            }

            //Fonction pour check si il y a des acteurs dans la range.
            bool CheckPositionOther(C_Actor thisActor, int position, C_Actor target)
            {
                if (thisActor.GetPosition() + position >= plateau.Count - 1)
                {
                    if (0 + position == target.GetPosition() && target != thisActor)
                    {
                        Debug.Log(target.name + " à été trouvé ! à la position: " + target.GetPosition());

                        thisActionClass.SetStatsTarget(Interaction.ETypeTarget.Other, target);

                        //Check si un mouvement pour "other" existe.
                        CheckIfTargetMove(Interaction.ETypeTarget.Other, target);
                        return true;
                    }
                }
                else if (thisActor.GetPosition() + position <= 0)
                {
                    if (0 + position == target.GetPosition() && target != thisActor)
                    {
                        Debug.Log(target.name + " à été trouvé ! à la position: " + target.GetPosition());

                        thisActionClass.SetStatsTarget(Interaction.ETypeTarget.Other, target);

                        //Check si un mouvement pour "other" existe.
                        CheckIfTargetMove(Interaction.ETypeTarget.Other, target);
                        return true;
                    }
                }
                else if (thisActor.GetPosition() + position == target.GetPosition() && target != thisActor)
                {
                    Debug.Log(target.name + " à été trouvé ! à la position: " + target.GetPosition());

                    thisActionClass.SetStatsTarget(Interaction.ETypeTarget.Other, target);

                    //Check si un mouvement pour "other" existe.
                    CheckIfTargetMove(Interaction.ETypeTarget.Other, target);
                    return true;
                }

                return false;
            }
        }
        #endregion

        //Fonction pour vérifier si un mouvement est nessecaire.
        void CheckIfTargetMove(Interaction.ETypeTarget target, C_Actor thisActor)
        {
            //Check si il y a un movement.
            if (thisActionClass.GetWhatMove(target) != TargetStats.ETypeMove.None)
            {
                //Regarde d'abord c'est quoi comme type de déplacement.
                if (!thisActionClass.GetIfTargetOrNot()) //Non ciblé par un actor ou acc.
                {
                    Debug.Log("Pas ciblé par un actor ou acc.");

                    //Check si un mouvement existe.
                    if (thisActionClass.GetValue(target, TargetStats.ETypeStatsTarget.Movement) != 0)
                    {
                        //Deplace l'actor avec l'info de déplacement + type de déplacement.
                        MoveActorInBoard(thisActor, thisActionClass.GetValue(target, TargetStats.ETypeStatsTarget.Movement), thisActionClass.GetWhatMove(target), thisActionClass.GetIsTp(target));
                    }
                }
                else //Ciblé par un actor ou acc.
                {
                    Debug.Log("Ciblé par un actor ou acc.");

                    //VOIR SI BESOIN DE SETUP ICI OU DANS L'ACTION DURECTEMENT POUR LES INFO DES ACC OU ACTOR POUR SETUP LES LIENS AVEC LES OBJ DU CHALLANGE.
                    if (thisActionClass.GetTarget().GetComponent<C_Actor>())
                    {
                        thisActionClass.SetStatsTarget(target, thisActionClass.GetTarget().GetComponent<C_Actor>());
                    }
                    else if (thisActionClass.GetTarget().GetComponent<C_Accessories>())
                    {
                        thisActionClass.SetTarget(GameObject.Find(thisActionClass.GetTarget().GetComponent<C_Accessories>().GetDataAcc().name));

                        thisActionClass.SetStatsTarget(target, thisActionClass.GetTarget().GetComponent<C_Accessories>());
                    }
                }
            }
        }
    }

    #region Deplace les actor
    //Fonction qui déplace les actor.
    public void MoveActorInBoard(C_Pion thisPion, int nbMove, TargetStats.ETypeMove whatMove, bool isTp)
    {
        //Setup la position de départ.
        thisPion.GetComponent<C_Actor>().SetStartPosition(plateau[thisPion.GetPosition()].GetComponentInParent<Transform>());

        //Check si c'est le mode normal de déplacement ou alors le mode target case.
        if (whatMove == TargetStats.ETypeMove.Right || whatMove == TargetStats.ETypeMove.Left) //Normal move mode.
        {
            //Check si cette valeur doit etre negative ou non pour setup correctement la direction.
            if (whatMove == TargetStats.ETypeMove.Left)
            {
                nbMove = -nbMove;
            }

            CheckIfNotExceed();
        }
        else if (whatMove == TargetStats.ETypeMove.OnTargetCase) //Passe en mode "targetCase". Pour permettre de bien setup le déplacement meme si la valeur est trop élevé par rapport au nombre de case dans la liste.
        {
            nbMove -= 1;

            //Check si le nombre de déplacement est trop élevé par rapport au nombre de case.
            if (nbMove > plateau.Count -1)
            {
                Debug.LogWarning("La valeur de déplacement est trop élevé par rapport au nombre de cases sur le plateau la valeur sera donc égale à 0.");

                nbMove = 0;
            }
            else if (nbMove < 0)
            {
                Debug.LogWarning("La valeur de déplacement est inférieur à 0, la valeur sera donc égale à 0.");
                nbMove = 0;
            }
        }

        //Check si un autre membre de l'équipe occupe deja a place. A voir pour le déplacer après que l'actor ait bougé.
        foreach (C_Actor thisOtherActor in myTeam)
        {
            //Si dans la list de l'équipe c'est pas égale à l'actor qui joue. Et si "i" est égale à "newPosition" pour décaler seulement l'actor qui occupe la case ou on souhaite ce déplacer.
            if (thisPion != thisOtherActor)
            {
                //Détection de si il y a un autres actor.
                if (nbMove == thisOtherActor.GetPosition())
                {
                    //Check si c'est une Tp ou non.
                    if (isTp)
                    {
                        //Place l'autre actor à la position de notre actor.
                        Debug.Log(TextUtils.GetColorText(thisPion.name, Color.cyan) + " a échangé sa place avec " + TextUtils.GetColorText(thisOtherActor.name, Color.green) + ".");
                        PlacePionOnBoard(thisOtherActor, thisPion.GetPosition(), isTp);
                    }
                    else
                    {
                        //Déplace le deuxieme actor. Fonctionne en récurrence.
                        Debug.Log(TextUtils.GetColorText(thisPion.name, Color.cyan) + " a prit la place de " + TextUtils.GetColorText(thisOtherActor.name, Color.green) + " et sera déplacer " + GetDirectionOfMovement());
                        MoveActorInBoard(thisOtherActor, 1, whatMove, isTp);
                    }
                }
            }
        }

        //Nouvelle position de l'actor visé sur une case visée.
        PlacePionOnBoard(thisPion, nbMove, isTp);

        //Permet de réduire/augmenter la valeur pour placer l'actor sur le plateau.
        void CheckIfNotExceed()
        {
            //Si la valeur ne dépasse pas la plateau alors pas besoin de modification de valeur.
            if (thisPion.GetPosition() + nbMove < plateau.Count && thisPion.GetPosition() + nbMove > -1)
            {
                nbMove = thisPion.GetPosition() + nbMove;
                return;
            }

            //Pour placer l'actor de l'autre coté du plateau correctement.
            if (nbMove > 0)
            {
                //Vers la droite.
                for (int i = 0; i <= nbMove; i++)
                {
                    //Detection de si le perso est au bord (à droite).
                    if (thisPion.GetPosition() + i > plateau.Count - 1)
                    {
                        //Replace le pion sur la case 0.
                        PlacePionOnBoard(thisPion, 0, isTp);

                        //On retire le nombre de déplacement fait.
                        nbMove -= i;
                    }
                }
            }
            else if (nbMove < 0)
            {
                //Vers la gauche.
                for (int i = 0; i >= nbMove; i--)
                {
                    if (thisPion.GetPosition() + i < 0)
                    {
                        Debug.Log(nbMove);
                        //Replace le pion sur la case sur la case la plus à droite.
                        PlacePionOnBoard(thisPion, plateau.Count - 1, isTp);

                        //On retire le nombre de déplacement fait.
                        nbMove -= i;
                    }
                }
            }

            //Puis recheck si le calcul est bon.
            CheckIfNotExceed();
        }

        //Pour connaitre la direction.
        string GetDirectionOfMovement()
        {
            if (nbMove < 0)
            {
                return " à gauche.";
            }
            else if (nbMove > 0)
            {
                return " à droite.";
            }

            return "Direction Inconu.";
        }
    }

    public virtual void PlacePionOnBoard(C_Pion thisPion, int thisCase, bool isTp)
    {
        Debug.Log(thisPion.name + " : " + thisCase);

        //Supprime la dernière position.
        plateau[thisPion.GetPosition()].ResetPion();

        //Place l'actor.
        plateau[thisCase].PlacePion(thisPion);

        #region Feedback
        //Feedback

        //Fonctionne que sur les actor.
        if (thisPion.GetComponent<C_Actor>())
        {
            //Check si c'est une tp ou non
            if (isTp)
            {
                //Setup l'arrivé de la position.
                thisPion.GetComponent<C_Actor>().SetEndPosition(plateau[thisCase].GetComponentInParent<Transform>());

                //Lance alors l'animation de tp (Rotation sur lui meme).
                thisPion.GetComponent<Animator>().SetTrigger("isTp");
            }
            else //Sinon lancer l'animation de déplacement (translation entre les 2 position).
            {
                //Lance alors l'animation de déplacement (Marche).
                thisPion.GetComponent<Animator>().SetTrigger("isBump");
            }
        }
       
        #endregion
    }
    #endregion

    #endregion

    #region Tour de la Cata
    //Fonction pour appliquer la cata.
    public void ApplyCatastrophy()
    {
        //Pour tous les nombres dans la liste de la cata.
        foreach (int thisCase in currentCata.targetCase)
        {
            #region VFX Cata
            //Check si la case possède un vfx.
            if (plateau[thisCase].GetVfxCata() != null)
            {
                //VFX de la cata qui s'applique.
                plateau[thisCase].GetComponentInChildren<Animator>().SetTrigger("cata_Kaboom");
            }
            #endregion

            //Check si c'est des dégats qui s'applique à tout le monde.  CETTE PARTIE DU DEV EST INUTILE !
            if (currentCata.applyForAll)
            {
                bool onCata = false;

                Debug.Log("Applique la cata sur tout le monde");

                //Pour tous les actor.
                foreach (C_Actor thisActor in myTeam)
                {
                    if (thisCase == thisActor.GetPosition())
                    {
                        onCata = true;
                    }
                }

                if (onCata)
                {
                    //Pour tous les actor.
                    foreach (C_Actor thisActor in myTeam)
                    {
                        //Applique des conséquence grace au finction de actionClass.
                        currentCata.actionClass.SetStatsTarget(Interaction.ETypeTarget.Soi, thisActor);

                        //Vfx
                        thisActor.GetComponent<Animator>().SetTrigger("isHit");

                        thisActor.CheckIsOut();
                    }
                }
            }
            else
            {
                Debug.Log("Applique la cata sur les cases visée seulement");

                foreach (C_Actor thisActor in myTeam)
                {
                    //Check si un joueur à marché sur le lézard.
                    if (thisActor.GetPosition() == thisCase)
                    {
                        //Applique des conséquence grace au finction de actionClass.
                        UseAction(currentCata.actionClass, thisActor);

                        //Vfx
                        thisActor.GetComponent<Animator>().SetTrigger("isHit");

                        thisActor.CheckIsOut();
                    }
                }

                //Check d'abord dans un premier temps si il a touché un acc.
                foreach (C_Accessories thisAcc in listAcc)
                {
                    foreach (C_Actor thisActor in myTeam)
                    {
                        //Check si un joueur à marché sur le lézard.
                        if (thisActor.GetPosition() == thisAcc.GetPosition())
                        {
                            //Applique des conséquence grace au finction de actionClass.
                            thisActor.SetCurrentStats(-2, TargetStats.ETypeStats.Calm);

                            //Vfx
                            thisActor.GetComponent<Animator>().SetTrigger("isHit");

                            thisActor.CheckIsOut();

                            //Ajoute dans la liste un texte.
                            listCurrentCataLogs.Add(thisActor.name + " à marché sur le lézard ! Ce dernier perd 2 de calme.");
                        }
                    }


                    //Check si l'acc est sur une cata.
                    if (thisCase == thisAcc.GetPosition())
                    {
                        if (thisAcc.GetDataAcc().typeAttack == SO_Accessories.ETypeAttack.All)
                        {
                            //Check si la position des actor est sur la meme case que l'acc.
                            foreach (var thisActor in myTeam)
                            {
                                thisActor.SetCurrentStats(-2, TargetStats.ETypeStats.Calm);

                                //Vfx
                                thisActor.GetComponent<Animator>().SetTrigger("isHit");

                                thisActor.CheckIsOut();
                            }

                            //Ajoute dans la liste un texte.
                            listCurrentCataLogs.Add("La cata à frappé le lézard ! Tous le monde perd -2 de calm !");
                        }
                    }
                }
            }
        }

        //Check si un actor à touché un acc.
    }

    #region Logs Cata
    [Header("Text (Logs)")]
    List<string> listCurrentCataLogs;
    string currentCataLogs;
    int logsCataCursor;

    public void ResetCataLogs()
    {
        logsCataCursor = 0;
        listCurrentCataLogs = new List<string>();
    }

    string GetListCataLogs()
    {
        if (listCurrentCataLogs.Count == 0 && logsCataCursor == 0)
        {
            //Check si c'est pas null
            if (listCurrentCataLogs.Count != 0)
            {
                logsCataCursor++;

                currentCataLogs = listCurrentCataLogs[0];

                //Retourne le premier element de la liste.
                return currentCataLogs;
            }
            else
            {
                Debug.LogWarning("La liste est vide");
                return null;
            }
        }
        if (logsCataCursor > listCurrentCataLogs.Count - 1)
        {
            //Retourne une valeur null.
            return null;
        }
        else
        {
            currentCataLogs = listCurrentCataLogs[logsCataCursor];

            logsCataCursor++;

            //Retourne un element ciblé avant d'augmenter le curseur.
            return currentCataLogs;
        }
    }
    #endregion

    //Pour lancer le tour du joueur.
    public void NextCata()
    {
        //Check si l'element de la liste n'est pas null. Si ce dernier est null il passera à la reso suivante.
        if (!string.IsNullOrEmpty(GetListCataLogs()))
        {
            //SFX
            if (AudioManager.instanceAM)
            {
                AudioManager.instanceAM.Play(sfxWriteText);
            }

            uiLogs.GetComponentInChildren<TMP_Text>().text = currentCataLogs;
        }
        else //Passe à la phase suivante.
        {
            Debug.Log("Fin de la phase de cata !");

            myPhaseDeJeu = PhaseDeJeu.PlayerTrun;
            vfxPlayerTurn.SetTrigger("PlayerTurn");

            //Efface les logs.
            uiLogs.GetComponentInChildren<TMP_Text>().text = "";
        }
    }

    //Pour lancer la cata. BESOIN D'AJOUTER UN SYSTEM POUR AFFICHER LEE TEXTE DE LA CAT + DU LEZARD.
    public void CataTurn()
    {
        Debug.Log("CataTurn");

        //Redonne les couleurs au actor.
        foreach (var thisActor in myTeam)
        {
            thisActor.SetSpriteChallenge();
        }

        //Applique la catastrophe.
        ApplyCatastrophy();

        //Re-Check si tous les perso sont "out".
        if (!CheckGameOver())
        {
            //Redéfini le début de la liste.
            currentActor = myTeam[0];

            //Update la prochaine Cata.
            //Check si c'étais la dernière Cata.
            if (myChallenge.listCatastrophy.IndexOf(currentCata) + 1 > myChallenge.listCatastrophy.Count - 1)
            {
                currentCata = myChallenge.listCatastrophy[0];
            }
            else
            {
                currentCata = myChallenge.listCatastrophy[myChallenge.listCatastrophy.IndexOf(currentCata) + 1];
            }
        }
    }
    #endregion

    #region Fin de partie

    void UpdateEtape()
    {
        Debug.Log("next step");

        if (myChallenge.listEtape.IndexOf(currentStep) + 1 > myChallenge.listEtape.Count - 1)
        {
            currentStep = null;
        }
        else
        {
            //Nouvelle étape.
            currentStep = myChallenge.listEtape[myChallenge.listEtape.IndexOf(currentStep) + 1];
        }

        canMakeTuto = true;
    }

    #region GameOver
    //Bool pour check si le challenge est fini.

    bool CheckGameOver()
    {
        int nbActorOut = 0;

        foreach (var thisActor in myTeam)
        {
            if (thisActor.GetIsOut()) { nbActorOut++; }
        }

        if (nbActorOut == myTeam.Count)
        {
            GameOver();            return true;
        }

        return false;
    }

    [ContextMenu("GameOver")]
    public void GameOver()
    {
        Debug.Log("GameOver");

        myPhaseDeJeu = PhaseDeJeu.GameOver;

        uiGameOver.SetActive(true);
        uiGameOverImage.sprite = myChallenge.ecranDefaite;

        if (canGoNext)
        {
            if (GameManager.instance)
            {
                foreach (C_Actor thisActor in myTeam)
                {
                    thisActor.GetComponent<Animator>().SetBool("isInDanger", false);
                    thisActor.transform.parent = GameManager.instance.transform;
                    thisActor.GetImageActor().enabled = false;
                }
            }

            #region Transition
            //Lance l'animation de transition.
            GameManager.instance.ClosseTransitionFlannel();

            //Setup dans quelle scene on souhaite aller.
            GameManager.instance.TS_flanel.GetComponent<C_TransitionManager>().SetupNextScene("S_Challenge", myChallenge.LevelChallenge);
            #endregion

        }
        else
        {
            canGoNext = true;
        }
    }
    #endregion

    //Fin du challenge.
    public void EndChallenge()
    {
        if (myPhaseDeJeu != PhaseDeJeu.EndGame)
        {
            myPhaseDeJeu = PhaseDeJeu.EndGame;
        }

        //Redonne leur couleur.
        foreach (C_Actor thisActor in myTeam)
        {
            thisActor.SetSpriteChallenge();
            thisActor.GetComponent<Animator>().SetBool("isInDanger", false);

        }
        foreach (GameObject thisActor in GameManager.instance.GetTeam())
        {
            foreach (InitialActorPosition position in myChallenge.GetInitialPlayersPosition())
            {
                //Check si dans les info du challenge est dans l'équipe stocké dans le GameManager.
                if (thisActor.GetComponent<C_Actor>().GetDataActor().name == position.perso.GetComponent<C_Actor>().GetDataActor().name)
                {
                    //Placement sur le plateau.
                    PlacePionOnBoard(thisActor.GetComponent<C_Actor>(), position.position, false);

                }
                //Check si il y a un outro de challenge.
                if (myChallenge.outroChallenge && GameManager.instance)
                {
                    Debug.Log("Dialogue outro");
                    //Cache l'ui du probleme résolue.
                    uiVictoire.SetActive(false);

                    //Entre en mode dialogue.
                    GameManager.instance.EnterDialogueMode(myChallenge.outroChallenge);
                    ShowUiChallenge(false);
                    onDialogue = true;
                }
                else
                {
                    Debug.Log("Pas d'outro de challenge");
                    FinishChallenge(null);
                }

                Debug.Log("Fin du challenge");
            }
        }
    }

    public void FinishChallenge(string name)
    {
        StartCoroutine(FinishChallenge());
    }

    IEnumerator FinishChallenge()
    {
        if (AudioManager.instanceAM)
        {
            AudioManager.instanceAM.Play("ProblemeRésolu");
        }
        
        Debug.Log("Go au niveau suivant !");
        onDialogue = false;

        if (GameManager.instance)
        {
            GameManager.instance.ExitDialogueMode();

            foreach (C_Actor thisActor in myTeam)
            {
                thisActor.transform.parent = GameManager.instance.transform;
                thisActor.GetImageActor().enabled = false;
                //Desactivation du contour blanc.
                thisActor.IsSelected(false);
            }

            yield return new WaitForEndOfFrame();

            if (plateauPreview.Count - 1 != 0)
            {
                foreach (Image ThisActorPreview in plateauPreview)
                {
                    Destroy(ThisActorPreview.gameObject);
                }

                plateauPreview.Clear();
            }

            GameManager.instance.WorldstartPoint = myChallenge.mapPointID;

            #region Transition
            //Lance l'animation de transition.
            GameManager.instance.ClosseTransitionFlannel();

            string nextScene = null;

            if (GameManager.instance.currentC.name == "SO_lvl3")
            {
                nextScene = sceneCredits;
            }
            if (GameManager.instance.currentC.name == "SO_Tuto")
            {
                GameManager.instance.SetDataLevel(tm1, c1);

                nextScene = sceneTM;
            }
            else
            {
                nextScene = sceneWM;
            }

            if (nextScene != null)
            {
                Debug.Log("Transition vers la scene " + nextScene);

                //Setup dans quelle scene on souhaite aller.
                GameManager.instance.TS_flanel.GetComponent<C_TransitionManager>().SetupNextScene(nextScene, myChallenge.LevelChallenge);

                //Transition.
                GameManager.instance.TS_flanel.GetComponent<Animator>().SetTrigger("Close");
            }
            else
            {
                Debug.LogError("Il manque des info dans les transition de scene !!!");
            }
            #endregion
        }
    }
    #endregion

    #region Data Interface
    public ActorResolution GetActorResolution()
    {
        return currentResolution;
    }

    public C_Actor GetCurrentActor()
    {
        return currentActor;
    }

    public List<SO_ActionClass> GetListActionOfCurrentStep()
    {
        return currentStep.actions;
    }

    public SO_Etape GetCurrentEtape()
    {
        return currentStep;
    }

    public PhaseDeJeu GetPhaseDeJeu()
    {
        return myPhaseDeJeu;
    }

    public List<C_Case> GetListCases() { return plateau; }

    public EventSystem GetEventSystem() { return eventSystem; }

    public bool GetOnDialogue()
    {
        return onDialogue;
    }

    #region Phase de résolution
    public List<ActorResolution> GetListResolutions()
    {
        return listRes;
    }

    public ActorResolution GetCurrentResolution()
    {
        return currentResolution;
    }

    public void SetCurrentResolution(ActorResolution newCurrentResolution)
    {
        currentResolution = newCurrentResolution;
    }
    #endregion
    #endregion

    #region Tuto
    public void LaunchTuto()
    {
        myInterface.SetCurrentInterface(C_Interface.Interface.Tuto);

        //Ferme tout.
        myInterface.GoBack();
    }

    public void EndTuto()
    {
        myInterface.SetCurrentInterface(C_Interface.Interface.Neutre);
    }

    public void NextTuto()
    {
        //Lance l'animation d'après.
        //GetComponentInChildren<C_Tuto>().NextTuto(myChallenge.listEtape.IndexOf(currentStep) + 1);

        //Check si c'est le premier niveau.
        if (myChallenge.name == "SO_Tuto(Clone)")
        {
            //ETAPE 1.
            if (currentStep.name == "SO_step1tuto(Clone)" || currentStep.name == "SO_step2tuto(Clone)" || currentStep.name == "SO_step3tuto(Clone)")
            {
                //Lance l'animation.
                GetComponentInChildren<C_Tuto>().NextTuto(myChallenge.listEtape.IndexOf(currentStep) + 1);
            }
        }
        else if (myChallenge.name == "SO_lvl2A(Clone)")
        {
            //Lance l'animation.
            GetComponentInChildren<C_Tuto>().NextTuto(4);
        }
    }

    public C_Interface GetInterface()
    {
        return myInterface;
    }

    public List<C_Actor> GetTeam()
    {
        return myTeam;
    }
    #endregion

    #region Preview
    public GameObject GetTextPreviewPrefab()
    {
        return uiLogsTextPreviewPrefab;
    }
    public Transform GetTransformPreview()
    {
        return uiLogsPreview;
    }
    #endregion

    public Animator GetuiLogs()
    {
        return uiLogsAnimator;
    }

    public void SetOnDialogue(bool value)
    {
        onDialogue = value;
    }
    #endregion
}
