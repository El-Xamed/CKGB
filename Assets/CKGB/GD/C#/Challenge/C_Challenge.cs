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
    [Header("UI")]
    GameObject canva;
    GameObject uiCases;
    [SerializeField] C_Stats uiStatsPrefab;
    [SerializeField] GameObject uiStats;
    [SerializeField] GameObject uiEtape;
    [SerializeField] GameObject uiAction;

    [Header("Data")]
    [SerializeField] SO_Challenge myChallenge;

    [SerializeField] List<C_Actor> myTeam;

    [Tooltip("Case")]
    [SerializeField] C_Case myCase;
    [SerializeField] List<C_Case> listCase;
    #endregion

    #region Challenge
    [Space(50)]
    bool canUpdateEtape = false;

    C_Actor currentActor;

    //S�lection d'actions
    [SerializeField] int currentAction;

    //D�finis l'�tape actuel.
    public SO_Etape currentStep;

    //Utilisation d'une class qui regroupe 1 bouton et 1 action.
    [SerializeField] List<ActionButton> listButton;
    #endregion

    #region Résolution
    [SerializeField] List<ActorResolution> listRes;
    ActorResolution currentResolution;
    int currentActionResolution;
    [SerializeField] TMP_Text uiLogs;
    #endregion
    #endregion

    //------------------------------------------------------------------------------------------------------------------------

    #region Input
    //RCOURSCIR CETTE PARTIE DU CODE
    public void SelectRight(InputAction.CallbackContext context)
    {
        if(!context.performed) { return; }

        if (context.performed && canSelectAction && currentAction < listButton.Count -1)
        {
            currentAction++;
        }
    }

    public void SelectLeft(InputAction.CallbackContext context)
    {
        if (!context.performed) { return; }

        if (context.performed && canSelectAction && currentAction > 0)
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
                //Lance la "Phase de résolution".
                canSelectAction = false;
                canUpdateRes = true;

                //Définis la valeur à "-1" pour naviguer sans problème dans la listRes.
                currentActionResolution = -1;
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
    void PlayerTrun()
    {
        //Initialise la prochaine cata.
        InitialiseCata();

        //Check si pendant la réso, un acteur a trouvé la bonne reponse. UTILISATION D4UN BOOL QUI SERA DESACTIVE APRES. PERMET DE UPDATE AU BON MOMENT.
        if (canUpdateEtape)
        {
            stepUpdate();

            canUpdateEtape = false;
        }

        //Débloque les commande.
        GetComponent<PlayerInput>().enabled = true;

        //redonne la possibilité de choisir ses action.
        canSelectAction = true;
        canUpdateRes = false;

        //Change l'UI.
        uiAction.SetActive(true);

        //Fait apparaitre les actions.
        SpawnActions();
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

    void UpdateActionSelected(int thisActor)
    {
        foreach (var myButton in listButton)
        {
            //Feedback du bouton non-selecioné.
        }

        //Feedback du bouton non-selecioné.
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

        //Change l'UI.
        uiAction.SetActive(false);

        //Applique toutes les actions. 1 par 1. EN CONSTRUCTION
        //Si la reso en question n'est pas dernier, alors il peut passer a la reso suivante sinon il lance la cat
        if (currentActionResolution < listRes.Count -1 || currentActionResolution == -1)
        {
            //Augmente la valeur.
            currentActionResolution++;

            //Reféfinis "currentResolution" avec 'index de base + 1.
            currentResolution = listRes[currentActionResolution];

            //Check si le perso est jouable
            if (!currentResolution.actor.GetIsOut())
            {
                //CHECK SI L'ACTION PEUT ETRE UTILISE, SI OUI IL ECRIT QUE C'EST BON SINON IL ECRIT L'AUTRE REPONSE + SI C'EST LA BONNE REPONSE ALORS IL LE SIGNALE.
                if (currentResolution.action.CanUse(currentResolution.actor))
                {
                    //Utilise l'action.
                    currentResolution.action.UseAction(currentResolution.actor, listCase);

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

                //Ecrit dans les logs le résultat de l'action.
                uiLogs.text = currentResolution.action.GetLogsChallenge();
            }
            else
            {
                ResolutionTurn();
            }
        }
        else
        {
            //Lance la phase "Cata".
            CataTrun();
            Debug.Log("Toutes les actions on été fait");
        }
    }
    #endregion

    #region Tour de la Cata
    //Pour lancer la cata.
    void CataTrun()
    {
        //Desactive tout input
        GetComponent<PlayerInput>().enabled = false;

        //Check au début si tous les perso sont "out".
        CheckGameOver();

        //Applique la catastrophe. FONCTIONNE AVEC 1 CATA, A MODIFIER POUR QU'IL UTILISE LES CATA
        ApplyCatastrophy(myChallenge.listCatastrophy[0]);
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
        if (CheckGameOver())
        {
            GameOver();
        }
        else
        {
            //Reviens au round du joueur.
            PlayerTrun();
        }
    }
    #endregion

    void UpdateActorSelected()
    {
        //Retire sur tout les actor le contours blanc
        foreach (var myActor in myTeam)
        {
            myActor.IsSelected(false);
        }

        //Fait apparaitre le contour blanc de l'actor selectionne.
        currentActor.IsSelected(true);
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
            return true;
        }

        return false;
    }

    void GameOver()
    {
        Debug.Log("GameOver");
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
