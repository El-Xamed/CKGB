using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class C_Actor : MonoBehaviour
{
    #region data
    int position;

    [SerializeField] public SO_Character dataActor;

    [Header("Stats")]
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
    }

    public void TakeHeal(int calm, int energy)
    {
        currentStress += calm;
        currentEnergy += energy;
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
}
