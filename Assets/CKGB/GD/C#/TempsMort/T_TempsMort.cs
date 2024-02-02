using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using TMPro;

public class T_TempsMort : MonoBehaviour
{
    #region Variables
    [SerializeField]EventSystem Es;

    [SerializeField]
    GameObject[] charactersLittleResume;

    [SerializeField]
    GameObject[] charactersCompleteResume;

    [SerializeField]
    TMP_Text[] littleCharactersSpecs;
    [SerializeField]
    TMP_Text[] CompleteCharactersSpecs1;
    [SerializeField]
    TMP_Text[] CompleteCharactersSpecs2;

    [SerializeField]
    GameObject[] charactersButton;
    [SerializeField]
    bool[] characterHasPlayed;

    [SerializeField]
    GameObject[] actions;
    [SerializeField]
    GameObject ChallengeButton;

    [SerializeField]
    List<StartPosition> listCharactersPositions;

    [SerializeField]
    GameObject[] characters;

    PlayerInput pi;

    [SerializeField]GameObject currentButton;
    [SerializeField] bool isAnActionButton=false;

    #region Animation
    Animation[] papoter;
    Animation[] observer;
    Animation[] revasser;
    #endregion

   [SerializeField] GameObject actorActif;
    #endregion

    private void Awake()
    {
       
    }

    private void Start()
    {
        initiateStats();
        #region affichage ou non des boutons d'ui
        foreach (GameObject button in actions)
        {
            if(button!=null)
            button.SetActive(false);
        }
        foreach (GameObject fiche in charactersCompleteResume)
        {
            if (fiche != null)
                fiche.SetActive(false);
        }
        ChallengeButton.SetActive(false);
        
        GameManager.instance.ChangeActionMap("TempsMort");
        initiatePosition();
        //Pour setup les perso
        //InitialisationTempsMort();

        //Lance le dialogue.
        //StartIntroduction();
        #endregion
    }
    #region input
    public void AffichageMiniCartePerso()
    {
        for(int i=0;i<charactersLittleResume.Length;i++)
        {
            if (currentButton == charactersButton[i])
            {
                charactersLittleResume[i].SetActive(true);
            }
            else
                charactersLittleResume[i].SetActive(false);
        }
    }
    public void AffichageCarteCompletePerso()
    {
        for (int i = 0; i < charactersCompleteResume.Length; i++)
        {
            if (actorActif == characters[i]&&isAnActionButton==true)
            {
                Debug.Log("oui");
                charactersCompleteResume[i].SetActive(true);
            }
            else
                charactersCompleteResume[i].SetActive(false);          
        }
    }
    public void updateButton()
    {
     for(int i=0;i<charactersButton.Length;i++)
        {
            if(currentButton==charactersButton[i])
            {
                actorActif = characters[i];
            }
        }
        currentButton = Es.currentSelectedGameObject;
        AffichageMiniCartePerso();
        AffichageCarteCompletePerso();     
    }
    public void SpawnActions()
    {
        isAnActionButton = true;
        for(int i=0;i<charactersButton.Length;i++)
        {
            if(charactersButton[i]==currentButton)
            {
                characterHasPlayed[i] = true;
                actorActif = characters[i];
            }
        }
        foreach (GameObject button in actions)
        {
            if (button != null)
                button.SetActive(true);
        }
        foreach(GameObject characters in charactersButton)
        {
            if (characters != null)
            {
                Debug.Log(characters);
                characters.SetActive(false);
            }
                
        }
        Es.SetSelectedGameObject(actions[0]);
        updateButton();
        Debug.Log(currentButton);
    }
    public void SpawnCharactersButtons()
    {
        isAnActionButton = false;
        actorActif = null;
        foreach (GameObject button in actions)
        {
            if (button != null)
                button.SetActive(false);
        }
        /*foreach (GameObject characters in charactersButton)
        {
            if (characters != null)
                characters.SetActive(true);
        }*/
        for (int i= 0;i< charactersButton.Length;i++)
        {
            if(characterHasPlayed[i]!=true)
            {
                charactersButton[i].SetActive(true);
                Es.SetSelectedGameObject(charactersButton[i]);
            }
        }
        if(characterHasPlayed[0]==true&& characterHasPlayed[1] == true && characterHasPlayed[2] == true)
        {
            SpawnChallengeButton();   
        }

        //Es.SetSelectedGameObject(charactersButton[0]);
       
        updateButton();
        Debug.Log(currentButton);
    }
    public void SpawnChallengeButton()
    {
        ChallengeButton.SetActive(true);
        Es.SetSelectedGameObject(ChallengeButton);
        updateButton();
    }
    public void GoToChallenge()
    {

    }
    public void Naviguate(InputAction.CallbackContext context)
    {
        if (!context.performed) { return; }

        if (context.performed)
        {
            updateButton();
        }
    }
    public void SwitchCharacterCard(InputAction.CallbackContext context)
    {
        if (!context.performed) { return; }

        if(context.performed)
        {

        }
    }



    #endregion
    private void InitialisationTempsMort()
    {
        if (GameObject.Find("GameManager"))
        {
            //Place les personnage selon la liste des positions.
            foreach (var myPosition in listCharactersPositions)
            {
                //Regarde l'enum de l'objet.
                switch (myPosition.GetEnum())
                {
                    case EActorClass.Koolkid:
                        //Place le personnage sur cette position avec .
                        SetTransform(myPosition.GetPosition().transform, GameManager.instance.GetRessournce()[0]);
                        Debug.Log("Koolkid");
                        break;
                    case EActorClass.Grandma:
                        SetTransform(myPosition.GetPosition().transform, GameManager.instance.GetRessournce()[1]);
                        Debug.Log("Grandma");
                        break;
                    case EActorClass.Clown:
                        SetTransform(myPosition.GetPosition().transform, GameManager.instance.GetRessournce()[3]);
                        Debug.Log("Clown");
                        break;
                }
            }
        }
        else
        {
            Debug.Log("Pas de GameManager de détecté.");
        }
       
        void SetTransform(Transform position, GameObject actor)
        {
            //Pour chaque perso dans l'équipe.
            foreach (var myActor in GameManager.instance.GetTeam())
            {
                //Regarde si dans la liste d'acteur du GameManager est égale au GameObject des ressources, et que la resource n'est pas null. 
                if (myActor.GetComponent<C_Actor>().GetDataActor().name == actor.GetComponent<C_Actor>().GetDataActor().name && actor != null)
                {
                    /*
                    //check si il existe dans la scene pour le placer ou alors il le fait spawn à la bonne position.
                    if (GameObject.Find(myActor.GetDataActor().name))
                    {
                        Debug.Log("Deja spawn");

                        GameObject.Find(myActor.GetDataActor().name).transform.SetParent(position);
                    }
                    else
                    {
                        Debug.Log("Spawn");

                        Instantiate(myActor, position);
                    }
                    */
                }
            }
        }
    }

    #region Mes fonctions

    void initiatePosition()
    {
     for(int i=0;i<characters.Length;i++)
        {
            Instantiate(characters[i], listCharactersPositions[i].InitialPosition.gameObject.transform);
        }
    }
    void initiateStats()
    {
      for(int i=0;i<characters.Length;i++)
        {
            littleCharactersSpecs[i].text = "PV " + characters[i].GetComponent<C_Actor>().dataActor.stressMax + "\nEnergy "+ characters[i].GetComponent<C_Actor>().dataActor.energyMax;
        }
    }
    //retour au choix de personnage et disparition des boutons de choix
    void NextActor()
    {

    }
    //fait apparaitre les boutons de choix
    void CharacterTurn(GameObject actualCharacter)
    {

    }

    void StartPapoter()
    {
        
    }

    void StartObserver()
    {

    }

    void StartRevasser()
    {

    }

    void StartIntroduction()
    {
        C_DialogueManager.instance.LetsTalk(0);
    }

    public void StartChallenge()
    {
        
    }
    #endregion
    #region buttons

  

    #endregion
}

[Serializable]
public class StartPosition
{
    [SerializeField]
    EActorClass actor;

    [SerializeField]
    public GameObject InitialPosition;
    
    public EActorClass GetEnum()
    {
        return actor;
    }

    public GameObject GetPosition()
    {
        return InitialPosition;
    }
}
