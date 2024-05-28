using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;

[CreateAssetMenu(fileName = "New Challenge", menuName = "ScriptableObjects/Challenge/Challenge", order = 1)]
public class SO_Challenge : ScriptableObject
{
    #region Mes variables

    [Header("Intro/Outro")]
    public TextAsset introChallenge;
    public TextAsset outroChallenge;

    [Header("Paramètre du challenge")]
    public Sprite background;
    public List<Sprite> element;
    public Sprite ecranDefaite;

    public string objectif;
    [Tooltip("Information pour faire spawn un nombre de case prédéfinis.")]
    public int nbCase;
    public float spaceCase;
    [Space]
    [Tooltip("Information SECONDAIRE pour faire spawn les personnages et Acc sur la scene")]
    public List<InitialAccPosition> listStartPosAcc;
    public List<InitialActorPosition> listStartPosTeam;
    [Header("Cata")]
    public List<SO_Catastrophy> listCatastrophy;
    public List<SO_Etape> listEtape;

    

    #endregion

    //Création d'une class pour positionner les perso
    [System.Serializable]
    public class InitialActorPosition
    {
        public int position;
        public C_Actor perso;
    }

    [System.Serializable]
    public class InitialAccPosition
    {
        public int position;
        public C_Accessories acc;
    }

    #region Data
    public List<InitialActorPosition> GetInitialPlayersPosition()
    {
        return listStartPosTeam;
    }

    public List<InitialAccPosition> GetInitialAccPosition()
    {
        return listStartPosAcc;
    }
    #endregion
}
