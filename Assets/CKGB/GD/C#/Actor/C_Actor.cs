using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class C_Actor : MonoBehaviour
{
    #region data
    int position;

    [SerializeField] public SO_Character dataActor;
    C_Stats UiStats;

    [Header("Stats")]
    bool isOut = false;
    [SerializeField] int currentStress;
    [SerializeField] int currentEnergy;
    [SerializeField] int currentPointTrait;
    #endregion


    private void Awake()
    {
        gameObject.name = dataActor.name;
    }

    #region Mes fonctions
    #region Challenge
    public void IniChallenge()
    {
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
        GetComponent<Image>().sprite = dataActor.MapTmSprite;
        GetComponent<Image>().preserveAspect = true;
        GetComponent<Image>().useSpriteMesh = true;
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
            isOut = true;

            GetComponent<Image>().sprite = dataActor.challengeSpriteIsOut;
        }
        else
        {
            //Check si le sprite est déjà possé.
            if (GetComponent<Image>().sprite != dataActor.challengeSprite)
            {
                isOut = false;

                GetComponent<Image>().sprite = dataActor.challengeSprite;
            }
        }
    }
    #endregion
}
