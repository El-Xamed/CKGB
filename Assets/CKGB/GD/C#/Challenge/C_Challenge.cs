using Febucci.UI;
using Ink;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
using static SO_Challenge;

public class C_Challenge : MonoBehaviour
{
    #region Mes variables
    [Header("Interface")]
    bool onDialogue = false;

    [Header("Interface")]
    [SerializeField] C_Interface myInterface;

    [Header("Data Challenge")]
    [SerializeField] SO_Challenge myChallenge;

    //Pour connaitre la phasse de jeu.
    public enum PhaseDeJeu { PlayerTrun, ResoTurn, CataTurn, EndGame }
    [Header("Phase de jeu")]
    [SerializeField] PhaseDeJeu myPhaseDeJeu = PhaseDeJeu.PlayerTrun;

    [SerializeField] GameObject plateauGameObject;

    [Header("Logs")]
    [SerializeField] Animator uiLogsAnimator;
    [SerializeField] GameObject uiLogs;
    [SerializeField] GameObject uiLogsTimeline;

    List<C_Actor> myTeam = new List<C_Actor>();
    List<C_Accessories> listAcc = new List<C_Accessories>();

    C_Actor currentActor;
    SO_Etape currentStep;
    SO_Catastrophy currentCata;

    bool canIniCata = false;

    [Tooltip("Case")]
    [SerializeField] C_Case myCase;
    List<C_Case> plateau = new List<C_Case>();
    List<Image> plateauPreview = new List<Image>();

    [SerializeField] EventSystem eventSystem;

    #region De base
    bool canGoNext = false;

    #region UI
    [Header("UI")]
    [SerializeField] GameObject background;
    [SerializeField] C_Stats uiStatsPrefab;
    [SerializeField] GameObject uiStats;
    [SerializeField] GameObject uiEtape;
    [SerializeField] GameObject uiGoodAction;
    [SerializeField] GameObject uiVictoire;
    [SerializeField] GameObject uiGameOver;

    #endregion

    #endregion

    #region Résolution
    [Header("Resolution")]
    bool twoActor = false;
    List<ActorResolution> listRes = new List<ActorResolution>();
    ActorResolution currentResolution;

    
    #endregion

    [SerializeField] SO_TempsMort tm1;
    [SerializeField] SO_Challenge c1;

    //VFX
    #region Vfx
    [Header("UI (VFX)")]
    [SerializeField] Animator vfxStartChallenge;
    [SerializeField] Animator vfxPlayerTurn;
    [SerializeField] Animator vfxResoTurn;
    bool animFinish;
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

    //Affiche les information de ce bouton dans la preview.
    public void Naviguate(InputAction.CallbackContext context)
    {
        if (!context.performed) { return; }

        if (context.performed /*&& context.ReadValue<Vector2>().x > 0.5f || context.performed && context.ReadValue<Vector2>().y > 0.5f*/)
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
            //Pour lier la transition au challenge.
            GameManager.instance.OpenTransitionFlannel();

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
        }
    }

    IEnumerator Start()
    {
        //Set le background
        background.GetComponent<Image>().sprite = myChallenge.background;

        //Desactive par default les logs timeleine.
        uiLogsTimeline.SetActive(false);
        uiVictoire.SetActive(false);

        //Apparition des cases
        SpawnCases();

        yield return new WaitForEndOfFrame();

        //Set les element en plus.
        SpawnElement();

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
    }
    public void SetCanContinueToNo()
    {
        myInterface.canContinue = false;
    }
    #endregion

    #region Début de partie
    void SpawnElement()
    {
        //Set les element en plus.
        if (myChallenge.element.Count != 0)
        {
            foreach (var thisElement in myChallenge.element)
            {

                GameObject newUI = new GameObject();

                newUI.transform.parent = GameObject.Find("Element").transform;

                newUI.name = thisElement.name;

                newUI.AddComponent<Image>();

                newUI.GetComponent<Image>().sprite = thisElement;

                newUI.GetComponent<Image>().material = Resources.Load<Material>("MatImageLit");

                newUI.GetComponent<RectTransform>().sizeDelta = new Vector2(1920, 1080);

                newUI.GetComponent<RectTransform>().position = GameObject.Find("Element").transform.position;

                newUI.GetComponent<RectTransform>().localScale = Vector3.one;
            }
        }
    }

    void SpawnCases()
    {
        plateauGameObject.GetComponent<HorizontalLayoutGroup>().spacing = myChallenge.spaceCase;

        //Spawn toutes les cases.
        for (int i = 0; i < myChallenge.nbCase; i++)
        {
            //Création d'une case
            C_Case newCase = Instantiate(myCase, plateauGameObject.transform);

            newCase.AddNumber(i + 1);

            Debug.Log("ICI : " + newCase.GetComponent<RectTransform>().rect.position.ToString());

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
                            //Ini data actor.
                            thisActor.GetComponent<C_Actor>().IniChallenge();

                            thisActor.transform.parent = GameObject.Find("BackGround").transform;

                            //Placement des perso depuis le GameManager
                            //Changement de parent
                            PlacePionOnBoard(thisActor.GetComponent<C_Actor>(), position.position, false);
                            thisActor.GetComponent<C_Actor>().CheckInDanger();
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
                                GameObject newVfxGoodAction = Instantiate(thisActor.GetComponent<C_Actor>().GetDataActor().vfxUiGoodAction.gameObject, uiGoodAction.transform);
                                //thisActor.GetComponent<C_Actor>().GetDataActor().vfxUiGoodAction = newVfxGoodAction.GetComponent<Animator>();
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

                //Pour attacher les fonction à tous les actor de ce challenge pour les dialogues.
                foreach (var item in myTeam)
                {
                    item.GetComponent<C_Actor>().txtHautGauche.GetComponent<TextAnimatorPlayer>().onTypewriterStart.AddListener(() => SetCanContinueToNo());
                    item.GetComponent<C_Actor>().txtHautDroite.GetComponent<TextAnimatorPlayer>().onTypewriterStart.AddListener(() => SetCanContinueToNo());
                    item.GetComponent<C_Actor>().txtBasGauche.GetComponent<TextAnimatorPlayer>().onTypewriterStart.AddListener(() => SetCanContinueToNo());
                    item.GetComponent<C_Actor>().txtBasDroite.GetComponent<TextAnimatorPlayer>().onTypewriterStart.AddListener(() => SetCanContinueToNo());

                    item.GetComponent<C_Actor>().txtHautGauche.GetComponent<TextAnimatorPlayer>().onTextShowed.AddListener(() => SetCanContinueToYes());
                    item.GetComponent<C_Actor>().txtHautDroite.GetComponent<TextAnimatorPlayer>().onTextShowed.AddListener(() => SetCanContinueToYes());
                    item.GetComponent<C_Actor>().txtBasGauche.GetComponent<TextAnimatorPlayer>().onTextShowed.AddListener(() => SetCanContinueToYes());
                    item.GetComponent<C_Actor>().txtBasDroite.GetComponent<TextAnimatorPlayer>().onTextShowed.AddListener(() => SetCanContinueToYes());
                }

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

        //Desactive les input.
        GetComponent<PlayerInput>().enabled = false;

        //Lance l'animation qui présente le challenge après la transition
        vfxStartChallenge.SetTrigger("start");

        if (GameManager.instance)
        {
            GameManager.instance.ExitDialogueMode();
        }

        //Re-active le fond des logs.

        #region Initialisation

        if (AudioManager.instance)
        {
            //AudioManager.instance.Play("MusiqueTuto");
        }

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

        //Change l'état du challenge.
        myPhaseDeJeu = PhaseDeJeu.PlayerTrun;
    }
    #endregion

    #region Tour du joueur
    //Fonction pour faire spawn les cata.
    void InitialiseCata()
    {
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
                eventSystem.currentSelectedGameObject.GetComponent<C_ActionButton>().ShowCurseur();
            }
            #endregion

            if (eventSystem.currentSelectedGameObject.GetComponent<C_ActionButton>())
            {
                //Affiche la preview.
                GetComponent<C_PreviewAction>().ShowPreview(eventSystem.currentSelectedGameObject.GetComponent<C_ActionButton>().GetActionClass(), currentActor);
            }
        }
    }

    //Fonction qui est stocké dans les button action donné par l'interface + permet de passer à l'acteur suivant ou alors de lancer la phase de résolution.
    public void ConfirmAction(SO_ActionClass thisAction)
    {
        //FeedBack
        currentActor.PlayAnimSelectAction();

        //Desactive la preview de l'actor.
        currentActor.GetUiStats().ResetUiPreview();

        if (AudioManager.instance)
        {
            AudioManager.instance.Play("SfxSonDeConfirmation");
        }

        //Création d'une nouvelle class pour ensuite ajouter dans la liste qui va etre utilisé dans la phase de résolution.
        ActorResolution actorResolution = new ActorResolution();

        //Renseigne l'actor actuel + l'action.
        actorResolution.actor = currentActor;
        actorResolution.action = thisAction;

        //Ajoute à la liste.
        listRes.Add(actorResolution);

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

            //Cache les boutons + ferme l'interface. CHANGER ÇA POUR AVOIR L'INTERFACE SANS LES TEXT A COTE.
            //Animation.
            myInterface.ResetTargetButton();
            myInterface.GetComponent<Animator>().SetTrigger("Close");
            myInterface.GetUiAction().SetActive(false);

            //Rend les couleurs sur tous les actor.
            foreach (C_Actor thisActor in myTeam)
            {
                thisActor.GetImageActor().sprite = thisActor.GetDataActor().challengeSprite;
            }

            animFinish = false;

            myPhaseDeJeu = PhaseDeJeu.ResoTurn;
            vfxResoTurn.SetTrigger("PlayerTurn");

            //Supprime toutes les preview.
            foreach (Image thisPreview in plateauPreview)
            {
                Destroy(thisPreview.gameObject);
            }
        }
    }

    public void PlayerTurn()
    {
        //Efface les logs.
        uiLogs.GetComponentInChildren<TMP_Text>().text = "";

        Debug.Log("Player turn !");

        //Défini la phase de jeu.
        myPhaseDeJeu = PhaseDeJeu.PlayerTrun;

        //Ini l'interface.
        myInterface.GetComponent<Animator>().SetTrigger("CloseAll");

        UpdateUi();

        //Vide la listeReso
        listRes = new List<ActorResolution>();

        //Ajout d'un bool pour executer le dev en dessosu après l'animation.
        if (animFinish)
        {
            //Initialise la prochaine cata.
            if (currentStep.useCata)
            {
                //Check si il n'est pas null.
                if (currentCata == null)
                {
                    currentCata = myChallenge.listCatastrophy[0];
                }

                InitialiseCata();

                canIniCata = false;
            }

            //Check si le perso est jouable
            if (!currentActor.GetIsOut())
            {
                //Pour effacer le texte de la cata.
                uiLogs.GetComponentInChildren<TMP_Text>().text = "";

                //Update le contour blanc
                UpdateActorSelected();
            }
            else
            {
                NextActor();
                PlayerTurn();
            }
        }
        else
        {
            //VFX
            vfxPlayerTurn.GetComponent<Animator>().SetTrigger("PlayerTurn");
        }
    }

    #region Preview
    //Pour afficher toutes les preview du challenge.
    public void CheckPreview(SO_ActionClass thisActionClass, Interaction.ETypeTarget target)
    {
        foreach (Interaction thisInteraction in thisActionClass.listInteraction)
        {
            //Check si c'est égale à "actorTarget".
            if (thisInteraction.whatTarget == target)
            {
                foreach (TargetStats thisTargetStats in thisInteraction.listTargetStats)
                {
                    //Check si c'est des stats ou un Mouvement.
                    if (thisTargetStats.whatStatsTarget == TargetStats.ETypeStatsTarget.Stats)
                    {
                        //Inscrit la preview de texte + ui. Avec les info de preview. (C_Challenge)
                        //C_PreviewAction.onPreview += TextPreview;
                    }

                    if (thisTargetStats.whatStatsTarget == TargetStats.ETypeStatsTarget.Movement)
                    {
                        Debug.Log(thisActionClass.name + " : " + thisTargetStats.whatStatsTarget);
                        //Inscrit la preview de movement. (C_Challenge)
                        //C_PreviewAction.onPreview += MovementPreview;
                    }
                }
            }
        }

        C_PreviewAction.onPreview += TextPreview;
    }

    void TextPreview(SO_ActionClass thisActionClass)
    {
        //Récupère toutes les info directement ?
        //Non si on veut faire apparaitre les autres actor avec leur description.
        //Oui car le joueur a besoin de savoir toutes les conséquence de l'action, meme si ça ne touche pas les autres actor.

        //Créer la liste pour "self"
        //GetLogsPreviewTarget(Interaction.ETypeTarget.Self);

        //Créer la liste pour "other"
        if (thisActionClass.CheckOtherInAction())
        {
            //GetOtherLogsPreview(actionClass.GetRange());
        }

        /*void GetLogsPreviewTarget(Interaction.ETypeTarget target)
        {
            //Check si pour le "target" les variables ne sont pas égale à 0, si c'est le cas alors un system va modifier le text qui va s'afficher.
            #region Stats string
            //Pour les stats.
            if (thisActionClass.GetValue(target, TargetStats.ETypeStatsTarget.Stats) != 0)
            {
                //Si les deux possède un int supérieur à 0.
                if (actionClass.GetStats(target, TargetStats.ETypeStatsTarget.Price, Stats.ETypeStats.Energy) != 0 && actionClass.GetStats(target, TargetStats.ETypeStatsTarget.Price, Stats.ETypeStats.Calm) != 0)
                {
                    listLogsPreview.Add(thisActor.name + " va perdre " + GetColorText(actionClass.GetStats(target, TargetStats.ETypeStatsTarget.Price, Stats.ETypeStats.Calm).ToString(), Color.blue) + " de calme et " + GetColorText(actionClass.GetStats(target, TargetStats.ETypeStatsTarget.Price, Stats.ETypeStats.Energy).ToString(), Color.yellow) + " d'énergie.");
                }
                else if (actionClass.GetStats(target, TargetStats.ETypeStatsTarget.Price, Stats.ETypeStats.Calm) != 0)
                {
                    listLogsPreview.Add(thisActor.name + " va perdre " + GetColorText(actionClass.GetStats(target, TargetStats.ETypeStatsTarget.Price, Stats.ETypeStats.Calm).ToString(), Color.blue) + " de calme.");
                }
                else if (actionClass.GetStats(target, TargetStats.ETypeStatsTarget.Price, Stats.ETypeStats.Energy) != 0)
                {
                    listLogsPreview.Add(thisActor.name + " va perdre " + GetColorText(actionClass.GetStats(target, TargetStats.ETypeStatsTarget.Price, Stats.ETypeStats.Energy).ToString(), Color.yellow) + " d'énergie.");
                }
            }
            #endregion

            #region Movement
            //Pour le mouvement.
            if (actionClass.GetMovement(target) != 0)
            {
                //Pour toutes les liste d'action.
                foreach (Interaction thisInteraction in actionClass.listInteraction)
                {
                    //Check si sont enum est égale à "target".
                    if (thisInteraction.whatTarget == target)
                    {
                        //Pour toutes les list de stats.
                        foreach (TargetStats thisStats in thisInteraction.listTargetStats)
                        {
                            //Check si sont enum est égale à "Movement".
                            if (thisStats.whatStatsTarget == TargetStats.ETypeStatsTarget.Movement)
                            {
                                MoveActor(thisStats.move);
                                Debug.Log(target + " : " + thisStats.move.whatMove);
                            }
                        }
                    }
                }

                void MoveActor(Move myMove)
                {
                    switch (myMove.whatMove)
                    {
                        //Si "otherActor" est dans la range alors lui aussi on lui affiche les preview mais avec les info pour "other".
                        case Move.ETypeMove.Right:
                            NormalMoveActor(myMove.nbMove);
                            break;
                        case Move.ETypeMove.Left:
                            NormalMoveActor(-myMove.nbMove);
                            break;
                        case Move.ETypeMove.OnTargetCase:
                            TargetMoveActor(myMove.nbMove);
                            break;
                    }

                    void NormalMoveActor(int newPosition)
                    {
                        int notBusyByActor = 0;

                        #region Detection de tous les autres membre de l'équipe.
                        foreach (C_Actor thisOtherActor in otherActor)
                        {
                            //Détection de si il y a un autres actor.
                            if (thisActor.GetPosition() + newPosition == thisOtherActor.GetPosition())
                            {
                                //Check si c'est une TP (donc un swtich) ou un déplacement normal (pousse le personnage).
                                if (myMove.isTp)
                                {
                                    listLogsPreview.Add(GetColorText(thisActor.name, Color.cyan) + " va échanger de place avec " + GetColorText(thisOtherActor.name, Color.green) + ".");
                                    Debug.Log(GetColorText(thisActor.name, Color.cyan) + " va échanger de place avec " + GetColorText(thisOtherActor.name, Color.green) + ".");
                                }
                                else
                                {
                                    listLogsPreview.Add(GetColorText(thisActor.name, Color.cyan) + " va prendre la place de " + GetColorText(thisOtherActor.name, Color.cyan) + " et sera déplacer " + GetDirectionOfMovement());
                                    Debug.Log(GetColorText(thisActor.name, Color.cyan) + " va prendre la place de " + GetColorText(thisOtherActor.name, Color.green) + " et sera déplacer " + GetDirectionOfMovement());
                                }
                            }
                            else
                            {
                                notBusyByActor++;

                                if (notBusyByActor == otherActor.Count)
                                {
                                    listLogsPreview.Add(GetColorText(thisActor.name, Color.cyan) + " va se déplacer de " + GetColorText(myMove.nbMove.ToString(), Color.green) + GetDirectionOfMovement());
                                    Debug.Log(GetColorText(thisActor.name, Color.cyan) + " va se déplacer de " + GetColorText(myMove.nbMove.ToString(), Color.green) + GetDirectionOfMovement());
                                }
                            }
                        }
                        #endregion

                        string GetDirectionOfMovement()
                        {
                            if (newPosition < 0)
                            {
                                return " à gauche.";
                            }
                            else if (newPosition > 0)
                            {
                                return " à droite.";
                            }

                            return "Direction Inconu.";
                        }
                    }

                    void TargetMoveActor(int newPosition)
                    {
                        foreach (C_Actor thisOtherActor in otherActor)
                        {
                            if (newPosition == thisOtherActor.GetPosition())
                            {
                                listLogsPreview.Add(GetColorText(thisActor.name, Color.cyan) + " va échanger de place avec " + GetColorText(thisOtherActor.name, Color.green) + ".");
                                Debug.Log(GetColorText(thisActor.name, Color.cyan) + " va échanger de place avec " + GetColorText(thisOtherActor.name, Color.green) + ".");
                            }
                            else
                            {
                                listLogsPreview.Add(GetColorText(thisActor.name, Color.cyan) + " va se déplacer sur la case " + GetColorText(newPosition.ToString(), Color.green) + ".");
                                Debug.Log(GetColorText(thisActor.name, Color.cyan) + " va se déplacer sur la case " + GetColorText(newPosition.ToString(), Color.green) + ".");
                            }
                        }
                    }
                }
            }
            else
            {
                //Pour toutes les liste d'action.
                foreach (Interaction thisInteraction in actionClass.listInteraction)
                {
                    //Check si sont enum est égale à "Self".
                    if (thisInteraction.whatTarget == target)
                    {
                        //Pour toutes les list de stats.
                        foreach (TargetStats thisStats in thisInteraction.listTargetStats)
                        {
                            //Check si sont enum est égale à "Movement".
                            if (thisStats.whatStatsTarget == TargetStats.ETypeStatsTarget.Movement)
                            {
                                if (thisStats.move.whatMove == Move.ETypeMove.SwitchWithActor)
                                {
                                    if (thisStats.move.actor != null)
                                    {
                                        listLogsPreview.Add(GetColorText(thisActor.name, Color.cyan) + " va échanger sa place avec " + GetColorText(actionClass.GetSwitchActor(target).name, Color.cyan) + ".");
                                        Debug.Log(GetColorText(thisActor.name, Color.cyan) + " va échanger sa place avec " + GetColorText(actionClass.GetSwitchActor(target).name, Color.cyan) + ".");
                                    }
                                    else { Debug.LogWarning(thisStats.move.actor); }
                                }
                                else if (thisStats.move.whatMove == Move.ETypeMove.SwitchWithAcc)
                                {
                                    if (thisStats.move.accessories != null)
                                    {
                                        listLogsPreview.Add(GetColorText(thisActor.name, Color.cyan) + " va échanger sa place avec " + GetColorText(actionClass.GetSwitchAcc(target).name, Color.cyan) + ".");
                                        Debug.Log(GetColorText(thisActor.name, Color.cyan) + " va échanger sa place avec " + GetColorText(actionClass.GetSwitchAcc(target).name, Color.cyan) + ".");
                                    }
                                    else { Debug.LogWarning(thisStats.move.accessories); }
                                }
                            }
                        }
                    }
                }
            }
            #endregion
        }*/

        //Affiche une preview sur les autres actor.
        /*void GetOtherLogsPreview(List<C_Actor> otherActor, C_Actor thisActor, int range, List<C_Case> plateau)
        {
            //Check si c'est ciblé ou non.
            if (!actionClass.GetIfTargetOrNot())
            {
                //Boucle avec le range.
                for (int i = 1; i < range; i++)
                {
                    //Boucle pour check sur tout les actor du challenge.
                    foreach (C_Actor thisOtherActor in otherActor)
                    {
                        //Check quel direction la range va faire effet.
                        switch (actionClass.GetTypeDirectionRange())
                        {
                            //Si "otherActor" est dans la range alors lui aussi on lui affiche les preview mais avec les info pour "other".
                            case Interaction.ETypeDirectionTarget.Right:
                                //Calcul vers la droite.
                                if (actionClass.CheckPositionOther(thisActor, i, plateau, thisOtherActor))
                                {
                                    GetLogsPreviewTarget(Interaction.ETypeTarget.Other, otherActor, thisOtherActor);
                                }

                                Debug.Log("Direction Range = droite.");
                                break;
                            case Interaction.ETypeDirectionTarget.Left:
                                //Calcul vers la gauche.
                                if (actionClass.CheckPositionOther(thisActor, -i, plateau, thisOtherActor))
                                {
                                    GetLogsPreviewTarget(Interaction.ETypeTarget.Other, otherActor, thisOtherActor);
                                }
                                Debug.Log("Direction Range = Gauche.");
                                break;
                            case Interaction.ETypeDirectionTarget.RightAndLeft:
                                //Calcul vers la droite + gauche.
                                if (actionClass.CheckPositionOther(thisActor, i, plateau, thisOtherActor))
                                {
                                    GetLogsPreviewTarget(Interaction.ETypeTarget.Other, otherActor, thisOtherActor);
                                }
                                else if (actionClass.CheckPositionOther(thisActor, -i, plateau, thisOtherActor))
                                {
                                    GetLogsPreviewTarget(Interaction.ETypeTarget.Other, otherActor, thisOtherActor);
                                }
                                Debug.Log("Direction Range = droite + gauche.");
                                break;
                        }
                    }
                }
            }
            else if (actionClass.GetIfTargetOrNot())
            {
                if (actionClass.GetTarget().GetComponent<C_Actor>())
                {
                    GetLogsPreviewTarget(Interaction.ETypeTarget.Other, otherActor, actionClass.GetTarget().GetComponent<C_Actor>());
                }
                else if (actionClass.GetTarget().GetComponent<C_Accessories>())
                {
                    actionClass.SetTarget(GameObject.Find(actionClass.GetTarget().GetComponent<C_Accessories>().GetDataAcc().name));

                    GetLogsPreviewTarget(Interaction.ETypeTarget.Other, otherActor, actionClass.GetTarget().GetComponent<C_Accessories>());
                }
            }
        }*/
    }

    void MovementPreview(SO_ActionClass thisActionClass)
    {
        foreach (Image ThisActorPreview in plateauPreview)
        {
            Destroy(ThisActorPreview.gameObject);
        }

        plateauPreview.Clear();

        Image thisPreview = new GameObject().AddComponent<Image>();

        //Création du pion preview.
        thisPreview.transform.parent = currentActor.transform;
        //Scale
        thisPreview.gameObject.transform.localScale = Vector3.one;
        //Taille
        thisPreview.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(currentActor.GetComponent<RectTransform>().rect.width, currentActor.GetComponent<RectTransform>().rect.height);
        //Set l'image
        thisPreview.sprite = currentActor.GetDataActor().challengeSprite;
        //Change le nom
        thisPreview.name = currentActor.name + "_Preview";
        //Change la couleur. (Assombri l'image) MARCHE PAS A CAUSE DE L'ANIMATION QUI TOURNE EN BOUCLE.
        thisPreview.color = Color.HSVToRGB(0, 0, 0.35f);
        //Et ajouté dans la liste des preview de movement.
        plateauPreview.Add(thisPreview);

        //Création de sa position sur le plateau.
        int position = currentActor.GetPosition();

        TargetStats.ETypeMove whatMove = thisActionClass.GetWhatMove(Interaction.ETypeTarget.Self);
        int nbMove = thisActionClass.GetValue(Interaction.ETypeTarget.Self, TargetStats.ETypeStatsTarget.Movement);
        
        bool isTp = thisActionClass.GetIsTp(Interaction.ETypeTarget.Self);

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

                        //On retire le nombre de déplacement fait.
                        nbMove = position + nbMove - i;
                    }
                }
            }
            else if (nbMove < 0)
            {
                Debug.Log(nbMove);
                //Vers la gauche.
                for (int i = 0; i >= nbMove; i--)
                {
                    Debug.Log(i);
                    if (position + i < 0)
                    {
                        //Replace le pion sur la case sur la case la plus à droite.
                        thisPreview.transform.position = new Vector3(plateau[plateau.Count - 1].transform.position.x, 0, plateau[plateau.Count - 1].transform.position.z);
                        position = plateau.Count - 1;

                        //On retire le nombre de déplacement fait.
                        nbMove = position + nbMove - i;
                    }
                }
            }

            //Puis recheck si le calcul est bon.
            CheckIfNotExceed();
        }
    }
    #endregion

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
        string nextLogs = currentResolution.action.GetListLogs();
        if (!string.IsNullOrEmpty(nextLogs))
        {
            uiLogs.GetComponentInChildren<TMP_Text>().text = nextLogs;
        }
        else if (listRes.IndexOf(currentResolution) < listRes.Count - 1)
        {
            currentResolution = listRes[listRes.IndexOf(currentResolution) + 1];

            ResolutionTurn();
        }
        else
        {
            Debug.Log("Fin de la phase de réso !");

            //Check si une cata est présente.
            if (GetCurrentEtape().useCata && !canIniCata)
            {
                //Check si après la phase de réso, tous les perso sont vivant.
                if (!CheckGameOver())
                {
                    //Lance la phase "Cata".
                    animFinish = false;
                    CataTrun();
                }
            }
            else
            {
                //Redéfini le début de la liste.
                currentActor = myTeam[0];
                //Ouvre l'interface.
                OpenInterface();
                animFinish = false;
                PlayerTurn();
            }
        }
    }

    public void ResolutionTurn()
    {
        Debug.Log("Resolution turn !");

        if (AudioManager.instance)
        {
            AudioManager.instance.Play("Sfxlogs");
        }

        eventSystem.SetSelectedGameObject(null);

        //Défini la phase de jeu.
        myPhaseDeJeu = PhaseDeJeu.ResoTurn;

        //Passe l'interface en neutre.
        myInterface.SetCurrentInterface(C_Interface.Interface.Neutre);

        //Met en noir et blanc tous les actor.
        foreach (C_Actor thisActor in myTeam)
        {
            if (thisActor != currentResolution.actor)
            {
                thisActor.SetSpriteChallengeBlackAndWhite();
            }
        }

        currentResolution.actor.SetSpriteChallenge();

        //Ajout d'un bool pour executer le dev en dessosu après l'animation.
        if (animFinish)
        {
            //Reset le bool pour detecter si il y a une action à 2.
            twoActor = false;

            //Applique toutes les actions. 1 par 1.
            UseAction(currentResolution);

            #region Check si c'est la bonne action
            //Check si c'est la bonne action.
            if (currentResolution.action.name == currentStep.rightAnswer.name)
            {
                Debug.Log("Bonne action");

                //Vfx de bonna action.
                GameObject.Find(currentResolution.actor.GetDataActor().vfxUiGoodAction.name + "(Clone)").GetComponent<Animator>().SetTrigger("GoodAction");
                //currentResolution.actor.GetDataActor().vfxUiGoodAction.SetTrigger("GoodAction");

                //bool pour empecher à la cata de l'etape d'apres de ce déclencher.
                canIniCata = true;

                //Check si c'est la fin.
                UpdateEtape();
            }
            else
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

            #region Logs
            //Ecrit dans les logs le résultat de l'action.
            currentResolution.action.ResetLogs();
            uiLogs.GetComponentInChildren<TMP_Text>().text = currentResolution.action.GetListLogs();
            #endregion
        }
        else
        {
            //VFX
            vfxResoTurn.GetComponent<Animator>().SetTrigger("PlayerTurn");
        }
    }

    //Utilise l'action.
    bool UseAction(ActorResolution thisActorResolution)
    {
        Debug.Log("Use this actionClass : " + thisActorResolution.action.buttonText);

        #region Raccourcis
        //Raccourcis.
        SO_ActionClass action = thisActorResolution.action;
        C_Actor actor = thisActorResolution.actor;
        #endregion

        //Check dans les data de cette action si la condition est bonne.
        if (CanUse(thisActorResolution))
        {
            Debug.Log("Peut faire l'action");

            //Applique les conséquences de stats.
            #region Self
            //Créer la liste pour "self"
            action.SetStatsTarget(Interaction.ETypeTarget.Self, actor);

            //Check si un mouvement pour "self" existe.
            CheckIfTargetMove(Interaction.ETypeTarget.Self, actor);
            #endregion

            #region Other
            //Créer la liste pour "other"
            if (action.CheckOtherInAction())
            {
                //Boucle avec la range.
                for (int i = 0; i < action.GetRange(); i++)
                {
                    Debug.Log("Je cherche sur la case " + i);

                    if (action.GetTypeDirectionRange() != Interaction.ETypeDirectionTarget.None)
                    {
                        //Boucle pour check sur tout les actor du challenge.
                        foreach (C_Actor thisOtherActor in myTeam)
                        {
                            //Check quel direction la range va faire effet.
                            switch (action.GetTypeDirectionRange())
                            {
                                //Si "otherActor" est dans la range alors lui aussi on lui affiche les preview mais avec les info pour "other".
                                case Interaction.ETypeDirectionTarget.Right:
                                    //Calcul vers la droite.
                                    CheckPositionOther(actor, i, thisOtherActor);
                                    Debug.Log("Direction Range = droite.");
                                    break;
                                case Interaction.ETypeDirectionTarget.Left:
                                    //Calcul vers la gauche.
                                    CheckPositionOther(actor, -i, thisOtherActor);
                                    Debug.Log("Direction Range = Gauche.");
                                    break;
                                case Interaction.ETypeDirectionTarget.RightAndLeft:
                                    //Calcul vers la droite + gauche.
                                    CheckPositionOther(actor, i, thisOtherActor);
                                    CheckPositionOther(actor, -i, thisOtherActor);
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

                            action.SetStatsTarget(Interaction.ETypeTarget.Other, target);

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

                            action.SetStatsTarget(Interaction.ETypeTarget.Other, target);

                            //Check si un mouvement pour "other" existe.
                            CheckIfTargetMove(Interaction.ETypeTarget.Other, target);
                            return true;
                        }
                    }
                    else if (thisActor.GetPosition() + position == target.GetPosition() && target != thisActor)
                    {
                        Debug.Log(target.name + " à été trouvé ! à la position: " + target.GetPosition());

                        action.SetStatsTarget(Interaction.ETypeTarget.Other, target);

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
                if (action.GetWhatMove(target) != TargetStats.ETypeMove.None)
                {
                    //Regarde d'abord c'est quoi comme type de déplacement.
                    if (!action.GetIfTargetOrNot()) //Non ciblé par un actor ou acc.
                    {
                        Debug.Log("Pas ciblé par un actor ou acc.");

                        //Check si un mouvement existe.
                        if (action.GetValue(target, TargetStats.ETypeStatsTarget.Movement) != 0)
                        {
                            //Deplace l'actor avec l'info de déplacement + type de déplacement.
                            MoveActorInBoard(thisActor, action.GetValue(target, TargetStats.ETypeStatsTarget.Movement), action.GetWhatMove(target), action.GetIsTp(target));
                        }
                    }
                    else //Ciblé par un actor ou acc.
                    {
                        Debug.Log("Ciblé par un actor ou acc.");

                        //VOIR SI BESOIN DE SETUP ICI OU DANS L'ACTION DURECTEMENT POUR LES INFO DES ACC OU ACTOR POUR SETUP LES LIENS AVEC LES OBJ DU CHALLANGE.
                        if (action.GetTarget().GetComponent<C_Actor>())
                        {
                            action.SetStatsTarget(target, action.GetTarget().GetComponent<C_Actor>());
                        }
                        else if (action.GetTarget().GetComponent<C_Accessories>())
                        {
                            action.SetTarget(GameObject.Find(action.GetTarget().GetComponent<C_Accessories>().GetDataAcc().name));

                            action.SetStatsTarget(target, action.GetTarget().GetComponent<C_Accessories>());
                        }
                    }
                }
            }

            return true;
        }
        else
        {
            return false;
        }
    }

    //vérifie la condition si l'action fonctionne. A DEPLACER DANS LE CHALLENGE POUR APPLIQUER LES CONEQUENCE DE L'ACTION SUR LE DEUXIEME ACTOR.
    bool CanUse(ActorResolution thisReso)
    {
        AdvancedCondition advancedCondition = thisReso.action.advancedCondition;

        //Check si l'actor en question possède assez d'energie.
        if (thisReso.actor.GetcurrentEnergy() >= thisReso.action.GetValue(Interaction.ETypeTarget.Self, TargetStats.ETypeStatsTarget.Stats))
        {
            //Check si les codition bonus sont activé.
            if (advancedCondition.advancedCondition)
            {
                //Check si l'action doit etre fait par un actor en particulier + Si "whatActor" n'est pas null + si "whatActor" est égal à "thisActor".
                if (advancedCondition.canMakeByOneActor && advancedCondition.whatActor && advancedCondition.whatActor == thisReso.actor)
                {
                    return true;
                }

                //Check si l'action doit etre fait par un acc en particulier + Si "whatAcc" n'est pas null + si "whatAcc" est égal à "thisActor".
                if (advancedCondition.needAcc && advancedCondition.needAcc && advancedCondition.whatAcc.GetPosition() == thisReso.actor.GetPosition())
                {
                    return true;
                }

                //Check si la condition de faire l'action à 2 est activé.
                if (advancedCondition.needTwoActor && twoActor != true)
                {
                    //Chec ksi dans toute les réso un autre actor à fait aussi la meme action.
                    foreach (var thisResoInList in listRes)
                    {
                        //Pour éviter qu'il se compte lui meme.
                        if (thisResoInList != thisReso)
                        {
                            //Check si dans la reso en cours et égale à une autre reso qui possède la meme action.
                            if (thisResoInList.action == thisReso.action)
                            {
                                //Active la detection de l'action à 2.
                                twoActor = true;

                                //Check si l'autre actor peut aussi faire l'action.
                                if (UseAction(thisResoInList))
                                {
                                    if (twoActor)
                                    {
                                        //Desactive l'autre actor qui à fait la meme action pour éviter que l'action se joue 2 fois. A VOIR OU PLACER SE BOUT DE CODE.
                                        //TESTER EN SUPPRIMANT L'AUTRE ACTOR QUI A FAIT LA MEME ACTION.
                                        listRes.Remove(thisResoInList);
                                    }

                                    return true;
                                }
                            }
                        }
                    }
                }
            }

            return true;
        }

        return false;
    }

    #region Deplace les actor
    //Fonction qui déplace les actor.
    public void MoveActorInBoard(C_Pion thisPion, int nbMove, TargetStats.ETypeMove whatMove, bool isTp)
    {
        //Setup la position de départ.
        thisPion.GetComponent<C_Actor>().SetStartPosition(plateau[thisPion.GetPosition()].GetComponentInParent<Transform>());

        Debug.Log(whatMove);
        Debug.Log(nbMove);

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
            //Check si le nombre de déplacement est trop élevé par rapport au nombre de case.
            if (nbMove > plateau.Count - 1)
            {
                Debug.LogWarning("La valeur de déplacement et trop élevé par rapport au nombre de cases sur le plateau la valeur sera donc égale à 0.");

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
                        nbMove = thisPion.GetPosition() + nbMove - i;
                        Debug.Log(nbMove);
                    }
                }
            }
            else if (nbMove < 0)
            {
                Debug.Log(nbMove);
                //Vers la gauche.
                for (int i = 0; i >= nbMove; i--)
                {
                    Debug.Log(i);
                    if (thisPion.GetPosition() + i < 0)
                    {
                        //Replace le pion sur la case sur la case la plus à droite.
                        PlacePionOnBoard(thisPion, plateau.Count - 1, isTp);

                        //On retire le nombre de déplacement fait.
                        Debug.Log(nbMove);
                        Debug.Log(i);
                        Debug.Log(nbMove - i);
                        nbMove = thisPion.GetPosition() + nbMove - i;
                        Debug.Log(nbMove);
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

        }
        #endregion
    }
    #endregion

    #endregion

    #region Tour de la Cata
    //Fonction pour appliquer la cata.
    public void ApplyCatastrophy()
    {
        //Pour tous les nombre dans la liste dela cata.
        foreach (var thisCase in currentCata.targetCase)
        {
            //Check si la case possède un vfx.
            if (plateau[thisCase].GetVfxCata() != null)
            {
                //VFX de la cata qui s'applique.
                plateau[thisCase].GetComponentInChildren<Animator>().SetTrigger("cata_Kaboom");
            }

            if (currentCata.applyForAll)
            {
                //Pour tous les actor.
                foreach (C_Actor thisActor in myTeam)
                {
                    //Applique des conséquence grace au finction de actionClass.
                    currentCata.actionClass.SetStatsTarget(Interaction.ETypeTarget.Self, thisActor);

                    thisActor.CheckIsOut();
                }
            }
            else
            {
                //Pour tous les actor.
                foreach (C_Actor thisActor in myTeam)
                {
                    if (thisCase == thisActor.GetPosition())
                    {
                        Debug.Log("La case " + thisCase + " est attaqué !");

                        //Applique des conséquence grace au finction de actionClass.
                        currentCata.actionClass.SetStatsTarget(Interaction.ETypeTarget.Self, thisActor);

                        thisActor.CheckIsOut();
                    }
                }
            }
        }
    }

    //Pour lancer la cata.
    public void CataTrun()
    {
        Debug.Log("CataTurn");

        //Défini la phase de jeu.
        myPhaseDeJeu = PhaseDeJeu.CataTurn;

        //Ecrit dans les logs le résultat de l'action.
        uiLogs.GetComponentInChildren<TMP_Text>().text = currentCata.catastrophyLog;

        //Applique la catastrophe.
        ApplyCatastrophy();

        //Re-Check si tous les perso sont "out".
        if (!CheckGameOver())
        {
            //Update les acc
            //UpdateAccessories();

            //Redéfini le début de la liste.
            currentActor = myTeam[0];

            //Update la prochaine Cata.
            //Check si c'étais la dernière Cata.
            if (myChallenge.listCatastrophy.IndexOf(currentCata) + 1 > myChallenge.listCatastrophy.Count - 1)
            {
                Debug.Log("Première Cata");
                currentCata = myChallenge.listCatastrophy[0];
            }
            else
            {
                Debug.Log("Next Cata");
                currentCata = myChallenge.listCatastrophy[myChallenge.listCatastrophy.IndexOf(currentCata) + 1];
            }

            //Active la passage CataTurn -> PlayerTurn.

        }
    }

    public void OpenInterface()
    {
        myInterface.GetComponent<Animator>().SetTrigger("OpenAll");
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

    #region Fin de partie

    #region GameOver
    //Bool pour check si le challenge est fini.
    void UpdateEtape()
    {
        Debug.Log("Update etape");

        //Check si il reste des étapes.
        if (myChallenge.listEtape.IndexOf(currentStep) != myChallenge.listEtape.Count - 1)
        {
            Debug.Log("next step");

            //Nouvelle étape.
            currentStep = myChallenge.listEtape[myChallenge.listEtape.IndexOf(currentStep) + 1];
        }
        else
        {
            //Fin du challenge.
            myPhaseDeJeu = PhaseDeJeu.EndGame;
            EndChallenge();

            Debug.Log("Fin du niveau");
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
        uiGameOver.GetComponent<Image>().sprite = myChallenge.ecranDefaite;
        return;
    }
    #endregion

    //Fin du challenge.
    public void EndChallenge()
    {
        //Check si il y a un outro de challenge.
        if (myChallenge.outroChallenge && GameManager.instance)
        {
            GameManager.instance.EnterDialogueMode(myChallenge.outroChallenge);
        }
        else
        {
            Debug.Log("Pas d'outro de challenge");
            FinishChallenge(null);
        }
    }

    public void FinishChallenge(string name)
    {
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

                GameManager.instance.WorldstartPoint = myChallenge.mapPointID;

                if (GameManager.instance.currentC.name == "SO_lvl3")
                {
                    SceneManager.LoadScene("S_MainMenu");
                }
                if (GameManager.instance.currentC.name == "SO_Tuto")
                {
                    GameManager.instance.SetDataLevel(tm1, c1);
                    SceneManager.LoadScene("S_TempsLibre");
                }
                else
                {
                    SceneManager.LoadScene("S_WorldMap");
                }
            }
            else
            {
                //UnityEditor.EditorApplication.isPlaying = false;
            }
        }
        else
        {
            canGoNext = true;
        }

        GameManager.instance.ExitDialogueMode();

        uiVictoire.SetActive(true);

        Debug.Log("Fin du challenge");
    }
    #endregion

    #endregion

    #region Data Vfx
    public void SetAnimFinish(bool value)
    {
        animFinish = value;
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

    public Animator GetuiLogs()
    {
        return uiLogsAnimator;
    }
}
