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
    List<GameObject> characters = new List<GameObject>();
    [SerializeField]
    SO_TempsMort TM;
    [SerializeField]
    public GameObject background;

    [Header("Eventsystem and Selected Gameobjects")]
    [SerializeField] EventSystem Es;
    [SerializeField] GameObject currentButton;

    [SerializeField] GameObject actorActif;

    [SerializeField]
    GameObject aquiletour;
    [SerializeField]
    GameObject faitesunchoix;
    [SerializeField]
    GameObject papoteravec;

    [SerializeField]
    bool[] characterHasPlayed;
    [SerializeField]
    bool isAnActionButton = false;

    #endregion
    private void Awake()
    {
        SetDataTM();
    }
    // Start is called before the first frame update
    void Start()
    {
        CharactersDataGet();
        faitesunchoix.SetActive(false);
        papoteravec.SetActive(false);
        #region HideUI
        Es = FindObjectOfType<EventSystem>();
        foreach (GameObject button in actions)
        {
            if (button != null)
                button.SetActive(false);
        }
        foreach (GameObject papot in PapotageChoiceButtons)
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
        currentButton = Es.currentSelectedGameObject;
        for (int i = 0; i < charactersButton.Length; i++)
        {
            if (currentButton == charactersButton[i])
            {
                actorActif = characters[i];
            }
        }
       
        AffichageMiniCartePerso();
        AffichageCarteCompletePerso();
    }
    public void AffichageMiniCartePerso()
    {
        for (int i = 0; i < charactersLittleResume.Length-1; i++)
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
        for (int i = 0; i < charactersCompleteResume1.Length-1; i++)
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
        faitesunchoix.SetActive(true);
        aquiletour.SetActive(false);
        isAnActionButton = true;
        for (int i = 0; i < characters.Count; i++)
        {
            if (currentButton == charactersButton[i])
            {
                characterHasPlayed[i] = true;
            }

        }
        for (int i = 0; i < actions.Length; i++)
        {
            actions[i].SetActive(true);
            charactersButton[i].GetComponent<Button>().enabled = false;
        }
        Es.SetSelectedGameObject(actions[0]);
        updateButton();
    }

    //active les boutons de choix de persos
    public void ActivateCharactersButton()
    {
        aquiletour.SetActive(true);
        faitesunchoix.SetActive(false);
        isAnActionButton = false;
        for (int i = 0; i < actions.Length; i++)
        {
            actions[i].SetActive(false);
            if (characterHasPlayed[i] == false)
            {
                charactersButton[i].GetComponent<Button>().enabled = true;
                Es.SetSelectedGameObject(charactersButton[i]);
            }
            if (characterHasPlayed[0] == true && characterHasPlayed[1] == true && characterHasPlayed[2] == true)
            {
                ChallengeButton.SetActive(true);
                Es.SetSelectedGameObject(ChallengeButton);
                for (int y = 0; y < charactersButton.Length; y++)
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
       
       foreach(var c in GameManager.instance.GetTeam())
        {
            Debug.Log(c);
            characters.Add(c.gameObject);
        }
        background.GetComponent<SpriteRenderer>().sprite = TM.TMbackground;
        for(int i=0;i<characters.Count;i++)
        {
            Instantiate(characters[i], listCharactersPositions[i],listCharactersPositions[i]);
            actorActif = characters[0];
        }       
    }
    public void CharactersDataGet()
    {
        for(int i=0;i<characters.Count;i++)
        {
            characters[i].GetComponent<Image>().sprite = characters[i].GetComponent<C_Actor>().GetDataActor().MapTmSprite;
            characters[i].GetComponent<C_Actor>().GetCurrentPointTrait().Equals(characters[i].GetComponent<C_Actor>().GetDataActor().currentPointTrait);
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
        papoteravec.SetActive(true);
        faitesunchoix.SetActive(false);
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
        papoteravec.SetActive(false);
        for (int i=0;i<PapotageChoiceButtons.Length;i++)
        {
            if(currentButton==PapotageChoiceButtons[i])
            {
                actorActif.GetComponent<C_Actor>().SetCurrentPointTrait();
                Debug.Log(actorActif.GetComponent<C_Actor>().GetCurrentPointTrait());
                characters[i].GetComponent<C_Actor>().SetCurrentPointTrait();
                Debug.Log(characters[i].GetComponent<C_Actor>().GetCurrentPointTrait());
                if (actorActif.GetComponent<C_Actor>().GetCurrentPointTrait()==2)
                {
                    actorActif.GetComponent<C_Actor>().GetCurrentPointTrait().Equals(0);
                    actorActif.GetComponent<C_Actor>().GiveNewTrait();
                    Debug.Log(actorActif.GetComponent<C_Actor>().GetDataActor().idTraitEnCours);

                }
                if (characters[i].GetComponent<C_Actor>().GetCurrentPointTrait() == 2)
                {
                    characters[i].GetComponent<C_Actor>().GetCurrentPointTrait().Equals(0);
                    characters[i].GetComponent<C_Actor>().GiveNewTrait();
                    Debug.Log(actorActif.GetComponent<C_Actor>().GetDataActor().idTraitEnCours);
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
        actorActif.GetComponent<C_Actor>().GetDataActor().energyMax+=1;
        //actorActif.GetComponent<C_Actor>().maxEnergy+=1;
        ActivateCharactersButton();
    }
    public void Revasser()
    {
        //calm
        actorActif.GetComponent<C_Actor>().GetDataActor().stressMax++;
        //actorActif.GetComponent<C_Actor>().maxStress++;
        ActivateCharactersButton();
    }
    public void Challenge()
    {
        SceneManager.LoadScene("S_DestinationTest");
    }
    public void SetDataTM()
    {
        return;
    }
}
