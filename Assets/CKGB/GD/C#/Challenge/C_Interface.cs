using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static C_Challenge;

public class C_Interface : MonoBehaviour
{
    //R�cup�ration du script.
    C_Challenge myChallenge;

    public enum Interface { Neutre, Logs, Actions, Traits, Back }
    [SerializeField] Interface currentInterface = Interface.Neutre;

    [Header("Actions / Traits")]
    [SerializeField] GameObject uiTrait;
    [SerializeField] GameObject uiAction;

    //Mes listes d'actions / traits
    [SerializeField] List<ActionButton> listButtonActions = new List<ActionButton>();
    // EN TEST ! AVOIR POUR SUPP LE SCRIPT AU DESSUS.
    [SerializeField] List<GameObject> listButtons = new List<GameObject>();

    [SerializeField] List<ActionButton> listButtonTraits = new List<ActionButton>();

    //Pour afficher l'ui
    GameObject uiButton;

    private void Awake()
    {
        myChallenge = GetComponentInParent<C_Challenge>();

        uiTrait.SetActive(false);
        uiAction.SetActive(false);
    }

    #region Racourcis
    //Racourcis pour r�cup�rer la liste des action d'une etape cibl� par le challenge.
    List<SO_ActionClass> GetListAction()
    {
        return myChallenge.GetListActionOfCurrentStep();
    }

    //Racourcis pour r�cuperer la liste de trait de l'actor s�lectionn� par le challenge.
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

    List<SO_ActionClass> GetActionOfCInterface()
    {
        List<SO_ActionClass> listCurrentActionOfInterface = new List<SO_ActionClass>();

        foreach (GameObject thisAction in listButtons)
        {
            listCurrentActionOfInterface.Add(thisAction.GetComponent<C_ActionButton>().GetActionClass());
        }

        return listCurrentActionOfInterface;
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
                        SetShowButton(uiTrait);
                        return;
                    }

                    if (input.y < 0)
                    {
                        GoAction();
                        SetShowButton(uiAction);
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
            }

            //Pour passer � la suite du jeu.
            if (input.y < 0 && GetPhaseDeJeu() == PhaseDeJeu.EndGame)
            {
                myChallenge.EndChallenge();
            }

            //Pour Update CataTurn.
            if (input.y < 0 && GetPhaseDeJeu() == PhaseDeJeu.CataTurn)
            {
                myChallenge.PlayerTurn();
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
    //Cr�ation d'une interface pour naviguer dans l'ui est les actions qu'on souhaite s�lectionner
    //Pour acc�der au actions.
    public void GoAction()
    {
        //Animation.
        GetComponent<Animator>().SetTrigger("OpenInterface");

        //Spawn actions
        SpawnActions();

        //Modifie l'�tat de navigation.
        currentInterface = Interface.Actions;
    }

    //Pour acc�der au logs.
    public void GoLogs()
    {
        Debug.Log("Pas disponible");
    }

    //Pour acc�der au traits.
    public void GoTraits()
    {
        //Animation.
        GetComponent<Animator>().SetTrigger("OpenInterface");

        //Spawn actions
        SpawnTraits();

        //Modifie l'�tat de navigation.
        currentInterface = Interface.Traits;
    }

    //Pour revenir au temps mort. Et aussi au autres boutons
    public void GoBack()
    {
        
        switch (currentInterface)
        {
            case Interface.Actions:
                GetComponent<Animator>().SetTrigger("CloseInterface");
                uiAction.SetActive(false);
                break;
            case Interface.Traits:
                GetComponent<Animator>().SetTrigger("CloseInterface");
                uiTrait.SetActive(false);
                break;
            case Interface.Logs:

                break;
        }

        currentInterface = Interface.Neutre;

        SetShowButton(null);
    }
    #endregion

    #region Actions / Traits

    [Serializable]
    public class ActionButton
    {
        public GameObject myButton;
        public SO_ActionClass myActionClass;
    }

    #region Spawn button
    //PEUT ETRE UTILISE PLUS TARD.
    void SetShowButton(GameObject thisUiButton)
    {
        uiButton = thisUiButton;
    }

    //Affiche les boutons d'actions.
    public void ShowButton()
    {
        uiButton.SetActive(true);

        myChallenge.GetEventSystem().SetSelectedGameObject(listButtons[0]);

        myChallenge.WriteStatsPreview();
    }

    void SpawnActions()
    {
        //Cr�er une liste qui rassemble toutes les actions de l'actor qui joue.
        List<SO_ActionClass> currentAction = new List<SO_ActionClass>();
        foreach (var myAction in listButtons)
        {
            currentAction.Add(myAction.GetComponent<C_ActionButton>().GetActionClass());
        }

        //Check si cette liste "currentAction" est �gal � la liste existante. Si oui alors spawn nouveau Action.
        if (listButtons != null && currentAction == GetActionOfCurrentEtape()) return;

        if (GetListAction() != null)
        {
            //Supprime les boutons pr�c�dent
            if (listButtons != null)
            {
                foreach (var myAction in listButtons)
                {
                    Destroy(myAction);
                }
            }

            //Cr�er une nouvelle liste.
            listButtons = new List<GameObject>();

            //Cr�er de nouveau boutons. EN TEST POUR DONNER LE SO_ACTIONCLASS DANS LE GAMEOBJECT DU BOUTON POUR POUVOIR LE RECUPERER AVEC L'EVENT SYSTEM.
            for (int i = 0; i < GetListAction().Count; i++)
            {
                //Ref�rence button.
                GameObject myButton = Instantiate(Resources.Load<GameObject>("ActionButton"), uiAction.transform.GetChild(0).transform);

                //Modifier le texte du nom du bouton + les stats ecrit dans les logs (AJOUTER POUR LES STATS)
                myButton.GetComponentInChildren<TMP_Text>().text = GetListAction()[i].buttonText;

                //Ref�rence Action.
                myButton.GetComponent<C_ActionButton>().SetActionClass(GetListAction()[i]);

                //Renseigne le "onClick" du nouveau buton pour qu'apr�s selection il passe au prochain actor.
                myButton.GetComponent<Button>().onClick.AddListener(() => myChallenge.UseAction(myButton.GetComponent<C_ActionButton>()));

                //Fait apparaitre le curseur.
                myButton.GetComponent<C_ActionButton>().HideCurseur();

                listButtons.Add(myButton);
            }
        }
        else
        {
            Debug.LogError("Erreur spawn actions");
        }
    }

    void SpawnTraits()
    {
        #region Check spawn + check same list of currentActor in PlayerTurn.
        //Cr�er une liste qui rassemble toutes les actions de l'actor qui joue.
        List<SO_ActionClass> currentTrait = new List<SO_ActionClass>();
        foreach (var myTrait in listButtonTraits)
        {
            currentTrait.Add(myTrait.myActionClass);
        }

        //Check si cette liste "currentTrait" est �gal � la liste existante. Si oui alors spawn nouveau trait.
        if (listButtonTraits != null && currentTrait == GetListTrait()) return;
        #endregion

        //Check si la list stock� dans le SO_Character est vide
        if (GetListTrait() != null)
        {
            //Supprime les boutons pr�c�dent si une liste est deja existante.
            if (listButtonTraits != null)
            {
                foreach (var myTraits in listButtonTraits)
                {
                    Destroy(myTraits.myButton);
                }
            }

            //Cr�er une nouvelle liste.
            listButtonTraits = new List<ActionButton>();

            //Cr�er de nouveau boutons (Traits)
            for (int i = 0; i < GetListTrait().Count; i++)
            {
                //Nouvelle class.
                ActionButton newTraitsButton = new ActionButton();

                //Ref�rence button.
                newTraitsButton.myButton = Instantiate(Resources.Load<GameObject>("ActionButton"), uiTrait.transform.GetChild(0).transform);

                //Modifier le texte du nom du bouton + les stats ecrit dans les logs (AJOUTER POUR LES STATS)
                newTraitsButton.myButton.GetComponentInChildren<TMP_Text>().text = GetListTrait()[i].buttonText;

                //Ref�rence Action.
                newTraitsButton.myActionClass = GetListTrait()[i];

                //Renseigne le "onClick" du nouveau buton.
                newTraitsButton.myButton.GetComponent<Button>().onClick.AddListener(() => myChallenge.UseAction(newTraitsButton.myButton.GetComponent<C_ActionButton>()));

                listButtonTraits.Add(newTraitsButton);

                //BESOIN D'UNE FONCTION POUR DETECTER SI UN NOUVEAU TRAIT A ETE DETECTE.
            }

            myChallenge.GetEventSystem().SetSelectedGameObject(listButtonTraits[0].myButton);
        }
        else
        {
            Debug.LogError("Erreur spawn traits. La liste de trait du perso est vide.");
        }
    }
    #endregion

    #endregion

    #region Partage de donn�
    public void SetCurrentInterface(Interface newCurrentInterface)
    {
        currentInterface = newCurrentInterface;
    }

    public List<GameObject> GetListActionButton()
    {
        return listButtons;
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
