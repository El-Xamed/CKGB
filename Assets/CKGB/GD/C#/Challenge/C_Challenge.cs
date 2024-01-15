using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class C_Challenge : MonoBehaviour
{
    #region Mes variables
    C_destination destination;

    GameObject canva;
    GameObject uiCases;

    [SerializeField]
    int nbCase;
    [SerializeField]
    List<C_Case> listCase;
    [SerializeField]
    C_Case prefabCase;

    int etape;
    int nbEtape;

    [SerializeField]
    int[] initialplayerPosition;

    #endregion

    private void Awake()
    {
        canva = transform.GetChild(0).gameObject;

        uiCases = canva.transform.GetChild(1).gameObject;

        //Pour récupérer les positions enregistrer dans la worldmap.
        if (GameManager.instance.GetInitialPlayerPosition() != null)
        {
            initialplayerPosition = GameManager.instance.GetInitialPlayerPosition();
        }
        else { Debug.LogWarning("Get initial players position on this scene and not by the GameManager."); }
    }

    private void Start()
    {
        //Spawn toutes les cases.
        for (int i = 0; i < nbCase; i++)
        {
            C_Case myCase = Instantiate(prefabCase, uiCases.transform);

            listCase.Add(myCase);
        }

        //Place les acteurs sir les cases.
        UpdateAllPosition();
    }

    #region Mes fonctions
    //Set la position de tous les acteurs sur les cases.
    public void UpdateAllPosition()
    {
        foreach (var actorPosition in GameManager.instance.GetTeam())
        {
            //Check si le GameObject en question exist.
            if (GameObject.Find(actorPosition.GetDataActor().name))
            {
                Debug.Log("Exists");

                GameObject.Find(actorPosition.GetDataActor().name).transform.SetParent(listCase[5].transform);
            }
            else
            {
                Debug.Log("Doesn't exist");

                Instantiate(actorPosition, listCase[actorPosition.GetPosition()].transform);
            }
        }
    }
    #endregion

    #region Fonctions pour l'éditor
    public List<C_Case> GetCasesList()
    {
        return listCase;
    }

    public int GetNbCases()
    {
        return nbCase;
    }

    public int[] GetInitialPlayersPosition()
    {
        return initialplayerPosition;
    }
    #endregion
}
