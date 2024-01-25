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
    [Header("Acc")]
    public List<GameObject> listAcc;
    public List<int> initialAccPosition;
    [Header("Actor")]
    [Tooltip("Information SECONDAIRE pour faire spawn les personnages et Acc sur la scene")]
    public List<int> initialplayerPosition;
    [Header("Cata")]
    public List<SO_Catastrophy> listCatastrophy;
    public List<SO_Etape> listEtape;

    public List<InitialPosition> startPosTeam;

    #endregion

    //Création d'une class pour positionner les perso
    [System.Serializable]
    public class InitialPosition
    {
        public int position;
        public C_Actor perso;
    }

    

    #region Data

    public int GetNbCases()
    {
        return nbCase;
    }

    public List<int> GetInitialPlayersPosition()
    {
        return initialplayerPosition;
    }
    #endregion
}
