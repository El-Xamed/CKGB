using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static SO_TempsMort;
using Ink.Runtime;

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
    [SerializeField] GameObject selectedActionButton;

    [SerializeField] GameObject actorActif;
    [SerializeField] GameObject Papoteur;

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

    [Header("Histoires")]
    [SerializeField]TextAsset inkAssetIntro;
    [SerializeField] Story _intro;
    [SerializeField] Story _outro;
    [SerializeField] TMP_Text naratteur;

    #endregion
    #region variables de retour en arrière
    [SerializeField] List<GameObject> ActionToAdd = new List<GameObject>();
    [SerializeField] List<GameObject> LastCharacterThatPlayed = new List<GameObject>();
    public int actiontoaddID=-1;
    public int charactertoaddID = -1;
    [SerializeField] bool nobodyHasPlayed = true;


    #endregion

    private void Awake()
    {  
       // _intro = new Story(inkAssetIntro.text);
    }
    // Start is called before the first frame update
    void Start()
    {
        Es = FindObjectOfType<EventSystem>();
        Es.SetSelectedGameObject(Es.firstSelectedGameObject);
        currentButton = Es.currentSelectedGameObject;
        faitesunchoix.SetActive(false);
        papoteravec.SetActive(false);
        #region HideUI
       
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
        CharactersDataGet();
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
            if(currentButton == actions[i])
            {
                selectedActionButton = currentButton;
            }


        }
        if (characterHasPlayed[0] == false && characterHasPlayed[1] == false && characterHasPlayed[2] == false)
        {
            nobodyHasPlayed = true;
        }
        else
            nobodyHasPlayed = false;
       
        AffichageMiniCartePerso();
        AffichageCarteCompletePerso();
    }
    public void AffichageMiniCartePerso()
    {
        for (int i = 0; i < charactersLittleResume.Length; i++)
        {
            if (characterHasPlayed[0] == true && characterHasPlayed[1] == true && characterHasPlayed[2] == true &&currentButton == charactersButton[i])
            {
                charactersLittleResume[i].SetActive(false);
                charactersCompleteResume1[i].SetActive(true);
                charactersCompleteResume2[i].SetActive(false);
            }
            else if (currentButton == charactersButton[i])
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
            if (characterHasPlayed[0] == true && characterHasPlayed[1] == true && characterHasPlayed[2] == true && currentButton == charactersButton[i])
            {
                charactersLittleResume[i].SetActive(false);
                charactersCompleteResume1[i].SetActive(true);
                charactersCompleteResume2[i].SetActive(false);
            }
            else if (actorActif == characters[i] && isAnActionButton == true)
            {
                charactersCompleteResume1[i].SetActive(true);
                charactersLittleResume[i].SetActive(false);
            }
            else
                charactersCompleteResume1[i].SetActive(false);
        }
    }

    //active les boutons de choix d'actions
    public void ActivateActionsButtons()
    {
            

        for (int i = 0; i < characters.Count; i++)
        {
            if (currentButton == charactersButton[i]&& actorActif == characters[i] &&characterHasPlayed[i] == false) //cas normal de choix de personnage
            {
                LastCharacterThatPlayed.Add(actorActif);
                charactertoaddID++;
                faitesunchoix.SetActive(true);
                faitesunchoix.GetComponent<TMP_Text>().text = "Que doit faire " + actorActif.GetComponent<C_Actor>().name + " ?";
                aquiletour.SetActive(false);
                isAnActionButton = true;
                for (int x = 0; x < actions.Length; x++)
                {
                    actions[x].SetActive(true);
                    charactersButton[x].GetComponent<Button>().enabled = false;
                }
                Es.SetSelectedGameObject(actions[0]);
                updateButton();
            }

     

        }
    
    }

    //active les boutons de choix de persos
    public void ActivateCharactersButton()
    {
        ActionToAdd.Add(selectedActionButton);
        actiontoaddID++;
        aquiletour.SetActive(true);
        faitesunchoix.SetActive(false);
        isAnActionButton = false;
        for (int i = 0; i < actions.Length; i++)
        {
            actions[i].SetActive(false);
            charactersButton[i].GetComponent<Button>().enabled = true;
            Es.SetSelectedGameObject(charactersButton[i]);       
            if (characterHasPlayed[0] == true && characterHasPlayed[1] == true && characterHasPlayed[2] == true)
            {
                
                ChallengeButton.SetActive(true);
                aquiletour.SetActive(false);
                Es.SetSelectedGameObject(ChallengeButton);
                actorActif = null;
                AffichageMiniCartePerso();
                updateButton();
            }

        }
        updateButton();
       
    }

    //recupere les stats des actors et du so_tempsmort
    public void initiateTMvariables()
    {
        if (GameManager.instance)
        {
            foreach (InitialActorPosition position in TM.startPos)
            {
                //Récupère les info du GameManager
                foreach (var thisActor in GameManager.instance.GetTeam())
                {
                    if(thisActor.GetComponent<C_Actor>()!=null)
                    {
                        //Check si dans les info du challenge est dans l'équipe stocké dans le GameManager.
                        if (thisActor.GetComponent<C_Actor>().GetDataActor().name == position.perso.GetComponent<C_Actor>().GetDataActor().name)
                        {
                            //Ini data actor.
                            thisActor.GetComponent<C_Actor>().IniTempsMort();

                            //Placement des perso depuis le GameManager
                            //Changement de parent
                            thisActor.transform.parent = listCharactersPositions[position.position].transform;
                            thisActor.transform.position = thisActor.transform.parent.transform.position;
                            thisActor.transform.localScale = new Vector3(0.65f,0.65f,0.65f);
                            //thisActor.transform.localScale = Vector3.one;

                            // Debug.Log(thisActor.GetComponent<Image>().sprite.rect.width);
                            thisActor.GetComponent<C_Actor>().SetPosition(position.position);

                            characters.Add(thisActor);
                            Debug.Log(thisActor);
                        }
                    }
                    
                }
            }
        }
        else
        {
            foreach (var c in GameManager.instance.GetTeam())
            {
                Debug.Log(c);
                characters.Add(c.gameObject);
            }
            /*for(int i=0;i<characters.Count;i++)
       {
           Instantiate(characters[i], listCharactersPositions[i],listCharactersPositions[i]);
           actorActif = characters[0];
       }*/
        }

        background.GetComponent<SpriteRenderer>().sprite = TM.TMbackground;
       
    }
    public void UpdateCharacterStat()
    {
        if(actorActif!=null)
        {
            actorActif.GetComponent<C_Actor>().BigResume1.transform.GetChild(1).GetComponent<TMP_Text>().text = "Energie : " + actorActif.GetComponent<C_Actor>().GetDataActor().energyMax;
            actorActif.GetComponent<C_Actor>().smallResume.transform.GetChild(1).GetComponent<TMP_Text>().text = "Energie : " + actorActif.GetComponent<C_Actor>().GetDataActor().energyMax;
            actorActif.GetComponent<C_Actor>().BigResume1.transform.GetChild(2).GetComponent<TMP_Text>().text = "Calme : " + actorActif.GetComponent<C_Actor>().GetDataActor().stressMax;
            actorActif.GetComponent<C_Actor>().smallResume.transform.GetChild(2).GetComponent<TMP_Text>().text = "Calme : " + actorActif.GetComponent<C_Actor>().GetDataActor().stressMax;
            actorActif.GetComponent<C_Actor>().BigResume2.transform.GetChild(1).GetComponent<TMP_Text>().text = "Points de trait : " + actorActif.GetComponent<C_Actor>().GetCurrentPointTrait();
            if(Papoteur!=null)
            {
                Papoteur.GetComponent<C_Actor>().BigResume2.transform.GetChild(1).GetComponent<TMP_Text>().text = "Points de trait : " + Papoteur.GetComponent<C_Actor>().GetCurrentPointTrait();
                Papoteur.GetComponent<C_Actor>().smallResume.transform.GetChild(4).GetComponent<TMP_Text>().text = "Points de trait : " + Papoteur.GetComponent<C_Actor>().GetCurrentPointTrait();           
            }               
            actorActif.GetComponent<C_Actor>().smallResume.transform.GetChild(4).GetComponent<TMP_Text>().text = "Points de trait : " + actorActif.GetComponent<C_Actor>().GetCurrentPointTrait();
                     
        }        
    }
    public void AddNewTraitToFiche(GameObject actor)
    {
        if(actor.GetComponent<C_Actor>().BigResume2.transform.GetChild(2).GetComponent<TMP_Text>().text!=null)
        {
            actor.GetComponent<C_Actor>().BigResume2.transform.GetChild(2).GetComponent<TMP_Text>().text += "\n" + actor.GetComponent<C_Actor>().GetDataActor().listNewTraits[actor.GetComponent<C_Actor>().GetDataActor().idTraitEnCours].buttonText;
        }
        else
            actor.GetComponent<C_Actor>().BigResume2.transform.GetChild(2).GetComponent<TMP_Text>().text += actor.GetComponent<C_Actor>().GetDataActor().listNewTraits[actor.GetComponent<C_Actor>().GetDataActor().idTraitEnCours].buttonText;

    }
    public void CharactersDataGet()
    {
        for(int i=0;i<characters.Count;i++)
        {
            characters[i].transform.GetChild(2).GetComponent<Image>().sprite = characters[i].GetComponent<C_Actor>().GetDataActor().MapTmSprite;
            characters[i].transform.GetChild(2).transform.GetChild(0).GetComponent<Image>().enabled = false;
            characters[i].transform.GetChild(2).transform.GetChild(1).GetComponent<Image>().enabled = false;
            characters[i].transform.GetChild(2).transform.GetChild(1).transform.GetChild(0).GetComponent<Image>().enabled = false;
            characters[i].transform.GetChild(0).GetComponent<Image>().enabled = false;
            characters[i].transform.GetChild(1).GetComponent<Image>().enabled = false;
            characters[i].transform.GetChild(3).GetComponent<Image>().enabled = false;
            characters[i].transform.GetChild(4).GetComponent<Image>().enabled = false;
            characters[i].GetComponent<C_Actor>().smallResume = charactersLittleResume[i]; charactersLittleResume[i].GetComponent<Image>().sprite = characters[i].GetComponent<C_Actor>().GetDataActor().smallResume;
            characters[i].GetComponent<C_Actor>().BigResume1 = charactersCompleteResume1[i]; charactersCompleteResume1[i].GetComponent<Image>().sprite = characters[i].GetComponent<C_Actor>().GetDataActor().BigResume1;
            characters[i].GetComponent<C_Actor>().BigResume2 = charactersCompleteResume2[i]; charactersCompleteResume2[i].GetComponent<Image>().sprite = characters[i].GetComponent<C_Actor>().GetDataActor().BigResume2;
            //characters[i].GetComponent<C_Actor>().GetCurrentPointTrait().Equals(characters[i].GetComponent<C_Actor>().GetDataActor().currentPointTrait);
            characters[i].GetComponent<C_Actor>().smallResume.transform.GetChild(0).GetComponent<Image>().sprite = characters[i].GetComponent<C_Actor>().GetDataActor().ProfilPhoto;
            characters[i].GetComponent<C_Actor>().BigResume1.transform.GetChild(0).GetComponent<Image>().sprite = characters[i].GetComponent<C_Actor>().GetDataActor().ProfilPhoto;
            characters[i].GetComponent<C_Actor>().BigResume2.transform.GetChild(0).GetComponent<Image>().sprite = characters[i].GetComponent<C_Actor>().GetDataActor().ProfilPhoto;
            characters[i].GetComponent<C_Actor>().smallResume.transform.GetChild(3).GetComponent<TMP_Text>().text = characters[i].GetComponent<C_Actor>().GetDataActor().name;
            characters[i].GetComponent<C_Actor>().BigResume1.transform.GetChild(3).GetComponent<TMP_Text>().text = characters[i].GetComponent<C_Actor>().GetDataActor().name;
            characters[i].GetComponent<C_Actor>().BigResume2.transform.GetChild(3).GetComponent<TMP_Text>().text = characters[i].GetComponent<C_Actor>().GetDataActor().name;
            characters[i].GetComponent<C_Actor>().BigResume1.transform.GetChild(4).GetComponent<TMP_Text>().text = characters[i].GetComponent<C_Actor>().GetDataActor().Description;
            characters[i].GetComponent<C_Actor>().BigResume1.transform.GetChild(1).GetComponent<TMP_Text>().text = "Energie : " + characters[i].GetComponent<C_Actor>().GetDataActor().energyMax;
            characters[i].GetComponent<C_Actor>().smallResume.transform.GetChild(1).GetComponent<TMP_Text>().text = "Energie : " + characters[i].GetComponent<C_Actor>().GetDataActor().energyMax;
            characters[i].GetComponent<C_Actor>().BigResume1.transform.GetChild(2).GetComponent<TMP_Text>().text = "Calme : " + characters[i].GetComponent<C_Actor>().GetDataActor().stressMax;
            characters[i].GetComponent<C_Actor>().smallResume.transform.GetChild(2).GetComponent<TMP_Text>().text = "Calme : " + characters[i].GetComponent<C_Actor>().GetDataActor().stressMax;
            characters[i].GetComponent<C_Actor>().BigResume2.transform.GetChild(1).GetComponent<TMP_Text>().text = "Points de trait : " + characters[i].GetComponent<C_Actor>().GetCurrentPointTrait();
            characters[i].GetComponent<C_Actor>().smallResume.transform.GetChild(4).GetComponent<TMP_Text>().text = "Points de trait : " + characters[i].GetComponent<C_Actor>().GetCurrentPointTrait();
            characters[i].GetComponent<C_Actor>().smallResume.transform.GetChild(5).GetComponent<TMP_Text>().text = characters[i].GetComponent<C_Actor>().GetDataActor().miniDescription;

            for (int y = 0;y < characters[i].GetComponent<C_Actor>().GetDataActor().listNewTraits.Count;y++)
            {
                characters[i].GetComponent<C_Actor>().BigResume2.transform.GetChild(2).GetComponent<TMP_Text>().text = characters[i].GetComponent<C_Actor>().BigResume2.transform.GetChild(2).GetComponent<TMP_Text>().text + "\n" + characters[i].GetComponent<C_Actor>().GetDataActor().listNewTraits[y].buttonText;
            }
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
        isAnActionButton = false;
        for (int i = 0; i < PapotageChoiceButtons.Length; i++)
        {
                PapotageChoiceButtons[i].SetActive(true);
                Es.SetSelectedGameObject(PapotageChoiceButtons[i]); 
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
            if (actorActif == characters[i])
            {
                characterHasPlayed[i] = true;
            }
            if (currentButton==PapotageChoiceButtons[i]&&currentButton!=actorActif)
            {
                Papoteur = characters[i];
                actorActif.GetComponent<C_Actor>().SetCurrentPointTrait();
                Debug.Log("Le papoteur possède : "+actorActif.GetComponent<C_Actor>().GetCurrentPointTrait()+" pts de traits");
                characters[i].GetComponent<C_Actor>().SetCurrentPointTrait();
                Debug.Log("Le papoté possède : " + characters[i].GetComponent<C_Actor>().GetCurrentPointTrait()+" pts de traits");
                UpdateCharacterStat();
                if (actorActif.GetComponent<C_Actor>().GetCurrentPointTrait()==2)
                {
                    actorActif.GetComponent<C_Actor>().ResetPointTrait();
                    actorActif.GetComponent<C_Actor>().UpdateNextTrait();
                    UpdateCharacterStat();
                    AddNewTraitToFiche(actorActif);
                    Debug.Log("Trait de "+ actorActif.name +" numéro "+actorActif.GetComponent<C_Actor>().GetDataActor().idTraitEnCours);

                }
                if (characters[i].GetComponent<C_Actor>().GetCurrentPointTrait() == 2)
                {
                    characters[i].GetComponent<C_Actor>().ResetPointTrait();
                    characters[i].GetComponent<C_Actor>().UpdateNextTrait();
                    UpdateCharacterStat();
                    AddNewTraitToFiche(characters[i]);
                    Debug.Log("Trait de "+ characters[i].name + " numéro " + characters[i].GetComponent<C_Actor>().GetDataActor().idTraitEnCours);
                }
                
            }
      
        }
        foreach(GameObject papot in PapotageChoiceButtons)
        {
            papot.SetActive(false);
        }
        ActivateCharactersButton();
        UpdateCharacterStat();
    }
    public void Respirer()
    {
     
        for (int i = 0; i < characters.Count; i++)
        {
            if (actorActif == characters[i])
            {
                characterHasPlayed[i] = true;
            }
        }
        //energy
        actorActif.GetComponent<C_Actor>().SetMaxEnergy();
        Debug.Log(actorActif.GetComponent<C_Actor>().getMaxEnergy());
        //actorActif.GetComponent<C_Actor>().maxEnergy+=1;
        ActivateCharactersButton();
        UpdateCharacterStat();
        
    }
    public void Revasser()
    {
    
        for (int i = 0; i < characters.Count; i++)
        {
            if (actorActif == characters[i])
            {
                characterHasPlayed[i] = true;
            }
        }
        //calm
        actorActif.GetComponent<C_Actor>().SetMaxStress();
        Debug.Log(actorActif.GetComponent<C_Actor>().getMaxStress());
        //actorActif.GetComponent<C_Actor>().maxStress++;
        ActivateCharactersButton();
        UpdateCharacterStat();
       
  
    }
    public void Challenge()
    {
        SceneManager.LoadScene("S_DestinationTest");
    }
    public void BACK(InputAction.CallbackContext context)
    {
        if(!context.performed)
        { return; }
        if(context.performed)
        {
                Debug.Log("going backward");
                if (isAnActionButton == true)
                {
                    for (int i = 0; i < actions.Length; i++)
                    {
                        if (currentButton == actions[i])
                        {
                        aquiletour.SetActive(true);
                        faitesunchoix.SetActive(false);
                        isAnActionButton = false;
                        for (int y = 0; y < actions.Length; y++)
                        {
                            actions[y].SetActive(false);
                            charactersButton[y].GetComponent<Button>().enabled = true;
                            Es.SetSelectedGameObject(charactersButton[y]);
                            if (characterHasPlayed[0] == true && characterHasPlayed[y] == true && characterHasPlayed[2] == true)
                            {

                                ChallengeButton.SetActive(true);
                                aquiletour.SetActive(false);
                                Es.SetSelectedGameObject(ChallengeButton);
                                actorActif = null;
                                AffichageMiniCartePerso();
                                updateButton();
                            }

                        }
                        updateButton();

                        }
                }
                    LastCharacterThatPlayed.RemoveAt(charactertoaddID);
                    charactertoaddID--;
                }
                else if (PapotageChoiceButtons[0].activeSelf == true)
                {
                    Debug.Log("Pas de papotage pour toi mon petit");
                    for (int i = 0; i < PapotageChoiceButtons.Length; i++)
                    {
                        PapotageChoiceButtons[i].SetActive(false);
                    }
                    faitesunchoix.SetActive(true);
                    faitesunchoix.GetComponent<TMP_Text>().text = "Que doit faire " + LastCharacterThatPlayed[charactertoaddID].GetComponent<C_Actor>().name + " ?";
                    for (int i = 0; i < characters.Count; i++)
                    {
                        if (currentButton == PapotageChoiceButtons[i]) //cas de retour au choix du personnage avec qui papoter
                        {
                            papoteravec.SetActive(false);
                            faitesunchoix.SetActive(true);
                            faitesunchoix.GetComponent<TMP_Text>().text = "Que doit faire " + actorActif.GetComponent<C_Actor>().name + " ?";
                            isAnActionButton = true;
                            for (int x = 0; x < actions.Length; x++)
                            {
                                charactersButton[x].SetActive(true);
                                actions[x].SetActive(true);
                                charactersButton[x].GetComponent<Button>().enabled = false;
                            }
                            Es.SetSelectedGameObject(actions[0]);
                            updateButton();
                        }
                    }

                }
                else if(nobodyHasPlayed == false)
                {
                    if (ActionToAdd[actiontoaddID] != null && ActionToAdd[actiontoaddID] == actions[0] && charactersButton[1].activeSelf == true)
                    {
                        Debug.Log("retour aux personnages après avoir papoté");
                   
                    actorActif = LastCharacterThatPlayed[charactertoaddID];
                    actorActif.GetComponent<C_Actor>().ReducePointTrait();
                    Debug.Log("(retour)Points de trait du papoteur : " + actorActif.GetComponent<C_Actor>().GetCurrentPointTrait());
                    Papoteur.GetComponent<C_Actor>().ReducePointTrait();
                    Debug.Log("(retour)Points de trait du papoté : " + Papoteur.GetComponent<C_Actor>().GetCurrentPointTrait());
                    faitesunchoix.SetActive(true);
                        faitesunchoix.GetComponent<TMP_Text>().text = "Que doit faire " + LastCharacterThatPlayed[LastCharacterThatPlayed.Count - 1].GetComponent<C_Actor>().name + " ?";
                        for (int i = 0; i < characters.Count; i++)
                        {
                            if (characterHasPlayed[i] == true && actorActif == characters[i] && ActionToAdd != null) //cas ou l'on fait retour depuis le choix de personnages
                            {
                                Debug.Log(actorActif);
                                aquiletour.SetActive(false);
                                isAnActionButton = true;
                                characterHasPlayed[i] = false;

                                for (int x = 0; x < actions.Length; x++)
                                {
                                    actions[x].SetActive(true);
                                    charactersButton[x].GetComponent<Button>().enabled = false;
                                }
                                Es.SetSelectedGameObject(ActionToAdd[actiontoaddID]);
                                updateButton();
                            }
                        }
                    ActionToAdd.RemoveAt(actiontoaddID);
                    actiontoaddID--;

                     }
                    else if (ActionToAdd[actiontoaddID] != null && ActionToAdd[actiontoaddID] == actions[1] && charactersButton[1].activeSelf == true)
                    {
                        Debug.Log("retour aux personnages après avoir révassé");
                        actorActif = LastCharacterThatPlayed[charactertoaddID];
                    actorActif.GetComponent<C_Actor>().ReduceMaxStress();
                    Debug.Log(actorActif.GetComponent<C_Actor>().getMaxStress());
                        faitesunchoix.SetActive(true);
                        faitesunchoix.GetComponent<TMP_Text>().text = "Que doit faire " + LastCharacterThatPlayed[charactertoaddID].GetComponent<C_Actor>().name + " ?";
                        for (int i = 0; i < characters.Count; i++)
                        {
                            if (characterHasPlayed[i] == true && actorActif == characters[i] && ActionToAdd != null) //cas ou l'on fait retour depuis le choix de personnages
                            {
                                Debug.Log(actorActif);
                                aquiletour.SetActive(false);
                                isAnActionButton = true;
                                characterHasPlayed[i] = false;

                                for (int x = 0; x < actions.Length; x++)
                                {
                                    actions[x].SetActive(true);
                                    charactersButton[x].GetComponent<Button>().enabled = false;
                                }
                                Es.SetSelectedGameObject(ActionToAdd[actiontoaddID]);
                                updateButton();
                            }
                        }
                    ActionToAdd.RemoveAt(actiontoaddID);
                    actiontoaddID--;
                }
                    else if (ActionToAdd[actiontoaddID] != null && ActionToAdd[actiontoaddID] == actions[2] && charactersButton[1].activeSelf == true)
                    {
                        Debug.Log("retour aux personnages après avoir fait l'autre truc");
                        actorActif = LastCharacterThatPlayed[charactertoaddID];
                    actorActif.GetComponent<C_Actor>().ReduceMaxEnergy();
                    Debug.Log(actorActif.GetComponent<C_Actor>().getMaxEnergy());
                        faitesunchoix.SetActive(true);
                        Debug.Log(faitesunchoix.activeSelf);
                        faitesunchoix.GetComponent<TMP_Text>().text = "Que doit faire " + LastCharacterThatPlayed[charactertoaddID].GetComponent<C_Actor>().name + " ?";
                        for (int i = 0; i < characters.Count; i++)
                        {
                            if (characterHasPlayed[i] == true && actorActif == characters[i] && ActionToAdd != null) //cas ou l'on fait retour depuis le choix de personnages
                            {
                                Debug.Log(actorActif);
                                aquiletour.SetActive(false);
                                isAnActionButton = true;
                                characterHasPlayed[i] = false;

                                for (int x = 0; x < actions.Length; x++)
                                {
                                    actions[x].SetActive(true);
                                    charactersButton[x].GetComponent<Button>().enabled = false;
                                }
                                Es.SetSelectedGameObject(ActionToAdd[actiontoaddID]);
                                updateButton();
                            }
                        }
                    ActionToAdd.RemoveAt(actiontoaddID);
                    actiontoaddID--;
                }

               

                }
            UpdateCharacterStat();
            if (characterHasPlayed[0] == true && characterHasPlayed[1] == true && characterHasPlayed[2] == true)
            {

            }
            else
                ChallengeButton.SetActive(false);
        }
     
    }
    public void StartDialogue()
    {

    }
    public SO_TempsMort GetDataTM()
    {
        return TM;
    }
}
