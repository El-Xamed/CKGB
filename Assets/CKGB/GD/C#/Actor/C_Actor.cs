using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;



public class C_Actor : MonoBehaviour
{
    #region data
    [SerializeField] int position;

    [SerializeField] public SO_Character dataActor;
    C_Stats UiStats;

    [Header("Stats")]
    [SerializeField] public bool isOut = false;
    [SerializeField] public int currentStress;
    [SerializeField] public int maxStress;
    [SerializeField] public int currentEnergy;
    [SerializeField] public int maxEnergy;
    [SerializeField] public int currentPointTrait;
    [SerializeField] public int maxtraitPoint;
    public List<SO_ActionClass> listTraits = new List<SO_ActionClass>();
    public SO_ActionClass[] traitaDebloquer;
    public GameObject smallResume;
    public GameObject BigResume1;
    public GameObject BigResume2;
    public TMP_Text smallStats;
    public TMP_Text BigStats1;
    public TMP_Text BigStats2;
    public TMP_Text description;

    bool newTrait;
    #endregion


    private void Awake()
    {
        gameObject.name = dataActor.name;

        Debug.Log("1");
        if (GameManager.instance)
        {
            Debug.Log("2");
            foreach (var myActor in GameManager.instance.GetComponent<C_TempsMort>().characters)
            {
                Debug.Log("3");
                if (myActor.GetComponent<C_Actor>().dataActor == dataActor)
                {
                    Debug.Log("4");
                    //Bidouille
                    dataActor.stressMax = myActor.GetComponent<C_Actor>().maxStress;
                    dataActor.energyMax = myActor.GetComponent<C_Actor>().maxEnergy;
                    listTraits = myActor.GetComponent<C_Actor>().listTraits;
                }
            }
        }

        //Desactive et renseigne le sprite en question.
        if (gameObject.GetComponent<SpriteRenderer>()==null)
        {
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(0).GetComponent<Image>().sprite = dataActor.challengeSpriteSlected;

            //Désactive le sprite KO.
            transform.GetChild(1).gameObject.SetActive(false);
        }
        dataActor = ScriptableObject.Instantiate(dataActor);

  
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

    public void UpdateUiStats()
    {
        UiStats.UpdateUi(this);
    }

    public void MoveActor(Transform newPosition)
    {
        transform.parent = newPosition;
    }
    #endregion

    #region WorldMap
    public void IniWorldMap()
    {
        //Sprite
        GetComponent<SpriteRenderer>().sprite = dataActor.MapTmSprite;

    }

    #endregion

    #endregion

    #region Partage de donné
    public SO_Character GetDataActor()
    {
        return dataActor;
    }

    public int GetPosition()
    {
        return position;     
    }

    public void SetPosition(int newPosition)
    {
        position = newPosition;
    }

    public void SetUiStats(C_Stats myUiStats)
    {
        UiStats = myUiStats;
    }

    public int GetCurrentStress()
    {
        return currentStress;
    }

    public int GetcurrentEnergy()
    {
        return currentEnergy;
    }

    public bool GetIsOut()
    {
        return isOut;
    }

    //Check si le joueur est encore jouable.
    public void CheckIsOut()
    {

        if (currentStress <= 0)
        {
            AudioManager.instance.PlaySFX(AudioManager.instance.tetaniser);
            isOut = true;

            GetComponent<Image>().sprite = dataActor.challengeSpriteIsOut;

            transform.GetChild(1).gameObject.SetActive(true);
        }
        else
        {
            //Check si le sprite est déjà possé.
            if (GetComponent<Image>().sprite != dataActor.challengeSprite && GetComponent<Image>().sprite != dataActor.challengeSpritePreviewCata)
            {
                isOut = false;

                GetComponent<Image>().sprite = dataActor.challengeSprite;

                transform.GetChild(1).gameObject.SetActive(false);
            }
        }
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
    #endregion
}
