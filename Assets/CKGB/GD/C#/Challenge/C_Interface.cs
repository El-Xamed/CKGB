using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static C_Challenge;

public class C_Interface : MonoBehaviour
{
    //R�cup�ration du script.
    C_Challenge myChallenge;

    enum Interface { Neutre, Logs, Actions, Traits, Back }
    Interface currentInterface = Interface.Neutre;

    [Header("Actions / Traits")]
    [SerializeField] GameObject uiTrait;
    [SerializeField] GameObject uiAction;

    //Mes listes d'actions / traits
    List<ActionButton> listButtonActions = new List<ActionButton>();

    List<ActionButton> listButtonTraits = new List<ActionButton>();

    #region Racourcis
    //Racourcis pour r�cup�rer la liste des action d'une etape cibl� par le challenge.
    SO_ActionClass[] GetListAction()
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

    #endregion

    #region Mes boutons
    //Cr�ation d'une interface pour naviguer dans l'ui est les actions qu'on souhaite s�lectionner
    //Pour acc�der au actions.
    public void GoAction()
    {
        //Animation.
        GetComponent<Animator>().SetTrigger("OpenActions");

        //Fait apparaitre la liste d'action.
        uiAction.SetActive(true);

        //Fait apparaitre les Action du Challenge.
        SpawnActions();

        //Modifie l'�tat de navigation.
        currentInterface = Interface.Actions;
    }

    //Pour acc�der au logs.
    public void GoLogs()
    {
        Debug.Log("Pas disponible");
        AudioManager.instance.PlaySFX(AudioManager.instance.logs);

    }

    //Pour acc�der au traits.
    public void GoTraits()
    {
        //Animation.
        GetComponent<Animator>().SetTrigger("OpenTraits");

        //Fait apparaitre la liste de trait.
        uiTrait.SetActive(true);

        //Fait apparaitre les traits du l'actor.
        SpawnTraits();

        //Modifie l'�tat de navigation.
        currentInterface = Interface.Traits;
    }

    //Pour revenir au temps mort. Et aussi au autres boutons
    public void GoBack()
    {

        AudioManager.instance.PlaySFX(AudioManager.instance.retourEnArriere);

        switch (currentInterface)
        {
            case Interface.Actions:
                GetComponent<Animator>().SetTrigger("CloseActions");
                uiAction.SetActive(false);
                break;
            case Interface.Traits:
                GetComponent<Animator>().SetTrigger("CloseTraits");
                uiTrait.SetActive(false);
                break;
            case Interface.Logs:

                break;
        }

        currentInterface = Interface.Neutre;
    }
    #endregion

    #region Actions / Traits

    public class ActionButton
    {
        public GameObject myButton;
        public SO_ActionClass myActionClass;
    }

    #region Spawn button
    //Fait spawn les bouton d'actions
    void SpawnActions()
    {
        //Cr�er une liste qui rassemble toutes les actions de l'actor qui joue.
        List<SO_ActionClass> currentAction = new List<SO_ActionClass>();
        foreach (var myAction in listButtonActions)
        {
            currentAction.Add(myAction.myActionClass);
        }

        //Check si cette liste "currentAction" est �gal � la liste existante. Si oui alors spawn nouveau Action.
        if (listButtonActions != null && currentAction == GetActionOfCurrentEtape()) return;

        if (GetListAction() != null)
        {
            //Supprime les boutons pr�c�dent
            if (listButtonActions != null)
            {
                foreach (var myAction in listButtonActions)
                {
                    Destroy(myAction.myButton);
                }
            }

            //Cr�er une nouvelle liste.
            listButtonActions = new List<ActionButton>();

            //Cr�er de nouveau boutons
            for (int i = 0; i < GetListAction().Length; i++)
            {
                //Nouvelle class.
                ActionButton newActionButton = new ActionButton();

                //Ref�rence button.
                newActionButton.myButton = Instantiate(Resources.Load<GameObject>("ActionButton"), uiAction.transform);
                newActionButton.myButton.GetComponentInChildren<TMP_Text>().text = GetListAction()[i].buttonText;

                //Ref�rence Action.
                newActionButton.myActionClass = GetListAction()[i];
                listButtonActions.Add(newActionButton);
            }

            uiAction.SetActive(false);
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
                newTraitsButton.myButton = Instantiate(Resources.Load<GameObject>("ActionButton"), uiTrait.transform);
                newTraitsButton.myButton.GetComponentInChildren<TMP_Text>().text = GetListTrait()[i].buttonText;



                //Ref�rence Action.
                newTraitsButton.myActionClass = GetListTrait()[i];



                listButtonTraits.Add(newTraitsButton);

                //BESOIN D'UNE FONCTION POUR DETECTER SI UN NOUVEAU TRAIT A ETE DETECTE.
                /*
                foreach (var thisNewTraits in GetListNewTrait())
                {
                    if (newTraitsButton.myActionClass == thisNewTraits)
                    {
                        newTraitsButton.myButton.transform.GetChild(2).gameObject.SetActive(true);
                    }
                }
                */
            }

            uiAction.SetActive(false);
        }
        else
        {
            Debug.LogError("Erreur spawn traits. La liste de trait du perso est vide.");
        }
    }
    #endregion

    //PASSER PAR L UI NEW INPUT SYSTEME
    /*
    void UpdateActionSelected()
    {
        switch (currentInterface)
        {
            case Interface.Actions:
                #region Action Challenge
                //Cache tous les boutons du challenge.
                foreach (var myButton in GetListAction())
                {
                    //Feedback du bouton non-selecion�.
                    myButton.myButton.transform.GetChild(0).gameObject.SetActive(false);
                }

                //Affiche le bouton que le joueur souhaite selectionner dans le challenge.
                listButton[currentAction].myButton.transform.GetChild(0).gameObject.SetActive(true);
                #endregion
                break;
            case Interface.Traits:
                #region Trait Actor
                if (currentInterface == Interface.Traits)
                {
                    //Cache tous les boutons de traits de l'actor.
                    foreach (var myTrait in listButtonTraits)
                    {
                        //Feedback du bouton non-selecion�.
                        myTrait.myButton.transform.GetChild(0).gameObject.SetActive(false);
                    }

                    //Affiche le bouton que le joueur souhaite selectionner dans les traits de l'actor.
                    listButtonTraits[currentTrait].myButton.transform.GetChild(0).gameObject.SetActive(true);
                }
                #endregion
                break;
        }
    }
    */
    #endregion
}