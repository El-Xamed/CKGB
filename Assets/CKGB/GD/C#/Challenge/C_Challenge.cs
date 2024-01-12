using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class C_Challenge : MonoBehaviour
{
    #region Mes variables
    C_destination destination;

    [SerializeField]
    List<pionclass> pions;

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
    
    int[] playerPosition;

    #endregion

    private void Awake()
    {
        canva = transform.GetChild(0).gameObject;

        uiCases = canva.transform.GetChild(1).gameObject;

        GetActorInChallenge();
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
        UpdateAllPosition(pions);
    }

    #region Mes fonctions
    //Récupération des acteurs dans le gameManager.
    void GetActorInChallenge()
    {
        foreach (var actor in GameManager.instance.GetTeam())
        {
            //Creation d'une nouvel class
            pionclass test = new pionclass();

            //Set l'actor.
            test.SetActor(actor.gameObject);

            //Set la position de facon aléatoire (!!! EN CONSRTUCTION !!!)
            test.SetPosition(4);

            pions.Add(test);
        }

    }

    //Set la position de tous les acteurs sur les cases.
    private void UpdateAllPosition(List<pionclass> pions)
    {
        foreach (var actorPosition in pions)
        {
            //actorPosition.GetActor().transform = listCase[actorPosition.GetPosition()].transform;

            //actorPosition.GetActor().transform.SetParent(listCase[actorPosition.GetPosition()].transform);
        }
    }

    //Set la position d'un acteur.
    private void UpdatePosition(pionclass pions)
    {

    }
    #endregion
}

//Création d'une class qui va rassembler les info d'un pion.
[Serializable]
public class pionclass
{
    //Acteur.
    [SerializeField]
    GameObject myActor;

    //Position dans le jeu.
    [SerializeField]
    int myPosition;

    public void SetActor(GameObject actor)
    {
        myActor = actor;
    }

    public GameObject GetActor()
    {
        return myActor;
    }

    public void SetPosition(int position)
    {
        myPosition = position;
    }

    public int GetPosition()
    {
        return myPosition;
    }
}
