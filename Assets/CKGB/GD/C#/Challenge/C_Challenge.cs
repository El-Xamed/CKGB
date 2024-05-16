using Febucci.UI;
using System;
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
    [Header("Interface")]
    [SerializeField] C_Interface myInterface;

    [Header("Data Challenge")]
    [SerializeField] SO_Challenge myChallenge;

    //Pour connaitre la phasse de jeu.
    public enum PhaseDeJeu { PlayerTrun, ResoTurn, CataTurn, EndGame }
    [Header("Phase de jeu")]
    PhaseDeJeu myPhaseDeJeu = PhaseDeJeu.PlayerTrun;

    [SerializeField] GameObject plateauGameObject;
    [SerializeField] GameObject uiLogs;

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

    private void Start()
    {
        //Set le background
        background.GetComponent<Image>().sprite = myChallenge.background;

        //Apparition des cases
        SpawnCases();

        //Place les acteurs sur les cases.
        InitialiseAllPosition();

        //Set les element en plus.
        SpawnElement();

        if (GameManager.instance)
        {
            //Cache toute l'Ui pour les dialogue.
            ShowUiChallenge(false);
        }

        //Lance l'animation qui présente le challenge après la transition
        vfxStartChallenge.SetTrigger("start");
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
        uiLogs.SetActive(active);
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
            StartChallenge(null);
        }
    }

    public void StartChallenge(string name)
    {
        //Lance l'animation de la phase.
        LunchPlayerPhase();

        if (GameManager.instance)
        {
            GameManager.instance.ExitDialogueMode();
        }

        //Re-active le fond des logs.
        GetuiLogs().GetComponentInChildren<Image>().enabled = true;

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
                myChallenge.listEtape[i].actions[j].Convert();
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

            //Fait apparaitre le curseur.
            eventSystem.currentSelectedGameObject.GetComponent<C_ActionButton>().ShowCurseur();
            #endregion

            //Affiche la preview.
            GetComponent<C_PreviewAction>().ShowPreview(eventSystem.currentSelectedGameObject.GetComponent<C_ActionButton>().GetActionClass(), currentActor);
        }
    }

    //Fonction qui est stocké dans les button action donné par l'interface + permet de passer à l'acteur suivant ou alors de lancer la phase de résolution.
    public void ConfirmAction(C_ActionButton thisActionButton)
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
        actorResolution.button = thisActionButton;

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
            ResolutionTurn();

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
    public void CheckPreview(SO_ActionClass thisActionClass, Interaction_NewInspector.ETypeTarget target)
    {
        foreach (Interaction_NewInspector thisInteraction in thisActionClass.newListInteractions)
        {
            //Check si c'est égale à "actorTarget".
            if (thisInteraction.whatTarget == target)
            {
                foreach (TargetStats_NewInspector thisTargetStats in thisInteraction.listTargetStats)
                {
                    //Check si c'est des stats ou un Mouvement.
                    if (thisTargetStats.whatStatsTarget == TargetStats_NewInspector.ETypeStatsTarget.Stats)
                    {
                        //Inscrit la preview de texte + ui. Avec les info de preview. (C_Challenge)
                        //C_PreviewAction.onPreview += TextPreview;
                    }

                    if (thisTargetStats.whatStatsTarget == TargetStats_NewInspector.ETypeStatsTarget.Movement)
                    {
                        Debug.Log(thisActionClass.name + " : " + thisTargetStats.whatStatsTarget);
                        //Inscrit la preview de movement. (C_Challenge)
                        //C_PreviewAction.onPreview += MovementPreview;
                    }
                }
            }
        }
    }

    void TextPreview(SO_ActionClass thisActionClass)
    {
        //Récupère toutes les info directement ?
        //Non si on veut faire apparaitre les autres actor avec leur description.
        //Oui car le joueur a besoin de savoir toutes les conséquence de l'action, meme si ça ne touche pas les autres actor.
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

        TargetStats_NewInspector.ETypeMove whatMove = thisActionClass.GetWhatMove(Interaction_NewInspector.ETypeTarget.Self);
        int nbMove = thisActionClass.GetValue(Interaction_NewInspector.ETypeTarget.Self, TargetStats_NewInspector.ETypeStatsTarget.Movement);
        
        bool isTp = thisActionClass.GetIsTp(Interaction_NewInspector.ETypeTarget.Self);

        //Check si c'est le mode normal de déplacement ou alors le mode target case.
        if (whatMove == TargetStats_NewInspector.ETypeMove.Right || whatMove == TargetStats_NewInspector.ETypeMove.Left) //Normal move mode.
        {
            //Check si cette valeur doit etre negative ou non pour setup correctement la direction.
            if (whatMove == TargetStats_NewInspector.ETypeMove.Left)
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
        public C_ActionButton button;
        public C_Actor actor;
    }

    //Fonction appelé par "C_Interface" pour passer à la résolution suivante.
    public void NextResolution()
    {
        string nextLogs = currentResolution.button.GetActionClass().GetListLogs();
        if (!string.IsNullOrEmpty(nextLogs))
        {
            uiLogs.GetComponentInChildren<TMP_Text>().text = nextLogs;
        }
        else if (listRes.IndexOf(currentResolution) < listRes.Count - 1)
        {
            //Reféfinis "currentResolution" avec 'index de base + 1.
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
        Debug.Log("Resolution trun !");

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
            //Applique toutes les actions. 1 par 1.
            //New : Utilise l'action directement dans le challenge.
            UseAction(currentResolution);

            #region Check si c'est la bonne action
            //Check si c'est la bonne action.
            if (currentResolution.button.GetActionClass().name == currentStep.rightAnswer.name)
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
                    if (thisReso.button.GetActionClass().nextAction != null)
                    {
                        for (int i = 0; i < myChallenge.listEtape[myChallenge.listEtape.IndexOf(currentStep)].actions.Count; i++)
                        {
                            if (myChallenge.listEtape[myChallenge.listEtape.IndexOf(currentStep)].actions[i] == thisReso.button.GetActionClass())
                            {
                                Debug.Log("Update next action");
                                myChallenge.listEtape[myChallenge.listEtape.IndexOf(currentStep)].actions[i] = thisReso.button.GetActionClass().nextAction;
                            }
                        }
                    }
                }
            }
            #endregion

            #region Logs
            //Ecrit dans les logs le résultat de l'action.
            currentResolution.button.GetActionClass().ResetLogs();
            uiLogs.GetComponentInChildren<TMP_Text>().text = currentResolution.button.GetActionClass().GetListLogs();
            #endregion
        }
        else
        {
            //VFX
            vfxResoTurn.GetComponent<Animator>().SetTrigger("PlayerTurn");
        }
    }

    //Utilise l'action. VOIR POUR RETIRE LE PUBLIC.
    public void UseAction(ActorResolution thisActorResolution)
    {
        Debug.Log("Use this actionClass : " + thisActorResolution.button.GetActionClass().buttonText);

        //Raccourcis.
        SO_ActionClass action = thisActorResolution.button.GetActionClass();
        C_Actor actor = thisActorResolution.actor;

        //Applique les conséquences de stats peut importe si c'est réusi ou non.
        #region Self
        //Créer la liste pour "self"
        action.SetStatsTarget(Interaction_NewInspector.ETypeTarget.Self, actor);

        //Check si un mouvement pour "self" existe.
        CheckIfTargetMove(Interaction_NewInspector.ETypeTarget.Self, actor);
        #endregion

        #region Other
        //Créer la liste pour "other"
        if (action.CheckOtherInAction())
        {
            //Boucle avec la range.
            for (int i = 0; i < action.GetRange(); i++)
            {
                Debug.Log("Je cherche sur la case " + i);

                if (action.GetTypeDirectionRange() != Interaction_NewInspector.ETypeDirectionTarget.None)
                {
                    //Boucle pour check sur tout les actor du challenge.
                    foreach (C_Actor thisOtherActor in myTeam)
                    {
                        //Check quel direction la range va faire effet.
                        switch (action.GetTypeDirectionRange())
                        {
                            //Si "otherActor" est dans la range alors lui aussi on lui affiche les preview mais avec les info pour "other".
                            case Interaction_NewInspector.ETypeDirectionTarget.Right:
                                //Calcul vers la droite.
                                CheckPositionOther(actor, i, thisOtherActor);
                                Debug.Log("Direction Range = droite.");
                                break;
                            case Interaction_NewInspector.ETypeDirectionTarget.Left:
                                //Calcul vers la gauche.
                                CheckPositionOther(actor, -i, thisOtherActor);
                                Debug.Log("Direction Range = Gauche.");
                                break;
                            case Interaction_NewInspector.ETypeDirectionTarget.RightAndLeft:
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

                        action.SetStatsTarget(Interaction_NewInspector.ETypeTarget.Other, target);

                        //Check si un mouvement pour "other" existe.
                        CheckIfTargetMove(Interaction_NewInspector.ETypeTarget.Other, target);
                        return true;
                    }
                }
                else if (thisActor.GetPosition() + position <= 0)
                {
                    if (0 + position == target.GetPosition() && target != thisActor)
                    {
                        Debug.Log(target.name + " à été trouvé ! à la position: " + target.GetPosition());

                        action.SetStatsTarget(Interaction_NewInspector.ETypeTarget.Other, target);

                        //Check si un mouvement pour "other" existe.
                        CheckIfTargetMove(Interaction_NewInspector.ETypeTarget.Other, target);
                        return true;
                    }
                }
                else if (thisActor.GetPosition() + position == target.GetPosition() && target != thisActor)
                {
                    Debug.Log(target.name + " à été trouvé ! à la position: " + target.GetPosition());

                    action.SetStatsTarget(Interaction_NewInspector.ETypeTarget.Other, target);

                    //Check si un mouvement pour "other" existe.
                    CheckIfTargetMove(Interaction_NewInspector.ETypeTarget.Other, target);
                    return true;
                }

                return false;
            }
        }
        #endregion

        #region Logs
        //Check dans les data de cette action si la condition est bonne.
        if (action.CanUse(actor))
        {
            //Renvoie un texte de condition réussite.
        }
        else
        {
            //Renvoie un texte de condition de non réussite.
            return;
        }
        #endregion

        //Fonction pour vérifier si un mouvement est nessecaire.
        void CheckIfTargetMove(Interaction_NewInspector.ETypeTarget target, C_Actor thisActor)
        {
            //Regarde d'abord c'est quoi comme type de déplacement.
            if (!action.GetIfTargetOrNot()) //Non ciblé par un actor ou acc.
            {
                Debug.Log("Pas ciblé par un actor ou acc.");

                //Check si un mouvement existe.
                if (action.GetValue(target, TargetStats_NewInspector.ETypeStatsTarget.Movement) != 0)
                {
                    //Deplace l'actor avec l'info de déplacement + type de déplacement.
                    MoveActorInBoard(thisActor, action.GetValue(target, TargetStats_NewInspector.ETypeStatsTarget.Movement), action.GetWhatMove(target), action.GetIsTp(target));
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

    #region Deplace les actor
    //Fonction qui déplace les actor.
    public void MoveActorInBoard(C_Pion thisPion, int nbMove, TargetStats_NewInspector.ETypeMove whatMove, bool isTp)
    {
        //Setup la position de départ.
        thisPion.GetComponent<C_Actor>().SetStartPosition(plateau[thisPion.GetPosition()].GetComponentInParent<Transform>());

        //Check si c'est le mode normal de déplacement ou alors le mode target case.
        if (whatMove == TargetStats_NewInspector.ETypeMove.Right || whatMove == TargetStats_NewInspector.ETypeMove.Left) //Normal move mode.
        {
            //Check si cette valeur doit etre negative ou non pour setup correctement la direction.
            if (whatMove == TargetStats_NewInspector.ETypeMove.Left)
            {
                nbMove = -nbMove;
            }

            CheckIfNotExceed();
        }
        else //Passe en mode "targetCase". Pour permettre de bien setup le déplacement meme si la valeur est trop élevé par rapport au nombre de case dans la liste.
        {
            //Check si le nombre de déplacement est trop élevé par rapport au nombre de case.
            if (nbMove > plateau.Count - 1)
            {
                Debug.LogWarning("La valeur de déplacement et trop élevé par rapport au nombre de cases sur le plateau la valeur sera donc égale à 0.");

                nbMove = 0;
            }
        }

        //Check si un autre membre de l'équipe occupe deja a place. A voir pour le déplacer après que l'actor ai bougé.
        foreach (C_Actor thisOtherActor in myTeam)
        {
            //Si dans la list de l'équipe c'est pas égale à l'actor qui joue. Et si "i" est égale à "newPosition" pour décaler seulement l'actor qui occupe la case ou on souhaite ce déplacer.
            if (thisPion != thisOtherActor)
            {
                //Détection de si il y a un autres actor.
                if (thisPion.GetPosition() + nbMove == thisOtherActor.GetPosition())
                {
                    //Check si c'est une Tp ou non.
                    if (isTp)
                    {
                        //Place l'autre actor à la position de notre actor.
                        Debug.Log(TextUtils.GetColorText(thisPion.name, Color.cyan) + " a échangé sa place avec " + TextUtils.GetColorText(thisOtherActor.name, Color.green) + ".");
                        PlacePionOnBoard(thisOtherActor, thisPion.GetPosition(),isTp);
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
            Debug.Log(thisPion.GetPosition());
            Debug.Log(nbMove);

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
                    currentCata.actionClass.SetStatsTarget(Interaction_NewInspector.ETypeTarget.Self, thisActor);

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
                        currentCata.actionClass.SetStatsTarget(Interaction_NewInspector.ETypeTarget.Self, thisActor);

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
    //Bool pour check si le vhallenge est fini.
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
        if (myChallenge.outroChallenge)
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
            foreach (C_Actor thisActor in myTeam)
            {
                thisActor.GetComponent<Animator>().SetBool("isInDanger", false);
                thisActor.transform.parent = GameManager.instance.transform;
            }

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
            canGoNext = true;
        }

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

    void LunchPlayerPhase()
    {
        //Lance le Vfx de l'annonce du problème.
        vfxStartChallenge.SetTrigger("start");
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

    public GameObject GetuiLogs()
    {
        return uiLogs;
    }
}
