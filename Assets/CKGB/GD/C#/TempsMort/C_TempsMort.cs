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
using Febucci.UI;

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
    [SerializeField] GameObject tree;
    [SerializeField] Animator[] charactersTrees;
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
    public List<GameObject> characters = new List<GameObject>();
    [SerializeField] public SO_TempsMort TM;
    [SerializeField]
    public GameObject background;

    [Header("Eventsystem and Selected Gameobjects")]
    [SerializeField] EventSystem Es;
    [SerializeField] GameObject currentButton;
    [SerializeField] GameObject selectedActionButton;

    [SerializeField] public GameObject actorActif;
    [SerializeField] public GameObject  Papoteur;
    [SerializeField] bool MorganAPapoteAvecEsthela = false;
    [SerializeField] bool MorganAPapoteAvecNimu = false;
    [SerializeField] bool NimuAPapoteAvecEsthela = false;


    [SerializeField]
    GameObject faitesunchoix;
    [SerializeField]
    GameObject papoteravec;
    [SerializeField]
    bool[] ApapoteAvec;

    [SerializeField]
    bool[] characterHasPlayed;
    [SerializeField]
    bool isAnActionButton = false;

    [Header("Histoires")]
    [SerializeField] public GameObject PageNarrateur;
    [SerializeField] public GameObject Cine;
    [SerializeField]TextAsset inkAssetIntro;
    [SerializeField] TextAsset _intro;
    [SerializeField] TextAsset _outro;
    [SerializeField] public TextAsset Observage;
    [SerializeField] public TMP_Text naratteur;
    [SerializeField] bool TMhasStarted = false;
    [SerializeField] public bool canContinue=false;


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
      
    }
    // Start is called before the first frame update
    void Start()
    {

        GameManager.instance.TM = this;
        initiateTMvariables();
        CharactersDataGet();
        GameManager.instance.ChangeActionMap("TempsMort");
        GameManager.instance.textToWriteIn = naratteur;
       
        StartCoroutine(StartIntro());


        faitesunchoix.SetActive(false);
        papoteravec.SetActive(false);
        tree.SetActive(false);
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
        foreach (GameObject fiche in charactersLittleResume)
        {
            if (fiche != null)
                fiche.SetActive(false);
        }
        foreach (GameObject button in charactersButton)
        {
            if (button != null)
                button.SetActive(false);
        }

        ChallengeButton.SetActive(false);
        #endregion

       
       
    

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void updateButton()
    {
        if (currentButton.transform.GetChild(0) != null)
        {
            currentButton.transform.GetChild(0).gameObject.SetActive(false);
        }
        currentButton = Es.currentSelectedGameObject;
        if (currentButton.transform.GetChild(0) != null)
        {
            currentButton.transform.GetChild(0).gameObject.SetActive(true);
        }
        for (int i = 0; i < charactersButton.Length; i++)
        {
            if(currentButton!=charactersButton[i])
            {
                charactersTrees[i].GetComponent<Animator>().SetBool("IsHover", false);
            }
            if (currentButton == charactersButton[i])
            {
                actorActif = characters[i];
                charactersTrees[i].GetComponent<Animator>().SetBool("IsHover", true);
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
            else if (actorActif == characters[i] && isAnActionButton == true&&GameManager.instance.isDialoguing==false)
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
        faitesunchoix.SetActive(false);
        isAnActionButton = false;
        for (int i = 0; i < actions.Length; i++)
        {
            actions[i].SetActive(false);
            if(charactersButton[i].activeSelf==false)
            {
                charactersButton[i].SetActive(true);
            }
            charactersButton[i].GetComponent<Button>().enabled = true;
            Es.SetSelectedGameObject(charactersButton[i]);       
            if (characterHasPlayed[0] == true && characterHasPlayed[1] == true && characterHasPlayed[2] == true)
            {
                
                ChallengeButton.SetActive(true);
                Es.SetSelectedGameObject(ChallengeButton);
                actorActif = null;
                AffichageMiniCartePerso();
                updateButton();
            }

        }
        tree.SetActive(true);
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
                            //INUTILE !
                            //thisActor.GetComponent<C_Actor>().SetPosition(position.position);

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
        _intro = TM.intro;
        _outro = TM.Outro;
        Observage = TM.Observer;
       
        
        for (int i=0;i<characters.Count; i++)
        {
            characters[i].transform.GetChild(0).GetComponentInChildren<TextAnimatorPlayer>().onTypewriterStart.AddListener(() => SetCanContinueToNo());
            characters[i].transform.GetChild(1).GetComponentInChildren<TextAnimatorPlayer>().onTypewriterStart.AddListener(() => SetCanContinueToNo());
            characters[i].transform.GetChild(3).GetComponentInChildren<TextAnimatorPlayer>().onTypewriterStart.AddListener(() => SetCanContinueToNo());
            characters[i].transform.GetChild(4).GetComponentInChildren<TextAnimatorPlayer>().onTypewriterStart.AddListener(() => SetCanContinueToNo());

            characters[i].transform.GetChild(0).GetComponentInChildren<TextAnimatorPlayer>().onTextShowed.AddListener(() => SetCanContinueToYes());
            characters[i].transform.GetChild(1).GetComponentInChildren<TextAnimatorPlayer>().onTextShowed.AddListener(() => SetCanContinueToYes());
            characters[i].transform.GetChild(3).GetComponentInChildren<TextAnimatorPlayer>().onTextShowed.AddListener(() => SetCanContinueToYes());
            characters[i].transform.GetChild(4).GetComponentInChildren<TextAnimatorPlayer>().onTextShowed.AddListener(() => SetCanContinueToYes());
        }
        naratteur.GetComponent<TextAnimatorPlayer>().onTextShowed.AddListener(() => SetCanContinueToYes());
        naratteur.GetComponent<TextAnimatorPlayer>().onTypewriterStart.AddListener(() => SetCanContinueToNo());
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
            actor.GetComponent<C_Actor>().BigResume2.transform.GetChild(2).GetComponent<TMP_Text>().text += "\n" + actor.GetComponent<C_Actor>().GetDataActor().listNewTraits[actor.GetComponent<C_Actor>().GetDataActor().idTraitEnCours-1].buttonText;
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

        if (context.performed&&TMhasStarted)
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
        tree.SetActive(true);
        papoteravec.SetActive(true);
        faitesunchoix.SetActive(false);
        isAnActionButton = false;
        for (int i = 0; i < PapotageChoiceButtons.Length; i++)
        {
                PapotageChoiceButtons[i].SetActive(true);
            charactersButton[i].SetActive(false);
                Es.SetSelectedGameObject(PapotageChoiceButtons[i]); 
            actions[i].SetActive(false);
        }
        updateButton();
        //traitpoint
        
    }

    //papotage + stats
    public void PapotageFin()
    {
        tree.SetActive(false);
        for (int i=0;i<PapotageChoiceButtons.Length;i++)
        { 
           if(currentButton==PapotageChoiceButtons[i])
            {
                Papoteur = characters[i];
            }
            if (actorActif == characters[0] && Papoteur == characters[1] && MorganAPapoteAvecEsthela == false)
            {
                foreach (GameObject papot in PapotageChoiceButtons)
                {
                    papot.SetActive(false);
                }
                foreach (GameObject c in charactersButton)
                {
                    c.SetActive(false);
                }
                papoteravec.SetActive(false);
                if (currentButton == PapotageChoiceButtons[i] && currentButton != actorActif)
                {
                    Papoteur = characters[i];
                    actorActif.GetComponent<C_Actor>().SetCurrentPointTrait();
                    Debug.Log("Le papoteur possède : " + actorActif.GetComponent<C_Actor>().GetCurrentPointTrait() + " pts de traits");
                    characters[i].GetComponent<C_Actor>().SetCurrentPointTrait();
                    Debug.Log("Le papoté possède : " + characters[i].GetComponent<C_Actor>().GetCurrentPointTrait() + " pts de traits");
                    UpdateCharacterStat();
                    if (actorActif.GetComponent<C_Actor>().GetCurrentPointTrait() == 2)
                    {
                        actorActif.GetComponent<C_Actor>().ResetPointTrait();
                        actorActif.GetComponent<C_Actor>().UpdateNextTrait();
                        UpdateCharacterStat();
                        AddNewTraitToFiche(actorActif);
                        Debug.Log("Trait de " + actorActif.name + " numéro " + actorActif.GetComponent<C_Actor>().GetDataActor().idTraitEnCours);
                    }
                    if (characters[i].GetComponent<C_Actor>().GetCurrentPointTrait() == 2)
                    {
                        characters[i].GetComponent<C_Actor>().ResetPointTrait();
                        characters[i].GetComponent<C_Actor>().UpdateNextTrait();
                        UpdateCharacterStat();
                        AddNewTraitToFiche(characters[i]);
                        Debug.Log("Trait de " + characters[i].name + " numéro " + characters[i].GetComponent<C_Actor>().GetDataActor().idTraitEnCours);
                    }
                }
                GameManager.instance.PapoterID[0]++;
                Cine.GetComponent<Animator>().SetBool("IsCinema", true);
                GameManager.instance.EnterDialogueMode(GameManager.instance.papotage[0]);
                MorganAPapoteAvecEsthela = true;
            }
            else if (actorActif == characters[0] && Papoteur == characters[2] && MorganAPapoteAvecNimu == false)
            {
                foreach (GameObject papot in PapotageChoiceButtons)
                {
                    papot.SetActive(false);
                }
                foreach (GameObject c in charactersButton)
                {
                    c.SetActive(false);
                }
                papoteravec.SetActive(false);
                if (currentButton == PapotageChoiceButtons[i] && currentButton != actorActif)
                {
                    Papoteur = characters[i];
                    actorActif.GetComponent<C_Actor>().SetCurrentPointTrait();
                    Debug.Log("Le papoteur possède : " + actorActif.GetComponent<C_Actor>().GetCurrentPointTrait() + " pts de traits");
                    characters[i].GetComponent<C_Actor>().SetCurrentPointTrait();
                    Debug.Log("Le papoté possède : " + characters[i].GetComponent<C_Actor>().GetCurrentPointTrait() + " pts de traits");
                    UpdateCharacterStat();
                    if (actorActif.GetComponent<C_Actor>().GetCurrentPointTrait() == 2)
                    {
                        actorActif.GetComponent<C_Actor>().ResetPointTrait();
                        actorActif.GetComponent<C_Actor>().UpdateNextTrait();
                        UpdateCharacterStat();
                        AddNewTraitToFiche(actorActif);
                        Debug.Log("Trait de " + actorActif.name + " numéro " + actorActif.GetComponent<C_Actor>().GetDataActor().idTraitEnCours);
                    }
                    if (characters[i].GetComponent<C_Actor>().GetCurrentPointTrait() == 2)
                    {
                        characters[i].GetComponent<C_Actor>().ResetPointTrait();
                        characters[i].GetComponent<C_Actor>().UpdateNextTrait();
                        UpdateCharacterStat();
                        AddNewTraitToFiche(characters[i]);
                        Debug.Log("Trait de " + characters[i].name + " numéro " + characters[i].GetComponent<C_Actor>().GetDataActor().idTraitEnCours);
                    }
                }
                GameManager.instance.PapoterID[1]++;
                Cine.GetComponent<Animator>().SetBool("IsCinema", true);
                GameManager.instance.EnterDialogueMode(GameManager.instance.papotage[1]);
                MorganAPapoteAvecNimu = true;
            }
            else if (actorActif == characters[1] && Papoteur == characters[0] && MorganAPapoteAvecEsthela == false)
            {
                foreach (GameObject papot in PapotageChoiceButtons)
                {
                    papot.SetActive(false);
                }
                foreach (GameObject c in charactersButton)
                {
                    c.SetActive(false);
                }
                papoteravec.SetActive(false);
                if (currentButton == PapotageChoiceButtons[i] && currentButton != actorActif)
                {
                    Papoteur = characters[i];
                    actorActif.GetComponent<C_Actor>().SetCurrentPointTrait();
                    Debug.Log("Le papoteur possède : " + actorActif.GetComponent<C_Actor>().GetCurrentPointTrait() + " pts de traits");
                    characters[i].GetComponent<C_Actor>().SetCurrentPointTrait();
                    Debug.Log("Le papoté possède : " + characters[i].GetComponent<C_Actor>().GetCurrentPointTrait() + " pts de traits");
                    UpdateCharacterStat();
                    if (actorActif.GetComponent<C_Actor>().GetCurrentPointTrait() == 2)
                    {
                        actorActif.GetComponent<C_Actor>().ResetPointTrait();
                        actorActif.GetComponent<C_Actor>().UpdateNextTrait();
                        UpdateCharacterStat();
                        AddNewTraitToFiche(actorActif);
                        Debug.Log("Trait de " + actorActif.name + " numéro " + actorActif.GetComponent<C_Actor>().GetDataActor().idTraitEnCours);
                    }
                    if (characters[i].GetComponent<C_Actor>().GetCurrentPointTrait() == 2)
                    {
                        characters[i].GetComponent<C_Actor>().ResetPointTrait();
                        characters[i].GetComponent<C_Actor>().UpdateNextTrait();
                        UpdateCharacterStat();
                        AddNewTraitToFiche(characters[i]);
                        Debug.Log("Trait de " + characters[i].name + " numéro " + characters[i].GetComponent<C_Actor>().GetDataActor().idTraitEnCours);
                    }
                }
                GameManager.instance.PapoterID[0]++;
                Cine.GetComponent<Animator>().SetBool("IsCinema", true);
                GameManager.instance.EnterDialogueMode(GameManager.instance.papotage[0]);
                MorganAPapoteAvecEsthela = true;
            }
            else if (actorActif == characters[1] && Papoteur == characters[2] && NimuAPapoteAvecEsthela == false)
            {
                foreach (GameObject papot in PapotageChoiceButtons)
                {
                    papot.SetActive(false);
                }
                foreach (GameObject c in charactersButton)
                {
                    c.SetActive(false);
                }
                papoteravec.SetActive(false);
                if (currentButton == PapotageChoiceButtons[i] && currentButton != actorActif)
                {
                    Papoteur = characters[i];
                    actorActif.GetComponent<C_Actor>().SetCurrentPointTrait();
                    Debug.Log("Le papoteur possède : " + actorActif.GetComponent<C_Actor>().GetCurrentPointTrait() + " pts de traits");
                    characters[i].GetComponent<C_Actor>().SetCurrentPointTrait();
                    Debug.Log("Le papoté possède : " + characters[i].GetComponent<C_Actor>().GetCurrentPointTrait() + " pts de traits");
                    UpdateCharacterStat();
                    if (actorActif.GetComponent<C_Actor>().GetCurrentPointTrait() == 2)
                    {
                        actorActif.GetComponent<C_Actor>().ResetPointTrait();
                        actorActif.GetComponent<C_Actor>().UpdateNextTrait();
                        UpdateCharacterStat();
                        AddNewTraitToFiche(actorActif);
                        Debug.Log("Trait de " + actorActif.name + " numéro " + actorActif.GetComponent<C_Actor>().GetDataActor().idTraitEnCours);
                    }
                    if (characters[i].GetComponent<C_Actor>().GetCurrentPointTrait() == 2)
                    {
                        characters[i].GetComponent<C_Actor>().ResetPointTrait();
                        characters[i].GetComponent<C_Actor>().UpdateNextTrait();
                        UpdateCharacterStat();
                        AddNewTraitToFiche(characters[i]);
                        Debug.Log("Trait de " + characters[i].name + " numéro " + characters[i].GetComponent<C_Actor>().GetDataActor().idTraitEnCours);
                    }
                }
                GameManager.instance.PapoterID[2]++;
                Cine.GetComponent<Animator>().SetBool("IsCinema", true);
                GameManager.instance.EnterDialogueMode(GameManager.instance.papotage[2]);
                NimuAPapoteAvecEsthela = true;
            }
            else if (actorActif == characters[2] && Papoteur == characters[0] && MorganAPapoteAvecNimu == false)
            {
                foreach (GameObject papot in PapotageChoiceButtons)
                {
                    papot.SetActive(false);
                }
                foreach (GameObject c in charactersButton)
                {
                    c.SetActive(false);
                }
                papoteravec.SetActive(false);
                if (currentButton == PapotageChoiceButtons[i] && currentButton != actorActif)
                {
                    Papoteur = characters[i];
                    actorActif.GetComponent<C_Actor>().SetCurrentPointTrait();
                    Debug.Log("Le papoteur possède : " + actorActif.GetComponent<C_Actor>().GetCurrentPointTrait() + " pts de traits");
                    characters[i].GetComponent<C_Actor>().SetCurrentPointTrait();
                    Debug.Log("Le papoté possède : " + characters[i].GetComponent<C_Actor>().GetCurrentPointTrait() + " pts de traits");
                    UpdateCharacterStat();
                    if (actorActif.GetComponent<C_Actor>().GetCurrentPointTrait() == 2)
                    {
                        actorActif.GetComponent<C_Actor>().ResetPointTrait();
                        actorActif.GetComponent<C_Actor>().UpdateNextTrait();
                        UpdateCharacterStat();
                        AddNewTraitToFiche(actorActif);
                        Debug.Log("Trait de " + actorActif.name + " numéro " + actorActif.GetComponent<C_Actor>().GetDataActor().idTraitEnCours);
                    }
                    if (characters[i].GetComponent<C_Actor>().GetCurrentPointTrait() == 2)
                    {
                        characters[i].GetComponent<C_Actor>().ResetPointTrait();
                        characters[i].GetComponent<C_Actor>().UpdateNextTrait();
                        UpdateCharacterStat();
                        AddNewTraitToFiche(characters[i]);
                        Debug.Log("Trait de " + characters[i].name + " numéro " + characters[i].GetComponent<C_Actor>().GetDataActor().idTraitEnCours);
                    }
                }
                GameManager.instance.PapoterID[1]++;
                Debug.Log("Nimu papot avec Morgan valeur : " + GameManager.instance.PapoterID[1]);
                Cine.GetComponent<Animator>().SetBool("IsCinema", true);
                GameManager.instance.EnterDialogueMode(GameManager.instance.papotage[1]);
                MorganAPapoteAvecNimu = true;
            }
            else if (actorActif == characters[2] && Papoteur == characters[1] && NimuAPapoteAvecEsthela == false)
            {
                foreach (GameObject papot in PapotageChoiceButtons)
                {
                    papot.SetActive(false);
                }
                foreach (GameObject c in charactersButton)
                {
                    c.SetActive(false);
                }
                papoteravec.SetActive(false);
                if (currentButton == PapotageChoiceButtons[i] && currentButton != actorActif)
                {
                    Papoteur = characters[i];
                    actorActif.GetComponent<C_Actor>().SetCurrentPointTrait();
                    Debug.Log("Le papoteur possède : " + actorActif.GetComponent<C_Actor>().GetCurrentPointTrait() + " pts de traits");
                    characters[i].GetComponent<C_Actor>().SetCurrentPointTrait();
                    Debug.Log("Le papoté possède : " + characters[i].GetComponent<C_Actor>().GetCurrentPointTrait() + " pts de traits");
                    UpdateCharacterStat();
                    if (actorActif.GetComponent<C_Actor>().GetCurrentPointTrait() == 2)
                    {
                        actorActif.GetComponent<C_Actor>().ResetPointTrait();
                        actorActif.GetComponent<C_Actor>().UpdateNextTrait();
                        UpdateCharacterStat();
                        AddNewTraitToFiche(actorActif);
                        Debug.Log("Trait de " + actorActif.name + " numéro " + actorActif.GetComponent<C_Actor>().GetDataActor().idTraitEnCours);
                    }
                    if (characters[i].GetComponent<C_Actor>().GetCurrentPointTrait() == 2)
                    {
                        characters[i].GetComponent<C_Actor>().ResetPointTrait();
                        characters[i].GetComponent<C_Actor>().UpdateNextTrait();
                        UpdateCharacterStat();
                        AddNewTraitToFiche(characters[i]);
                        Debug.Log("Trait de " + characters[i].name + " numéro " + characters[i].GetComponent<C_Actor>().GetDataActor().idTraitEnCours);
                    }
                }
                GameManager.instance.PapoterID[2]++;
                Cine.GetComponent<Animator>().SetBool("IsCinema", true);
                GameManager.instance.EnterDialogueMode(GameManager.instance.papotage[2]);
                NimuAPapoteAvecEsthela = true;
            }
        }  
        /* ActivateCharactersButton();
         UpdateCharacterStat();*/
    }
    public void RetourAuTMAfterPapotage(string name)
    {
        StartCoroutine(ReprendreTMAfterPapotage());
        
    }
    IEnumerator ReprendreTMAfterPapotage()
    {
        yield return new WaitForSeconds(0.6f);
        Cine.GetComponent<Animator>().SetBool("IsCinema", false);
        GameManager.instance.ExitDialogueMode();
        for (int i = 0; i < characters.Count; i++)
        {
            if (actorActif == characters[i])
            {
                characterHasPlayed[i] = true;
            }
        }
        Debug.Log("Retour TM apres papotage");
        ActivateCharactersButton();
        UpdateCharacterStat();
    }
    public void Respirer()
    {
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
        foreach (GameObject fiche in charactersLittleResume)
        {
            if (fiche != null)
                fiche.SetActive(false);
        }
        foreach (GameObject button in charactersButton)
        {
            if (button != null)
                button.SetActive(false);
        }

        ChallengeButton.SetActive(false);
        #endregion
        for (int y = 0; y < characters.Count; y++)
        {
            if (actorActif == characters[y])
            {
                GameManager.instance.RespirerID++;
            }

        }
        tree.SetActive(false);
        Cine.GetComponent<Animator>().SetBool("IsCinema", true);
        GameManager.instance.EnterDialogueMode(Observage);
    }
    public void RetourAuTMAfterRespirer(string text)
    {
        StartCoroutine(CoroutineRetourAuTMAfterRespirer());
    
    }
    IEnumerator CoroutineRetourAuTMAfterRespirer()
    {
        yield return new WaitForSeconds(0.6f);
        Cine.GetComponent<Animator>().SetBool("IsCinema", false);
        GameManager.instance.ExitDialogueMode();
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
        foreach (GameObject fiche in charactersLittleResume)
        {
            if (fiche != null)
                fiche.SetActive(false);
        }
        foreach (GameObject button in charactersButton)
        {
            if (button != null)
                button.SetActive(false);
        }

        ChallengeButton.SetActive(false);
        #endregion
        for(int y=0;y<characters.Count;y++)
        {
            if(actorActif==characters[y])
            {
                GameManager.instance.RevasserID[y]++;
            }
        
        }
        tree.SetActive(false);
        Cine.GetComponent<Animator>().SetBool("IsCinema", true);
        GameManager.instance.EnterDialogueMode(actorActif.GetComponent<C_Actor>().GetDataActor().Revasser);
    }
    public void RetourAuTMAfterRevasser(string text)
    {
        StartCoroutine(CoroutineRetourAuTMAfterRevasser());
    }
    IEnumerator CoroutineRetourAuTMAfterRevasser()
    {
        yield return new WaitForSeconds(0.6f);
        Cine.GetComponent<Animator>().SetBool("IsCinema", false);
        GameManager.instance.ExitDialogueMode();
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
        for(int i=0;i<charactersButton.Length;i++)
        {
            charactersButton[i].SetActive(false);
            ChallengeButton.SetActive(false);
        }
        updateButton();
        StartOutro();
        //SceneManager.LoadScene("S_DestinationTest");
    }
    public void BACK(InputAction.CallbackContext context)
    {
        if(!context.performed)
        { return; }
        if(context.performed&&GameManager.instance.isDialoguing==false)
        {
                Debug.Log("going backward");
                if (isAnActionButton == true)
                {
                    for (int i = 0; i < actions.Length; i++)
                    {
                        if (currentButton == actions[i])
                        {
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
    //Fonction qui lance une histoire et preciser en argument le texte a lire de type Text_Asset

    IEnumerator StartIntro()
    {
        Cine.GetComponent<Animator>().SetBool("IsCinema", true);
        yield return new WaitForSeconds(0.8f);
        GameManager.instance.EnterDialogueMode(_intro);
    }
    public void StartTempsMort(string name)
    {
        StartCoroutine(TempsMortUnleashed());
    }
    IEnumerator TempsMortUnleashed()
    {
  
        yield return new WaitForSeconds(0.6f);
        tree.SetActive(true);
        TMhasStarted = true;
        Es = FindObjectOfType<EventSystem>();
        Cine.GetComponent<Animator>().SetBool("IsCinema", false);
        GameManager.instance.ExitDialogueMode();
        foreach (GameObject button in charactersButton)
        {
            if (button != null)
                button.SetActive(true);
        }
        Es.SetSelectedGameObject(Es.firstSelectedGameObject);
        currentButton = Es.currentSelectedGameObject;

        updateButton();
     
    }
    public void StartOutro()
    {
        Cine.GetComponent<Animator>().SetBool("IsCinema", true);
        GameManager.instance.EnterDialogueMode(_outro);

    }
    //fonction qui verifie si tu peut continuer l'histoire et la continue ou l'arrete
    public void continueStory(InputAction.CallbackContext context)
    {

        if (context.performed&&GameManager.instance.isDialoguing==true&&canContinue==true)
        {
            GameManager.instance.ContinueStory();
        }
        else if(context.performed && GameManager.instance.isDialoguing == true && canContinue == false)
        {
            GameManager.instance.textToWriteIn.GetComponent<TextAnimatorPlayer>().SkipTypewriter();
        }
            return;

    }
    public void SetCanContinueToYes()
    {
        canContinue = true;
    }
    public void SetCanContinueToNo()
    {
        canContinue = false;
    }
    public void GoChallenge(string named)
    {
        foreach(GameObject c in characters)
        {
            c.transform.parent = GameManager.instance.transform;
        }
        SceneManager.LoadScene(named);
    }
    public SO_TempsMort GetDataTM()
    {
        return TM;
    }
}
