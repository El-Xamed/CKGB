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

    #region De base
    //Pour connaitre la phasse de jeu.
    public enum PhaseDeJeu { PlayerTrun, ResoTurn, CataTurn }
    [Header("Phase de jeu")]
    [SerializeField] PhaseDeJeu myPhaseDeJeu = PhaseDeJeu.PlayerTrun;

    GameObject canva;
    GameObject uiCases;
    [Header("UI")]
    [SerializeField] Sprite background;
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



    [Header("Data")]
    [SerializeField] SO_Challenge myChallenge;

    List<C_Actor> myTeam = new List<C_Actor>();
    List<C_Accessories> listAcc = new List<C_Accessories>();

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
    C_Actor currentActor;

    //D�finis l'�tape actuel. RETIRER LE PUBLIC
    [SerializeField] SO_Etape currentStep;
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

        if (!context.performed)
        {
            SceneManager.LoadScene("Destination_Test");
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
        //Set le background
        background = myChallenge.background;

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

    //CHECK SI ÇA RECPUPE BIEN LES INFO DU GAMEMANAGER EST QUE LE LIEN ENTRE SES DEUX LISTE FONCTIONNE.
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
            foreach (InitialActorPosition position in listPosition)
            {
                if (GameManager.instance)
                {
                    //Récupère les info du GameManager
                    foreach (var thisActor in GameManager.instance.GetTeam())
                    {
                        //Check si dans les info du challenge est dans l'équipe stocké dans le GameManager.
                        if (thisActor.GetComponent<C_Actor>().GetDataActor().name == position.perso.GetComponent<C_Actor>().GetDataActor().name)
                        {
                            //Placement des perso depuis le GameManager
                            thisActor.transform.parent = listCase[position.position].transform;
                            thisActor.transform.localScale = Vector3.one;
                            thisActor.transform.localPosition = Vector3.zero;
                            thisActor.GetComponent<C_Actor>().IniChallenge();
                            thisActor.GetComponent<C_Actor>().SetPosition(position.position);

                            //New Ui stats
                            C_Stats newStats = Instantiate(uiStatsPrefab, uiStats.transform);

                            //Add Ui Stats
                            thisActor.GetComponent<C_Actor>().SetUiStats(newStats);

                            //Update UI
                            thisActor.GetComponent<C_Actor>().UpdateUiStats();

                            myTeam.Add(thisActor.GetComponent<C_Actor>());
                        }
                    }
                }
                else
                {
                    //New actor
                    C_Actor myActor = Instantiate(position.perso, listCase[position.position].transform);
                    myActor.IniChallenge();
                    myActor.SetPosition(position.position);

                    //New Ui stats
                    C_Stats newStats = Instantiate(uiStatsPrefab, uiStats.transform);

                    //Add Ui Stats
                    myActor.SetUiStats(newStats);

                    //Update UI
                    myActor.UpdateUiStats();

                    myTeam.Add(myActor);
                }
            }
        }

        void SpawnAcc(List<InitialAccPosition> listPosition)
        {
            foreach (InitialAccPosition position in listPosition)
            {
                C_Accessories myAcc = Instantiate(position.acc, listCase[position.position].transform);
                myAcc.SetPosition(position.position);

                listAcc.Add(myAcc);
            }
        }
    }

    

    #endregion

    #region Tour du joueur
    //A SUPP ?
    /*
    //Utilise l'action
    void UseAction()
    {
        AudioManager.instance.PlaySFX(AudioManager.instance.confirmation);

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
    }*/

    void PlayerTrun()
    {
        //Défini la phase de jeu.
        myPhaseDeJeu = PhaseDeJeu.PlayerTrun;

        //Initialise la prochaine cata.
        InitialiseCata();

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

        //Initialise la cata (Random)
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

    void ResolutionTurn()
    {
        Debug.Log("Resolution trun !");

        //Défini la phase de jeu.
        myPhaseDeJeu = PhaseDeJeu.ResoTurn;

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

            //Pour switch avec l'acc FONCTION QUE AVEC L'ACC 0 DE LA LISTE.
            if (currentResolution.action.GetSwitchWithAcc())
            {
                //Deplace l'actor
                currentResolution.actor.transform.parent = listCase[listAcc[0].currentPosition].transform;
                currentResolution.actor.GetComponent<RectTransform>().localPosition = new Vector3(0, currentResolution.actor.GetComponent<RectTransform>().localPosition.y, 0);

                //Deplace l'acc
                listAcc[0].transform.parent = listCase[currentResolution.actor.GetPosition()].transform;
                listAcc[0].GetComponent<RectTransform>().localPosition = new Vector3(0, currentResolution.actor.GetComponent<RectTransform>().localPosition.y, 0);

                //Defini dans le code leut nouvelle position
                int newPositionAcc = currentResolution.actor.GetPosition();
                currentResolution.actor.SetPosition(listAcc[0].currentPosition);
                listAcc[0].currentPosition = newPositionAcc;
            }

            //Ecrit dans les logs le résultat de l'action.
            uiLogs.text = currentResolution.action.GetLogsChallenge();

            //Si c'est la bonne réponse. LE FAIRE DANS L'ACTION DIRECTEMENT
            if (currentResolution.action == currentStep.rightAnswer)
            {
                Debug.Log("Bonne action");

                uiGoodAction.GetComponentInChildren<Image>().sprite = currentResolution.actor.GetDataActor().challengeSpriteUiGoodAction;

                uiGoodAction.GetComponent<Animator>().SetTrigger("GoodAction");
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
        uiVictoire.SetActive(true);
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

        foreach (var thisCase in myChallenge.listCatastrophy[0].targetCase)
        {
            if (thisCase == thisAcc.currentPosition)
            {
                if (thisAcc.dataAcc.typeAttack == SO_Accessories.ETypeAttack.All)
                {
                    //Check si la position des actor est sur la meme case que l'acc.
                    foreach (var thisActor in myTeam)
                    {
                        thisActor.TakeDamage(thisAcc.dataAcc.reducStress, thisAcc.dataAcc.reducEnergie);

                        thisActor.CheckIsOut();
                    }

                    //Ecrit dans les logs le résultat de l'action.
                    uiLogs.text = "La cata à frappé le lézard ! Tous le monde perd -2 de calm !";
                }
            }
        }
        

        //Check si la position des actor est sur la meme case que l'acc.
        foreach (var thisActor in myTeam)
        {
            if (thisActor.GetPosition() == thisAcc.currentPosition)
            {
                thisActor.TakeDamage(thisAcc.dataAcc.reducStress, thisAcc.dataAcc.reducEnergie);

                thisActor.CheckIsOut();

                //Ecrit dans les logs le résultat de l'action.
                uiLogs.text = thisAcc.dataAcc.damageLogs;
            }
        }

        

        //Check si le jeu est fini "GameOver".
        CheckGameOver();
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

    public SO_ActionClass[] GetListActionOfCurrentStep()
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
    #endregion
}
