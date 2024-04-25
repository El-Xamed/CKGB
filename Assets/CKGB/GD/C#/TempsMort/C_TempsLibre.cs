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
    [SerializeField] bool canTalk=true;
    [SerializeField] List<string> allTalk = new List<string>();
    [SerializeField]int nbActort = 0;


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
    [SerializeField] C_Fiche f;

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
        //prepare les datas en recuperant le so temps libre, les personnages si ils existent pour simplifier la comm entre fonctions et scripts
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
        //lance l'intro dialogue
        GameManager.instance.textToWriteIn = naratteurText;
        StartCoroutine(StartIntro());
    }

    private void CharactersDataGet()
    {
        //prepare les datas du temps libre en modifiant les sprites et donnees pour passer du challenge a ce mode la
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
            characters[i].GetComponent<C_Actor>().charaTree = TreeParent.transform.GetChild(i).GetChild(0).GetComponent<C_Tree>();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void HideUI()
    {
        //cache les parents d'ui principaux
        FichePersoParent.SetActive(false);
        TreeParent.SetActive(false);
        ActionsParents.SetActive(false);
    }
    public void GoToActions()
    {
       //ouvre le menu de choix parmi les 3 actions
        if(actorActif.GetComponent<C_Actor>().HasPlayed==false)
        {

            charactertoaddID++;
            LastCharacterThatPlayed.Add(actorActif);
            //ajoute le personnage actif a la liste de tout les personnages ayant deja joues
            for (int i = 0; i < characters.Count; i++)
            {
                TreeParent.transform.GetChild(i).GetChild(0).GetChild(0).GetComponent<Button>().enabled = false;
            }
            //TreeParent.SetActive(false);
            ActionsParents.SetActive(true);
            //change le curseur en fonction du personnage actif
            PapoterButton.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = actorActif.GetComponent<C_Actor>().GetDataActor().smaller;
            ObserverButton.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = actorActif.GetComponent<C_Actor>().GetDataActor().smaller;
            RevasserButton.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = actorActif.GetComponent<C_Actor>().GetDataActor().smaller;
            Es.SetSelectedGameObject(actions[0]);
            updateButton();

            
            allTalk.Clear();
            nbActort = 0;
            //liste toutes les possibilites d avec qui le personnage actif peut discuter et dans quel ordre
            for (int i=0;i<characters.Count;i++)
            {
                if(actorActif.name!=characters[i].name)
                {
                    allTalk.Add(actorActif.name + "APapoteAvec" + characters[i].name);
                    allTalk.Add(characters[i].name + "APapoteAvec" + actorActif.name);
                }
            }
            //compare ensuite si iel a deja parle avec tout les personnages possibles ou non
            for(int i=0;i< allTalk.Count;i++)
            {
               
                if (allTalk[i] == "MorganAPapoteAvecEsthela"&&MorganAPapoteAvecEsthela)
                {
                    nbActort++;
                }
                if (allTalk[i] == "MorganAPapoteAvecNimu" && MorganAPapoteAvecNimu)
                {
                    Debug.Log("test si le personnage a deja parlé avec Morgan ou Nimu en etant l un ou l aut");
                    nbActort++;
                }
                if (allTalk[i] =="NimuAPapoteAvecEsthela" && NimuAPapoteAvecEsthela)
                {
                    nbActort++;
                }
            }
            //condamne ou non le bouton papoter
            if(nbActort==characterNB)
            {
                PapoterButton.GetComponent<Image>().color = Color.gray;
                canTalk = false;
            }
            else
            {
                PapoterButton.GetComponent<Image>().color = Color.white;
                canTalk = true;
            }
        }
       
    }
    public void ActivateTreeCharacterChoice()
    {
        //active l arbre de choix de personnages avec les boutons de choix de quel perso va jouer
        TreeParent.SetActive(true);
        for (int i = 0; i < characters.Count; i++)
        {
            TreeParent.transform.GetChild(i).GetChild(0).GetChild(0).gameObject.SetActive(true);
            TreeParent.transform.GetChild(i).GetChild(0).GetChild(1).gameObject.SetActive(false);
        }
    }
    public void ActivateTreePapotageChoice()
    {
        //active l arbre de choix de personnages avec les boutons de choix de quel perso va papoter avec le perso actif
        TreeParent.SetActive(true);
        for (int i=0;i<characters.Count; i++)
        {
            TreeParent.transform.GetChild(i).GetChild(0).GetChild(0).gameObject.SetActive(false);
            TreeParent.transform.GetChild(i).GetChild(0).GetChild(1).gameObject.SetActive(true);
        }
        
       
    }
    private void initiateTMvariables()
    {
        //genere les valeurs des so en jeu et les prefabs precedemment renseigne dans la world map 
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
                        thisActor.GetComponent<C_Actor>().BulleHautGauche.SetActive(true);
                        thisActor.GetComponent<C_Actor>().BulleHautDroite.SetActive(true);
                        thisActor.GetComponent<C_Actor>().BulleBasGauche.SetActive(true);
                        thisActor.GetComponent<C_Actor>().BulleBasDroite.SetActive(true);
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
            //ajoute des fonctions pour le bouton A pour passer ou non un dialogue
            characters[i].GetComponent<C_Actor>().txtHautGauche.GetComponent<TextAnimatorPlayer>().onTypewriterStart.AddListener(() => SetCanContinueToNo());
            characters[i].GetComponent<C_Actor>().txtHautDroite.GetComponent<TextAnimatorPlayer>().onTypewriterStart.AddListener(() => SetCanContinueToNo());
            characters[i].GetComponent<C_Actor>().txtBasGauche.GetComponent<TextAnimatorPlayer>().onTypewriterStart.AddListener(() => SetCanContinueToNo());
            characters[i].GetComponent<C_Actor>().txtBasDroite.GetComponent<TextAnimatorPlayer>().onTypewriterStart.AddListener(() => SetCanContinueToNo());

            characters[i].GetComponent<C_Actor>().txtHautGauche.GetComponent<TextAnimatorPlayer>().onTextShowed.AddListener(() => SetCanContinueToYes());
            characters[i].GetComponent<C_Actor>().txtHautDroite.GetComponent<TextAnimatorPlayer>().onTextShowed.AddListener(() => SetCanContinueToYes());
            characters[i].GetComponent<C_Actor>().txtBasGauche.GetComponent<TextAnimatorPlayer>().onTextShowed.AddListener(() => SetCanContinueToYes());
            characters[i].GetComponent<C_Actor>().txtBasDroite.GetComponent<TextAnimatorPlayer>().onTextShowed.AddListener(() => SetCanContinueToYes());
            characters[i].GetComponent<C_Actor>().GetDataActor().characterTree.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => GoToActions());
        }
        naratteurText.GetComponent<TextAnimatorPlayer>().onTextShowed.AddListener(() => SetCanContinueToYes());
        naratteurText.GetComponent<TextAnimatorPlayer>().onTypewriterStart.AddListener(() => SetCanContinueToNo());
    }
    IEnumerator StartIntro()
    {
        GameManager.instance.transform.GetChild(1).gameObject.SetActive(true); 
        GameManager.instance.TS_flanel.GetComponent<Animator>().SetTrigger("Open"); 
        yield return new WaitForSeconds(1f);
        GameManager.instance.transform.GetChild(1).gameObject.SetActive(false);
        GameManager.instance.TL_anim.GetComponentInChildren<Animator>().SetTrigger("triggerTL");
        yield return new WaitForSeconds(2f);
        Cine.GetComponent<Animator>().SetBool("IsCinema", true);
        yield return new WaitForSeconds(0.8f);
        GameManager.instance.EnterDialogueMode(_intro);
        GameManager.instance.TL_anim.GetComponentInChildren<Animator>().ResetTrigger("triggerTL");
    }
    public void StartTempsMort(string name)
    {
        StartCoroutine(TempsMortUnleashed());
    }
    IEnumerator TempsMortUnleashed()
    {
        //reactive l ui apres l intro 
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
        //check si l histoire continue ou pas
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
        //desactive si il existe le curseur du bouton precedent avant de le changer
        if (currentButton.transform.GetChild(0) != null)
        {
            currentButton.transform.GetChild(0).gameObject.SetActive(false);
        }
        //enleve l animation de hover sur l arbre de choix de perso
        for(int i=0;i<characters.Count;i++)
        {
            TreeParent.transform.GetChild(i).GetComponent<Animator>().SetBool("IsHover", false);
        }
        //check la nouvelle valeur du bouton en hover
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
                PapoterButton.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = actorActif.GetComponent<C_Actor>().GetDataActor().smaller;
                ObserverButton.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = actorActif.GetComponent<C_Actor>().GetDataActor().smaller;
                RevasserButton.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = actorActif.GetComponent<C_Actor>().GetDataActor().smaller;
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
        f.Reduite.SetActive(true);
        f.Agrandi1.SetActive(false);
        f.Agrandi2.SetActive(false);
        DisplayFicheInfos();
    }
    public void AfficherGrandeFiche()
    {
        FichePersoParent.SetActive(true);
        f.Reduite.SetActive(false);
        f.Agrandi1.SetActive(true);
        f.Agrandi2.SetActive(false);
        DisplayFicheInfos();
    }
    public void DisplayFicheInfos()
    {
        if(f.Reduite.activeSelf == true)
        {
            f.r_PP.GetComponent<Image>().sprite = actorActif.GetComponent<C_Actor>().GetDataActor().ProfilPhoto;
            f.r_energy.text = "Energie : " + actorActif.GetComponent<C_Actor>().GetDataActor().energyMax;
            f.r_calm.text = "Calme : " + actorActif.GetComponent<C_Actor>().GetDataActor().stressMax;
            f.r_name.text = actorActif.GetComponent<C_Actor>().GetDataActor().name;
            f.r_ptstrait.text = "Pts de trait : " + actorActif.GetComponent<C_Actor>().GetCurrentPointTrait();
            f.r_description.text = actorActif.GetComponent<C_Actor>().GetDataActor().miniDescription;
            if(actorActif.GetComponent<C_Actor>().HasObserved)
            {
                f.r_menergy.SetActive(true);
            }
            else { f.r_menergy.SetActive(false); }
            if (actorActif.GetComponent<C_Actor>().HasRevassed)
            {
                f.r_mcalm.SetActive(true);
            }
            else { f.r_mcalm.SetActive(false); }
            if (actorActif.GetComponent<C_Actor>().HasPapoted)
            {
                f.r_mtraits.text = "+" + actorActif.GetComponent<C_Actor>().GetCurrentPointTrait();
                if (f.r_mtraits.text == "+0.5")
                {
                    f.r_mtraits.gameObject.SetActive(true);
                }
                else if (f.r_mtraits.text == "+0")
                {
                    f.r_mtraits.text = "+1";
                    f.r_mtraits.gameObject.SetActive(true);
                }
            }
            else if(actorActif.GetComponent<C_Actor>().HasPapoted&& actorActif.GetComponent<C_Actor>().HasTraited)
            {
                f.r_new.SetActive(true);
            }
            else { f.r_mtraits.gameObject.SetActive(false); f.r_new.SetActive(false); }

        }
        if (f.Agrandi1.activeSelf == true)
        {
            f.g1_PP.GetComponent<Image>().sprite = actorActif.GetComponent<C_Actor>().GetDataActor().ProfilPhoto;
            f.g1_energy.text = "Energie : " + actorActif.GetComponent<C_Actor>().GetDataActor().energyMax;
            f.g1_calm.text = "Calme : " + actorActif.GetComponent<C_Actor>().GetDataActor().stressMax;
            f.g1_name.text = actorActif.GetComponent<C_Actor>().GetDataActor().name;
            f.g1_description.text = actorActif.GetComponent<C_Actor>().GetDataActor().Description;
            if (actorActif.GetComponent<C_Actor>().HasObserved||currentButton==ObserverButton)
            {
                f.g1_menergy.SetActive(true);
            }
            else { f.g1_menergy.SetActive(false); }
            if (actorActif.GetComponent<C_Actor>().HasRevassed||currentButton==RevasserButton)
            {
                f.g1_mcalm.SetActive(true);
            }
            else { f.g1_mcalm.SetActive(false); }
            if (actorActif.GetComponent<C_Actor>().HasTraited||currentButton==PapoterButton&& actorActif.GetComponent<C_Actor>().GetCurrentPointTrait()+0.5f==1)
            {
                f.g1_new.SetActive(true);
            }
            else { f.g1_new.SetActive(false); }
        }
        if (f.Agrandi2.activeSelf == true)
        {
            f.g2_PP.GetComponent<Image>().sprite = actorActif.GetComponent<C_Actor>().GetDataActor().ProfilPhoto;
            f.g2_name.text = actorActif.GetComponent<C_Actor>().GetDataActor().name;
            f.g2_ptstrait.text = "Pts de trait : " + actorActif.GetComponent<C_Actor>().GetCurrentPointTrait();
            string listtrait = "Liste Traits : ";
            for (int y = 0; y < actorActif.GetComponent<C_Actor>().GetDataActor().listNewTraits.Count; y++)
            {
                if(actorActif.GetComponent<C_Actor>().GetDataActor().listNewTraits[y]!=null)
                {
                    listtrait +="\n" + actorActif.GetComponent<C_Actor>().GetDataActor().listNewTraits[y].buttonText;
                }   
            }
            f.g2_traitlist.text = listtrait;
            if (actorActif.GetComponent<C_Actor>().HasPapoted)
            {
                f.g2_mtraits.text = "+" + actorActif.GetComponent<C_Actor>().GetCurrentPointTrait();
                if(f.g2_mtraits.text=="+0.5")
                {
                    f.g2_mtraits.gameObject.SetActive(true);
                }
                else if(f.g2_mtraits.text == "+0")
                {
                    f.g2_mtraits.text = "+1";
                    f.g2_mtraits.gameObject.SetActive(true);
                }

            }
            else if (currentButton == PapoterButton)
            {
               f.g2_mtraits.text = "+" + actorActif.GetComponent<C_Actor>().GetCurrentPointTrait()+0.5f;
               f.g2_mtraits.gameObject.SetActive(true);
            }
            else if (actorActif.GetComponent<C_Actor>().HasPapoted && actorActif.GetComponent<C_Actor>().HasTraited || currentButton == PapoterButton)
            {
               f.g2_new.gameObject.SetActive(true);
            }
            else { f.g2_mtraits.gameObject.SetActive(false); f.g2_new.gameObject.SetActive(false); }
        }
    }
    public void SwitchCharacterCard(InputAction.CallbackContext context)
    {
        if (!context.performed) { return; }

        if (context.performed)
        {
            Debug.Log("switch card");
            if(f.Agrandi1.activeSelf==true)
            {
                f.Agrandi2.SetActive(true);
                f.Agrandi1.SetActive(false);
                DisplayFicheInfos();
                Debug.Log("Card 2/2");
            }
            else if (f.Agrandi2.activeSelf == true)
            {
                f.Agrandi2.SetActive(false);
                f.Agrandi1.SetActive(true);
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
                characters[i].GetComponent<C_Actor>().charaTree.transform.GetChild(0).GetComponent<Image>().color = Color.gray;
                characters[i].GetComponent<C_Actor>().charaTree.transform.GetChild(0).GetChild(0).GetChild(1).gameObject.SetActive(false);
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
                characters[i].GetComponent<C_Actor>().charaTree.transform.GetChild(0).GetComponent<Image>().color = Color.gray;
                characters[i].GetComponent<C_Actor>().charaTree.transform.GetChild(0).GetChild(0).GetChild(1).gameObject.SetActive(false);
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
        if(canTalk)
        {
            foreach (var chara in characters)
            {
                if (chara==actorActif)
                {
                    chara.GetComponent<C_Actor>().charaTree.GetComponent<C_Tree>().p_button.GetComponent<Image>().color = Color.gray;
                }
                else
                {
                    chara.GetComponent<C_Actor>().charaTree.GetComponent<C_Tree>().p_button.GetComponent<Image>().color = Color.white;
                }

            }
            ActivateTreePapotageChoice();
            Es.SetSelectedGameObject(TreeParent.transform.GetChild(0).GetChild(0).GetChild(1).gameObject);
            ActionsParents.SetActive(false);

            updateButton();
            //traitpoint
            actiontoaddID++;
            LastAction.Add(PapoterButton);
        }
       
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
                characters[i].GetComponent<C_Actor>().charaTree.transform.GetChild(0).GetComponent<Image>().color = Color.gray;
                characters[i].GetComponent<C_Actor>().charaTree.transform.GetChild(0).GetChild(0).GetChild(1).gameObject.SetActive(false);
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
        Challenge(named);
    }
    IEnumerator Challenge(string scenename)
    {
        GameManager.instance.transform.GetChild(1).gameObject.SetActive(true);
        GameManager.instance.TS_flanel.GetComponent<Animator>().SetTrigger("Open");
        yield return new WaitForSeconds(1f);
        GameManager.instance.transform.GetChild(1).gameObject.SetActive(false);
        SceneManager.LoadScene(scenename);
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
                if(ChallengeButton.activeSelf==true)
                {
                    ChallengeButton.SetActive(false);
                }
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
                    LastCharacterThatPlayed[charactertoaddID].GetComponent<C_Actor>().HasPlayed = false;
                    LastCharacterThatPlayed[charactertoaddID].GetComponent<C_Actor>().charaTree.transform.GetChild(0).GetComponent<Image>().color = Color.white;
                    LastCharacterThatPlayed[charactertoaddID].GetComponent<C_Actor>().charaTree.transform.GetChild(0).GetChild(0).GetChild(1).gameObject.SetActive(true);
                    LastCharacterThatPlayed.RemoveAt(charactertoaddID);
                    charactertoaddID--;
                    ActionsParents.SetActive(false);
                    AfficherFichereduite();
                }
                else if (!ActionsParents.activeSelf && actiontoaddID != -1)
                {
                    for (int i = 0; i < characters.Count; i++)
                    {
                        if (!TreeParent.transform.GetChild(i).GetChild(0).GetChild(1).gameObject.activeSelf && actiontoaddID != -1)
                        {
                            for (int y = 0; y < characters.Count; y++)
                            {
                                TreeParent.transform.GetChild(y).GetChild(0).GetChild(0).GetComponent<Button>().enabled = false;
                            }
                            //TreeParent.SetActive(false);
                            ActionsParents.SetActive(true);

                            Debug.Log(LastAction[actiontoaddID]);
                            Es.SetSelectedGameObject(LastAction[actiontoaddID]);
                            updateButton();
                            LastAction.RemoveAt(actiontoaddID);
                            actiontoaddID--;
                        }
                        else if(TreeParent.transform.GetChild(i).GetChild(0).GetChild(1).gameObject.activeSelf == true && actiontoaddID != -1)
                        {
                            for (int y = 0; y < characters.Count; y++)
                            {
                                TreeParent.transform.GetChild(y).GetChild(0).GetChild(1).gameObject.SetActive(false);
                                TreeParent.transform.GetChild(y).GetChild(0).GetChild(0).gameObject.SetActive(true);
                                TreeParent.transform.GetChild(y).GetChild(0).GetChild(0).GetComponent<Button>().enabled = false;
                            }
                            //TreeParent.SetActive(false);
                            ActionsParents.SetActive(true);

                            Debug.Log(LastAction[actiontoaddID]);
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
