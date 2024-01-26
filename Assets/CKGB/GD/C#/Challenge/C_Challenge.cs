using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UIElements;
using static SO_Challenge;

public class C_Challenge : MonoBehaviour
{
    #region Mes variables

    GameObject canva;
    GameObject uiCases;

    [SerializeField] SO_Challenge myChallenge;

    [Tooltip("Team")]
    [SerializeField] List<SO_Character> myTeam;

    [Tooltip("Case")]
    [SerializeField] C_Case myCase;
    [SerializeField] List<C_Case> listCase;

    #endregion

    private void Awake()
    {
        #region Racourcis
        canva = transform.GetChild(0).gameObject;

        uiCases = canva.transform.GetChild(1).gameObject;
        #endregion
    }

    private void Start()
    {
        //Apparition des cases
        SpawnCases();

        //Place les acteurs sur les cases.
        InitialiseAllPosition();
    }

    #region Mes fonctions
    void SpawnCases()
    {
        //Spawn toutes les cases.
        for (int i = 0; i < myChallenge.nbCase; i++)
        {
            C_Case newCase = Instantiate(myCase, uiCases.transform);

            listCase.Add(newCase);
        }
    }

    //Set la position de tous les acteurs sur les cases.
    void InitialiseAllPosition()
    {
        ActorPosition();

        AccPosition();

        //Fonction de spawn "actor".
        void ActorPosition()
        {
            if (myChallenge.listStartPosTeam != null)
            {
                //Fait spawn avec des info random.
                SpawnActor(myChallenge.GetInitialPlayersPosition());
            }
            else //Possède l'info.
            {
                Debug.LogError("ERROR : Aucune informations de trouvé la liste, aucun acteur n'a pu spawn.");
            }
        }

        //Fonction de spawn "Accessories".
        void AccPosition()
        {
            if (myChallenge.listStartPosAcc != null)
            {
                //Fait spawn avec des info random.
                SpawnAcc(myChallenge.GetInitialAccPosition());
            }
            else 
            {
                Debug.Log("Aucune informations de trouvé la liste des acc.");
            }
        }
    }

    #endregion

    #region Fonctions
    //Déplace ou fait spawn les acteurs.
    public void SpawnActor(List<InitialActorPosition> listPosition)
    {
        foreach (InitialActorPosition position in listPosition)
        {
            SO_Character myActor = Instantiate(position.perso, listCase[position.position].transform);
            myTeam.Add(myActor);
        }
    }

    public void SpawnAcc(List<InitialAccPosition> listPosition)
    {
        foreach (InitialAccPosition position in listPosition)
        {
            Instantiate(position.acc, listCase[position.position].transform);
        }
    }

    //A SUPP DANS LE FUTUT !!!
    public void SpawnOrUpdatePosition(List<int> listPosition, List<GameObject> listActor)
    {
        //Instantiate(listActor[1], listCase[listPosition[1]].transform);
        for (int i = 0; i < listPosition.Count; i++)
        {
            if (GameObject.Find(listActor[i].name)) //Regarde si il existe deja dans la scene.
            {
                //Déplace l'objet.
                GameObject.Find(listActor[i].name).transform.SetParent(listCase[listPosition[i]].transform);
            }
            else //Fait spawn un nouvel objet.
            {
                //Apparition de l'objet.
                GameObject newGameObject = Instantiate(listActor[i], listCase[listPosition[i]].transform);
                //Modifie le nom.
                newGameObject.name = listActor[i].GetComponent<SO_Accessories>().name;
                //Renseigne le GameObject.
                newGameObject.GetComponent<SO_Accessories>().gameObject = newGameObject;
                Debug.Log("Spawn Acc");
            }
        }
    }

    //Pour faire déplacer les accessoires.
    void UpdateAccessories()
    {

    }

    //Applique la cata.
    void ApplyCata()
    {

    }
    #endregion
}
