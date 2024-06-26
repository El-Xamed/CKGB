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

    [Header("UI (G�n�ral)")]
    public Sprite headButton;

    [Header("Traits")]
    public List<SO_ActionClass> listTraits = new List<SO_ActionClass>();
    public List<SO_ActionClass> listNewTraits = new List<SO_ActionClass>();
    public SO_ActionClass nextTrait;
    public SO_ActionClass traitToWrite;
    public int idTraitEnCours=0;

    #region Challenge Data
    [Header("Challenge data")]
    public float widthChallenge;
    public float heightChallenge;

    [Header("Challenge (Sprite)")]
    public Sprite challengeSprite;
    public Sprite challengeSpriteBlackAndWhite;
    public Sprite challengeSpriteOnCata;
    public Sprite challengeSpriteOnCataBlackAndWhite;
    public Sprite challengeSpriteSlected;
    public Sprite challengeSpriteIsOut;
    public Sprite challengeSpriteUi;
    public Animator vfxUiGoodAction;

    [Header("Challenge (Stats)")]
    public int stressMax;
    public int energyMax;
    public int nbTraits;


    [Header("Tempsmort (Sprite)")]
    public Sprite ProfilPhoto;
    public Sprite smaller;
    public Sprite MapTmSprite;
    public Sprite NormalBulle;
    public Sprite ThinkBulle;
    public GameObject characterTree;
    #endregion

    //A VOIR AVEC MAX
    [Header("Tempsmort (Stats)")]   
    [TextAreaAttribute]
    public string Description;
    [TextAreaAttribute]
    public string miniDescription;
    public float maxTraitPoint;

    [Header("Histoires")]
    [SerializeField] public TextAsset Revasser;
    [SerializeField] public TextAsset Respirer;
    [SerializeField] public TextAsset PapoterAvecEsthela;
    [SerializeField] public TextAsset PapoterAvecNimu;
    [SerializeField] public TextAsset PapoterAvecMorgan;
    [Header("Animations")]
    public RuntimeAnimatorController RevasserAnimPatern;
    #endregion
}