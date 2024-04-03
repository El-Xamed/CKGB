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
using System;

public class C_TempsLibre : MonoBehaviour
{

    [Header("Personnages")]
    [SerializeField] public GameObject SpawnParent;
    [SerializeField] List<Transform> listCharactersPositions;
    [SerializeField] public GameObject actorActif;
    [SerializeField] public GameObject Papote;
    [SerializeField] public List<GameObject> characters = new List<GameObject>();
    [SerializeField] int characterNB = -1;
    [SerializeField] public GameObject Morgan;
    [SerializeField] public GameObject Esthela;
    [SerializeField] public GameObject Nimu;


    [Header("Character page")]
    [SerializeField] GameObject FichePersoParent;

    [Header("Character Tree")]
    [SerializeField] GameObject TreeParent;

    [Header("Actions")]
    [SerializeField] GameObject ActionsParents;
    [SerializeField] public List<GameObject> actions = new List<GameObject>();
    [SerializeField] GameObject RevasserButton;
    [SerializeField] GameObject ObserverButton;
    [SerializeField] GameObject PapoterButton;
    [SerializeField] GameObject ChallengeButton;

    [SerializeField] bool MorganAPapoteAvecEsthela = false;
    [SerializeField] bool MorganAPapoteAvecNimu = false;
    [SerializeField] bool NimuAPapoteAvecEsthela = false;


    [Header("Histoires")]

    [SerializeField] public GameObject NarrateurParent;
    [SerializeField] public TMP_Text naratteurText;

    [SerializeField] public GameObject Cine;
    [SerializeField] TextAsset _intro;
    [SerializeField] TextAsset _outro;
    [SerializeField] public TextAsset Observage;

    [SerializeField] bool TMhasStarted = false;
    [SerializeField] public bool canContinue = false;


    [Header("Else")]
    [SerializeField] public SO_TempsMort TM;
    [SerializeField] public GameObject background;

    [Header("Eventsystem and Selected Gameobjects")]
    [SerializeField] EventSystem Es;
    [SerializeField] GameObject currentButton;

    #region variables de retour en arrière
    [SerializeField] List<GameObject> LastAction = new List<GameObject>();
    [SerializeField] List<GameObject> LastCharacterThatPlayed = new List<GameObject>();
    public int actiontoaddID = -1;
    public int charactertoaddID = -1;
    [SerializeField] bool nobodyHasPlayed = true;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.TM = this;
        TM = GameManager.instance.currentTM;
        HideUI();
        initiateTMvariables();
        CharactersDataGet();
        if(GameObject.Find("Morgan")!=null)
        {
            Morgan = GameObject.Find("Morgan");
        }
        if (GameObject.Find("Esthela") != null)
        {
            Esthela = GameObject.Find("Esthela");
        }
        if (GameObject.Find("Nimu") != null)
        {
            Nimu = GameObject.Find("Nimu");
        }
        GameManager.instance.textToWriteIn = naratteurText;
        StartCoroutine(StartIntro());
    }

    private void CharactersDataGet()
    {
        for (int i = 0; i < characters.Count; i++)
        {
            characters[i].transform.GetChild(2).GetComponent<Image>().sprite = characters[i].GetComponent<C_Actor>().GetDataActor().MapTmSprite;
            characters[i].transform.GetChild(2).transform.GetChild(0).GetComponent<Image>().enabled = false;
            characters[i].transform.GetChild(2).transform.GetChild(1).GetComponent<Image>().enabled = false;
            characters[i].transform.GetChild(2).transform.GetChild(1).transform.GetChild(0).GetComponent<Image>().enabled = false;
            characters[i].transform.GetChild(0).GetComponent<Image>().enabled = false;
            characters[i].transform.GetChild(1).GetComponent<Image>().enabled = false;
            characters[i].transform.GetChild(3).GetComponent<Image>().enabled = false;
            characters[i].transform.GetChild(4).GetComponent<Image>().enabled = false;
            characters[i].GetComponent<C_Actor>().GetDataActor().characterTree.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => GoToActions());
            Instantiate(characters[i].GetComponent<C_Actor>().GetDataActor().characterTree, TreeParent.transform.GetChild(i));
            TreeParent.transform.GetChild(i).GetChild(0).GetChild(0).GetComponent<Button>().onClick.AddListener(() => GoToActions());
            TreeParent.transform.GetChild(i).GetChild(0).GetChild(1).GetComponent<Button>().onClick.AddListener(() => PapotageFin());
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void HideUI()
    {
        FichePersoParent.SetActive(false);
        TreeParent.SetActive(false);
        ActionsParents.SetActive(false);
    }
    public void GoToActions()
    {
       
        if(actorActif.GetComponent<C_Actor>().HasPlayed==false)
        {
            charactertoaddID++;
            LastCharacterThatPlayed.Add(actorActif);
            for (int i = 0; i < characters.Count; i++)
            {
                TreeParent.transform.GetChild(i).GetChild(0).GetChild(0).GetComponent<Button>().enabled = false;
            }
            //TreeParent.SetActive(false);
            ActionsParents.SetActive(true);
            Es.SetSelectedGameObject(actions[0]);
            updateButton();
        }
       
    }
    public void ActivateTreeCharacterChoice()
    {
        TreeParent.SetActive(true);
        for (int i = 0; i < characters.Count; i++)
        {
            TreeParent.transform.GetChild(i).GetChild(0).GetChild(0).gameObject.SetActive(true);
            TreeParent.transform.GetChild(i).GetChild(0).GetChild(1).gameObject.SetActive(false);
        }
    }
    public void ActivateTreePapotageChoice()
    {
        TreeParent.SetActive(true);
        for (int i=0;i<characters.Count; i++)
        {
            TreeParent.transform.GetChild(i).GetChild(0).GetChild(0).gameObject.SetActive(false);
            TreeParent.transform.GetChild(i).GetChild(0).GetChild(1).gameObject.SetActive(true);
        }
        
       
    }
    private void initiateTMvariables()
    {
        if (GameManager.instance)
        {
            Instantiate(GameManager.instance.GetDataTempsMort().TMbackground, background.transform);
            _intro = GameManager.instance.GetDataTempsMort().intro;
            _outro = GameManager.instance.GetDataTempsMort().Outro;
            Observage = GameManager.instance.GetDataTempsMort().Observer;
            for (int i = 0; i < GameManager.instance.GetDataTempsMort().startPos.Length; i++)
            {
                SpawnParent.transform.GetChild(i).GetComponent<RectTransform>().anchoredPosition = GameManager.instance.GetDataTempsMort().startPos[i].position;
            }
            foreach (GameObject thisActor in GameManager.instance.GetTeam())
            {
                foreach (InitialActorPosition position in TM.startPos)
                {
                    //Check si dans les info du challenge est dans l'équipe stocké dans le GameManager.
                    if (thisActor.GetComponent<C_Actor>().GetDataActor().name == position.perso.GetComponent<C_Actor>().GetDataActor().name)
                    {
                        //Ini data actor.
                        thisActor.GetComponent<C_Actor>().IniTempsMort();
                        thisActor.transform.localScale = new Vector3(0.55f, 0.55f, 0.55f);

                        characters.Add(thisActor);
                        characterNB++;
                        thisActor.GetComponent<RectTransform>().parent= SpawnParent.transform.GetChild(characterNB).GetComponent<RectTransform>() ;
                        thisActor.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);
                        thisActor.transform.GetChild(0).gameObject.SetActive(true);
                        thisActor.transform.GetChild(1).gameObject.SetActive(true);
                        thisActor.transform.GetChild(3).gameObject.SetActive(true);
                        thisActor.transform.GetChild(4).gameObject.SetActive(true);
                        thisActor.transform.GetChild(5).gameObject.SetActive(false);
                    }
                    else
                    {
                        //Cache les actor qui ne seront pas présent dans ce challenge.
                        //thisActor.SetActive(false);
                    }
                }
            }

        }
        for (int i = 0; i < characters.Count; i++)
        {
            characters[i].transform.GetChild(0).GetComponentInChildren<TextAnimatorPlayer>().onTypewriterStart.AddListener(() => SetCanContinueToNo());
            characters[i].transform.GetChild(1).GetComponentInChildren<TextAnimatorPlayer>().onTypewriterStart.AddListener(() => SetCanContinueToNo());
            characters[i].transform.GetChild(3).GetComponentInChildren<TextAnimatorPlayer>().onTypewriterStart.AddListener(() => SetCanContinueToNo());
            characters[i].transform.GetChild(4).GetComponentInChildren<TextAnimatorPlayer>().onTypewriterStart.AddListener(() => SetCanContinueToNo());

            characters[i].transform.GetChild(0).GetComponentInChildren<TextAnimatorPlayer>().onTextShowed.AddListener(() => SetCanContinueToYes());
            characters[i].transform.GetChild(1).GetComponentInChildren<TextAnimatorPlayer>().onTextShowed.AddListener(() => SetCanContinueToYes());
            characters[i].transform.GetChild(3).GetComponentInChildren<TextAnimatorPlayer>().onTextShowed.AddListener(() => SetCanContinueToYes());
            characters[i].transform.GetChild(4).GetComponentInChildren<TextAnimatorPlayer>().onTextShowed.AddListener(() => SetCanContinueToYes());
            characters[i].GetComponent<C_Actor>().GetDataActor().characterTree.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => GoToActions());
        }
        naratteurText.GetComponent<TextAnimatorPlayer>().onTextShowed.AddListener(() => SetCanContinueToYes());
        naratteurText.GetComponent<TextAnimatorPlayer>().onTypewriterStart.AddListener(() => SetCanContinueToNo());
    }
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
        TreeParent.SetActive(true);
        TMhasStarted = true;
        Es = FindObjectOfType<EventSystem>();
        Cine.GetComponent<Animator>().SetBool("IsCinema", false);
        GameManager.instance.ExitDialogueMode();
        ActivateTreeCharacterChoice();
        Es.SetSelectedGameObject(TreeParent.transform.GetChild(0).GetChild(0).GetChild(0).gameObject);
        currentButton = Es.currentSelectedGameObject;
        for (int i = 0; i < characters.Count; i++)
        {
            if (currentButton.name == characters[i].name +"CharacterChoice")
            {
                actorActif = characters[i];   
            }
        }
        updateButton();
        AfficherFichereduite();
    }
    public void continueStory(InputAction.CallbackContext context)
    {
        Debug.Log("continue");
        if (context.performed && GameManager.instance.isDialoguing == true && canContinue == true)
        {
            GameManager.instance.ContinueStory();
        }
        else if (context.performed && GameManager.instance.isDialoguing == true && canContinue == false)
        {
            GameManager.instance.textToWriteIn.GetComponent<TextAnimatorPlayer>().SkipTypewriter();
        }
        return;

    }
    private void updateButton()
    {
        if (currentButton.transform.GetChild(0) != null)
        {
            currentButton.transform.GetChild(0).gameObject.SetActive(false);
        }
        for(int i=0;i<characters.Count;i++)
        {
            TreeParent.transform.GetChild(i).GetComponent<Animator>().SetBool("IsHover", false);
        }
        currentButton = Es.currentSelectedGameObject;
        if (currentButton.transform.GetChild(0) != null)
        {
            currentButton.transform.GetChild(0).gameObject.SetActive(true);
        }
        for(int i =0;i<characters.Count;i++)
        {    
            if (currentButton.name == characters[i].name + "CharacterChoice")
            {
                Debug.Log(characters[i].name + "CharacterChoice");
                actorActif = characters[i];
                TreeParent.transform.GetChild(i).GetComponent<Animator>().SetBool("IsHover", true);
            }
            if (currentButton.name == characters[i].name + "PapotageChoice")
            {
                Papote = characters[i];
                TreeParent.transform.GetChild(i).GetComponent<Animator>().SetBool("IsHover", true);
            }
        }
        for(int i=0;i<actions.Count;i++)
        {
            if(currentButton==actions[i]&&GameManager.instance.isDialoguing==false)
            {
                AfficherGrandeFiche();
            }
        }
        DisplayFicheInfos();
    }
    public void Naviguate(InputAction.CallbackContext context)
    {
        if (!context.performed) { return; }

        if (context.performed && TMhasStarted)
        {
            updateButton();
        }
    }
    public void SetCanContinueToYes()
    {
        canContinue = true;
        GameManager.instance.textToWriteIn.transform.GetChild(0).gameObject.SetActive(true);
    }
    public void SetCanContinueToNo()
    {
        canContinue = false;
    }
    public void AfficherFichereduite()
    {
        FichePersoParent.SetActive(true);
        FichePersoParent.transform.GetChild(0).gameObject.SetActive(true);
        FichePersoParent.transform.GetChild(1).gameObject.SetActive(false);
        FichePersoParent.transform.GetChild(2).gameObject.SetActive(false);
        DisplayFicheInfos();
    }
    public void AfficherGrandeFiche()
    {
        FichePersoParent.SetActive(true);
        FichePersoParent.transform.GetChild(0).gameObject.SetActive(false);
        FichePersoParent.transform.GetChild(1).gameObject.SetActive(true);
        FichePersoParent.transform.GetChild(2).gameObject.SetActive(false);
        DisplayFicheInfos();
    }
    public void DisplayFicheInfos()
    {
        if(FichePersoParent.transform.GetChild(0).gameObject.activeSelf == true)
        {
            FichePersoParent.transform.GetChild(0).GetChild(3).GetComponent<Image>().sprite = actorActif.GetComponent<C_Actor>().GetDataActor().ProfilPhoto;
            FichePersoParent.transform.GetChild(0).GetChild(4).GetComponent<TMP_Text>().text = "Energie : " + actorActif.GetComponent<C_Actor>().GetDataActor().energyMax;
            FichePersoParent.transform.GetChild(0).GetChild(5).GetComponent<TMP_Text>().text = "Calme : " + actorActif.GetComponent<C_Actor>().GetDataActor().stressMax;
            FichePersoParent.transform.GetChild(0).GetChild(6).GetComponent<TMP_Text>().text = actorActif.GetComponent<C_Actor>().GetDataActor().name;
            FichePersoParent.transform.GetChild(0).GetChild(7).GetComponent<TMP_Text>().text = "Pts de trait : " + actorActif.GetComponent<C_Actor>().GetCurrentPointTrait();
            FichePersoParent.transform.GetChild(0).GetChild(8).GetComponent<TMP_Text>().text = actorActif.GetComponent<C_Actor>().GetDataActor().miniDescription;
            if(actorActif.GetComponent<C_Actor>().HasObserved)
            {
                FichePersoParent.transform.GetChild(0).GetChild(9).gameObject.SetActive(true);
            }
            else { FichePersoParent.transform.GetChild(0).GetChild(9).gameObject.SetActive(false); }
            if (actorActif.GetComponent<C_Actor>().HasRevassed)
            {
                FichePersoParent.transform.GetChild(0).GetChild(10).gameObject.SetActive(true);
            }
            else { FichePersoParent.transform.GetChild(0).GetChild(10).gameObject.SetActive(false); }
            if (actorActif.GetComponent<C_Actor>().HasPapoted)
            {
                FichePersoParent.transform.GetChild(0).GetChild(11).GetComponent<TMP_Text>().text = "+" + actorActif.GetComponent<C_Actor>().GetCurrentPointTrait();
                FichePersoParent.transform.GetChild(0).GetChild(11).gameObject.SetActive(true);
            }
            else if(actorActif.GetComponent<C_Actor>().HasPapoted&& actorActif.GetComponent<C_Actor>().HasTraited)
            {
                FichePersoParent.transform.GetChild(0).GetChild(12).gameObject.SetActive(true);
            }
            else { FichePersoParent.transform.GetChild(0).GetChild(11).gameObject.SetActive(false); FichePersoParent.transform.GetChild(0).GetChild(12).gameObject.SetActive(false); }

        }
        if (FichePersoParent.transform.GetChild(1).gameObject.activeSelf == true)
        {
            FichePersoParent.transform.GetChild(1).GetChild(2).GetComponent<Image>().sprite = actorActif.GetComponent<C_Actor>().GetDataActor().ProfilPhoto;
            FichePersoParent.transform.GetChild(1).GetChild(3).GetComponent<TMP_Text>().text = "Energie : " + actorActif.GetComponent<C_Actor>().GetDataActor().energyMax;
            FichePersoParent.transform.GetChild(1).GetChild(4).GetComponent<TMP_Text>().text = "Calme : " + actorActif.GetComponent<C_Actor>().GetDataActor().stressMax;
            FichePersoParent.transform.GetChild(1).GetChild(5).GetComponent<TMP_Text>().text = actorActif.GetComponent<C_Actor>().GetDataActor().name;
            FichePersoParent.transform.GetChild(1).GetChild(6).GetComponent<TMP_Text>().text = actorActif.GetComponent<C_Actor>().GetDataActor().Description;
            if (actorActif.GetComponent<C_Actor>().HasObserved||currentButton==ObserverButton)
            {
                FichePersoParent.transform.GetChild(1).GetChild(8).gameObject.SetActive(true);
            }
            else { FichePersoParent.transform.GetChild(1).GetChild(8).gameObject.SetActive(false); }
            if (actorActif.GetComponent<C_Actor>().HasRevassed||currentButton==RevasserButton)
            {
                FichePersoParent.transform.GetChild(1).GetChild(9).gameObject.SetActive(true);
            }
            else { FichePersoParent.transform.GetChild(1).GetChild(9).gameObject.SetActive(false); }
            if (actorActif.GetComponent<C_Actor>().HasTraited||currentButton==PapoterButton&& actorActif.GetComponent<C_Actor>().GetCurrentPointTrait()+0.5f==1)
            {
                FichePersoParent.transform.GetChild(1).GetChild(10).gameObject.SetActive(true);
            }
            else { FichePersoParent.transform.GetChild(1).GetChild(10).gameObject.SetActive(false); }
        }
        if (FichePersoParent.transform.GetChild(2).gameObject.activeSelf == true)
        {
            FichePersoParent.transform.GetChild(2).GetChild(0).GetComponent<Image>().sprite = actorActif.GetComponent<C_Actor>().GetDataActor().ProfilPhoto;
            FichePersoParent.transform.GetChild(2).GetChild(1).GetComponent<TMP_Text>().text = actorActif.GetComponent<C_Actor>().GetDataActor().name;
            FichePersoParent.transform.GetChild(2).GetChild(3).GetComponent<TMP_Text>().text = "Pts de trait : " + actorActif.GetComponent<C_Actor>().GetCurrentPointTrait();
            for (int y = 0; y < actorActif.GetComponent<C_Actor>().GetDataActor().listNewTraits.Count; y++)
            {
                FichePersoParent.transform.GetChild(2).GetChild(4).GetComponent<TMP_Text>().text = FichePersoParent.transform.GetChild(0).GetChild(3).GetComponent<TMP_Text>().text + "\n" + actorActif.GetComponent<C_Actor>().GetDataActor().listNewTraits[y].buttonText;
            }
            if (actorActif.GetComponent<C_Actor>().HasPapoted)
            {
                FichePersoParent.transform.GetChild(2).GetChild(6).GetComponent<TMP_Text>().text = "+" + actorActif.GetComponent<C_Actor>().GetCurrentPointTrait();
                FichePersoParent.transform.GetChild(2).GetChild(6).gameObject.SetActive(true);
            }
            else if (currentButton == PapoterButton)
            {
                FichePersoParent.transform.GetChild(2).GetChild(6).GetComponent<TMP_Text>().text = "+0,5";
                FichePersoParent.transform.GetChild(2).GetChild(6).gameObject.SetActive(true);
            }
            else if (actorActif.GetComponent<C_Actor>().HasPapoted && actorActif.GetComponent<C_Actor>().HasTraited || currentButton == PapoterButton)
            {
                FichePersoParent.transform.GetChild(2).GetChild(7).gameObject.SetActive(true);
            }
            else { FichePersoParent.transform.GetChild(2).GetChild(6).gameObject.SetActive(false); FichePersoParent.transform.GetChild(2).GetChild(7).gameObject.SetActive(false); }
        }
    }
    public void SwitchCharacterCard(InputAction.CallbackContext context)
    {
        if (!context.performed) { return; }

        if (context.performed)
        {
            Debug.Log("switch card");
            if(FichePersoParent.transform.GetChild(1).gameObject.activeSelf==true)
            {
                FichePersoParent.transform.GetChild(2).gameObject.SetActive(true);
                FichePersoParent.transform.GetChild(1).gameObject.SetActive(false);
                DisplayFicheInfos();
                Debug.Log("Card 2/2");
            }
            else if (FichePersoParent.transform.GetChild(2).gameObject.activeSelf == true)
            {
                FichePersoParent.transform.GetChild(2).gameObject.SetActive(false);
                FichePersoParent.transform.GetChild(1).gameObject.SetActive(true);
                DisplayFicheInfos();
                Debug.Log("card 1/2");
            }
        }
    }
    public void Revasser()
    {
        HideUI();       
        for (int y = 0; y < characters.Count; y++)
        {
            if (actorActif == characters[y])
            {
                GameManager.instance.RevasserID[y]++;
            }

        }
        actiontoaddID++;
        LastAction.Add(RevasserButton);
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
                characters[i].GetComponent<C_Actor>().HasPlayed = true;
                actorActif.GetComponent<C_Actor>().HasRevassed = true;
            }
        }
        //calm
        actorActif.GetComponent<C_Actor>().SetMaxStress();
        Debug.Log(actorActif.GetComponent<C_Actor>().getMaxStress());
        //actorActif.GetComponent<C_Actor>().maxStress++;
        ActivateCharactersButton();
    }
    public void Observer()
    {
        HideUI();
       
        for (int y = 0; y < characters.Count; y++)
        {
            if (actorActif == characters[y])
            {
                GameManager.instance.RespirerID++;
            }

        }
        actiontoaddID++;
        LastAction.Add(ObserverButton);
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
                characters[i].GetComponent<C_Actor>().HasPlayed = true;
                actorActif.GetComponent<C_Actor>().HasObserved = true;
            }
        }
        //energy
        actorActif.GetComponent<C_Actor>().SetMaxEnergy();
        Debug.Log(actorActif.GetComponent<C_Actor>().getMaxEnergy());
        //actorActif.GetComponent<C_Actor>().maxEnergy+=1;
        ActivateCharactersButton();

    }
    public void Papoter()
    {
        ActivateTreePapotageChoice();
        Es.SetSelectedGameObject(TreeParent.transform.GetChild(0).GetChild(0).GetChild(1).gameObject);
        ActionsParents.SetActive(false);

        updateButton();
        //traitpoint
        actiontoaddID++;
        LastAction.Add(PapoterButton);
    }
    public void PapotageFin()
    {

        for (int i = 0; i < characters.Count; i++)
        {
            if (currentButton == TreeParent.transform.GetChild(i).GetChild(0).GetChild(1))
            {
                Papote = characters[i];
            }
            if (actorActif == Morgan && Papote == Esthela && MorganAPapoteAvecEsthela == false)
            {
                HideUI();

                actorActif.GetComponent<C_Actor>().SetCurrentPointTrait();
                Debug.Log(actorActif.name + " possède : " + actorActif.GetComponent<C_Actor>().GetCurrentPointTrait() + " pts de traits");
                Papote.GetComponent<C_Actor>().SetCurrentPointTrait();
                Debug.Log(Papote.name + " possède : " + Papote.GetComponent<C_Actor>().GetCurrentPointTrait() + " pts de traits");
                if (actorActif.GetComponent<C_Actor>().GetCurrentPointTrait() == 1)
                {
                    actorActif.GetComponent<C_Actor>().HasTraited = true;
                    actorActif.GetComponent<C_Actor>().ResetPointTrait();
                    actorActif.GetComponent<C_Actor>().UpdateNextTrait();                  
                    Debug.Log("Trait de " + actorActif.name + " numéro " + actorActif.GetComponent<C_Actor>().GetDataActor().idTraitEnCours);
                }
                if (Papote.GetComponent<C_Actor>().GetCurrentPointTrait() == 1)
                {
                    Papote.GetComponent<C_Actor>().HasTraited = true;
                    Papote.GetComponent<C_Actor>().ResetPointTrait();
                    Papote.GetComponent<C_Actor>().UpdateNextTrait();
                   
                    Debug.Log("Trait de " + Papote.name + " numéro " + Papote.GetComponent<C_Actor>().GetDataActor().idTraitEnCours);
                }               
                GameManager.instance.PapoterID[0]++;
                Cine.GetComponent<Animator>().SetBool("IsCinema", true);
                GameManager.instance.EnterDialogueMode(GameManager.instance.papotage[0]);
                MorganAPapoteAvecEsthela = true;
            }
            else if (actorActif == Morgan && Papote == Nimu && MorganAPapoteAvecNimu == false)
            {
                HideUI();
                actorActif.GetComponent<C_Actor>().SetCurrentPointTrait();
                Debug.Log(actorActif.name + " possède : " + actorActif.GetComponent<C_Actor>().GetCurrentPointTrait() + " pts de traits");
                Papote.GetComponent<C_Actor>().SetCurrentPointTrait();
                Debug.Log(Papote.name + " possède : " + Papote.GetComponent<C_Actor>().GetCurrentPointTrait() + " pts de traits");
                if (actorActif.GetComponent<C_Actor>().GetCurrentPointTrait() == 1)
                {
                    actorActif.GetComponent<C_Actor>().HasTraited = true;
                    actorActif.GetComponent<C_Actor>().ResetPointTrait();
                    actorActif.GetComponent<C_Actor>().UpdateNextTrait();                 
                    Debug.Log("Trait de " + actorActif.name + " numéro " + actorActif.GetComponent<C_Actor>().GetDataActor().idTraitEnCours);
                }                
                if (Papote.GetComponent<C_Actor>().GetCurrentPointTrait() == 1)
                {
                    Papote.GetComponent<C_Actor>().HasTraited = true;
                    Papote.GetComponent<C_Actor>().ResetPointTrait();
                    Papote.GetComponent<C_Actor>().UpdateNextTrait();                    
                    Debug.Log("Trait de " + Papote.name + " numéro " + Papote.GetComponent<C_Actor>().GetDataActor().idTraitEnCours);
                }             
                GameManager.instance.PapoterID[1]++;
                Cine.GetComponent<Animator>().SetBool("IsCinema", true);
                GameManager.instance.EnterDialogueMode(GameManager.instance.papotage[1]);
                MorganAPapoteAvecNimu = true;
            }
            else if (actorActif == Esthela && Papote == Morgan && MorganAPapoteAvecEsthela == false)
            {
                HideUI();
                actorActif.GetComponent<C_Actor>().SetCurrentPointTrait();
                Debug.Log(actorActif.name + " possède : " + actorActif.GetComponent<C_Actor>().GetCurrentPointTrait() + " pts de traits");
                Papote.GetComponent<C_Actor>().SetCurrentPointTrait();
                Debug.Log(Papote.name + " possède : " + Papote.GetComponent<C_Actor>().GetCurrentPointTrait() + " pts de traits");   
             if (actorActif.GetComponent<C_Actor>().GetCurrentPointTrait() == 1)
                {
                    actorActif.GetComponent<C_Actor>().HasTraited = true;
                    actorActif.GetComponent<C_Actor>().ResetPointTrait();
                    actorActif.GetComponent<C_Actor>().UpdateNextTrait();                   
                    Debug.Log("Trait de " + actorActif.name + " numéro " + actorActif.GetComponent<C_Actor>().GetDataActor().idTraitEnCours);
                }              
                if (Papote.GetComponent<C_Actor>().GetCurrentPointTrait() == 1)
                {
                    Papote.GetComponent<C_Actor>().HasTraited = true;
                    Papote.GetComponent<C_Actor>().ResetPointTrait();
                    Papote.GetComponent<C_Actor>().UpdateNextTrait();                    
                    Debug.Log("Trait de " + Papote.name + " numéro " + Papote.GetComponent<C_Actor>().GetDataActor().idTraitEnCours);
                }               
                GameManager.instance.PapoterID[0]++;
                Cine.GetComponent<Animator>().SetBool("IsCinema", true);
                GameManager.instance.EnterDialogueMode(GameManager.instance.papotage[0]);
                MorganAPapoteAvecEsthela = true;
            }
            else if (actorActif == Esthela && Papote == Nimu && NimuAPapoteAvecEsthela == false)
            {
                HideUI();
                actorActif.GetComponent<C_Actor>().SetCurrentPointTrait();
                Debug.Log(actorActif.name + " possède : " + actorActif.GetComponent<C_Actor>().GetCurrentPointTrait() + " pts de traits");
                Papote.GetComponent<C_Actor>().SetCurrentPointTrait();
                Debug.Log(Papote.name + " possède : " + Papote.GetComponent<C_Actor>().GetCurrentPointTrait() + " pts de traits");               
                if (actorActif.GetComponent<C_Actor>().GetCurrentPointTrait() == 1)
                {
                    actorActif.GetComponent<C_Actor>().HasTraited = true;
                    actorActif.GetComponent<C_Actor>().ResetPointTrait();
                    actorActif.GetComponent<C_Actor>().UpdateNextTrait();
                    Debug.Log("Trait de " + actorActif.name + " numéro " + actorActif.GetComponent<C_Actor>().GetDataActor().idTraitEnCours);
                }               
                if (Papote.GetComponent<C_Actor>().GetCurrentPointTrait() == 1)
                {
                    Papote.GetComponent<C_Actor>().HasTraited = true;
                    Papote.GetComponent<C_Actor>().ResetPointTrait();
                    Papote.GetComponent<C_Actor>().UpdateNextTrait();                  
                    Debug.Log("Trait de " + Papote.name + " numéro " + Papote.GetComponent<C_Actor>().GetDataActor().idTraitEnCours);
                }                
                GameManager.instance.PapoterID[2]++;
                Cine.GetComponent<Animator>().SetBool("IsCinema", true);
                GameManager.instance.EnterDialogueMode(GameManager.instance.papotage[2]);
                NimuAPapoteAvecEsthela = true;
            }
            else if (actorActif == Nimu && Papote == Morgan && MorganAPapoteAvecNimu == false)
            {
                HideUI();
                actorActif.GetComponent<C_Actor>().SetCurrentPointTrait();
                Debug.Log(actorActif.name + " possède : " + actorActif.GetComponent<C_Actor>().GetCurrentPointTrait() + " pts de traits");
                Papote.GetComponent<C_Actor>().SetCurrentPointTrait();
                Debug.Log(Papote.name + " possède : " + Papote.GetComponent<C_Actor>().GetCurrentPointTrait() + " pts de traits");
                if (actorActif.GetComponent<C_Actor>().GetCurrentPointTrait() == 1)
                {
                    actorActif.GetComponent<C_Actor>().HasTraited = true;
                    actorActif.GetComponent<C_Actor>().ResetPointTrait();
                    actorActif.GetComponent<C_Actor>().UpdateNextTrait();
                    Debug.Log("Trait de " + actorActif.name + " numéro " + actorActif.GetComponent<C_Actor>().GetDataActor().idTraitEnCours);
                }               
                if (Papote.GetComponent<C_Actor>().GetCurrentPointTrait() == 1)
                {
                    Papote.GetComponent<C_Actor>().HasTraited = true;
                    Papote.GetComponent<C_Actor>().ResetPointTrait();
                    Papote.GetComponent<C_Actor>().UpdateNextTrait();                   
                    Debug.Log("Trait de " + Papote.name + " numéro " + Papote.GetComponent<C_Actor>().GetDataActor().idTraitEnCours);
                }
               
                GameManager.instance.PapoterID[1]++;
                Debug.Log("Nimu papot avec Morgan valeur : " + GameManager.instance.PapoterID[1]);
                Cine.GetComponent<Animator>().SetBool("IsCinema", true);
                GameManager.instance.EnterDialogueMode(GameManager.instance.papotage[1]);
                MorganAPapoteAvecNimu = true;
            }
            else if (actorActif == Nimu && Papote == Esthela && NimuAPapoteAvecEsthela == false)
            {
                HideUI();
                actorActif.GetComponent<C_Actor>().SetCurrentPointTrait();
                Debug.Log(actorActif.name + " possède : " + actorActif.GetComponent<C_Actor>().GetCurrentPointTrait() + " pts de traits");
                Papote.GetComponent<C_Actor>().SetCurrentPointTrait();
                Debug.Log(Papote.name + " possède : " + Papote.GetComponent<C_Actor>().GetCurrentPointTrait() + " pts de traits");
                if (actorActif.GetComponent<C_Actor>().GetCurrentPointTrait() == 1)
                {
                    actorActif.GetComponent<C_Actor>().HasTraited = true;
                    actorActif.GetComponent<C_Actor>().ResetPointTrait();
                    actorActif.GetComponent<C_Actor>().UpdateNextTrait();                    
                    Debug.Log("Trait de " + actorActif.name + " numéro " + actorActif.GetComponent<C_Actor>().GetDataActor().idTraitEnCours);
                }               
                if (Papote.GetComponent<C_Actor>().GetCurrentPointTrait() == 1)
                {
                    Papote.GetComponent<C_Actor>().HasTraited = true;
                    Papote.GetComponent<C_Actor>().ResetPointTrait();
                    Papote.GetComponent<C_Actor>().UpdateNextTrait();                    
                    Debug.Log("Trait de " + Papote.name + " numéro " + Papote.GetComponent<C_Actor>().GetDataActor().idTraitEnCours);

                }               
                GameManager.instance.PapoterID[2]++;
                Cine.GetComponent<Animator>().SetBool("IsCinema", true);
                GameManager.instance.EnterDialogueMode(GameManager.instance.papotage[2]);
                NimuAPapoteAvecEsthela = true;
            }
            else
            {
                Debug.Log("peut pas papoter");
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
                actorActif.GetComponent<C_Actor>().HasPlayed = true;
                actorActif.GetComponent<C_Actor>().HasPapoted = true;
                Papote.GetComponent<C_Actor>().HasPapoted=true;
            }
        }
        Debug.Log("Retour TM apres papotage");
        ActivateCharactersButton();
    }
    private void ActivateCharactersButton()
    {
        ActivateTreeCharacterChoice();
        AfficherFichereduite();
        for (int i = 0; i < characters.Count; i++)
        {
            TreeParent.transform.GetChild(i).GetChild(0).GetChild(0).GetComponent<Button>().enabled = true;
        }
        //TreeParent.SetActive(false);
        ActionsParents.SetActive(false);
        Es.SetSelectedGameObject(TreeParent.transform.GetChild(0).GetChild(0).GetChild(0).gameObject);
        updateButton();
        int nbActorOut = 0;

        foreach (var thisActor in characters)
        {
            if (thisActor.GetComponent<C_Actor>().HasPlayed==true) { nbActorOut++; }
        }

        if (nbActorOut == characters.Count)
        {
            ChallengeButton.SetActive(true);
        }
    }
    public void StartOutro()
    {
        HideUI();
        ChallengeButton.SetActive(false);
        Cine.GetComponent<Animator>().SetBool("IsCinema", true);
        GameManager.instance.EnterDialogueMode(_outro);

    }
    public void GoChallenge(string named)
    {
        foreach (GameObject c in characters)
        {
            Debug.Log(c.name + " Passe dans le GameManager");

            Debug.Log(c.GetComponent<C_Actor>().GetDataActor().vfxUiGoodAction);
            c.transform.parent = GameManager.instance.transform;
            c.GetComponent<C_Actor>().HasObserved = false;
            c.GetComponent<C_Actor>().HasPapoted = false;
            c.GetComponent<C_Actor>().HasPlayed = false;
            c.GetComponent<C_Actor>().HasRevassed = false;
            c.GetComponent<C_Actor>().HasTraited = false;
        }
        SceneManager.LoadScene(named);
    }
    public void BACK(InputAction.CallbackContext context)
    {
        if (!context.performed)
        { return; }
        if (context.performed && GameManager.instance.isDialoguing == false)
        {
            Debug.Log("going backward");
            if(GameManager.instance.isDialoguing!=true)
            {
                if (ActionsParents.activeSelf == true)
                {
                    for (int i = 0; i < characters.Count; i++)
                    {
                        TreeParent.transform.GetChild(i).GetChild(0).GetChild(0).GetComponent<Button>().enabled = true;
                        if (LastCharacterThatPlayed[charactertoaddID] == characters[i])
                        {
                            Es.SetSelectedGameObject(TreeParent.transform.GetChild(i).GetChild(0).GetChild(0).gameObject);
                            updateButton();
                        }
                    }
                    LastCharacterThatPlayed.RemoveAt(charactertoaddID);
                    charactertoaddID--;
                    ActionsParents.SetActive(false);
                }
                if (ActionsParents.activeSelf == false && actiontoaddID != -1)
                {
                    for (int i = 0; i < characters.Count; i++)
                    {
                        if (TreeParent.transform.GetChild(i).GetChild(0).GetChild(0).gameObject.activeSelf == true)
                        {
                            for (int y = 0; y < characters.Count; y++)
                            {
                                TreeParent.transform.GetChild(y).GetChild(0).GetChild(0).GetComponent<Button>().enabled = false;
                            }
                            //TreeParent.SetActive(false);
                            ActionsParents.SetActive(true);

                            
                            Es.SetSelectedGameObject(LastAction[actiontoaddID]);
                            updateButton();
                            LastAction.RemoveAt(actiontoaddID);
                            actiontoaddID--;
                        }
                        else
                        {
                            if(LastAction[actiontoaddID]==RevasserButton)
                            {
                                for (int y = 0; y < characters.Count; y++)
                                {
                                    if (LastCharacterThatPlayed[charactertoaddID] == characters[y])
                                    {
                                        GameManager.instance.RevasserID[y]--;
                                    }
                                }
                                LastCharacterThatPlayed[charactertoaddID].GetComponent<C_Actor>().GetDataActor().stressMax--;
                            }
                            if (LastAction[actiontoaddID] == ObserverButton)
                            {
                                GameManager.instance.RespirerID--;
                               LastCharacterThatPlayed[charactertoaddID].GetComponent<C_Actor>().GetDataActor().energyMax--;
                            }
                            if (LastAction[actiontoaddID] == PapoterButton)
                            {
                                if (LastCharacterThatPlayed[charactertoaddID] == Morgan && Papote == Esthela)
                                {                                   
                                    if (LastCharacterThatPlayed[charactertoaddID].GetComponent<C_Actor>().GetCurrentPointTrait() == 0&& LastCharacterThatPlayed[charactertoaddID].GetComponent<C_Actor>().GetDataActor().idTraitEnCours!=-1)
                                    {
                                        LastCharacterThatPlayed[charactertoaddID].GetComponent<C_Actor>().HasTraited = false;
                                        LastCharacterThatPlayed[charactertoaddID].GetComponent<C_Actor>().currentPointTrait=0.5f;
                                        LastCharacterThatPlayed[charactertoaddID].GetComponent<C_Actor>().GetDataActor().listNewTraits.RemoveAt(LastCharacterThatPlayed[charactertoaddID].GetComponent<C_Actor>().GetDataActor().idTraitEnCours);
                                    }
                                    else
                                    {
                                        LastCharacterThatPlayed[charactertoaddID].GetComponent<C_Actor>().currentPointTrait = 0;
                                    }
                                    if (Papote.GetComponent<C_Actor>().GetCurrentPointTrait() == 0 && Papote.GetComponent<C_Actor>().GetDataActor().idTraitEnCours != -1)
                                    {
                                        Papote.GetComponent<C_Actor>().HasTraited = false;
                                        Papote.GetComponent<C_Actor>().currentPointTrait = 0.5f;
                                        Papote.GetComponent<C_Actor>().GetDataActor().listNewTraits.RemoveAt(Papote.GetComponent<C_Actor>().GetDataActor().idTraitEnCours);
                                    }
                                    else
                                    {
                                        Papote.GetComponent<C_Actor>().currentPointTrait = 0;
                                    }
                                    GameManager.instance.PapoterID[0]--;  
                                    MorganAPapoteAvecEsthela = false;
                                }
                                else if (LastCharacterThatPlayed[charactertoaddID] == Morgan && Papote == Nimu && MorganAPapoteAvecNimu == false)
                                {
                                    if (LastCharacterThatPlayed[charactertoaddID].GetComponent<C_Actor>().GetCurrentPointTrait() == 0 && LastCharacterThatPlayed[charactertoaddID].GetComponent<C_Actor>().GetDataActor().idTraitEnCours != -1)
                                    {
                                        LastCharacterThatPlayed[charactertoaddID].GetComponent<C_Actor>().HasTraited = false;
                                        LastCharacterThatPlayed[charactertoaddID].GetComponent<C_Actor>().currentPointTrait = 0.5f;
                                        LastCharacterThatPlayed[charactertoaddID].GetComponent<C_Actor>().GetDataActor().listNewTraits.RemoveAt(LastCharacterThatPlayed[charactertoaddID].GetComponent<C_Actor>().GetDataActor().idTraitEnCours);
                                    }
                                    else
                                    {
                                        LastCharacterThatPlayed[charactertoaddID].GetComponent<C_Actor>().currentPointTrait = 0;
                                    }
                                    if (Papote.GetComponent<C_Actor>().GetCurrentPointTrait() == 0 && Papote.GetComponent<C_Actor>().GetDataActor().idTraitEnCours != -1)
                                    {
                                        Papote.GetComponent<C_Actor>().HasTraited = false;
                                        Papote.GetComponent<C_Actor>().currentPointTrait = 0.5f;
                                        Papote.GetComponent<C_Actor>().GetDataActor().listNewTraits.RemoveAt(Papote.GetComponent<C_Actor>().GetDataActor().idTraitEnCours);
                                    }
                                    else
                                    {
                                        Papote.GetComponent<C_Actor>().currentPointTrait = 0;
                                    }
                                    GameManager.instance.PapoterID[1]--;
                                    MorganAPapoteAvecNimu = false;
                                }
                                else if (LastCharacterThatPlayed[charactertoaddID] == Esthela && Papote == Morgan && MorganAPapoteAvecEsthela == false)
                                {
                                    if (LastCharacterThatPlayed[charactertoaddID].GetComponent<C_Actor>().GetCurrentPointTrait() == 0 && LastCharacterThatPlayed[charactertoaddID].GetComponent<C_Actor>().GetDataActor().idTraitEnCours != -1)
                                    {
                                        LastCharacterThatPlayed[charactertoaddID].GetComponent<C_Actor>().HasTraited = false;
                                        LastCharacterThatPlayed[charactertoaddID].GetComponent<C_Actor>().currentPointTrait = 0.5f;
                                        LastCharacterThatPlayed[charactertoaddID].GetComponent<C_Actor>().GetDataActor().listNewTraits.RemoveAt(LastCharacterThatPlayed[charactertoaddID].GetComponent<C_Actor>().GetDataActor().idTraitEnCours);
                                    }
                                    else
                                    {
                                        LastCharacterThatPlayed[charactertoaddID].GetComponent<C_Actor>().currentPointTrait = 0;
                                    }
                                    if (Papote.GetComponent<C_Actor>().GetCurrentPointTrait() == 0 && Papote.GetComponent<C_Actor>().GetDataActor().idTraitEnCours != -1)
                                    {
                                        Papote.GetComponent<C_Actor>().HasTraited = false;
                                        Papote.GetComponent<C_Actor>().currentPointTrait = 0.5f;
                                        Papote.GetComponent<C_Actor>().GetDataActor().listNewTraits.RemoveAt(Papote.GetComponent<C_Actor>().GetDataActor().idTraitEnCours);
                                    }
                                    else
                                    {
                                        Papote.GetComponent<C_Actor>().currentPointTrait = 0;
                                    }
                                    GameManager.instance.PapoterID[0]--;
                                    MorganAPapoteAvecEsthela = false;
                                }
                                else if (LastCharacterThatPlayed[charactertoaddID] == Esthela && Papote == Nimu && NimuAPapoteAvecEsthela == false)
                                {
                                    if (LastCharacterThatPlayed[charactertoaddID].GetComponent<C_Actor>().GetCurrentPointTrait() == 0 && LastCharacterThatPlayed[charactertoaddID].GetComponent<C_Actor>().GetDataActor().idTraitEnCours != -1)
                                    {
                                        LastCharacterThatPlayed[charactertoaddID].GetComponent<C_Actor>().HasTraited = false;
                                        LastCharacterThatPlayed[charactertoaddID].GetComponent<C_Actor>().currentPointTrait = 0.5f;
                                        LastCharacterThatPlayed[charactertoaddID].GetComponent<C_Actor>().GetDataActor().listNewTraits.RemoveAt(LastCharacterThatPlayed[charactertoaddID].GetComponent<C_Actor>().GetDataActor().idTraitEnCours);
                                    }
                                    else
                                    {
                                        LastCharacterThatPlayed[charactertoaddID].GetComponent<C_Actor>().currentPointTrait = 0;
                                    }
                                    if (Papote.GetComponent<C_Actor>().GetCurrentPointTrait() == 0 && Papote.GetComponent<C_Actor>().GetDataActor().idTraitEnCours != -1)
                                    {
                                        Papote.GetComponent<C_Actor>().HasTraited = false;
                                        Papote.GetComponent<C_Actor>().currentPointTrait = 0.5f;
                                        Papote.GetComponent<C_Actor>().GetDataActor().listNewTraits.RemoveAt(Papote.GetComponent<C_Actor>().GetDataActor().idTraitEnCours);
                                    }
                                    else
                                    {
                                        Papote.GetComponent<C_Actor>().currentPointTrait = 0;
                                    }
                                    GameManager.instance.PapoterID[2]--;
                                    NimuAPapoteAvecEsthela = false;
                                }
                                else if (LastCharacterThatPlayed[charactertoaddID] == Nimu && Papote == Morgan && MorganAPapoteAvecNimu == false)
                                {
                                    if (LastCharacterThatPlayed[charactertoaddID].GetComponent<C_Actor>().GetCurrentPointTrait() == 0 && LastCharacterThatPlayed[charactertoaddID].GetComponent<C_Actor>().GetDataActor().idTraitEnCours != -1)
                                    {
                                        LastCharacterThatPlayed[charactertoaddID].GetComponent<C_Actor>().HasTraited = false;
                                        LastCharacterThatPlayed[charactertoaddID].GetComponent<C_Actor>().currentPointTrait = 0.5f;
                                        LastCharacterThatPlayed[charactertoaddID].GetComponent<C_Actor>().GetDataActor().listNewTraits.RemoveAt(LastCharacterThatPlayed[charactertoaddID].GetComponent<C_Actor>().GetDataActor().idTraitEnCours);
                                    }
                                    else
                                    {
                                        LastCharacterThatPlayed[charactertoaddID].GetComponent<C_Actor>().currentPointTrait = 0;
                                    }
                                    if (Papote.GetComponent<C_Actor>().GetCurrentPointTrait() == 0 && Papote.GetComponent<C_Actor>().GetDataActor().idTraitEnCours != -1)
                                    {
                                        Papote.GetComponent<C_Actor>().HasTraited = false;
                                        Papote.GetComponent<C_Actor>().currentPointTrait = 0.5f;
                                        Papote.GetComponent<C_Actor>().GetDataActor().listNewTraits.RemoveAt(Papote.GetComponent<C_Actor>().GetDataActor().idTraitEnCours);
                                    }
                                    else
                                    {
                                        Papote.GetComponent<C_Actor>().currentPointTrait = 0;
                                    }
                                    GameManager.instance.PapoterID[1]--;
                                    MorganAPapoteAvecNimu = false;
                                }
                                else if (LastCharacterThatPlayed[charactertoaddID] == Nimu && Papote == Esthela && NimuAPapoteAvecEsthela == false)
                                {
                                    if (LastCharacterThatPlayed[charactertoaddID].GetComponent<C_Actor>().GetCurrentPointTrait() == 0 && LastCharacterThatPlayed[charactertoaddID].GetComponent<C_Actor>().GetDataActor().idTraitEnCours != -1)
                                    {
                                        LastCharacterThatPlayed[charactertoaddID].GetComponent<C_Actor>().HasTraited = false;
                                        LastCharacterThatPlayed[charactertoaddID].GetComponent<C_Actor>().currentPointTrait = 0.5f;
                                        LastCharacterThatPlayed[charactertoaddID].GetComponent<C_Actor>().GetDataActor().listNewTraits.RemoveAt(LastCharacterThatPlayed[charactertoaddID].GetComponent<C_Actor>().GetDataActor().idTraitEnCours);
                                    }
                                    else
                                    {
                                        LastCharacterThatPlayed[charactertoaddID].GetComponent<C_Actor>().currentPointTrait = 0;
                                    }
                                    if (Papote.GetComponent<C_Actor>().GetCurrentPointTrait() == 0 && Papote.GetComponent<C_Actor>().GetDataActor().idTraitEnCours != -1)
                                    {
                                        Papote.GetComponent<C_Actor>().HasTraited = false;
                                        Papote.GetComponent<C_Actor>().currentPointTrait = 0.5f;
                                        Papote.GetComponent<C_Actor>().GetDataActor().listNewTraits.RemoveAt(Papote.GetComponent<C_Actor>().GetDataActor().idTraitEnCours);
                                    }
                                    else
                                    {
                                        Papote.GetComponent<C_Actor>().currentPointTrait = 0;
                                    }
                                    GameManager.instance.PapoterID[2]--;
                                    NimuAPapoteAvecEsthela = false;
                                }
                            }
                            ActivateTreeCharacterChoice();
                            for (int y = 0; y < characters.Count; y++)
                            {
                                TreeParent.transform.GetChild(y).GetChild(0).GetChild(0).GetComponent<Button>().enabled = false;
                            }
                            //TreeParent.SetActive(false);
                            ActionsParents.SetActive(true);


                            Es.SetSelectedGameObject(LastAction[actiontoaddID]);
                            updateButton();
                            LastAction.RemoveAt(actiontoaddID);
                            actiontoaddID--;
                        }
                    }
                }
            }
           
        }
    }
    }
