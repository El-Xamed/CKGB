using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Personne", menuName = "Personne")]
public class Proto_SO_Character : ScriptableObject
{

    #region
    public new string name;
    public Sprite challengeSprite;
    public Sprite MapTmSprite;
    public int stressMax;
    int currentStress;
    public int energyMax;
    int currentEnergy;
    public int nbtraitpointMax;
    int nbtraitpoint;
    SO_ActionClass[] listTrait;
    #endregion
    #region


    public void TakeDamage()
    {

    }
    public void Heal()
    {

    }
    #endregion
}

