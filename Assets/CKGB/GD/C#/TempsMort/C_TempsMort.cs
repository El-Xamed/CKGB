using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class C_TempsMort : MonoBehaviour
{
    #region Variables

    [Header("Characters pages")]
    [SerializeField]
    GameObject[] charactersLittleResume;
    [SerializeField]
    GameObject[] charactersCompleteResume1;
    [SerializeField]
    GameObject[] charactersCompleteResume2;

    [SerializeField]
    TMP_Text[] littleCharactersSpecs;
    [SerializeField]
    TMP_Text[] CompleteCharactersSpecs1;
    [SerializeField]
    TMP_Text[] CompleteCharactersSpecs2;

    [Header("Buttons Lists")]
    [SerializeField]
    GameObject[] charactersButton;
    [SerializeField]
    GameObject[] actions;
    [SerializeField]
    GameObject ChallengeButton;
    [SerializeField]
    GameObject[] PapotageChoiceButtons;

    [Header("Characters and TM")]
    [SerializeField]
    List<Transform> listCharactersPositions;

    [SerializeField]
    GameObject[] characters;
    [SerializeField]
    SO_TempsMort TM;
    [SerializeField]
    public GameObject background;

    [Header("Eventsystem and Selected Gameobjects")]
    [SerializeField] EventSystem Es;
    [SerializeField] GameObject currentButton;

    [SerializeField] GameObject actorActif;

    [SerializeField]
    bool[] characterHasPlayed;
    [SerializeField] 
    bool isAnActionButton = false;

    #endregion
    // Start is called before the first frame update
    void Start()
    {
        #region HideUI
        Es = FindObjectOfType<EventSystem>();
        foreach (GameObject button in actions)
        {
            if (button != null)
                button.SetActive(false);
        }
        foreach(GameObject papot in PapotageChoiceButtons)
        {
            papot.SetActive(false);
        }
        foreach (GameObject fiche in charactersCompleteResume1)
        {
            if (fiche != null)
                fiche.SetActive(false);
        }
        foreach (GameObject fiche in charactersCompleteResume2)
        {
            if (fiche != null)
                fiche.SetActive(false);
        }
        ChallengeButton.SetActive(false);
        #endregion

        GameManager.instance.ChangeActionMap("TempsMort");
        initiateTMvariables();
        updateButton();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
  
    public void updateButton()
    {
        for (int i = 0; i < charactersButton.Length; i++)
        {
            if (currentButton == charactersButton[i])
            {
                actorActif = characters[i];
            }
        }
        currentButton = Es.currentSelectedGameObject;
        AffichageMiniCartePerso();
        AffichageCarteCompletePerso();
    }
    public void AffichageMiniCartePerso()
    {
        for (int i = 0; i < charactersLittleResume.Length; i++)
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
        for (int i = 0; i < charactersCompleteResume1.Length; i++)
        {
            if (actorActif == characters[i] && isAnActionButton == true)
            {   
                charactersCompleteResume1[i].SetActive(true);               
            }
            else
                charactersCompleteResume1[i].SetActive(false);
        }
    }

    //active les boutons de choix d'actions
    public void ActivateActionsButtons()
    {
        isAnActionButton = true;
        for (int i = 0; i < characters.Length; i++)
        {
            if(currentButton==charactersButton[i])
            {
                characterHasPlayed[i] = true;
            }
                
        }
        for(int i=0;i<actions.Length;i++)
        {
            actions[i].SetActive(true);
            charactersButton[i].GetComponent<Button>().enabled=false;
        }
        Es.SetSelectedGameObject(actions[0]);
        updateButton();
    }

    //active les boutons de choix de persos
    public void ActivateCharactersButton()
    {
        isAnActionButton = false;
        for (int i = 0; i < actions.Length; i++)
        {
            actions[i].SetActive(false);
            if (characterHasPlayed[i] == false)
            {
                charactersButton[i].GetComponent<Button>().enabled = true;
                Es.SetSelectedGameObject(charactersButton[i]);
            }
            if(characterHasPlayed[0]==true && characterHasPlayed[1]==true && characterHasPlayed[2]==true)
            {
                ChallengeButton.SetActive(true);
                Es.SetSelectedGameObject(ChallengeButton);
                for(int y=0;y<charactersButton.Length;y++)
                {
                    charactersButton[y].SetActive(false);
                    actions[y].SetActive(false);
                }
                updateButton();
            }
        }
        updateButton();
    }

    //recupere les stats des actors et du so_tempsmort
    public void initiateTMvariables()
    {
        characters = TM.Team;
        background.GetComponent<SpriteRenderer>().sprite = TM.TMbackground;
        for(int i=0;i<characters.Length;i++)
        {        
            Instantiate(characters[i], listCharactersPositions[i]);
            characters[i].GetComponent<SpriteRenderer>().sprite = characters[i].GetComponent<C_Actor>().dataActor.MapTmSprite;
            characters[i].GetComponent<C_Actor>().maxEnergy = characters[i].GetComponent<C_Actor>().dataActor.energyMax;
            characters[i].GetComponent<C_Actor>().maxStress = characters[i].GetComponent<C_Actor>().dataActor.stressMax;
            characters[i].GetComponent<C_Actor>().maxtraitPoint = characters[i].GetComponent<C_Actor>().dataActor.nbtraitpointMax;
            characters[i].GetComponent<C_Actor>().currentPointTrait = characters[i].GetComponent<C_Actor>().dataActor.currentPointTrait;
            characters[i].GetComponent<C_Actor>().traitaDebloquer = characters[i].GetComponent<C_Actor>().dataActor.traitsAdebloquer;
            characters[i].GetComponent<C_Actor>().listTraits = characters[i].GetComponent<C_Actor>().dataActor.listTraits;
            characters[i].GetComponent<C_Actor>().smallResume = charactersLittleResume[i];
            characters[i].GetComponent<C_Actor>().smallResume.GetComponent<Image>().sprite= characters[i].GetComponent<C_Actor>().dataActor.smallResume;
            characters[i].GetComponent<C_Actor>().BigResume1 = charactersCompleteResume1[i];
            characters[i].GetComponent<C_Actor>().BigResume1.GetComponent<Image>().sprite = characters[i].GetComponent<C_Actor>().dataActor.BigResume1;
            characters[i].GetComponent<C_Actor>().BigResume2 = charactersCompleteResume2[i];
            characters[i].GetComponent<C_Actor>().BigResume2.GetComponent<Image>().sprite = characters[i].GetComponent<C_Actor>().dataActor.BigResume2;
            characters[i].GetComponent<C_Actor>().smallStats = littleCharactersSpecs[i];
            littleCharactersSpecs[i].text = "Calme Max : " + characters[i].GetComponent<C_Actor>().maxStress + "\n Energie Max : " + characters[i].GetComponent<C_Actor>().maxEnergy + "\n Points de trait : " + characters[i].GetComponent<C_Actor>().currentPointTrait + "/" + characters[i].GetComponent<C_Actor>().maxtraitPoint;
            characters[i].GetComponent<C_Actor>().BigStats1 = CompleteCharactersSpecs1[i];
            CompleteCharactersSpecs1[i].text = "Calme Max : " + characters[i].GetComponent<C_Actor>().maxStress + "\n Energie Max : " + characters[i].GetComponent<C_Actor>().maxEnergy + "\n" + characters[i].GetComponent<C_Actor>().dataActor.Description;
            characters[i].GetComponent<C_Actor>().BigStats2 = CompleteCharactersSpecs2[i];
            CompleteCharactersSpecs2[i].text = "Points de trait " + characters[i].GetComponent<C_Actor>().currentPointTrait + "/" + characters[i].GetComponent<C_Actor>().maxtraitPoint + "\n" + characters[i].GetComponent<C_Actor>().listTraits[0].buttonText;
        }
    }
    //chaque deplacement de curseur dans l'ui
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

        if (context.performed)
        {
           if(actorActif!=null)
            {
                if(actorActif.GetComponent<C_Actor>().BigResume1.activeSelf==true)
                {
                    actorActif.GetComponent<C_Actor>().BigResume2.SetActive(true);
                    actorActif.GetComponent<C_Actor>().BigResume1.SetActive(false);
                }
                else if(actorActif.GetComponent<C_Actor>().BigResume2.activeSelf == true)
                {
                    actorActif.GetComponent<C_Actor>().BigResume2.SetActive(false);
                    actorActif.GetComponent<C_Actor>().BigResume1.SetActive(true);
                }
            }
        }
    }

    //provoque le choix du personnage avec qui papoter
    public void Papoter()
    {
        for (int i = 0; i < PapotageChoiceButtons.Length; i++)
        {
            if (characters[i] != actorActif)
            {
                PapotageChoiceButtons[i].SetActive(true);
                Es.SetSelectedGameObject(PapotageChoiceButtons[i]);
            }
            actions[i].SetActive(false);
        }
        updateButton();
        //traitpoint
    }

    //papotage + stats
    public void PapotageFin()
    {
        for(int i=0;i<PapotageChoiceButtons.Length;i++)
        {
            if(currentButton==PapotageChoiceButtons[i])
            {
                actorActif.GetComponent<C_Actor>().currentPointTrait++;
                characters[i].GetComponent<C_Actor>().currentPointTrait++;
                if(actorActif.GetComponent<C_Actor>().currentPointTrait== actorActif.GetComponent<C_Actor>().maxtraitPoint)
                {
                    actorActif.GetComponent<C_Actor>().currentPointTrait = 0;
                    actorActif.GetComponent<C_Actor>().listTraits.Add(actorActif.GetComponent<C_Actor>().dataActor.traitsAdebloquer[0]);
                }
                if (characters[i].GetComponent<C_Actor>().currentPointTrait == characters[i].GetComponent<C_Actor>().maxtraitPoint)
                {
                    characters[i].GetComponent<C_Actor>().currentPointTrait = 0;
                    characters[i].GetComponent<C_Actor>().listTraits.Add(characters[i].GetComponent<C_Actor>().dataActor.traitsAdebloquer[0]);
                }
            }
        }
        foreach(GameObject papot in PapotageChoiceButtons)
        {
            papot.SetActive(false);
        }
        ActivateCharactersButton();
    }
    public void Respirer()
    {
        //energy
        actorActif.GetComponent<C_Actor>().dataActor.energyMax+=1;
        actorActif.GetComponent<C_Actor>().maxEnergy+=1;
        ActivateCharactersButton();
    }
    public void Revasser()
    {
        //calm
        actorActif.GetComponent<C_Actor>().dataActor.stressMax++;
        actorActif.GetComponent<C_Actor>().maxStress++;
        ActivateCharactersButton();
    }
    public void Challenge()
    {
        SceneManager.LoadScene("S_DestinationTest");
    }
}