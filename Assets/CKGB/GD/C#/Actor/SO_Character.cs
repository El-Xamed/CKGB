using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;


[CreateAssetMenu(fileName = "New Actor", menuName = "ScriptableObjects/Personne")]
public class SO_Character : ScriptableObject
{
    #region Data
    [Header("Nom")]
    public new string name;

    [Header("Traits")]
    public List<SO_ActionClass> listTraits = new List<SO_ActionClass>();
    public List<SO_ActionClass> listNewTraits = new List<SO_ActionClass>();
    public SO_ActionClass nextTrait;
    public SO_ActionClass traitToWrite;
    public int idTraitEnCours=0;

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
    public int nbTraits;


    [Header("Tempsmort (Sprite)")]
    public Sprite ProfilPhoto;
    public Sprite MapTmSprite;
    public Sprite smallResume;
    public Sprite BigResume1;
    public Sprite BigResume2;

    //A VOIR AVEC MAX
    [Header("Tempsmort (Stats)")]   
    [TextAreaAttribute]
    public string Description;
    [TextAreaAttribute]
    public string miniDescription;

    [Header("Histoires")]
    [SerializeField] public TextAsset Revasser;
    [SerializeField] public TextAsset Respirer;
    [SerializeField] public TextAsset PapoterAvecEsthela;
    [SerializeField] public TextAsset PapoterAvecNimu;
    [SerializeField] public TextAsset PapoterAvecMorgan;
    #endregion
}


