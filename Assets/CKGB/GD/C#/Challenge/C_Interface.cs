using Febucci.UI;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static C_Challenge;

public class C_Interface : MonoBehaviour
{
    bool onPause;

    //Variable pour bloquer les interaction dans le dev quand les anim se joue.
    bool onAnim = false;

    #region Dialogue
    //Pour passer les dialogue.
    public bool canContinue;
    #endregion

    //Récupération du script.
    C_Challenge myChallenge;

    //Le "new" permet de ne pas avoir une valeur null.
    UnityEvent currentEvent = new UnityEvent();

    #region Interface data
    bool onLogs = false;
    public enum Interface {Tuto, Neutre, Logs, Actions, Traits, Back}
    Interface currentInterface = Interface.Neutre;
    //Variable stocké pour l'appliquer à "currentInterface" pour éviter des bugs dans la nav rapide.
    Interface nextInterface;

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

        canContinue = true;
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
        return myChallenge.GetCurrentActor().GetDataActor().listNewTraits;
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
            if(!onPause)
            {
                Vector2 input = context.ReadValue<Vector2>();

                #region Tuto
                //Pour le tuto.
                if (input.y < 0 && currentInterface == Interface.Tuto)
                {
                    Debug.Log("Next tuto");
                    myChallenge.NextTuto();
                    return;
                }
                #endregion

                #region Dialogue
                //Pour passer au dialogue suivant.
                if (input.y < 0 && myChallenge.GetOnDialogue())
                {
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
                    #region Neutre
                    //Pour la navigation dans l'interface "Neutre"
                    if (currentInterface == Interface.Neutre && !myChallenge.GetOnDialogue())
                    {
                        #region Traits
                        if (input.x < 0)
                        {
                            Debug.Log("Go Traits");
                            GoTraits();
                            return;
                        }
                        #endregion

                        #region Actions
                        if (input.y < 0)
                        {
                            Debug.Log("Go Actions");
                            GoAction();
                            return;
                        }
                        #endregion

                        #region Logs (Desactivé)
                        if (input.y > 0)
                        {
                            //GoLogs();
                            return;
                        }
                        #endregion
                    }
                    #endregion

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
                if (input.y < 0 && GetPhaseDeJeu() == PhaseDeJeu.EndGame && !myChallenge.GetOnDialogue())
                {
                    myChallenge.EndChallenge();
                }

                if (input.y < 0 && GetPhaseDeJeu() == PhaseDeJeu.GameOver)
                {
                    myChallenge.GameOver();
                }

                //Pour Update CataTurn.
                if (input.y < 0 && GetPhaseDeJeu() == PhaseDeJeu.CataTurn)
                {
                    myChallenge.NextCata();
                }

                //Pour Update ResoTrun.
                if (input.y < 0 && GetPhaseDeJeu() == PhaseDeJeu.ResoTurn)
                {
                    Debug.Log("Next Réso !");
                    myChallenge.NextResolution();
                }
            }
            
        }
    }
    #endregion

    #region Mes boutons
    //Création d'une interface pour naviguer dans l'ui est les actions qu'on souhaite sélectionner
    //Pour accéder au actions.
    public void GoAction()
    {
        if (!onAnim)
        {
            //Spawn actions
            SpawnActions(GetListActorAction());

            //Setup quelle fonction va etre lancé.
            //Retire toutes les fonctions stocké dans l'event.
            currentEvent.RemoveAllListeners();

            //Setup automatiquement l'event de transition.
            currentEvent.AddListener(ShowButton);

            //Animation.
            targetbutton = buttonActionsBackground;
            GetComponent<Animator>().SetTrigger("Open");

            if (currentInterface != Interface.Tuto)
            {
                //Modifie l'état de navigation.
                nextInterface = Interface.Actions;
            }

            onAnim = true;
        }
    }

    //Pour accéder au logs.
    public void GoLogs()
    {
        if (!onAnim)
        {
            //Setup quelle fonction va etre lancé.
            //Retire toutes les fonctions stocké dans l'event.
            currentEvent.RemoveAllListeners();

            //Setup automatiquement l'event de transition.
            currentEvent.AddListener(OpenLogs);

            //Spawn actions
            SpawnActions(GetListActorAction());

            //Animation interface.
            targetbutton = buttonLogsBackground;
            GetComponent<Animator>().SetTrigger("Open");

            if (currentInterface != Interface.Tuto)
            {
                //Modifie l'état de navigation.
                currentInterface = Interface.Logs;
            }

            onAnim = true;
        }
    }

    //Pour accéder au traits.
    public void GoTraits()
    {
        if (!onAnim)
        {
            //Spawn actions
            SpawnActions(GetListTrait());

            //Setup quelle fonction va etre lancé.
            //Retire toutes les fonctions stocké dans l'event.
            currentEvent.RemoveAllListeners();

            //Setup automatiquement l'event de transition.
            currentEvent.AddListener(ShowButton);

            //Animation.
            targetbutton = buttonTraitsBackground;
            GetComponent<Animator>().SetTrigger("Open");

            if (currentInterface != Interface.Tuto)
            {
                //Modifie l'état de navigation.
                nextInterface = Interface.Traits;
            }

            onAnim |= true;
        }
    }

    //Pour revenir au temps mort. Et aussi au autres boutons
    public void GoBack()
    {
        if (!onAnim)
        {
            //Animation
            targetbutton = buttonBackBackground;
            GetComponent<Animator>().SetTrigger("Close");

            //Detect si l'objet parent qui sert à contenir les actions/traits est activé.
            if (uiAction.activeSelf)
            {
                uiAction.SetActive(false);

                //Pour retirer le bouton dans l'event system.
                myChallenge.GetEventSystem().SetSelectedGameObject(null);
            }

            //A SUP PEUT ETRE
            /*if (GameManager.onPause == true)
            {
                Debug.Log("back from options");
                GameManager.instance.BackFromPause(); 
            }
            else if (GameManager.onPause == true)
            {
                Debug.Log("back from pause");
                GameManager.instance.BackFromPause();
            }
            else
            {
                switch (currentInterface)
                {
                    case Interface.Actions:
                        uiAction.SetActive(false);
                        //Desactive la preview.
                        GetComponentInParent<C_PreviewAction>().DestroyAllPreview(GetComponentInParent<C_Challenge>().GetTeam());
                        break;
                    case Interface.Traits:
                        uiAction.SetActive(false);
                        GetComponentInParent<C_PreviewAction>().DestroyAllPreview(GetComponentInParent<C_Challenge>().GetTeam());
                        break;
                    case Interface.Logs:
                        onLogs = false;
                        //Animation Logs.
                        myChallenge.GetuiLogs().SetTrigger("Close");
                        break;
                }

                if (currentInterface != Interface.Tuto)
                {
                    currentInterface = Interface.Neutre;
                }
                else
                {
                    uiAction.SetActive(false);
                }
            }*/

            switch (currentInterface)
            {
                case Interface.Actions:
                    //Desactive la preview.
                    GetComponentInParent<C_PreviewAction>().DestroyAllPreview(GetComponentInParent<C_Challenge>().GetTeam());
                    break;
                case Interface.Traits:
                    GetComponentInParent<C_PreviewAction>().DestroyAllPreview(GetComponentInParent<C_Challenge>().GetTeam());
                    break;
                case Interface.Logs:
                    onLogs = false;
                    //Animation Logs.
                    myChallenge.GetuiLogs().SetTrigger("Close");
                    break;
            }

            if (currentInterface != Interface.Tuto)
            {
                currentInterface = Interface.Neutre;
            }

            onAnim = true;
        }
    }
    #endregion

    #region Actions / Traits

    #region Spawn button
    
    //Affiche les logs.
    void OpenLogs()
    {
        //Animation Logs.
        myChallenge.GetuiLogs().SetTrigger("Open");

        onLogs = true;
        myChallenge.GetEventSystem().SetSelectedGameObject(uiLogsTimeline.GetComponentInChildren<Scrollbar>().gameObject);
    }

    //Affiche les boutons d'actions.
    void ShowButton()
    {
        //Affiche les actions
        uiAction.SetActive(true);

        //Set dans quel interface on est. (Pour éviter un bug dans la navigation rapide)
        if (currentInterface != Interface.Tuto)
        {
            //Modifie l'état de navigation.
            currentInterface = nextInterface;
        }

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
                //myButton.GetComponent<Button>().onClick.AddListener(() => myChallenge.ConfirmAction(myButton.GetComponent<C_ActionButton>()));
                myButton.GetComponent<Button>().onClick.AddListener(() => myChallenge.ConfirmAction(myButton.GetComponent<C_ActionButton>().GetActionClass()));

                //Fait dispparaitre le curseur.
                myButton.GetComponent<C_ActionButton>().HideCurseur();

                //Ajoute l'action à la liste de currentButton.
                listCurrentButton.Add(myButton);
            }

            //Check si il est pas en mode "Tuto".
            if (currentInterface != Interface.Tuto)
            {
                //Vise le premier bouton.
                myChallenge.GetEventSystem().SetSelectedGameObject(listCurrentButton[0]);

                //Lance la preview
                GetComponentInParent<C_Challenge>().WriteStatsPreview();
            }
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
    public Interface GetCurrentInterface()
    {
        return currentInterface;
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

    public void EndInterfaceAnimationOpen()
    {
        currentEvent.Invoke();

        onAnim = false;
    }

    public void EndInterfaceAnimationClose()
    {
        onAnim = false;
    }

    //Pour animer visuellement le bouton. (Partie 1)
    public void SetPressedButton()
    {
        if (targetbutton)
        {
            targetbutton.color = Color.HSVToRGB(0, 0, 0.35f);

            //Ajouter un parametre pour reconnaitre si c'est un retour en arrière ou non.
            /*if (AudioManager.instance && currentInterface == Interface.Back)
            {
                AudioManager.instance.Play("SfxSonDeConfirmation");
            }*/

            if (AudioManager.instanceAM)
            {
                AudioManager.instanceAM.Play("SfxRetourArriere");
            }
        }
    }

    //Pour animer visuellement le bouton. (Partie 2)
    public void SetUnPressedButton()
    {
        if (targetbutton)
        {
            targetbutton.color = Color.HSVToRGB(0, 0, 1);
        }
    }

    public void ResetTargetButton()
    {
        targetbutton = null;
    }
    #endregion
}
