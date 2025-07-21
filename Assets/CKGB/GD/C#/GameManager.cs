using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public enum EActorClass
{
    Koolkid, Grandma, Clown
}

public class GameManager : MonoBehaviour
{
    //Pour le dev.
    [Header("Parametre de dev")]
    [Tooltip("Ceci est un param�tre de dev (Paul) ce dernier � pour objectif de rediriger correctement les object en question pour la cr�ation de l'�quipe.")]
    [SerializeField]
    List<GameObject> ressourceActor = new List<GameObject>();

    #region Variables
    public static GameManager instance;
    //R�cup�ration en variable qui apparait dans l'inspector.
    [SerializeField]
    List<EActorClass> myActor = new List<EActorClass>();

    [Header("Map")]
    [SerializeField]
    int[] niveauTermine;
    [SerializeField] public int WorldcurrentPoint = 0;
    [SerializeField] public int WorldstartPoint = 0;

    [SerializeField] public List<C_destination> levels = new List<C_destination>();

    [SerializeField]
    List<GameObject> team = new List<GameObject>();

    //Variable pour les challenge. DOIT RESTER CACHE C'EST UNE INFORMATION QUI RECUPERE SUR LA WORLDMAP AVANT DE LANCER LE NIVEAU.
    List<int> initialPlayerPositionOnThisDestination;

    //Information qu'il r�cup�re pour le Temps mort / Challenge.
    public SO_TempsMort currentTM = null;
    public SO_Challenge currentC = null;

    public SO_TempsMort TM1;
    public SO_Challenge Tuto;

    [Header("Dialogue")]
    //zone d�di�e aux  dialogues
    [SerializeField] public GameObject blackscreen;
    [SerializeField] public Story currentStory;
    [SerializeField] public C_Actor currentTalkingCharacter;
    [SerializeField] public int[] RevasserID;
    [SerializeField] public int RespirerID;
    [SerializeField] public int[] PapoterID;
    [SerializeField] public TMP_Text textToWriteIn;
    [SerializeField] public TextAsset[]papotage;
    public TextAsset currentTextAsset;
    public RuntimeAnimatorController[] PapotageAnimPatern;
    [SerializeField] public bool isDialoguing = false;
    [SerializeField] GameObject CharacterTalking;


    private const string Bulle_Tag = "Bulle";
    private const string anim_Tag = "Anim";
    private const string emotion_Tag = "emot";
    private const string Character_Tag = "Character";
    private const string Type_Tag = "Type";

    public C_TempsLibre TM;
    public C_Worldmap W;
    public C_Challenge C;
    public GameObject lastButton;

    [Header("Transition")]
    [SerializeField] Animator flanel;
    [SerializeField] Animator maskRond;
    [SerializeField] Animator softBlackSwipe;
    public GameObject TL_anim;
    public GameObject TS_flanel;
    public GameObject TS_maskRond;
    public GameObject TS_softblackswipe;


    EventSystem eventSystem;

    #endregion

    #region Menu Pause/Options
    [Header("Pause/Options")]
    [SerializeField] GameObject pauseBackground;
    [SerializeField] GameObject optionsMenu;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject recommencerButton;
    [SerializeField] Toggle baseToggle;
    [SerializeField] GameObject reprendre;
    [SerializeField] GameObject PauseParent;
    public static bool onPause;


    #endregion

    private void Awake()
    {
        #region Singleton
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
            
        #endregion

        DontDestroyOnLoad(gameObject);

        //Renseigne l'eventSystem.
        eventSystem = gameObject.GetComponentInChildren<EventSystem>();
    }

    private void Start()
    {
        if (FindObjectOfType<C_TempsLibre>() != null)
        {
            TM = FindObjectOfType<C_TempsLibre>();
        }
        if (FindObjectOfType<C_Challenge>() != null)
        {
            C = FindObjectOfType<C_Challenge>();
        }

        //Pour créer l'équipe.
        SetUpTeam();

        //Cache les paramètres
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(false);
    }

    #region Mes Fonctions

    void SetUpTeam()
    {
        foreach (EActorClass thisActor in myActor)
        {
            //D�finition des acteurs dans une nouvelle list par l'enum.
            switch (thisActor)
            {
                //R�cup�ration automatique dans le dossier Resources.
                case EActorClass.Koolkid:
                    if (ressourceActor[0])
                    {
                        GameObject newActor = Instantiate(ressourceActor[0], transform);
                        team.Add(newActor);
                    }
                    else Debug.LogWarning("Il n'y a pas de redirection pour cette objet.");
                    break;
                case EActorClass.Clown:
                    if (ressourceActor[1])
                    {
                        GameObject newActor = Instantiate(ressourceActor[1], transform);
                        team.Add(newActor);
                    }
                    else Debug.LogWarning("Il n'y a pas de redirection pour cette objet.");
                    break;
                case EActorClass.Grandma:
                    if (ressourceActor[2])
                    {
                        GameObject newActor = Instantiate(ressourceActor[2], transform);
                        team.Add(newActor);
                    }
                    else Debug.LogWarning("Il n'y a pas de redirection pour cette objet.");
                    break;
            }
        }
    }

    //Pour set le premier bouton selectione.
    public void SetFirtButton(GameObject thisbutton)
    {
        eventSystem.SetSelectedGameObject(thisbutton);
    }
    #endregion

    #region Pour la WorldMap
    public void SetInitialPlayerPosition(List<int> newPosition)
    {
        initialPlayerPositionOnThisDestination = newPosition;
    }

    public void SetDataLevel(SO_TempsMort dataTM,SO_Challenge dataC)
    {
        currentTM = dataTM;
        currentC = dataC;

        Debug.Log(currentTM);
        Debug.Log(currentC);
    }

    public SO_TempsMort GetDataTempsMort()
    {
        return currentTM;
    }

    public SO_Challenge GetDataChallenge()
    {
        return currentC;
    }
    #endregion

    #region Pour le temps mort/Challenge

    public List<GameObject> GetTeam()
    {
        return team;
    }

    public List<GameObject> GetRessournce()
    {
        return ressourceActor;
    }

    public List<int> GetInitialPlayerPosition()
    {
        return initialPlayerPositionOnThisDestination;
    }

    //Update toutes les positions des acteurs pour le challenge.
    public void UpdateInitialPlayerPosition()
    {
        for (int i = 0; i < team.Count -1; i++)
        {
            //team[i].GetComponent<C_Actor>().SetPosition(initialPlayerPositionOnThisDestination[i]);
        }
    }
    #region Dialogues
    public void EnterDialogueMode(TextAsset InkJSON)
    {
        Debug.Log(InkJSON.name);
        currentTextAsset = InkJSON;
        currentStory = new Story(InkJSON.text);
        currentStory.BindExternalFunction("Trigger", (string name) => { TM.Trigger(name); });
        isDialoguing = true;
        if(TM!=null)
        {
            if (InkJSON.name == "OutroTM2A" || InkJSON.name == "OutroTM2B" || InkJSON.name == "OutroTM1" || InkJSON.name == "OutroTM3")
            {
                currentStory.BindExternalFunction("StartChallenge", (string name) => { TM.GoChallenge(name); });
            }
            if (InkJSON.name == "IntroTM2A" || InkJSON.name == "IntroTM2B" || InkJSON.name == "IntroTM1" || InkJSON.name == "IntroTM3")
            {
                currentStory.BindExternalFunction("StartTM", (string name) => { TM.StartTempsMort(name); });
            }
            if (InkJSON.name == "RevasserEsthela" || InkJSON.name == "RevasserMorgan" || InkJSON.name == "RevasserNimu")
            {
                currentStory.BindExternalFunction("RetourAuTMAfterRevasser", (string name) => { TM.RetourAuTMAfterRevasser(name); });
                if (TM.actorActif == TM.Morgan)
                {
                    currentStory.variablesState["IDrevasser"] = RevasserID[0];
                }
                else if (TM.actorActif == TM.Nimu)
                {
                    currentStory.variablesState["IDrevasser"] = RevasserID[1];
                }
                else if (TM.actorActif == TM.Esthela)
                {
                    currentStory.variablesState["IDrevasser"] = RevasserID[2];
                }

            }
            if (InkJSON.name == TM.Observage.name)
            {
                Debug.Log("Observer ID " + RespirerID);

                currentStory.variablesState["IDobserver"] = RespirerID;
                currentStory.BindExternalFunction("RetourAuTMAfterRespirer", (string name) => { TM.RetourAuTMAfterRespirer(name); });
            }
            if (InkJSON.name == "PapoterMorganEsthela" || InkJSON.name == "PapoterMorganNimu" || InkJSON.name == "PapoterNimuEsthela")
            {
                currentStory.BindExternalFunction("RetourAuTMAfterPapotage", (string name) => { TM.RetourAuTMAfterPapotage(name); });
                if (TM.actorActif.name == "Morgan" && TM.Papote.name == "Esthela")
                {
                    currentStory.variablesState["IdPapoter"] = PapoterID[0];
                }
                else if (TM.actorActif.name == "Morgan" && TM.Papote.name == "Nimu")
                {
                    currentStory.variablesState["IdPapoter"] = PapoterID[1];
                }
                else if (TM.actorActif.name == "Esthela" && TM.Papote.name == "Morgan")
                {
                    currentStory.variablesState["IdPapoter"] = PapoterID[0];
                }
                else if (TM.actorActif.name == "Esthela" && TM.Papote.name == "Nimu")
                {
                    currentStory.variablesState["IdPapoter"] = PapoterID[2];
                }
                else if (TM.actorActif.name == "Nimu" && TM.Papote.name == "Morgan")
                {
                    currentStory.variablesState["IdPapoter"] = PapoterID[1];
                }
                else if (TM.actorActif.name == "Nimu" && TM.Papote.name == "Esthela")
                {
                    currentStory.variablesState["IdPapoter"] = PapoterID[2];
                }
            }
         }
       
        if (InkJSON.name == "IntroC0" || InkJSON.name == "IntroC1" || InkJSON.name == "IntroC2" || InkJSON.name == "IntroC3")
        {
            currentStory.BindExternalFunction("StartChallenge", (string name) => {C.StartChallenge(name); });
        }
        if (InkJSON.name == "OutroC0" || InkJSON.name == "OutroC1" || InkJSON.name == "OutroC2A" || InkJSON.name == "OutroC3")
        {
            currentStory.BindExternalFunction("FinishChallenge", (string name) => { C.FinishChallenge(name); });

        }
        ContinueStory();
    }
    public void ContinueStory()
    {

        if (currentStory.canContinue)
        {
            
            string Contenue = currentStory.Continue();
            Debug.Log(currentStory.currentTags);
            HandleTags(currentStory.currentTags,Contenue);
        }
        else
        {
            ExitDialogueMode();
        }
    }

    //LES TAGS A ECRIRE DANS INK ET LEUR UTILITE SONT DEFINIS ICI
    private void HandleTags(List<string> currentTags,string text)
    {
        Debug.Log("Story " + text);

        foreach (string tag in currentTags)
        {
            string[] splitTag = tag.Split(':');
            if (splitTag.Length != 2)
            {
                Debug.Log("erreur Tag " + tag);
            }
            string tagKey = splitTag[0].Trim();
            string tagValue = splitTag[1].Trim();

            switch (tagKey)
            {

               
                case Bulle_Tag:
                    //Debug.Log("Bulle : " + tagValue);
                    switch (tagValue)
                    {
                        case "MorganHautGauche":
                            textToWriteIn.text = "";
                            if (textToWriteIn.transform.childCount==1)
                            {
                                textToWriteIn.transform.GetChild(0).gameObject.SetActive(false);
                            }
                            if (textToWriteIn.GetComponentInParent<Image>()!=null)
                            {
                                textToWriteIn.GetComponentInParent<Image>().enabled = false;
                            }
                            CleanEmot();
                            textToWriteIn = GameObject.Find("Morgan").GetComponent<C_Actor>().txtHautGauche;
                            GameObject.Find("Morgan").GetComponent<C_Actor>().BulleHautGauche.GetComponent<Image>().enabled = true;
                            if(SceneManager.GetActiveScene().name== "S_TempsLibre")
                            {
                                currentTalkingCharacter =TM.Morgan;
                            }
                            //Debug.Log(textToWriteIn.name);
                          //  textToWriteIn.text = text;
                            break;
                        case "MorganHautDroite":
                            textToWriteIn.text = "";
                            if (textToWriteIn.transform.childCount == 1)
                            {
                                textToWriteIn.transform.GetChild(0).gameObject.SetActive(false);
                            }
                            if (textToWriteIn.GetComponentInParent<Image>() != null)
                            {
                                textToWriteIn.GetComponentInParent<Image>().enabled = false;
                            }
                            CleanEmot();
                            textToWriteIn = GameObject.Find("Morgan").GetComponent<C_Actor>().txtHautDroite;
                            GameObject.Find("Morgan").GetComponent<C_Actor>().BulleHautDroite.GetComponent<Image>().enabled = true;
                            if (SceneManager.GetActiveScene().name == "S_TempsLibre")
                            {
                                currentTalkingCharacter = TM.Morgan;
                            }
                            //Debug.Log(textToWriteIn.name);
                          //  textToWriteIn.text = text;
                            break;
                        case "MorganBasGauche":
                            textToWriteIn.text = "";
                            if (textToWriteIn.transform.childCount == 1)
                            {
                                textToWriteIn.transform.GetChild(0).gameObject.SetActive(false);
                            }
                            if (textToWriteIn.GetComponentInParent<Image>() != null)
                            {
                                textToWriteIn.GetComponentInParent<Image>().enabled = false;
                            }
                            CleanEmot();
                            textToWriteIn = GameObject.Find("Morgan").GetComponent<C_Actor>().txtBasGauche;
                            GameObject.Find("Morgan").GetComponent<C_Actor>().BulleBasGauche.GetComponent<Image>().enabled = true;
                            if (SceneManager.GetActiveScene().name == "S_TempsLibre")
                            {
                                currentTalkingCharacter = TM.Morgan;
                            }
                            //Debug.Log(textToWriteIn.name);
                         //   textToWriteIn.text = text;
                            break;
                        case "MorganBasDroite":
                            textToWriteIn.text = "";
                            if (textToWriteIn.transform.childCount == 1)
                            {
                                textToWriteIn.transform.GetChild(0).gameObject.SetActive(false);
                            }
                            if (textToWriteIn.GetComponentInParent<Image>() != null)
                            {
                                textToWriteIn.GetComponentInParent<Image>().enabled = false;
                            }
                            CleanEmot();
                            textToWriteIn = GameObject.Find("Morgan").GetComponent<C_Actor>().txtBasDroite;
                            GameObject.Find("Morgan").GetComponent<C_Actor>().BulleBasDroite.GetComponent<Image>().enabled = true;
                            if (SceneManager.GetActiveScene().name == "S_TempsLibre")
                            {
                                currentTalkingCharacter = TM.Morgan;
                            }
                            //Debug.Log(textToWriteIn.name);
                          //  textToWriteIn.text = text;
                            break;

                        case "EsthelaHautGauche":
                            textToWriteIn.text = "";
                            if (textToWriteIn.transform.childCount == 1)
                            {
                                textToWriteIn.transform.GetChild(0).gameObject.SetActive(false);
                            }
                            if (textToWriteIn.GetComponentInParent<Image>() != null)
                            {
                                textToWriteIn.GetComponentInParent<Image>().enabled = false;
                            }
                            CleanEmot();
                            textToWriteIn = TM.Esthela.GetComponent<C_Actor>()?.txtHautGauche ?? TM.Esthela.GetComponent<C_Actor>().txtBasGauche;
                            GameObject.Find("Esthela").GetComponent<C_Actor>().BulleHautGauche.GetComponent<Image>().enabled = true;
                            if (SceneManager.GetActiveScene().name == "S_TempsLibre")
                            {
                                currentTalkingCharacter = TM.Esthela;
                            }
                            Debug.Log(textToWriteIn.name);
                           // textToWriteIn.text = text;
                            break;
                        case "EsthelaHautDroite":
                            textToWriteIn.text = "";
                            if (textToWriteIn.transform.childCount == 1)
                            {
                                textToWriteIn.transform.GetChild(0).gameObject.SetActive(false);
                            }
                            if (textToWriteIn.GetComponentInParent<Image>() != null)
                            {
                                textToWriteIn.GetComponentInParent<Image>().enabled = false;
                            }
                            CleanEmot();
                            textToWriteIn = GameObject.Find("Esthela").GetComponent<C_Actor>().txtHautDroite;
                            GameObject.Find("Esthela").GetComponent<C_Actor>().BulleHautDroite.GetComponent<Image>().enabled = true;
                            if (SceneManager.GetActiveScene().name == "S_TempsLibre")
                            {
                                currentTalkingCharacter = TM.Esthela;
                            }
                            Debug.Log(textToWriteIn.name);
                           // textToWriteIn.text = text;
                            break;
                        case "EsthelaBasGauche":
                            textToWriteIn.text = "";
                            if (textToWriteIn.transform.childCount == 1)
                            {
                                textToWriteIn.transform.GetChild(0).gameObject.SetActive(false);
                            }
                            if (textToWriteIn.GetComponentInParent<Image>() != null)
                            {
                                textToWriteIn.GetComponentInParent<Image>().enabled = false;
                            }
                            CleanEmot();
                            textToWriteIn = GameObject.Find("Esthela").GetComponent<C_Actor>().txtBasGauche;
                            GameObject.Find("Esthela").GetComponent<C_Actor>().BulleBasGauche.GetComponent<Image>().enabled = true;
                            if (SceneManager.GetActiveScene().name == "S_TempsLibre")
                            {
                                currentTalkingCharacter = TM.Esthela;
                            }
                            Debug.Log(textToWriteIn.name);
                          //  textToWriteIn.text = text;
                            break;
                        case "EsthelaBasDroite":
                            textToWriteIn.text = "";
                            if (textToWriteIn.transform.childCount == 1)
                            {
                                textToWriteIn.transform.GetChild(0).gameObject.SetActive(false);
                            }
                            if (textToWriteIn.GetComponentInParent<Image>() != null)
                            {
                                textToWriteIn.GetComponentInParent<Image>().enabled = false;
                            }
                            CleanEmot();
                            textToWriteIn = GameObject.Find("Esthela").GetComponent<C_Actor>().txtBasDroite;
                            GameObject.Find("Esthela").GetComponent<C_Actor>().BulleBasDroite.GetComponent<Image>().enabled = true;
                            if (SceneManager.GetActiveScene().name == "S_TempsLibre")
                            {
                                currentTalkingCharacter = TM.Esthela;
                            }
                            Debug.Log(textToWriteIn.name);
                            //textToWriteIn.text = text;
                            break;

                        case "NimuHautGauche":
                            textToWriteIn.text = "";
                            if (textToWriteIn.transform.childCount == 1)
                            {
                                textToWriteIn.transform.GetChild(0).gameObject.SetActive(false);
                            }
                            if (textToWriteIn.GetComponentInParent<Image>() != null)
                            {
                                textToWriteIn.GetComponentInParent<Image>().enabled = false;
                            }
                            CleanEmot();
                            textToWriteIn = GameObject.Find("Nimu").GetComponent<C_Actor>().txtHautGauche;
                            GameObject.Find("Nimu").GetComponent<C_Actor>().BulleHautGauche.GetComponent<Image>().enabled = true;
                            if (SceneManager.GetActiveScene().name == "S_TempsLibre")
                            {
                                currentTalkingCharacter = TM.Nimu;
                            }
                            //Debug.Log(textToWriteIn.name);
                           // textToWriteIn.text = text;
                            break;
                        case "NimuHautDroite":
                            textToWriteIn.text = "";
                            if (textToWriteIn.transform.childCount == 1)
                            {
                                textToWriteIn.transform.GetChild(0).gameObject.SetActive(false);
                            }
                            if (textToWriteIn.GetComponentInParent<Image>() != null)
                            {
                                textToWriteIn.GetComponentInParent<Image>().enabled = false;
                            }
                            CleanEmot();
                            textToWriteIn = GameObject.Find("Nimu").GetComponent<C_Actor>().txtHautDroite;
                            GameObject.Find("Nimu").GetComponent<C_Actor>().BulleHautDroite.GetComponent<Image>().enabled = true;
                            if (SceneManager.GetActiveScene().name == "S_TempsLibre")
                            {
                                currentTalkingCharacter = TM.Nimu;
                            }
                            //Debug.Log(textToWriteIn.name);
                           // textToWriteIn.text = text;
                            break;
                        case "NimuBasGauche":
                            textToWriteIn.text = "";
                            if (textToWriteIn.transform.childCount == 1)
                            {
                                textToWriteIn.transform.GetChild(0).gameObject.SetActive(false);
                            }
                            if (textToWriteIn.GetComponentInParent<Image>() != null)
                            {
                                textToWriteIn.GetComponentInParent<Image>().enabled = false;
                            }
                            CleanEmot();
                            textToWriteIn = GameObject.Find("Nimu").GetComponent<C_Actor>().txtBasGauche;
                            GameObject.Find("Nimu").GetComponent<C_Actor>().BulleBasGauche.GetComponent<Image>().enabled = true;
                            if (SceneManager.GetActiveScene().name == "S_TempsLibre")
                            {
                                currentTalkingCharacter = TM.Nimu;
                            }
                            //Debug.Log(textToWriteIn.name);
                            //textToWriteIn.text = text;
                            break;
                        case "NimuBasDroite":
                            textToWriteIn.text = "";
                            if (textToWriteIn.transform.childCount == 1)
                            {
                                textToWriteIn.transform.GetChild(0).gameObject.SetActive(false);
                            }
                            if (textToWriteIn.GetComponentInParent<Image>() != null)
                            {
                                textToWriteIn.GetComponentInParent<Image>().enabled = false;
                            }
                            CleanEmot();
                            textToWriteIn = GameObject.Find("Nimu").GetComponent<C_Actor>().txtBasDroite;
                            GameObject.Find("Nimu").GetComponent<C_Actor>().BulleBasDroite.GetComponent<Image>().enabled = true;
                            if (SceneManager.GetActiveScene().name == "S_TempsLibre")
                            {
                                currentTalkingCharacter = TM.Nimu;
                            }
                            //Debug.Log(textToWriteIn.name);
                            //textToWriteIn.text = text;
                            break;
                        case "Narrateur":
                            //Pour savoir dans quelle scene on est pour le narrateur.
                            if (SceneManager.GetActiveScene().name=="S_TempsLibre")
                            {
                                textToWriteIn.text = "";
                                if (textToWriteIn.transform.childCount == 1)
                                {
                                    textToWriteIn.transform.GetChild(0).gameObject.SetActive(false);
                                }
                                if (textToWriteIn.GetComponentInParent<Image>() != null)
                                {
                                    textToWriteIn.GetComponentInParent<Image>().enabled = false;
                                }
                                CleanEmot();
                                textToWriteIn = TM.naratteurText;
                                TM.NarrateurParent.GetComponent<Animator>().SetBool("Active", true);
                                Debug.Log(textToWriteIn.name);

                            }
                            else
                            {
                                //Vide le dialogue du narrateur.
                                textToWriteIn.text = "";

                                //Pour la petite flèche. A VOIR SI IL MARCHE BIEN.
                                if (textToWriteIn.transform.childCount == 1)
                                {
                                    textToWriteIn.transform.GetChild(0).gameObject.SetActive(false);
                                }
                                if (textToWriteIn.GetComponentInParent<Image>() != null)
                                {
                                    textToWriteIn.GetComponentInParent<Image>().enabled = false;
                                }

                                //Pour re-préciser l'endroit ou il faut écrire.
                                textToWriteIn = C.GetuiLogs().GetComponentInChildren<TMP_Text>();
                                if (textToWriteIn.transform.childCount == 1)
                                {
                                    textToWriteIn.transform.GetChild(0).gameObject.SetActive(true);
                                }
                                //Pour faire apparaitre le fond de texte.
                                C.GetuiLogs().gameObject.SetActive(true);
                                C.GetuiLogs().GetComponentInChildren<Image>().enabled = true;

                                Debug.Log(textToWriteIn.name);

                                //SFX
                                if (AudioManager.instanceAM)
                                {
                                    AudioManager.instanceAM.Play("Narrateur");
                                }
                            }
                            break;
                        default:
                            //textToWriteIn.text = text;
                            break;
                    }

                    break;
               
                case emotion_Tag:
                    switch(tagValue)
                    {
                        case "surprise":
                          
                                if(currentTalkingCharacter!=null)
                                {
                                    GameObject surprise = Instantiate(currentTalkingCharacter.GetComponent<C_Actor>().surprise, currentTalkingCharacter.GetComponent<C_Actor>().emotContainer.transform);
                                    surprise.GetComponent<RectTransform>().anchoredPosition = new Vector3(-84, 111, 0);
                                } 
                            
                            break;
                        case "question":
                           
                                if (currentTalkingCharacter != null)
                                {
                                    Debug.Log("questionMark");
                                    GameObject question = Instantiate(currentTalkingCharacter.GetComponent<C_Actor>().question, currentTalkingCharacter.GetComponent<C_Actor>().emotContainer.transform);
                                    question.GetComponent<RectTransform>().anchoredPosition = new Vector3(-65, 136, 0);
                                }
                            
                            break;
                        case "Dots":
                            if (SceneManager.GetActiveScene().name == "S_TempsLibre")
                            {
                                if (currentTalkingCharacter != null)
                                {
                                    GameObject Dots = Instantiate(currentTalkingCharacter.GetComponent<C_Actor>().Dots, currentTalkingCharacter.GetComponent<C_Actor>().emotContainer.transform);
                                    Dots.GetComponent<RectTransform>().anchoredPosition = new Vector3(-5.44f, 75, 0);
                                }
                            }
                            break;
                        case "Drop":
                            if (SceneManager.GetActiveScene().name == "S_TempsLibre")
                            {
                                if (currentTalkingCharacter != null)
                                {
                                    GameObject Drop = Instantiate(currentTalkingCharacter.GetComponent<C_Actor>().Drops, currentTalkingCharacter.GetComponent<C_Actor>().emotContainer.transform);
                                    Drop.GetComponent<RectTransform>().anchoredPosition = new Vector3(88, 280, 0);
                                }
                            }
                            break;
                        case "Sparkles":
                            if (SceneManager.GetActiveScene().name == "S_TempsLibre")
                            {
                                if (currentTalkingCharacter != null)
                                {
                                    GameObject Sparkles = Instantiate(currentTalkingCharacter.GetComponent<C_Actor>().Sparkles, currentTalkingCharacter.GetComponent<C_Actor>().emotContainer.transform);
                                    Sparkles.GetComponent<RectTransform>().anchoredPosition = new Vector3(88, 135, 0);
                                }
                            }
                            break;
                        case "Deception":
                            if (SceneManager.GetActiveScene().name == "S_TempsLibre")
                            {
                                if (currentTalkingCharacter != null)
                                {
                                    GameObject Deception = Instantiate(currentTalkingCharacter.GetComponent<C_Actor>().Deception, currentTalkingCharacter.GetComponent<C_Actor>().emotContainer.transform);
                                    Deception.GetComponent<RectTransform>().anchoredPosition = new Vector3(75, 190, 0);
                                }
                            }
                            break;
                        case "Anger":
                            if (SceneManager.GetActiveScene().name == "S_TempsLibre")
                            {
                                if (currentTalkingCharacter != null)
                                {
                                    GameObject Anger = Instantiate(currentTalkingCharacter.GetComponent<C_Actor>().Anger, currentTalkingCharacter.GetComponent<C_Actor>().emotContainer.transform);
                                    Anger.GetComponent<RectTransform>().anchoredPosition = new Vector3(63, 121, 0);
                                }
                            }
                            break;
                        case "JoyLeft":
                            if (SceneManager.GetActiveScene().name == "S_TempsLibre")
                            {
                                if (currentTalkingCharacter != null)
                                {
                                    GameObject JoyLeft = Instantiate(currentTalkingCharacter.GetComponent<C_Actor>().JoyLeft, currentTalkingCharacter.GetComponent<C_Actor>().emotContainer.transform);
                                    JoyLeft.GetComponent<RectTransform>().anchoredPosition = new Vector3(-148, 114, 0);
                                }
                            }
                            break;
                        case "Heart":
                            if (SceneManager.GetActiveScene().name == "S_TempsLibre")
                            {
                                if (currentTalkingCharacter != null)
                                {
                                    GameObject Heart = Instantiate(currentTalkingCharacter.GetComponent<C_Actor>().Heart, currentTalkingCharacter.GetComponent<C_Actor>().emotContainer.transform);
                                    Heart.GetComponent<RectTransform>().anchoredPosition = new Vector3(47, -58, 0);
                                }
                            }
                            break;
                        case "Rainbow":
                            if (SceneManager.GetActiveScene().name == "S_TempsLibre")
                            {
                                if (currentTalkingCharacter != null)
                                {
                                    GameObject Rainbow = Instantiate(currentTalkingCharacter.GetComponent<C_Actor>().Rainbow, currentTalkingCharacter.GetComponent<C_Actor>().emotContainer.transform);
                                    Rainbow.GetComponent<RectTransform>().anchoredPosition = new Vector3(-6.42f, 16, 0);
                                }
                            }
                            break;
                        case "BlackIn":
                            {
                                blackscreen = GameObject.Find("BlackScreenDialogue");
                                blackscreen.GetComponent<Animator>().SetTrigger("Go");
                                blackscreen.SetActive(true);
                                break;
                            }
                        case "BlackOut":
                            {
                                blackscreen.GetComponent<Animator>().SetTrigger("Go");
                                StartCoroutine("hideBlackScreen");
                                break;
                            }
                        case "walkMorgan":
                            {
                                if(SceneManager.GetActiveScene().name=="S_TempsLibre")
                                {
                                    TM.Morgan.GetComponent<C_Actor>().mainchild.GetComponent<Animator>().enabled=true;
                                }

                                break;
                            }
                        case "walkNimu":
                            {
                                if (SceneManager.GetActiveScene().name == "S_TempsLibre")
                                {
                                    TM.Nimu.GetComponent<C_Actor>().mainchild.GetComponent<Animator>().enabled = true;
                                }

                                break;
                            }
                        case "walkEsthela":
                            {
                                if (SceneManager.GetActiveScene().name == "S_TempsLibre")
                                {
                                    TM.Esthela.GetComponent<C_Actor>().mainchild.GetComponent<Animator>().enabled = true;
                                }

                                break;
                            }
                        case "StopWalkMorgan":
                            {
                                if (SceneManager.GetActiveScene().name == "S_TempsLibre")
                                {
                                    TM.Morgan.GetComponent<C_Actor>().mainchild.GetComponent<Animator>().enabled = false;
                                }

                                break;
                            }
                        case "StopWalkNimu":
                            {
                                if (SceneManager.GetActiveScene().name == "S_TempsLibre")
                                {
                                    TM.Nimu.GetComponent<C_Actor>().mainchild.GetComponent<Animator>().enabled = false;
                                }

                                break;
                            }
                        case "StopWalkEsthela":
                            {
                                if (SceneManager.GetActiveScene().name == "S_TempsLibre")
                                {
                                    TM.Esthela.GetComponent<C_Actor>().mainchild.GetComponent<Animator>().enabled = false;
                                }

                                break;
                            }
                        case "WalkAll":
                            {
                                if (SceneManager.GetActiveScene().name == "S_TempsLibre")
                                {
                                    for(int i=0;i<TM.characters.Count;i++)
                                    {
                                        TM.characters[i].GetComponent<C_Actor>().mainchild.GetComponent<Animator>().enabled = true;
                                    }

                                }
                                    break;
                            }
                        case "StopAll":
                            {
                                if (SceneManager.GetActiveScene().name == "S_TempsLibre")
                                {
                                    for (int i = 0; i < TM.characters.Count; i++)
                                    {
                                        TM.characters[i].GetComponent<C_Actor>().mainchild.GetComponent<Animator>().enabled = false;
                                    }

                                }
                                break; 
                            }
                    }
                    break;
                case Type_Tag:
                    /*Debug.Log("Type " + tagValue);
                    switch (tagValue)
                    {
                        case "pensee":
                            
                            if (textToWriteIn.GetComponentInParent<Image>() != null)
                            {
                                textToWriteIn.GetComponentInParent<Image>().sprite = textToWriteIn.GetComponentInParent<C_Actor>().GetDataActor().ThinkBulle;
                            }
                            break;
                        case "normal":
                            if (textToWriteIn.GetComponentInParent<Image>() != null)
                            {
                                textToWriteIn.GetComponentInParent<Image>().sprite = textToWriteIn.GetComponentInParent<C_Actor>().GetDataActor().NormalBulle;
                            }
                            break;
                    }*/
                    break;

                default:
                    //textToWriteIn.text = text;
                    break;

            }
        }
        if(TM!=null)
        {
            if (textToWriteIn != TM.naratteurText)
            {
                TM.NarrateurParent.GetComponent<Animator>().SetBool("Active", false);
            }
        }
       
      textToWriteIn.text = text;       
    }
    IEnumerator hideBlackScreen()
    {
        yield return new WaitForSeconds(1f);
        blackscreen.SetActive(false);
    }
    public void CleanEmot()
    {
        if (SceneManager.GetActiveScene().name == "S_TempsLibre")
        {
            if (currentTalkingCharacter != null)
            {
                for (int i = 0; i < currentTalkingCharacter.GetComponent<C_Actor>().emotContainer.transform.childCount; i++)
                {
                    Destroy(currentTalkingCharacter.GetComponent<C_Actor>().emotContainer.transform.GetChild(i).gameObject);
                }

            }

        }
    }
    public void ExitDialogueMode()
    {
        if (TM != null)
        {
            TM.NarrateurParent.GetComponent<Animator>().SetBool("Active", false);
        }

        if (textToWriteIn)
        {
            textToWriteIn.text = "";

            if (textToWriteIn.transform.childCount == 1)
            {
                textToWriteIn.transform.GetChild(0).gameObject.SetActive(false);
            }

            if (textToWriteIn.GetComponentInParent<Image>() != null)
            {
                textToWriteIn.GetComponentInParent<Image>().enabled = false;
            }
        }

        isDialoguing = false;
    }
    #endregion
    #endregion
   
    public void BackFromPause()
    {
        if (!onPause)
            return;

        if(optionsMenu.activeSelf)
        {
            optionsMenu.SetActive(false);
            pauseMenu.SetActive(true);
            if (SceneManager.GetActiveScene().name == "S_TempsLibre")
            {
                recommencerButton.SetActive(false);
                TM.Es.SetSelectedGameObject(GameManager.instance.pauseMenu.transform.GetChild(1).GetChild(0).gameObject);
                TM.updateButton();
            }
            else if (SceneManager.GetActiveScene().name == "S_Challenge")
            {
                recommencerButton.SetActive(true);
                C.GetEventSystem().SetSelectedGameObject(GameManager.instance.pauseMenu.transform.GetChild(1).GetChild(0).gameObject);
            }
            else if (SceneManager.GetActiveScene().name == "S_MainMenu")
            {
               
            }

        }
        else
        {
            pauseBackground.SetActive(false);
            pauseMenu.SetActive(false);
            if (SceneManager.GetActiveScene().name == "S_TempsLibre")
            {
                if (lastButton != null)
                {
                    TM.Es.SetSelectedGameObject(lastButton.gameObject);
                    TM.updateButton();
                }
                else
                    TM.Es.SetSelectedGameObject(TM.TreeParent.transform.GetChild(0).GetChild(0).GetChild(0).gameObject);
                
            }
            else if (SceneManager.GetActiveScene().name == "S_Challenge")
            {
                if (lastButton != null)
                {
                    C.GetEventSystem().SetSelectedGameObject(lastButton.gameObject); 
                }
            }
        }
        
    }
    public void ResetChallenge()
    {
       
            foreach (GameObject c in GetTeam())
            {
                c.transform.parent = transform;
            }
            Debug.Log("Reload");
            pauseBackground.SetActive(false);
        if(pauseMenu.activeSelf)
        {
            pauseMenu.SetActive(false);
        }
        if (optionsMenu.activeSelf)
        {
            optionsMenu.SetActive(false);
        }
        SceneManager.LoadScene("S_Challenge");
        
    }
    public void BackPauseMenu(InputAction.CallbackContext context)
    {
        if (!onPause) return;

        if (optionsMenu.activeSelf == true)
        {
            Debug.Log("back from options");
            BackFromPause();
        }
        else if (pauseMenu.activeSelf == true)
        {
            Debug.Log("back from pause");
            BackFromPause();
        }
        if(SceneManager.GetActiveScene().name=="S_Challenge")
        {
            if(C.GetEventSystem().currentSelectedGameObject!=null)
            {
                C.GetEventSystem().SetSelectedGameObject(lastButton);
            }
               
        }
        else if(SceneManager.GetActiveScene().name=="S_TempsLibre")
        {
            Debug.Log("back from tm");

            if (TM.Es.currentSelectedGameObject != null)
            {
                TM.Es.SetSelectedGameObject(lastButton);
            }
                
        }
    }
    public void OpenPause(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (pauseBackground.activeSelf == false)
            {
                onPause = true;

                if(!isDialoguing)
                {
                    pauseBackground.SetActive(true);
                    PauseParent.GetComponent<Animator>().SetTrigger("trigger");
                    pauseMenu.SetActive(true);
                    if (SceneManager.GetActiveScene().name == "S_Challenge")
                    {
                        if (C.GetInterface().GetCurrentInterface() != C_Interface.Interface.Neutre)
                        {
                            C.GetEventSystem().SetSelectedGameObject(null);
                        }
                        lastButton = C.GetEventSystem().currentSelectedGameObject;
                        C.GetEventSystem().SetSelectedGameObject(pauseMenu.transform.GetChild(1).GetChild(0).gameObject);
                        //recommencerButton.SetActive(false);
                    }
                    if (SceneManager.GetActiveScene().name == "S_TempsLibre")
                    {
                        TM.Es.SetSelectedGameObject(pauseMenu.transform.GetChild(1).GetChild(0).gameObject);
                        TM.updateButton();
                    }

                    //optionsParent.SetActive(true);
                    Debug.Log("Pause");
                }
                
            }
            else
            {
                BackFromPause();
            }

        }
    }
    public void OpenOptions()
    {
        pauseBackground.SetActive(true);
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(true);

        if (SceneManager.GetActiveScene().name == "S_TempsLibre")
        {
            TM.Es.SetSelectedGameObject(optionsMenu.transform.GetChild(2).gameObject);
            TM.updateButton();
        }
        if (SceneManager.GetActiveScene().name == "S_Challenge")
        {
            C.GetEventSystem().SetSelectedGameObject(optionsMenu.transform.GetChild(2).gameObject);
    
        }

        //Set le premier bouton des options.
        SetFirtButton(baseToggle.gameObject);
        Debug.Log("Options");
    }
    public EventSystem GetEventSystem()
    {
        return eventSystem;
    }

    #region Transition

    //Lance l'anim d'ouvertur + enregistrer la première fonction qui va etre utilisé.
    public void OpenTransitionFlannel()
    {
        flanel.SetTrigger("Open");
    }
    public void CloseTransitionFlannel()
    {
        flanel.SetTrigger("Close");
    }
    #endregion
}
