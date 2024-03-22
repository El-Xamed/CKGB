using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using Ink.Runtime;
using System;
using UnityEngine.UI;

public enum EActorClass
{
    Koolkid, Grandma, Clown
}

public class GameManager : MonoBehaviour
{
    //Pour le dev.
    [Header("Param�tre de dev")]
    [Tooltip("Ceci est un param�tre de dev (Paul) ce dernier � pour objectif de rediriger correctement les object en question pour la cr�ation de l'�quipe.")]
    [SerializeField]
    List<GameObject> ressourceActor = new List<GameObject>();

    #region Variables
    public static GameManager instance;

    [Header("Param�tre de dev")]
    //R�cup�ration en variable qui apparait dans l'inspector.
    [SerializeField]
    List<EActorClass> myActor = new List<EActorClass>();

    [SerializeField]
    int[] niveauTermine;

    [SerializeField]
    List<GameObject> team = new List<GameObject>();

    //Variable pour les challenge. DOIT RESTER CACHE C'EST UNE INFORMATION QUI RECUPERE SUR LA WORLDMAP AVANT DE LANCER LE NIVEAU.
    List<int> initialPlayerPositionOnThisDestination;

    //Information qu'il r�cup�re pour le Temps mort / Challenge.
    public SO_TempsMort currentTM = null;
    public SO_Challenge currentC = null;

    //zone d�di�e aux  dialogues
    [SerializeField] public Story currentStory;
    [SerializeField] public int[] RevasserID;
    [SerializeField] public int RespirerID;
    [SerializeField] public int[] PapoterID;
    [SerializeField] public TMP_Text textToWriteIn;
    [SerializeField] public TextAsset[]papotage;
    [SerializeField] public bool isDialoguing = false;
    [SerializeField] GameObject CharacterTalking;


    private const string Bulle_Tag = "Bulle";
    private const string anim_Tag = "Anim";
    private const string emotion_Tag = "emot";
    private const string Character_Tag = "Character";

    public C_TempsMort TM;
    #endregion

    private void Awake()
    {
        
        #region Singleton
        if (instance == null)
            instance = this;
        #endregion

        DontDestroyOnLoad(gameObject);

        
    }
    private void Start()
    {
        if (FindObjectOfType<C_TempsMort>() != null)
        {
            TM = FindObjectOfType<C_TempsMort>();
        }
        SetUpTeam();
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
                        Debug.Log(newActor.GetComponent<C_Actor>().GetDataActor());
                    }
                    else Debug.LogWarning("Il n'y a pas de redirection pour cette objet.");
                    break;
                case EActorClass.Clown:
                    if (ressourceActor[1])
                    {
                        GameObject newActor = Instantiate(ressourceActor[1], transform);
                        team.Add(newActor);
                        Debug.Log(newActor.GetComponent<C_Actor>().GetDataActor());
                    }
                    else Debug.LogWarning("Il n'y a pas de redirection pour cette objet.");
                    break;
                case EActorClass.Grandma:
                    if (ressourceActor[2])
                    {
                        GameObject newActor = Instantiate(ressourceActor[2], transform);
                        team.Add(newActor);
                        Debug.Log(newActor.GetComponent<C_Actor>().GetDataActor());
                    }
                    else Debug.LogWarning("Il n'y a pas de redirection pour cette objet.");
                    break;
            }
        }
    }

    public void ChangeActionMap(string actionMap)
    {
        GetComponent<PlayerInput>().SwitchCurrentActionMap(actionMap);
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

    public void EnterDialogueMode(TextAsset InkJSON)
    {
        Debug.Log(InkJSON.name);
        currentStory = new Story(InkJSON.text);

        isDialoguing = true;
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
            if(TM.actorActif==TM.characters[0])
            {
                currentStory.variablesState["IDrevasser"] = RevasserID[0];
            }
            else if (TM.actorActif == TM.characters[1])
            {
                currentStory.variablesState["IDrevasser"] = RevasserID[1];
            }
            else if (TM.actorActif == TM.characters[2])
            {
                currentStory.variablesState["IDrevasser"] = RevasserID[2];
            }

        }
        if (InkJSON.name == TM.Observage.name)
        {
            currentStory.variablesState["IDobserver"] = RespirerID;
            currentStory.BindExternalFunction("RetourAuTMAfterRespirer", (string name) => { TM.RetourAuTMAfterRespirer(name); });
        }
        if (InkJSON.name == "PapoterMorganEsthela" || InkJSON.name == "PapoterMorganNimu" || InkJSON.name == "PapoterNimuEsthela")
        {
            currentStory.BindExternalFunction("RetourAuTMAfterPapotage", (string name) => { TM.RetourAuTMAfterPapotage(name); });
            if (TM.actorActif == TM.characters[0]&&TM.Papoteur==TM.characters[1])
            {
                currentStory.variablesState["IdPapoter"] = PapoterID[0];
            }
            else if (TM.actorActif == TM.characters[0] && TM.Papoteur == TM.characters[2])
            {
                currentStory.variablesState["IdPapoter"] = PapoterID[1];
            }
            else if (TM.actorActif == TM.characters[1] && TM.Papoteur == TM.characters[0])
            {
                currentStory.variablesState["IdPapoter"] = PapoterID[0];
            }
            else if (TM.actorActif == TM.characters[1] && TM.Papoteur == TM.characters[2])
            {
                currentStory.variablesState["IdPapoter"] = PapoterID[2];
            }
            else if (TM.actorActif == TM.characters[2] && TM.Papoteur == TM.characters[0])
            {
                currentStory.variablesState["IdPapoter"] = PapoterID[1];
            }
            else if (TM.actorActif == TM.characters[2] && TM.Papoteur == TM.characters[1])
            {
                currentStory.variablesState["IdPapoter"] = PapoterID[2];
            }
        }
        ContinueStory();
    }
    public void ContinueStory()
    {

        if (currentStory.canContinue)
        {
            string Contenue = currentStory.Continue();
            Debug.Log(Contenue);
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
                    Debug.Log("Bulle : " + tagValue);
                    switch (tagValue)
                    {
                        case "MorganHautGauche":
                            textToWriteIn.text = "";
                            if(textToWriteIn.GetComponentInParent<Image>()!=null)
                            {
                                textToWriteIn.GetComponentInParent<Image>().enabled = false;
                            }                           
                            textToWriteIn = TM.characters[0].transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>();
                            TM.characters[0].transform.GetChild(0).gameObject.GetComponent<Image>().enabled = true;
                            Debug.Log(textToWriteIn.name);
                          //  textToWriteIn.text = text;
                            break;
                        case "MorganHautDroite":
                            textToWriteIn.text = "";
                            if (textToWriteIn.GetComponentInParent<Image>() != null)
                            {
                                textToWriteIn.GetComponentInParent<Image>().enabled = false;
                            }
                            textToWriteIn = TM.characters[0].transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>();
                            TM.characters[0].transform.GetChild(1).gameObject.GetComponent<Image>().enabled = true;
                            Debug.Log(textToWriteIn.name);
                          //  textToWriteIn.text = text;
                            break;
                        case "MorganBasGauche":
                            textToWriteIn.text = "";
                            if (textToWriteIn.GetComponentInParent<Image>() != null)
                            {
                                textToWriteIn.GetComponentInParent<Image>().enabled = false;
                            }
                            textToWriteIn = TM.characters[0].transform.GetChild(3).GetChild(0).GetComponent<TMP_Text>();
                            TM.characters[0].transform.GetChild(3).gameObject.GetComponent<Image>().enabled = true;
                            Debug.Log(textToWriteIn.name);
                         //   textToWriteIn.text = text;
                            break;
                        case "MorganBasDroite":
                            textToWriteIn.text = "";
                            if (textToWriteIn.GetComponentInParent<Image>() != null)
                            {
                                textToWriteIn.GetComponentInParent<Image>().enabled = false;
                            }
                            textToWriteIn = TM.characters[0].transform.GetChild(4).GetChild(0).GetComponent<TMP_Text>();
                            TM.characters[0].transform.GetChild(4).gameObject.GetComponent<Image>().enabled = true;
                            Debug.Log(textToWriteIn.name);
                          //  textToWriteIn.text = text;
                            break;

                        case "EsthelaHautGauche":
                            textToWriteIn.text = "";
                            if (textToWriteIn.GetComponentInParent<Image>() != null)
                            {
                                textToWriteIn.GetComponentInParent<Image>().enabled = false;
                            }
                            textToWriteIn = TM.characters[1].transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>();
                            TM.characters[1].transform.GetChild(0).gameObject.GetComponent<Image>().enabled = true;
                            Debug.Log(textToWriteIn.name);
                           // textToWriteIn.text = text;
                            break;
                        case "EsthelaHautDroite":
                            textToWriteIn.text = "";
                            if (textToWriteIn.GetComponentInParent<Image>() != null)
                            {
                                textToWriteIn.GetComponentInParent<Image>().enabled = false;
                            }
                            textToWriteIn = TM.characters[1].transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>();
                            TM.characters[1].transform.GetChild(1).gameObject.GetComponent<Image>().enabled = true;
                            Debug.Log(textToWriteIn.name);
                           // textToWriteIn.text = text;
                            break;
                        case "EsthelaBasGauche":
                            textToWriteIn.text = "";
                            if (textToWriteIn.GetComponentInParent<Image>() != null)
                            {
                                textToWriteIn.GetComponentInParent<Image>().enabled = false;
                            }
                            textToWriteIn = TM.characters[1].transform.GetChild(3).GetChild(0).GetComponent<TMP_Text>();
                            TM.characters[1].transform.GetChild(3).gameObject.GetComponent<Image>().enabled = true;
                            Debug.Log(textToWriteIn.name);
                          //  textToWriteIn.text = text;
                            break;
                        case "EsthelaBasDroite":
                            textToWriteIn.text = "";
                            if (textToWriteIn.GetComponentInParent<Image>() != null)
                            {
                                textToWriteIn.GetComponentInParent<Image>().enabled = false;
                            }
                            textToWriteIn = TM.characters[1].transform.GetChild(4).GetChild(0).GetComponent<TMP_Text>();
                            TM.characters[1].transform.GetChild(4).gameObject.GetComponent<Image>().enabled = true;
                            Debug.Log(textToWriteIn.name);
                            //textToWriteIn.text = text;
                            break;

                        case "NimuHautGauche":
                            textToWriteIn.text = "";
                            if (textToWriteIn.GetComponentInParent<Image>() != null)
                            {
                                textToWriteIn.GetComponentInParent<Image>().enabled = false;
                            }
                            textToWriteIn = TM.characters[2].transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>();
                            TM.characters[2].transform.GetChild(0).gameObject.GetComponent<Image>().enabled = true;
                            Debug.Log(textToWriteIn.name);
                           // textToWriteIn.text = text;
                            break;
                        case "NimuHautDroite":
                            textToWriteIn.text = "";
                            if (textToWriteIn.GetComponentInParent<Image>() != null)
                            {
                                textToWriteIn.GetComponentInParent<Image>().enabled = false;
                            }
                            textToWriteIn = TM.characters[2].transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>();
                            TM.characters[2].transform.GetChild(1).gameObject.GetComponent<Image>().enabled = true;
                            Debug.Log(textToWriteIn.name);
                           // textToWriteIn.text = text;
                            break;
                        case "NimuBasGauche":
                            textToWriteIn.text = "";
                            if (textToWriteIn.GetComponentInParent<Image>() != null)
                            {
                                textToWriteIn.GetComponentInParent<Image>().enabled = false;
                            }
                            textToWriteIn = TM.characters[2].transform.GetChild(3).GetChild(0).GetComponent<TMP_Text>();
                            TM.characters[2].transform.GetChild(3).gameObject.GetComponent<Image>().enabled = true;
                            Debug.Log(textToWriteIn.name);
                            //textToWriteIn.text = text;
                            break;
                        case "NimuBasDroite":
                            textToWriteIn.text = "";
                            if (textToWriteIn.GetComponentInParent<Image>() != null)
                            {
                                textToWriteIn.GetComponentInParent<Image>().enabled = false;
                            }
                            textToWriteIn = TM.characters[2].transform.GetChild(4).GetChild(0).GetComponent<TMP_Text>();
                            TM.characters[2].transform.GetChild(4).gameObject.GetComponent<Image>().enabled = true;
                            Debug.Log(textToWriteIn.name);
                            //textToWriteIn.text = text;
                            break;
                        case "Narrateur":
                            textToWriteIn.text = "";
                            if (textToWriteIn.GetComponentInParent<Image>() != null)
                            {
                                textToWriteIn.GetComponentInParent<Image>().enabled = false;
                            }
                            textToWriteIn = TM.naratteur;
                            TM.PageNarrateur.GetComponent<Animator>().SetBool("Active", true);
                            Debug.Log(textToWriteIn.name);
                            //textToWriteIn.text = text;
                            break;
                        default:
                            //textToWriteIn.text = text;
                            break;
                    }

                    break;
                case anim_Tag:
                    switch(tagValue)
                    {
                        case "TM1_Papoter1"://animation morgan et coolkid qu'il faut donner le meme nom

                            break;
                        case "TM1_Papoter2":

                            break;
                        case "TM2A_Papoter1":

                            break;
                        case "TM2A_Papoter2":

                            break;
                        case "TM2A_Papoter3":

                            break;
                        case "TM2B_Papoter1":

                            break;
                        case "TM2B_Papoter2":

                            break;
                        case "TM2B_Papoter3":

                            break;
                        case "TM3_Papoter1":

                            break;
                        case "TM3_Papoter2":

                            break;
                        case "TM3_Papoter3":

                            break;
                        case "TM1_Observer1":

                            break;
                        case "TM1_Observer2":

                            break;
                        case "TM2A_Observer1":

                            break;
                        case "TM2A_Observer2":

                            break;
                        case "TM2A_Observer3":

                            break;
                        case "TM2B_Observer1":

                            break;
                        case "TM2B_Observer2":

                            break;
                        case "TM2B_Observer3":

                            break;
                        case "TM3_Observer1":

                            break;
                        case "TM3_Observer2":

                            break;
                        case "TM3_Observer3":

                            break;
                        case "TM1_Revasser1":

                            break;
                        case "TM1_Revasser2":

                            break;
                        case "TM2A_Revasser1":

                            break;
                        case "TM2A_Revasser2":

                            break;
                        case "TM2A_Revasser3":

                            break;
                        case "TM2B_Revasser1":

                            break;
                        case "TM2B_Revasser2":

                            break;
                        case "TM2B_Revasser3":

                            break;
                        case "TM3_Revasser1":

                            break;
                        case "TM3_Revasser2":

                            break;
                        case "TM3_Revasser3":

                            break;
                        case "TM1_intro":

                            break;
                        case "TM1_outro":

                            break;
                        case "TM2A_intro":

                            break;
                        case "TM2A_outro":

                            break;
                        case "TM2B_intro":

                            break;
                        case "TM2B_outro":

                            break;
                        case "TM3_intro":

                            break;
                        case "TM3_outro":

                            break;
                    }
                    break;
                case emotion_Tag:
                    switch(tagValue)
                    {
                        case "!":

                            break;
                        case "?":

                            break;
                        case "Dots":

                            break;
                        case "Drop":

                            break;
                        case "Sparkles":

                            break;
                        case "Deception":

                            break;
                        case "Anger":

                            break;
                        case "JoyLeft":

                            break;
                        case "JoyRight":

                            break;
                        case "Heart":

                            break;
                        case "Rainbow":

                            break;
                    }
                    break;
                default:
                    //textToWriteIn.text = text;
                    break;

            }
        }
        if(textToWriteIn!=TM.naratteur)
        {
            TM.PageNarrateur.GetComponent<Animator>().SetBool("Active", false);
        }
      textToWriteIn.text = text;       
    }

    public void ExitDialogueMode()
    {
        TM.PageNarrateur.GetComponent<Animator>().SetBool("Active", false);
        textToWriteIn.text = "";
        isDialoguing = false;
        if (textToWriteIn.GetComponentInParent<Image>() != null)
        {
            textToWriteIn.GetComponentInParent<Image>().enabled = false;
        }
    }
    #endregion
}
