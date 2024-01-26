using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class C_Actor : MonoBehaviour
{
    #region
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
        GetComponent<Image>().sprite = dataActor.challengeSprite;

        //Activer cette fonction DANS le CHALLENGE.
        IniStatsChallenge();
    }

    #region Mes fonctions

    void IniStatsChallenge()
    {
        currentStress = dataActor.stressMax;
        currentEnergy = dataActor.energyMax;
    }

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
