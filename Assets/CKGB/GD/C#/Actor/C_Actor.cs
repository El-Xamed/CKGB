using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;



public class C_Actor : C_Pion
{
    #region data
    #region Challenge
    //Pour les feedback
    Transform pos1 = null;
    Transform pos2 = null;

    [SerializeField] SO_Character dataActor;

    //Current stats.
    [SerializeField] int currentStress;
    [SerializeField] int currentEnergy;

    //Ui Challenge
    C_Stats uiStats;

    //Si l'actor peut encore jouer.
    [SerializeField] bool isOut = false;

    //Character.
    [SerializeField] Image character;
    [SerializeField] GameObject chains;
    [SerializeField] Image ombre;
    [SerializeField]public GameObject sweats;

    [Space]
    //Animation Stats
    [SerializeField] Animator sfxAnimator;
    [SerializeField] Image currentStats;
    [SerializeField] Sprite vfxWaveCalm;
    [SerializeField] Sprite vfxWaveEnergy;

    #endregion

    #region Temps Mort
    [Header("Temps mort")]
    [SerializeField] public List<GameObject> bulles = new List<GameObject>();
    [SerializeField] public GameObject BulleHautGauche;
    [SerializeField] public TMP_Text txtHautGauche;
    [SerializeField] public GameObject BulleHautDroite;
    [SerializeField] public TMP_Text txtHautDroite;
    [SerializeField] public GameObject BulleBasGauche;
    [SerializeField] public TMP_Text txtBasGauche;
    [SerializeField] public GameObject BulleBasDroite;
    [SerializeField] public TMP_Text txtBasDroite;
    [SerializeField] public GameObject mainchild;

    //Stats
    [SerializeField] public float currentPointTrait;
    [SerializeField] public bool HasPlayed = false;
    [SerializeField] public bool HasRevassed = false;
    [SerializeField] public bool HasObserved = false;
    [SerializeField] public bool HasPapoted = false;
    [SerializeField] public bool HasTraited = false;
    [SerializeField] public C_Tree charaTree;
    #endregion

    #endregion

    private void Awake()
    {
        gameObject.name = dataActor.name; 

        dataActor = ScriptableObject.Instantiate(dataActor);

        if(dataActor.listNewTraits.Contains(dataActor.listTraits[0])==false)
        {
            dataActor.nextTrait = dataActor.listTraits[0];
        }
        else
        {
            dataActor.idTraitEnCours = dataActor.listNewTraits.Count - 1;
            dataActor.nextTrait = dataActor.listTraits[dataActor.idTraitEnCours + 1];
            dataActor.traitToWrite = dataActor.listNewTraits[dataActor.idTraitEnCours];
        }
    }

    private void Start()
    {
        character.enabled = false;

        //Setup le contoure blanc.
        mainchild.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = dataActor.challengeSpriteSlected;
        IsSelected(false);

        bulles.Add(BulleHautGauche);
        bulles.Add(BulleHautDroite);
        bulles.Add(BulleBasGauche);
        bulles.Add(BulleBasDroite);

        foreach (var thisBulle in bulles)
        {
            thisBulle.GetComponent<Image>().enabled = false;
            Debug.Log(thisBulle.name);
        }
    }

    #region Mes fonctions
    #region Challenge
    #region Feedback
    public void PlayAnimSelectAction()
    {
        GetComponent<Animator>().SetTrigger("ActionSelected");
    }

    //Pour l'animation de tp.
    public void SetStartPosition(Transform startPosition)
    {
        pos1 = startPosition;
    }

    public void SetEndPosition(Transform endPosition)
    {
        pos2 = endPosition;
    }

    public void GetStartPosition() 
    {
        GetComponentInParent<RectTransform>().position = new Vector3(pos1.position.x, 0, pos1.position.z);
    }

    public void GetEndPosition()
    {
        CheckInDanger();
        GetComponentInParent<RectTransform>().position = new Vector3(pos2.position.x, 0, pos2.position.z);
    }
    #endregion

    public void IniChallenge()
    {
        //dataActor.energyMax;
        character.enabled = true;

        //Stats
        currentStress = dataActor.stressMax;
        currentEnergy = dataActor.energyMax;

        //Active l'ombre du challenge.
        ombre.gameObject.SetActive(true);

        //Active le gameObject qui contien le sprite du coolkid. A RETIRER PLUS TARD + FAIRE SPAWN LES BULLE DE DIALOGUE DU TM AU LIEUX DE LES AVOIR CONSTAMENT DANS LE GAMEOBJECT.
        mainchild.gameObject.SetActive(true);

        mainchild.GetComponent<Image>().sprite = dataActor.challengeSprite;
        mainchild.GetComponent<Image>().preserveAspect = true;
        mainchild.GetComponent<Image>().useSpriteMesh = true;


        CheckIsOut();

        Debug.Log("Init challenge : " + name);
    }

    //Cette partie du dev va etre optimisé ! les 2 fonctions réunis en 1.
    public void SetCurrentStats(int value, TargetStats_NewInspector.ETypeStats onWhatStats)
    {
        //Check quelle stats changer.
        if (onWhatStats == TargetStats_NewInspector.ETypeStats.Calm)
        {
            //Change la valeur de calm
            currentStress += value;

            if (vfxWaveCalm)
            {
                currentStats.sprite = vfxWaveCalm;
            }
            else
            {
                Debug.LogWarning("Pas de vfx de Wave calm !");
            }
        }
        else if (onWhatStats == TargetStats_NewInspector.ETypeStats.Energy)
        {
            //Change la valeur d'energie.
            currentEnergy += value;

            if (vfxWaveEnergy)
            {
                currentStats.sprite = vfxWaveEnergy;
            }
            else
            {
                Debug.LogWarning("Pas de vfx de Wave energy !");
            }
        }

        //Lance l'animation du sfx.
        if (value < 0)
        {
            if (sfxAnimator)
            {
                sfxAnimator.SetTrigger("statsDown");
            }
            else
            {
                Debug.LogWarning("Pas d'animator pour les vfx de Wave !");
            }
        }
        else if (value > 0)
        {
            if (sfxAnimator)
            {
                sfxAnimator.SetTrigger("statsUp");
            }
            else
            {
                Debug.LogWarning("Pas d'animator pour les vfx de Wave !");
            }
        }
        

        #region Check si il ne dépasse pas la limite
        //Check si il ne dépasse pas la limite.
        #region Energy
        //Pour l'energie.
        if (currentEnergy < 0)
        {
            currentEnergy = 0;
        }
        if (currentEnergy > GetMaxEnergy())
        {
            currentEnergy = GetMaxEnergy();
        }
        #endregion

        #region Calm
        //Pour le calm.
        if (currentStress < 0)
        {
            currentStress = 0;
        }
        if (currentStress > GetMaxStress())
        {
            currentStress = GetMaxStress();
        }
        #endregion
        #endregion

        //Check si le joueur est encore jouable.
        CheckIsOut();

        //Update les UI stats.
        UpdateUiStats();
    }

    //Pour lier la stats qui va le suivre dans tous le challenge.
    public void SetUiStats(C_Stats myStats)
    {
        uiStats = myStats;
        GetUiStats().SetActor(this);
    }

    //Utile ???
    public C_Stats GetUiStats() { return uiStats; }

    public void UpdateUiStats()
    {
        uiStats.UpdateUi(this);
    }

    //Pour faire apparaitre le contoure blanc du joueur sélectionné.
    public void IsSelected(bool isSelected)
    {
        if (isSelected)
        {
            mainchild.transform.GetChild(0).gameObject.SetActive(true);
        }
        else
        {
            mainchild.transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    //Check si le joueur est encore jouable.
    public void CheckIsOut()
    {
        if (currentStress <= 0)
        {
            currentStress = 0;

            isOut = true;

            character.sprite = dataActor.challengeSpriteIsOut;

            chains.gameObject.SetActive(true);
        }
        else
        {
            isOut = false;

            //Check si le sprite est déjà possé.
            if (character.sprite != dataActor.challengeSprite && character.sprite != dataActor.challengeSpriteOnCata)
            {
                character.sprite = dataActor.challengeSprite;
            }

            chains.gameObject.SetActive(false);
        }
    }

    public void CheckInDanger()
    {
        if (inDanger)
        {
            GetImageActor().sprite = GetDataActor().challengeSpriteOnCata;
            transform.GetChild(3).gameObject.SetActive(true);
        }
        else
        {
            GetImageActor().sprite = GetDataActor().challengeSprite;
            transform.GetChild(3).gameObject.SetActive(false);
        }

        GetComponent<Animator>().SetBool("isInDanger", GetInDanger());
    }
    #endregion

    #region WorldMap
    public void IniWorldMap()
    {
        //Sprite
       mainchild.GetComponent<SpriteRenderer>().sprite = dataActor.MapTmSprite;
    }
    public void IniTempsMort()
    {
        mainchild.GetComponent<Image>().sprite = dataActor.MapTmSprite;
        character.enabled = true;
        //Active l'ombre du challenge.
        ombre.gameObject.SetActive(false);
    }

    #endregion

    #endregion

    #region Partage de donné
    public SO_Character GetDataActor()
    {
        return dataActor;
    }
    public void UpdateNextTrait()
    {
        dataActor.listNewTraits.Add(dataActor.nextTrait);
        dataActor.idTraitEnCours++;
        dataActor.nextTrait = dataActor.listTraits[dataActor.idTraitEnCours];       
    }
    public void SetDataActor(SO_Character thisSO_Character)
    {
        dataActor = thisSO_Character;
    }

    public int GetCurrentStress()
    {
        return currentStress;
    }
    public int GetMaxStress()
    {
        return dataActor.stressMax;
    }
    public void SetMaxStress()
    {
        dataActor.stressMax++;
    }
    public void ReduceMaxStress()
    {
        dataActor.stressMax--;
    }

    public int GetcurrentEnergy()
    {
        return currentEnergy;
    }
    public int GetMaxEnergy()
    {
        return dataActor.energyMax;
    }
    public void SetMaxEnergy()
    {
        dataActor.energyMax++;
    }
    public void ReduceMaxEnergy()
    {
        dataActor.energyMax--;
    }
    public float GetCurrentPointTrait()
    {
        return currentPointTrait;
    }
    public void SetCurrentPointTrait()
    {
         currentPointTrait=currentPointTrait +0.5f;
    }
    public void ReducePointTrait()
    {
        currentPointTrait-=0.5f;
        if(currentPointTrait==-0.5)
        {
            currentPointTrait = 0;
            if(dataActor.listNewTraits.Count!=0)
            {
                dataActor.listNewTraits.Remove(dataActor.listNewTraits[dataActor.idTraitEnCours]);
            }
            dataActor.idTraitEnCours--;
            dataActor.nextTrait = dataActor.listTraits[dataActor.idTraitEnCours];
        }
    }
    public void ResetPointTrait()
    {
        currentPointTrait = 0;
    }
 
    public bool GetIsOut()
    {
        return isOut;
    }

    public Image GetImageActor()
    {
        return character;
    }

    #endregion

    #region Pour l'animation
    public void SetSpriteChallenge()
    {
        //Check si l'actor est sur une cata pour sauvegarder la bonne image.
        if (!inDanger)
        {
            mainchild.GetComponent<Image>().sprite = GetDataActor().challengeSprite;
        }
        else
        {
            mainchild.GetComponent<Image>().sprite = GetDataActor().challengeSpriteOnCata;
        }
    }

    public void SetSpriteChallengeBlackAndWhite()
    {
        //Check si l'actor est sur une cata pour sauvegarder la bonne image.
        if (!inDanger)
        {
            mainchild.GetComponent<Image>().sprite = GetDataActor().challengeSpriteBlackAndWhite;
        }
        else
        {
            mainchild.GetComponent<Image>().sprite = GetDataActor().challengeSpriteOnCataBlackAndWhite;
        }
    }
    #endregion
}
