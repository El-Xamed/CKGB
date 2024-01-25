using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "New Challenge", menuName = "ScriptableObjects/Challenge/Challenge", order = 1)]
public class SO_Challenge : ScriptableObject
{
    #region Mes variables

    [Header("Paramètre du challenge")]
    [Tooltip("Information pour faire spawn un nombre de case prédéfinis.")]
    public int nbCase;
    [Space]
    public List<InitialAccPosition> listStartPosAcc;
    [Tooltip("Information SECONDAIRE pour faire spawn les personnages et Acc sur la scene")]
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

    public class InitialAccPosition
    {
        public int position;
        public SO_Accessories acc;
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
