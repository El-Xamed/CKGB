using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_Challenge : MonoBehaviour
{
    #region Mes variables
    C_destination destination;

    GameObject canva;
    GameObject uiCases;

    [SerializeField]
    int nbCase;
    [SerializeField]
    C_Case prefabCase;

    int etape;
    int nbEtape;
    
    int[] playerPosition;

    #endregion

    private void Awake()
    {
        canva = transform.GetChild(0).gameObject;

        uiCases = canva.transform.GetChild(1).gameObject;
    }

    private void Start()
    {
        //Spawn de toutes les cases.
        for (int i = 0; i < nbCase; i++)
        Instantiate(prefabCase, uiCases.transform);
    }

    #region Mes fonctions

    #endregion
}
