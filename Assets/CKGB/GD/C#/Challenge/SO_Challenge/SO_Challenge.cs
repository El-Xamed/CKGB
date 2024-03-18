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

    [Header("Param�tre du challenge")]
    public Sprite background;
    public string objectif;
    [Tooltip("Information pour faire spawn un nombre de case pr�d�finis.")]
    public int nbCase;
    [Space]
    [Tooltip("Information SECONDAIRE pour faire spawn les personnages et Acc sur la scene")]
    public List<InitialAccPosition> listStartPosAcc;
    public List<InitialActorPosition> listStartPosTeam;
    [Header("Cata")]
    public List<SO_Catastrophy> listCatastrophy;
    public List<SO_Etape> listEtape;

    

    #endregion

    //Cr�ation d'une class pour positionner les perso
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
