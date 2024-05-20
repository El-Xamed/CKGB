using Febucci.UI;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static C_Challenge;

public class C_Interface : MonoBehaviour
{
    #region Dialogue
    //Pour passer les dialogue.
    public bool canContinue;
    #endregion

    //Récupération du script.
    C_Challenge myChallenge;

    #region Interface data
    bool onLogs = false;
    public enum Interface {Neutre, Logs, Actions, Traits, Back }
    [SerializeField]Interface currentInterface = Interface.Neutre;

    bool dialogueMode;

    [Header("Logs")]
    [SerializeField] GameObject uiLogsTimeline;

    [Header("Actions / Traits")]
    [SerializeField] GameObject uiAction;

    //Listes d'actions / traits
    List<GameObject> listCurrentButton = new List<GameObject>();
    #endregion

    #region vfx
    Image targetbutton;

    [SerializeField] Image buttonLogsBackground;
    [SerializeField] Image buttonBackBackground;
    [SerializeField] Image buttonActionsBackground;
    [SerializeField] Image buttonTraitsBackground;
    #endregion

    private void Awake()
    {
        myChallenge = GetComponentInParent<C_Challenge>();
        uiAction.SetActive(false);
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

            #region Dialogue
            //Pour passer au dialogue suivant.
            if (input.y < 0 && myChallenge.GetOnDialogue())
            {
                //check si l histoire continue ou pas
                Debug.Log("continue");

                //Check si il y a le GameManager.
                if (GameManager.instance)
                {
                    if (context.performed && GameManager.instance.isDialoguing == true && canContinue == true)
                    {
                        GameManager.instance.ContinueStory();
                    }
                    else if (context.performed && GameManager.instance.isDialoguing == true && canContinue == false)
                    {
                        GameManager.instance.textToWriteIn.GetComponent<TextAnimatorPlayer>().SkipTypewriter();
                    }
                }
                return;
            }
            #endregion

            //Check si l'interaction avec l'interface et possible => Phase du joueur.
            if (GetPhaseDeJeu() == PhaseDeJeu.PlayerTrun)
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
            if (input.y < 0 && GetPhaseDeJeu() == PhaseDeJeu.EndGame)
            {
                myChallenge.FinishChallenge(null);
            }

            //Pour Update CataTurn.
            if (input.y < 0 && GetPhaseDeJeu() == PhaseDeJeu.CataTurn)
            {
                myChallenge.PlayerTurn();
                myChallenge.SetAnimFinish(false);
                //Ouvre l'interface.
                myChallenge.OpenInterface();
            }

            //Pour Update ResoTrun.
            if (input.y < 0 && GetPhaseDeJeu() == PhaseDeJeu.ResoTurn)
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
        targetbutton = buttonActionsBackground;
        GetComponent<Animator>().SetTrigger("Open");

        //Spawn actions
        SpawnActions(GetListActorAction());

        //Modifie l'état de navigation.
        currentInterface = Interface.Actions;
    }

    //Pour accéder au logs.
    public void GoLogs()
    {
        //Animation interface.
        targetbutton = buttonLogsBackground;
        GetComponent<Animator>().SetTrigger("Open");

        //Animation Logs.
        myChallenge.GetuiLogs().SetTrigger("Open");

        //Modifie l'état de navigation.
        currentInterface = Interface.Logs;
        onLogs = true;
        myChallenge.GetEventSystem().SetSelectedGameObject(uiLogsTimeline.GetComponentInChildren<Scrollbar>().gameObject);
    }

    //Pour accéder au traits.
    public void GoTraits()
    {
        //Animation.
        targetbutton = buttonTraitsBackground;
        GetComponent<Animator>().SetTrigger("Open");

        //Spawn actions
        SpawnActions(GetListTrait());

        //Modifie l'état de navigation.
        currentInterface = Interface.Traits;
    }

    //Pour revenir au temps mort. Et aussi au autres boutons
    public void GoBack()
    {
        //Animation
        targetbutton = buttonBackBackground;
        GetComponent<Animator>().SetTrigger("Close");

        switch (currentInterface)
        {
            case Interface.Actions:
                uiAction.SetActive(false);
                break;
            case Interface.Traits:
                uiAction.SetActive(false);
                break;
            case Interface.Logs:
                onLogs = false;
                //Animation Logs.
                myChallenge.GetuiLogs().SetTrigger("Close");
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

            //Lance la preview
            GetComponentInParent<C_PreviewAction>().ShowPreview(myChallenge.GetEventSystem().currentSelectedGameObject.GetComponent<C_ActionButton>().GetActionClass(), myChallenge.GetCurrentActor());
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

    public bool GetOnLogs()
    {
        return onLogs;
    }
    #endregion

    #region Animation Event
    public void SetPositionInHiearchie(GameObject thisButton)
    {
        thisButton.transform.parent = transform;
    }

    public void SetPressedButton()
    {
        if (targetbutton)
        {
            targetbutton.color = UnityEngine.Color.HSVToRGB(0, 0, 0.35f);

            //Ajouter un parametre pour reconnaitre si c'est un retour en arrière ou non.
            /*if (AudioManager.instance && currentInterface == Interface.Back)
            {
                AudioManager.instance.Play("SfxSonDeConfirmation");
            }*/

            if (AudioManager.instance)
            {
                AudioManager.instance.Play("SfxRetourArriere");
            }
        }
    }

    public void SetUnPressedButton()
    {
        if (targetbutton)
        {
            targetbutton.color = UnityEngine.Color.HSVToRGB(0, 0, 1);
        }
    }

    public void ResetTargetButton()
    {
        targetbutton = null;
    }
    #endregion
}
