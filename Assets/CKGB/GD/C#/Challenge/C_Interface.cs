using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static C_Challenge;

public class C_Interface : MonoBehaviour
{
    //Récupération du script.
    C_Challenge myChallenge;

    enum Interface { Neutre, Logs, Actions, Traits, Back }
    Interface currentInterface = Interface.Neutre;

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
    //Racourcis pour récupérer la liste des action d'une etape ciblé par le challenge.
    SO_ActionClass[] GetListAction()
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

            //Pour Update ResoTrun
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
        GetComponent<Animator>().SetTrigger("OpenInterface");

        //Spawn actions
        SpawnActions();

        //Modifie l'état de navigation.
        currentInterface = Interface.Actions;
    }

    //Pour accéder au logs.
    public void GoLogs()
    {
        Debug.Log("Pas disponible");
    }

    //Pour accéder au traits.
    public void GoTraits()
    {
        //Animation.
        GetComponent<Animator>().SetTrigger("OpenInterface");

        //Spawn actions
        SpawnTraits();

        //Modifie l'état de navigation.
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
    void SetShowButton(GameObject thisUiButton)
    {
        uiButton = thisUiButton;
    }

    public void ShowButton()
    {
        uiButton.SetActive(true);
    }

    void SpawnActions()
    {
        //Créer une liste qui rassemble toutes les actions de l'actor qui joue.
        List<SO_ActionClass> currentAction = new List<SO_ActionClass>();
        foreach (var myAction in listButtons)
        {
            currentAction.Add(myAction.GetComponent<C_ActionButton>().GetActionClass());
        }

        //Check si cette liste "currentAction" est égal à la liste existante. Si oui alors spawn nouveau Action.
        if (listButtons != null && currentAction == GetActionOfCurrentEtape()) return;

        if (GetListAction() != null)
        {
            //Supprime les boutons précédent
            if (listButtons != null)
            {
                foreach (var myAction in listButtons)
                {
                    Destroy(myAction);
                }
            }

            //Créer une nouvelle liste.
            listButtonActions = new List<ActionButton>();

            //Créer de nouveau boutons.  EN TEST POUR DONNER LE SO_ACTIONCLASS DANS LE GAMEOBJECT DU BOUTON POUR POUVOIR LE RECUPERER AVEC L'EVENT SYSTEM.
            for (int i = 0; i < GetListAction().Length; i++)
            {
                //Reférence button.
                GameObject myButton = Instantiate(Resources.Load<GameObject>("ActionButton"), uiAction.transform.GetChild(0).transform);

                //Modifier le texte du nom du bouton + les stats ecrit dans les logs (AJOUTER POUR LES STATS)
                myButton.GetComponentInChildren<TMP_Text>().text = GetListAction()[i].buttonText;

                //Reférence Action.
                myButton.GetComponent<C_ActionButton>().SetActionClass(ScriptableObject.Instantiate(GetListAction()[i]));

                
                //Check si "currentActor" possède l'energie pour utiliser cette action.
                if (GetCurrentActor().GetcurrentEnergy() >= myButton.GetComponent<C_ActionButton>().GetStats(Interaction.ETypeTarget.Self, TargetStats.ETypeStatsTarget.Price, Stats.ETypeStats.Energy))
                {
                    //Renseigne le "onClick" du nouveau buton pour qu'après selection il passe au prochain actor.
                    myButton.GetComponent<Button>().onClick.AddListener(() => myChallenge.UseAction(myButton.GetComponent<C_ActionButton>()));
                }
                else //Sinon setup une fonction qui lui quand le joueur va appuier dessus va recevoir en retour des VFX + SFX qui montre bien au joueur qu'il ne peut pas utiliser cette action.
                {
                    //Modifie le visu du bouton.
                    myButton.GetComponent<Image>().sprite = Resources.Load<Sprite>("EtiquetteBW");

                    //Renseigne le "onClick" du nouveau buton pour afficher les Feedback qui montre que le joueur ne peut pas sélectionner cette action.
                    myButton.GetComponent<Button>().onClick.AddListener(() => myChallenge.CantUseAction());

                    Debug.Log(myButton.GetComponent<C_ActionButton>().GetActionClass().buttonText + " sera impossible d'utilisation pour cette acteur.");
                }

                listButtons.Add(myButton);
            }

            myChallenge.GetEventSystem().SetSelectedGameObject(listButtons[0]);

            myChallenge.WriteStatsPreview();
        }
        else
        {
            Debug.LogError("Erreur spawn actions");
        }
    }

    void SpawnTraits()
    {
        #region Check spawn + check same list of currentActor in PlayerTurn.
        //Créer une liste qui rassemble toutes les actions de l'actor qui joue.
        List<SO_ActionClass> currentTrait = new List<SO_ActionClass>();
        foreach (var myTrait in listButtonTraits)
        {
            currentTrait.Add(myTrait.myActionClass);
        }

        //Check si cette liste "currentTrait" est égal à la liste existante. Si oui alors spawn nouveau trait.
        if (listButtonTraits != null && currentTrait == GetListTrait()) return;
        #endregion

        //Check si la list stocké dans le SO_Character est vide
        if (GetListTrait() != null)
        {
            //Supprime les boutons précédent si une liste est deja existante.
            if (listButtonTraits != null)
            {
                foreach (var myTraits in listButtonTraits)
                {
                    Destroy(myTraits.myButton);
                }
            }

            //Créer une nouvelle liste.
            listButtonTraits = new List<ActionButton>();

            //Créer de nouveau boutons (Traits)
            for (int i = 0; i < GetListTrait().Count; i++)
            {
                //Nouvelle class.
                ActionButton newTraitsButton = new ActionButton();

                //Reférence button.
                newTraitsButton.myButton = Instantiate(Resources.Load<GameObject>("ActionButton"), uiTrait.transform.GetChild(0).transform);

                //Modifier le texte du nom du bouton + les stats ecrit dans les logs (AJOUTER POUR LES STATS)
                newTraitsButton.myButton.GetComponentInChildren<TMP_Text>().text = GetListTrait()[i].buttonText;

                //Reférence Action.
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

    #region Partage de donné
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
