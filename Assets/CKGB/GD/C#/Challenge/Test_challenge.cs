using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class Test_challenge : MonoBehaviour
{
    #region Mes variables
    #region De Bases
    GameObject canva;
    GameObject uiCases;

    [Header("Paramètre du challenge")]
    [Tooltip("Information pour faire spawn un nombre de case prédéfinis.")]
    int nbCase;
    [Tooltip("Prefab de case")]
    C_Case prefabCase;
    [Tooltip("Information SECONDAIRE pour faire spawn les personnages et Acc sur la scene")]
    int[] initialplayerPosition;

    [Header("Info dev")]
    List<C_Case> listCase;
    #endregion

    //Tableau de toutes les étapes.
    [SerializeField] SO_Etape[] allSteps;
    //Position des boutons.
    [SerializeField] public Transform[] buttonPlacements;
    //Définis l'étape actuel.
    [SerializeField] public SO_Etape currentStep;
    [SerializeField] int currentStepID;

    //Utilisation d'une class qui regroupe 1 bouton et 1 action.
    [SerializeField] List<Action> listActions;

    #endregion

    private void Start()
    {
        currentStep = allSteps[0];
        SpawnActions();
        //Apparition des cases
        //SpawnCases();
       
    }

    private void OnEnable()
    {
        //Place les acteurs sur les cases.
       // InitialiseAllActorPosition();
    }

    #region Mes fonctions
    //Fonction de spawn les boutons.
    void SpawnActions()
    {
        for (int i = 0; i < currentStep.actions.Length; i++)
        {
            //Création d'une nouvelle class qui sera ajouté dans une liste.
            Action myAction = new Action();
            listActions.Add(myAction);

            //Création d'un boutton qui sera en ref dans la class action.
            Button myButton =  Instantiate(currentStep.actions[i].actionButton, buttonPlacements[i].transform.position, buttonPlacements[i].transform.rotation,FindObjectOfType<Canvas>().transform);
            //myButton.onClick.AddListener(listActions[i].GetAction().IsPossible); //MARCHE PAS QUE 1 FOIS.
            //myButton.onClick.AddListener(currentStep.actions[i].IsPossible);
            myButton.onClick.AddListener(listActions[i].UseAction);
            //Change les info de la class.
            listActions[i].SetButton(myButton);
            listActions[i].SetAction(currentStep.actions[i]);
            listActions[i].SetTestChallenge(this);

            currentStepID = i;
        }
    }

    void SpawnCases()
    {
        //Spawn toutes les cases.
        for (int i = 0; i < nbCase; i++)
        {
            C_Case myCase = Instantiate(prefabCase, uiCases.transform);

            listCase.Add(myCase);
        }
    }

    #endregion

    #region Fonctions pour partager les info

    #region Data
    public List<C_Case> GetCasesList()
    {
        return listCase;
    }

    public int GetNbCases()
    {
        return nbCase;
    }

    public int[] GetInitialPlayersPosition()
    {
        return initialplayerPosition;
    }
    #endregion

    #region Fonction

    //Fait spawn les acc.
    public void SpawnAcc(int position)
    {

    }
    public void NextStep()
    {
        Destroy(currentStep.actions[currentStep.actions.Length].actionButton);
        if (allSteps[currentStepID + 1] != null)
        {
            currentStep = allSteps[currentStepID + 1];
            Debug.Log("next step yay");
            for (int i = 0; i < currentStep.actions.Length; i++)
            {
                Instantiate(currentStep.actions[i].actionButton, buttonPlacements[i].transform.position, buttonPlacements[i].transform.rotation, FindObjectOfType<Canvas>().transform);
                currentStepID = i;
            }
        }   
        else
        {
            Debug.Log("win");
        }
        
    }

    //Pour faire déplacer les accessoires.
    void MoveAccessories()
    {

    }
    #endregion

    #endregion
}

[Serializable]
public class Action
{
    Test_challenge testChallenge;
    [SerializeField] SO_ActionClass actionClass;
    [SerializeField] Button button;

    public void SetButton(Button myButton)
    {
        button = myButton;
    }

    public void SetAction(SO_ActionClass myAction)
    {
        actionClass = myAction;
    }

    public void SetTestChallenge(Test_challenge myChallenge)
    {
        testChallenge = myChallenge;
    }

    public SO_ActionClass GetAction()
    {
        return actionClass;
    }

    public void UseAction()
    {
        if(testChallenge.currentStep.rightAnswer == actionClass)
        {
            Debug.Log("Bonne action");

            //Check si il possède une sous action.
        }
        else
        {
            Debug.Log("Mauvaise action");
        }
    }
}

