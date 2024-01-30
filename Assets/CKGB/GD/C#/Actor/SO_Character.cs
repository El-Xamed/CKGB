using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Actor", menuName = "ScriptableObjects/Personne")]
public class SO_Character : ScriptableObject
{
    #region Data
    public new string name;
    public Sprite challengeSprite;
    public Sprite MapTmSprite;
    public Sprite smallResume;
    public Sprite BigResume1;
    public Sprite BigResume2;

    public int stressMax;
    public int energyMax;
    public int nbtraitpointMax;
    SO_ActionClass[] listTrait;
    #endregion
}

