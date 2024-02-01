using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Actor", menuName = "ScriptableObjects/Personne")]
public class SO_Character : ScriptableObject
{
    #region Data
    [Header("Nom")]
    public new string name;

    [Header("Challenge (Sprite)")]
    public Sprite challengeSprite;
    public Sprite challengeSpriteUi;
    [Header("Challenge (Stats)")]
    public int stressMax;
    public int energyMax;
    public int nbtraitpointMax;
    SO_ActionClass[] listTrait;

    [Header("Tempsmort")]
    public Sprite MapTmSprite;
    public Sprite smallResume;
    public Sprite BigResume1;
    public Sprite BigResume2;
    #endregion
}

