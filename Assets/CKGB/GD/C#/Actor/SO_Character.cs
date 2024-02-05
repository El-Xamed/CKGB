using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[CreateAssetMenu(fileName = "New Actor", menuName = "ScriptableObjects/Personne")]
public class SO_Character : ScriptableObject
{
    #region Data
    [Header("Nom")]
    public new string name;

    [Header("Traits")]
    public List<SO_ActionClass> listTraits = new List<SO_ActionClass>();

    [Header("Challenge (Sprite)")]
    public Sprite challengeSprite;
    public Sprite challengeSpritePreviewCata;
    public Sprite challengeSpriteSlected;
    public Sprite challengeSpriteIsOut;
    public Sprite challengeSpriteUi;
    public Sprite challengeSpriteUiGoodAction;
    [Header("Challenge (Stats)")]
    public int stressMax;
    public int energyMax;
    public int nbtraitpointMax;
    SO_ActionClass[] listTrait;

    [Header("Tempsmort (Sprite)")]
    public Sprite MapTmSprite;
    public Sprite smallResume;
    public Sprite BigResume1;
    public Sprite BigResume2;

    [Header("Tempsmort (Stats)")]
    public SO_ActionClass[] traitsAdebloquer;
    public int currentPointTrait;
    public TMP_Text description;
    #endregion
}


