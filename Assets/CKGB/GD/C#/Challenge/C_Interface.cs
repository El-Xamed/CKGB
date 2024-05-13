using Febucci.UI;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static C_Challenge;

public class C_Interface : MonoBehaviour
{
    //Pour passer les dialogue.
    public bool canContinue;

    //Récupération du script.
    C_Challenge myChallenge;

    public enum Interface {None ,Neutre, Logs, Actions, Traits, Back }
    [SerializeField]Interface currentInterface = Interface.None;

    [Header("Logs")]
    [SerializeField] GameObject uiLogs;
    [SerializeField] GameObject uiLogsScrollbar;

    [Header("Actions / Traits")]
    [SerializeField] GameObject uiAction;

    //Listes d'actions / traits
    List<GameObject> listCurrentButton = new List<GameObject>();

    private void Awake()
    {
        myChallenge = GetComponentInParent<C_Challenge>();
        uiLogs.SetActive(false);
        uiAction.SetActive(false);
    }

    private void Start()
    {
        /*INUTILE SUREMENT
        if (!GameManager.instance.isDialoguing)
        {
            currentInterface = Interface.None;
        }*/

        currentInterface = Interface.None;
    }

    #region Racourcis
    //Racourcis pour récupérer la liste des action d'une etape ciblé par le challenge.
    List<SO_ActionClass> GetListActorAction()
    {
        return myChallenge.GetListActionOfCurrentStep();
    }

    //Racourcis pour récuperer la liste de trait de l'actor sélectionné par le challenge.
    List<SO_ActionClass> GetListTrait()
    {
        return myChallenge.GetCurrentActor().GetDataActor().listTraits;
    }

    List<SO_ActionClass> GetListNewTrait()
    {
        return myChallenge.GetCurrentActor().GetDataActor().listNewTraits;
    }

    List<SO_ActionClass> GetActionOfCurrentEtape()
    {
        List<SO_ActionClass> listCurrentAction = new List<SO_ActionClass>();

        foreach (var thisAction in myChallenge.GetCurrentEtape().actions)
        {
            listCurrentAction.Add(thisAction);
        }

        return listCurrentAction;
    }

    PhaseDeJeu GetPhaseDeJeu()
    {
        return myChallenge.GetPhaseDeJeu();
    }

    C_Actor GetCurrentActor()
    {
        return myChallenge.GetCurrentActor();
    }

    List<C_Case> GetListCases()
    { 
        return myChallenge.GetListCases(); 
    }
    #endregion

    #region Input
    public void SelectInterface(InputAction.CallbackContext context)
    {
        if (!context.performed) { return; }

        if (context.performed)
        {
            Vector2 input = context.ReadValue<Vector2>();

            //Pour passer au dialogue suivant.
            if (currentInterface == Interface.None)
            {
                if (input.y < 0)
                {
                    //check si l histoire continue ou pas
                    Debug.Log("continue");
                    if (context.performed && GameManager.instance.isDialoguing == true && canContinue == true)
                    {
                        GameManager.instance.ContinueStory();
                    }
                    else if (context.performed && GameManager.instance.isDialoguing == true && canContinue == false)
                    {
                        GameManager.instance.textToWriteIn.GetComponent<TextAnimatorPlayer>().SkipTypewriter();
                    }
                    return;
                }
            }

            //Check si l'interaction avec l'interface et possible => Phase du joueur.
            if (GetPhaseDeJeu() == PhaseDeJeu.PlayerTrun && currentInterface != Interface.None)
            {
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
                }

                //Quand il est dans le logs.
                if (currentInterface == Interface.Logs)
                {
                    if (input.x > 0)
                    {
                        GoBack();
                        return;
                    }
                }
            }

            //Pour passer à la suite du jeu.
            if (input.y < 0 && GetPhaseDeJeu() == PhaseDeJeu.EndGame && currentInterface != Interface.None)
            {
                myChallenge.FinishChallenge(null);
            }

            //Pour Update CataTurn.
            if (input.y < 0 && GetPhaseDeJeu() == PhaseDeJeu.CataTurn && currentInterface != Interface.None)
            {
                myChallenge.PlayerTurn();
                myChallenge.SetAnimFinish(false);
                //Ouvre l'interface.
                myChallenge.OpenInterface();
            }

            //Pour Update ResoTrun.
            if (input.y < 0 && GetPhaseDeJeu() == PhaseDeJeu.ResoTurn && currentInterface != Interface.None)
            {
                myChallenge.NextResolution();
            }
        }
    }
    #endregion

    #region Mes boutons
    //Création d'une interface pour naviguer dans l'ui est les actions qu'on souhaite sélectionner
    //Pour accéder au actions.
    public void GoAction()
    {
        //Animation.
        GetComponent<Animator>().SetTrigger("OpenInterface");

        //Spawn actions
        SpawnActions(GetListActorAction());

        //Modifie l'état de navigation.
        currentInterface = Interface.Actions;

        if (AudioManager.instance)
        {
            AudioManager.instance.Play("SfxSonDeConfirmation");
        }
    }

    //Pour accéder au logs.
    public void GoLogs()
    {
        //Modifie l'état de navigation.
        currentInterface = Interface.Logs;
        uiLogs.SetActive(true);
        myChallenge.GetEventSystem().SetSelectedGameObject(uiLogsScrollbar);
    }

    //Pour accéder au traits.
    public void GoTraits()
    {
        //Animation.
        GetComponent<Animator>().SetTrigger("OpenInterface");

        //Spawn actions
        SpawnActions(GetListTrait());

        //Modifie l'état de navigation.
        currentInterface = Interface.Traits;

        if (AudioManager.instance)
        {
            AudioManager.instance.Play("SfxSonDeConfirmation");
        }
    }

    //Pour revenir au temps mort. Et aussi au autres boutons
    public void GoBack()
    {

        if (AudioManager.instance)
        {
            AudioManager.instance.Play("SfxRetourArriere");
        }

        switch (currentInterface)
        {
            case Interface.Actions:
                GetComponent<Animator>().SetTrigger("CloseInterface");
                uiAction.SetActive(false);
                break;
            case Interface.Traits:
                GetComponent<Animator>().SetTrigger("CloseInterface");
                uiAction.SetActive(false);
                break;
            case Interface.Logs:
                GetComponent<Animator>().SetTrigger("CloseInterface");
                uiLogs.SetActive(false);
                break;
        }

        currentInterface = Interface.Neutre;
    }
    #endregion

    #region Actions / Traits

    #region Spawn button
    //Affiche les boutons d'actions.
    public void ShowButton()
    {
        uiAction.SetActive(true);

        myChallenge.WriteStatsPreview();
    }

    void SpawnActions(List<SO_ActionClass> spawnListAction)
    {
        //Créer une liste qui rassemble toutes les actions de l'actor qui joue. A MODIFER + DEPLACER.
        List<SO_ActionClass> currentAction = new List<SO_ActionClass>();
        foreach (GameObject myAction in listCurrentButton)
        {
            currentAction.Add(myAction.GetComponent<C_ActionButton>().GetActionClass());
        }

        //Check la liste "listCurrentAction" n'est pas vide + si c'est bien egale a la liste d'action de l'etape.
        if (listCurrentButton != null && currentAction == GetActionOfCurrentEtape()) return;

        if (spawnListAction != null)
        {
            //Supprime les boutons précédent
            if (listCurrentButton != null)
            {
                foreach (var myAction in listCurrentButton)
                {
                    Destroy(myAction);
                }
            }

            //Créer une nouvelle liste.
            listCurrentButton = new List<GameObject>();

            //Créer de nouveau boutons. EN TEST POUR DONNER LE SO_ACTIONCLASS DANS LE GAMEOBJECT DU BOUTON POUR POUVOIR LE RECUPERER AVEC L'EVENT SYSTEM.
            for (int i = 0; i < spawnListAction.Count; i++)
            {
                //Reférence button.
                GameObject myButton = Instantiate(Resources.Load<GameObject>("ActionButton_Box"), uiAction.transform.GetChild(0).transform);

                //Modifier le texte du nom du bouton + les stats ecrit dans les logs (AJOUTER POUR LES STATS)
                myButton.GetComponentInChildren<TMP_Text>().text = spawnListAction[i].buttonText;

                //Reférence Action.
                myButton.GetComponent<C_ActionButton>().SetActionClass(spawnListAction[i]);

                //Renseigne le "onClick" du nouveau buton pour qu'après selection il passe au prochain actor.
                myButton.GetComponent<Button>().onClick.AddListener(() => myChallenge.ConfirmAction(myButton.GetComponent<C_ActionButton>()));

                //Fait dispparaitre le curseur.
                myButton.GetComponent<C_ActionButton>().HideCurseur();

                //Ajoute l'action à la liste de currentButton.
                listCurrentButton.Add(myButton);
            }

            //Vise le premier bouton.
            myChallenge.GetEventSystem().SetSelectedGameObject(listCurrentButton[0]);
        }
        else
        {
            Debug.LogError("Erreur spawn actions");
        }
    }
    #endregion

    #endregion

    #region Partage de donné
    public void SetCurrentInterface(Interface newCurrentInterface)
    {
        currentInterface = newCurrentInterface;
    }

    public List<GameObject> GetListActionButton()
    {
        return listCurrentButton;
    }

    public GameObject GetUiAction()
    {
        return uiAction;
    }

    public GameObject GetUiTrait()
    {
        return uiAction;
    }
    #endregion

    #region Animation Event
    public void SetPositionInHiearchie(GameObject thisButton)
    {
        thisButton.transform.parent = transform;
    }
    #endregion
}
