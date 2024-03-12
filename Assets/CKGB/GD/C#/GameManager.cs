using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using Ink.Runtime;
using System;

public enum EActorClass
{
    Koolkid, Grandma, Clown
}

public class GameManager : MonoBehaviour
{
    //Pour le dev.
    [Header("Paramètre de dev")]
    [Tooltip("Ceci est un paramètre de dev (Paul) ce dernier à pour objectif de rediriger correctement les object en question pour la création de l'équipe.")]
    [SerializeField]
    List<GameObject> ressourceActor = new List<GameObject>();

    #region Variables
    public static GameManager instance;

    [Header("Paramètre de dev")]
    //Récupération en variable qui apparait dans l'inspector.
    [SerializeField]
    List<EActorClass> myActor = new List<EActorClass>();

    [SerializeField]
    int[] niveauTerminé;

    [SerializeField]
    List<GameObject> team = new List<GameObject>();

    //Variable pour les challenge. DOIT RESTER CACHE C'EST UNE INFORMATION QUI RECUPERE SUR LA WORLDMAP AVANT DE LANCER LE NIVEAU.
    List<int> initialPlayerPositionOnThisDestination;

    //Information qu'il récupère pour le Temps mort / Challenge.
    SO_TempsMort currentTM = null;
    SO_Challenge currentC = null;

    //zone dédiée aux  dialogues
    [SerializeField] public Story currentStory;
    [SerializeField] public TMP_Text textToWriteIn;
    [SerializeField] public bool isDialoguing = false;
    [SerializeField] GameObject CharacterTalking;

    private const string Bulle_Tag = "Bulle";
    private const string Character_Tag = "Character";

    public C_TempsMort TM;
    #endregion

    private void Awake()
    {
        if(FindObjectOfType<C_TempsMort>()!=null)
        {
            TM = FindObjectOfType<C_TempsMort>();
        }
        #region Singleton
        if (instance == null)
            instance = this;
        #endregion

        DontDestroyOnLoad(gameObject);

        
    }
    private void Start()
    {
        SetUpTeam();
    }

    #region Mes Fonctions
    void SetUpTeam()
    {
        foreach (EActorClass thisActor in myActor)
        {
            //Définition des acteurs dans une nouvelle list par l'enum.
            switch (thisActor)
            {
                //Récupération automatique dans le dossier Resources.
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
        if(InkJSON.name=="OutroTM2A"|| InkJSON.name == "OutroTM2B" || InkJSON.name == "OutroTM1" || InkJSON.name == "OutroTM3" )
        {
            currentStory.BindExternalFunction("StartChallenge", (string name) => { TM.GoChallenge(name); });
        }

        ContinueStory();
    }
    public void ContinueStory()
    {
        
        if (currentStory.canContinue)
        {
           
            textToWriteIn.text = currentStory.Continue();
            HandleTags(currentStory.currentTags);
        }
        else
        {
            ExitDialogueMode();
        }
    }
    private void HandleTags(List<string> currentTags)
    {
        foreach(string tag in currentTags)
        {
            string[] splitTag = tag.Split(':');
            if(splitTag.Length!=2)
            {
                Debug.Log("erreur Tag "+tag);
            }
            string tagKey = splitTag[0].Trim();
            string tagValue = splitTag[1].Trim();

            switch(tagKey)
            {
        
                case Character_Tag:
                    Debug.Log("Character " + tagValue);
                    for(int i=0;i<TM.characters.Count-1; i++)
                    {
                         if(tagValue==TM.characters[i].name)
                        {
                            CharacterTalking = TM.characters[i];
                        }
                         else if(tagValue=="Narrateur")
                        {
                            textToWriteIn.text = "";
                            CharacterTalking.GetComponent<C_Actor>().CheckBulle();
                            textToWriteIn = TM.naratteur;
                            CharacterTalking.GetComponent<C_Actor>().CheckBulle();
                        }
                    }
                    break;
                case Bulle_Tag:
                    Debug.Log("Bulle : " + tagValue);
                    switch(tagValue)
                    {
                        case "HautGauche":
                            textToWriteIn.text = "";
                            CharacterTalking.GetComponent<C_Actor>().CheckBulle();
                            textToWriteIn = CharacterTalking.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>();
                            CharacterTalking.GetComponent<C_Actor>().CheckBulle();
                            break;
                        case "HautDroite":
                            textToWriteIn.text = "";
                            CharacterTalking.GetComponent<C_Actor>().CheckBulle();
                            textToWriteIn = CharacterTalking.transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>();
                            CharacterTalking.GetComponent<C_Actor>().CheckBulle();
                            break;
                        case "BasGauche":
                            textToWriteIn.text = "";
                            CharacterTalking.GetComponent<C_Actor>().CheckBulle();
                            textToWriteIn = CharacterTalking.transform.GetChild(3).GetChild(0).GetComponent<TMP_Text>();
                            CharacterTalking.GetComponent<C_Actor>().CheckBulle();
                            break;
                        case "BasDroite":
                            textToWriteIn.text = "";
                            CharacterTalking.GetComponent<C_Actor>().CheckBulle();
                            textToWriteIn = CharacterTalking.transform.GetChild(4).GetChild(0).GetComponent<TMP_Text>();
                            CharacterTalking.GetComponent<C_Actor>().CheckBulle();
                            break;

                    }
                    
                    break;
                default:
                    return;
  
            }
        }
    }

    public void ExitDialogueMode()
    {
        textToWriteIn.text = "";
        isDialoguing = false;
    }
    #endregion
}
