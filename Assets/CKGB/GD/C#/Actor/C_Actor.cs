using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;



public class C_Actor : C_Pion
{
    #region data
    #region Challenge
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
    [SerializeField] Image chains;
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
        //Setup le contoure blanc.
        mainchild.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = dataActor.challengeSpriteSlected;
        IsSelected(false);

        bulles.Add(BulleHautGauche);
        bulles.Add(BulleHautDroite);
        bulles.Add(BulleBasGauche);
        bulles.Add(BulleBasDroite);
    }

    #region Mes fonctions
    #region Challenge
    #region Feedback
    public void PlayAnimSelectAction()
    {
        GetComponent<Animator>().SetTrigger("ActionSelected");
    }
    #endregion


    public void IniChallenge()
    {
        //dataActor.energyMax;

        //Stats
        currentStress = dataActor.stressMax;
        currentEnergy = dataActor.energyMax;

        //Sprite
        //Desactivation des child. A RETIRER PLUS TARD + FAIRE SPAWN LES BULLE DE DIALOGUE DU TM AU LIEUX DE LES AVOIR CONSTAMENT DANS LE GAMEOBJECT.
        for (int i = 0; i < bulles.Count; i++)
        {
            bulles[i].gameObject.SetActive(false);
        }

        //Active le gameObject qui contien le sprite du coolkid. A RETIRER PLUS TARD + FAIRE SPAWN LES BULLE DE DIALOGUE DU TM AU LIEUX DE LES AVOIR CONSTAMENT DANS LE GAMEOBJECT.
        mainchild.gameObject.SetActive(true);

        mainchild.GetComponent<Image>().sprite = dataActor.challengeSprite;
        mainchild.GetComponent<Image>().preserveAspect = true;
        mainchild.GetComponent<Image>().useSpriteMesh = true;


        CheckIsOut();

        Debug.Log("Init challenge : " + name);
    }

    //Cette partie du dev va etre optimisé ! les 2 fonctions réunis en 1.
    //New version
    public void SetCurrentStats(int value, TargetStats_NewInspector.ETypeStats onWhatStats)
    {
        //Check quelle stats changer.
        if (onWhatStats == TargetStats_NewInspector.ETypeStats.Calm)
        {
            //Change la valeur de calm
            currentStress += value;
        }
        else if (onWhatStats == TargetStats_NewInspector.ETypeStats.Energy)
        {
            //Change la valeur d'energie.
            currentEnergy += value;
        }

        //Check si il ne dépasse pas la limite.
        //Pour l'energie.
        if (currentEnergy < 0)
        {
            currentEnergy = 0;
        }
        if (currentEnergy > GetMaxEnergy())
        {
            currentEnergy = GetMaxEnergy();
        }

        //Pour le calm.
        if (currentStress < 0)
        {
            currentStress = 0;
        }
        if (currentStress > GetMaxStress())
        {
            currentStress = GetMaxStress();
        }

        //Check si le joueur est encore jouable.
        CheckIsOut();

        //Update les UI stats.
        UpdateUiStats();
    }

    //Old version
    public void SetCurrentStatsPrice(int stressPrice, int energyPrice)
    {
        currentStress -= stressPrice;
        currentEnergy -= energyPrice;

        //Check si il ne dépasse pas la limite.
        if (currentEnergy < 0)
        {
            currentEnergy = 0;
        }

        //Check si le joueur est encore jouable.
        CheckIsOut();

        //Update les UI stats.
        UpdateUiStats();
    }
    //Old version
    public void SetCurrentStatsGain(int stressGain, int energyGain)
    {
        currentStress += stressGain;
        currentEnergy += energyGain;

        //Check si il ne dépasse pas la limite.
        if (currentStress > GetMaxStress())
        {
            currentStress = GetMaxStress();
        }
        if (currentEnergy > GetMaxEnergy())
        {
            currentEnergy = GetMaxEnergy();
        }

        //Check si le joueur est encore jouable.
        CheckIsOut();

        //Update les UI stats.
        UpdateUiStats();
    }

    //Pour lier la stats qui va le suivre dans tous le challenge.
    public void SetUiStats(C_Stats myStats)
    {
        uiStats = myStats;
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
