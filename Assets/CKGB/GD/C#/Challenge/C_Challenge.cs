using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
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

    #region De base
    //Pour connaitre la phasse de jeu.
    public enum PhaseDeJeu { PlayerTrun, ResoTurn, CataTurn , EndGame}
    [Header("Phase de jeu")]
    [SerializeField] PhaseDeJeu myPhaseDeJeu = PhaseDeJeu.PlayerTrun;

    [SerializeField] EventSystem eventSystem;

    bool canGoNext = false;

    #region UI
    GameObject canva;
    GameObject uiCases;
    [Header("UI")]
    [SerializeField] GameObject background;
    [SerializeField] C_Stats uiStatsPrefab;
    [SerializeField] GameObject uiStats;
    [SerializeField] GameObject uiEtape;
    [SerializeField] GameObject uiGoodAction;
    [SerializeField] GameObject uiVictoire;
    [SerializeField] GameObject uiGameOver;

    [Header("UI (VFX)")]
    [SerializeField] GameObject vfxPlayerTurn;
    [SerializeField] GameObject vfxResoTurn;
    [SerializeField] GameObject vfxCataTurn;
    #endregion

    [Header("Data")]
    [SerializeField] SO_Challenge myChallenge;

    [SerializeField] GameObject plateau;

    List<C_Actor> myTeam = new List<C_Actor>();
    List<C_Accessories> listAcc = new List<C_Accessories>();

    bool canIniCata = false;

    [Tooltip("Case")]
    [SerializeField] C_Case myCase;
    List<C_Case> listCase = new List<C_Case>();
    #endregion

    #region Interface
    [Header ("UI (Interface)")] // A VOIR POUR FAIRE UN SCRIPT A PART
    [SerializeField] C_Interface myInterface;
    #endregion

    #region Challenge
    [Space(50)]
    [SerializeField] C_Actor currentActor;

    //D�finis l'�tape actuel. RETIRER LE PUBLIC
    [SerializeField] SO_Etape currentStep;

    [SerializeField] SO_Catastrophy currentCata;
    #endregion

    #region Résolution
    [Header("Resolution")]
    List<ActorResolution> listRes = new List<ActorResolution>();
    [SerializeField] ActorResolution currentResolution;

    //RANGER CETTE VARIABLE
    [SerializeField] TMP_Text uiLogs;
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
            //BESOIN DE TRAVAILLER DESSUS POUR L'ALPHA !!!!!!!
            //Cache tout les autres preview.
            //eventSystem.currentSelectedGameObject.GetComponent<C_ActionButton>().HideUiStatsPreview(myTeam);
            //Active une fonction qui affiche toutes les preview de stats sur les actor.
            //eventSystem.currentSelectedGameObject.GetComponent<C_ActionButton>().ShowUiStatsPreview(myTeam, currentActor);

            //Ecrit dans les logs le résultat de l'action.
            //Ecrit directement dans les logs via à une fonction du "SO_ActionClass".
            WriteStatsPreview();
        }
    }
    #endregion

    //------------------------------------------------------------------------------------------------------------------------

    private void Awake()
    {
        #region Racourcis
        canva = transform.GetChild(0).gameObject;


        #endregion

        if (GameManager.instance)
        {
            //GameManager.instance.ChangeActionMap("Challenge");

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
        #region Initialisation
        //Transition
        C_Transition.instance.OpenTransFlannel();

        //Set le background
        background.GetComponent<Image>().sprite = myChallenge.background;

        if (myChallenge.element.Count != 0)
        {
            foreach (var thisElement in myChallenge.element)
            {
                GameObject newUI = Instantiate(new GameObject(), GameObject.Find("Element").transform);

                newUI.name = thisElement.name;

                newUI.AddComponent<Image>();

                newUI.GetComponent<Image>().sprite = thisElement;

                newUI.GetComponent<RectTransform>().sizeDelta = new Vector2(1920,1080);
            }
        }

        //Apparition des cases
        SpawnCases();

        //Place les acteurs sur les cases.
        InitialiseAllPosition();

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
        }
        #endregion

        //Set l'étape en question.
        currentStep = myChallenge.listEtape[0];
        currentActor = myTeam[0];
        UpdateUi();

        //Lance directement le tour du joueur
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
            C_Case newCase = Instantiate(myCase, plateau.transform);
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
                            thisActor.GetComponent<C_Actor>().MoveActor(listCase, position.position);
                            thisActor.transform.localScale = Vector3.one;

                            //Centrage sur la case et position sur Y.
                            thisActor.transform.localPosition = new Vector3();

                            //New Ui stats
                            C_Stats newStats = Instantiate(uiStatsPrefab, uiStats.transform);

                            //Add Ui Stats
                            thisActor.GetComponent<C_Actor>().SetUiStats(newStats);
                            thisActor.GetComponent<C_Actor>().GetUiStats().InitUiStats(thisActor.GetComponent<C_Actor>());

                            //Update UI
                            thisActor.GetComponent<C_Actor>().UpdateUiStats();

                            //Mise en place du VFX de bonne action.
                            GameObject newVfxGoodAction = Instantiate(thisActor.GetComponent<C_Actor>().GetDataActor().vfxUiGoodAction.gameObject, uiGoodAction.transform);
                            thisActor.GetComponent<C_Actor>().GetDataActor().vfxUiGoodAction = newVfxGoodAction.GetComponent<Animator>();

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
                    thisActor.GetComponent<C_Actor>().MoveActor(listCase, position.position);
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
                    GameObject newVfxGoodAction = Instantiate(thisActor.GetComponent<C_Actor>().GetDataActor().vfxUiGoodAction.gameObject, uiGoodAction.transform);
                    thisActor.GetComponent<C_Actor>().GetDataActor().vfxUiGoodAction = newVfxGoodAction.GetComponent<Animator>();

                    myTeam.Add(thisActor);
                }
            }
        }

        void SpawnAcc(List<InitialAccPosition> listPosition)
        {
            foreach (InitialAccPosition position in listPosition)
            {
                C_Accessories myAcc = Instantiate(position.acc, listCase[position.position].transform);
                myAcc.MoveActor(listCase, position.position);

                listAcc.Add(myAcc);
            }
        }
    }
    #endregion

    #region Tour du joueur
    //Pour Update l'UI. CHANGER LA FONCTION !!!!
    void UpdateUi()
    {
        uiEtape.GetComponentInChildren<TMP_Text>().text = myChallenge.objectif;
    }

    public void WriteStatsPreview()
    {
        if (eventSystem.currentSelectedGameObject != null && eventSystem.currentSelectedGameObject.activeSelf)
        {
            //Cache les autres curseur.
            foreach (GameObject thisActionButton in myInterface.GetListActionButton())
            {
                thisActionButton.GetComponent<C_ActionButton>().HideCurseur();
            }

            //Cache les autres curseur.
            foreach (GameObject thisTraitButton in myInterface.GetListTraitButton())
            {
                thisTraitButton.GetComponent<C_ActionButton>().HideCurseur();
            }

            //Fait apparaitre le curseur.
            eventSystem.currentSelectedGameObject.GetComponent<C_ActionButton>().ShowCurseur();

            //Affiche la preview.
            uiLogs.text = eventSystem.currentSelectedGameObject.GetComponent<C_ActionButton>().GetLogsPreview(myTeam, currentActor, listCase);
        }
    }

    //Fonction qui est stocké dans les button action donné par l'interface + permet de passer à l'acteur suivant ou alors de lancer la phase de résolution.
    public void UseAction(C_ActionButton thisActionButton)
    {
        //FeedBack
        currentActor.PlayAnimSelectAction();

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
            currentActor = null;
            UpdateActorSelected();
            currentResolution = listRes[0];

            //Cache les boutons + ferme l'interface. CHANGER ÇA POUR AVOIR L'INTERFACE SANS LES TEXT A COTE.
            myInterface.GetComponent<Animator>().SetTrigger("CloseAll");
            myInterface.GetUiAction().SetActive(false);
            myInterface.GetUiTrait().SetActive(false);

            //Lance la phase de réso. BESOIN D'AJOUTER UN DELAY A CAUSE DE L'ANIMATION QUI EMPECHE D'APPLIQUER LES SPRITES.
            //Debug.Log(currentResolution.actor.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.name);
            ResolutionTurn();
        }
    }

    public void PlayerTurn()
    {
        Debug.Log("Player turn !");

        //VFX
        vfxPlayerTurn.GetComponent<Animator>().SetTrigger("PlayerTurn");

        //Défini la phase de jeu.
        myPhaseDeJeu = PhaseDeJeu.PlayerTrun;

        UpdateUi();

        //Vide la listeReso
        listRes = new List<ActorResolution>();

        //Initialise la prochaine cata.
        if (currentStep.useCata)
        {
            //Check si il n'est pas null.
            if (currentCata == null)
            {
                currentCata = myChallenge.listCatastrophy[0];
            }

            currentCata.InitialiseCata(listCase, myTeam);

            foreach (C_Actor thisActor in myTeam)
            {
                thisActor.SetCurrentCata(currentCata);
            }

            canIniCata = false;
        }

        //Joue l'animation.
        vfxPlayerTurn.GetComponent<Animator>().enabled = true;

        //Check si le perso est jouable
        if (!currentActor.GetIsOut())
        {
            //Pour effacer le texte de la cata.
            uiLogs.text = "";

            //Update le contour blanc
            UpdateActorSelected();
        }
        else
        {
            NextActor();
            PlayerTurn();
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
        if (listRes.IndexOf(currentResolution) < listRes.Count - 1)
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
                    CataTrun();
                }
            }
            else
            {
                //Redéfini le début de la liste.
                currentActor = myTeam[0];
                //Ouvre l'interface.
                OpenInterface();
                PlayerTurn();
            }
        }
    }

    void ResolutionTurn()
    {
        Debug.Log("Resolution trun !");
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

        //Joue l'animation (PASSER PAR UNE FONCTION QUI AVEC UN SWITCH LANCE LA BONNE ANIM)
        vfxResoTurn.GetComponent<Animator>().enabled = true;

        //Applique toutes les actions. 1 par 1.
        currentResolution.button.GetActionClass().UseAction(currentResolution.actor, listCase, myTeam);

        //Check si c'est la bonne action.
        if (currentResolution.button.GetActionClass().name == currentStep.rightAnswer.name)
        {
            Debug.Log("Bonne action");

            //Vfx de bonna action.
            currentResolution.actor.GetDataActor().vfxUiGoodAction.SetTrigger("GoodAction");

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

        //Ecrit dans les logs le résultat de l'action.
        uiLogs.text = currentResolution.button.GetActionClass().LogsMakeAction;
    }
    #endregion

    #region Tour de la Cata
    //Pour lancer la cata.
    public void CataTrun()
    {
        Debug.Log("CataTurn");

        //Défini la phase de jeu.
        myPhaseDeJeu = PhaseDeJeu.CataTurn;

        //Ecrit dans les logs le résultat de l'action.
        uiLogs.text = currentCata.catastrophyLog;

        //Applique la catastrophe.
        currentCata.ApplyCatastrophy(listCase, myTeam);

        //Re-Check si tous les perso sont "out".
        if (!CheckGameOver())
        {
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
        if (canGoNext)
        {
            SceneManager.LoadScene("S_WorldMap");
        }
        else
        {
            canGoNext = true;
        }

        uiVictoire.SetActive(true);

        Debug.Log("Fin du challenge");
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

    public List<C_Case> GetListCases() { return listCase; }

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
}
