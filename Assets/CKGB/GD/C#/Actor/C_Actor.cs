using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;



public class C_Actor : MonoBehaviour
{
    #region data
    int position;

    [SerializeField] SO_Character dataActor;

    [Header("Stats")]
    //Si l'actor peut encore jouer.
    [SerializeField] public bool isOut = false;

    //Stats
    [SerializeField] int currentStress;
    [SerializeField] int currentEnergy;
    [SerializeField] int currentPointTrait;

    //Ui Challenge
    C_Stats uiStats;

    //A voir avec MAX.
    public GameObject smallResume;
    public GameObject BigResume1;
    public GameObject BigResume2;

    #endregion


    private void Awake()
    {
        gameObject.name = dataActor.name; 

        dataActor = ScriptableObject.Instantiate(dataActor);
        if(dataActor.listNewTraits==null)
        {
            dataActor.nextTrait = dataActor.listTraits[0];
        }
        else
        {
            dataActor.idTraitEnCours = dataActor.listNewTraits.Count - 1;
            dataActor.nextTrait = dataActor.listTraits[dataActor.idTraitEnCours + 1];
            dataActor.traitToWrite = dataActor.listNewTraits[dataActor.idTraitEnCours];
        }
        

        //Setup le contoure blanc.
        transform.GetChild(0).gameObject.GetComponent<Image>().sprite = dataActor.challengeSpriteSlected;
        IsSelected(false);
    }

    #region Mes fonctions
    #region Challenge
    public void IniChallenge()
    {
        //dataActor.energyMax;

        //Stats
        currentStress = dataActor.stressMax;
        currentEnergy = dataActor.energyMax;

        //Sprite
        GetComponent<Image>().sprite = dataActor.challengeSprite;
        GetComponent<Image>().preserveAspect = true;
        GetComponent<Image>().useSpriteMesh = true;

        CheckIsOut();

        Debug.Log("Init challenge : " + name);
    }

    public void TakeDamage(int calm, int energy)
    {
        currentStress -= calm;
        currentEnergy -= energy;

        UpdateUiStats();
    }

    public void TakeHeal(int calm, int energy)
    {
        currentStress += calm;
        currentEnergy += energy;

        UpdateUiStats();
    }

    public void SetUiStats(C_Stats myStats)
    {
        uiStats = myStats;
    }

    public void UpdateUiStats()
    {
        uiStats.UpdateUi(this);
    }

    public void MoveActor(Transform newPosition)
    {
        transform.parent = newPosition;
    }

    public void CheckIsInDanger(SO_Catastrophy listDangerCases)
    {
        foreach (var thisCase in listDangerCases.targetCase)
        {
            if (thisCase == position)
            {
                GetComponent<Image>().sprite = dataActor.challengeSpritePreviewCata;
            }
            else if (isOut)
            {
                GetComponent<Image>().sprite = dataActor.challengeSpritePreviewCata;
            }
        }
    }

    public void IsSelected(bool isSelected)
    {
        if (isSelected)
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }
        else
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    //Check si le joueur est encore jouable.
    public void CheckIsOut()
    {

        if (currentStress <= 0)
        {
            isOut = true;

            GetComponent<Image>().sprite = dataActor.challengeSpriteIsOut;

            transform.GetChild(1).gameObject.SetActive(true);
        }
        else
        {
            isOut = false;

            //Check si le sprite est déjà possé.
            if (GetComponent<Image>().sprite != dataActor.challengeSprite && GetComponent<Image>().sprite != dataActor.challengeSpritePreviewCata)
            {
                GetComponent<Image>().sprite = dataActor.challengeSprite;
            }

            transform.GetChild(1).gameObject.SetActive(false);
        }
    }
    #endregion

    #region WorldMap
    public void IniWorldMap()
    {
        //Sprite
        GetComponent<SpriteRenderer>().sprite = dataActor.MapTmSprite;
    }
    public void IniTempsMort()
    {
        GetComponent<Image>().sprite = dataActor.MapTmSprite;
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

    public int GetPosition()
    {
        return position;     
    }

    public void SetPosition(int newPosition)
    {
        position = newPosition;
    }

    public int GetCurrentStress()
    {
        return currentStress;
    }
    public void SetCurrentStress()
    {
        currentStress++;
    }
    public int getMaxStress()
    {
        return dataActor.stressMax;
    }
    public void SetMaxStress()
    {
        dataActor.stressMax++;
    }

    public int GetcurrentEnergy()
    {
        return currentEnergy;
    }
    public void SetCurrentEnergy()
    {
        currentEnergy++;
    }
    public int getMaxEnergy()
    {
        return dataActor.energyMax;
    }
    public void SetMaxEnergy()
    {
        dataActor.energyMax++;
    }
    public int GetCurrentPointTrait()
    {
        return currentPointTrait;
    }
    public void SetCurrentPointTrait()
    {
         currentPointTrait++;
    }
    public void ResetPointTrait()
    {
        currentPointTrait = 0;
    }
 
    public bool GetIsOut()
    {
        return isOut;
    }

    #endregion
}
