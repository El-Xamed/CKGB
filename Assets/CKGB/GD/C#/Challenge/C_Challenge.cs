using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UIElements;

public class C_Challenge : MonoBehaviour
{
    #region Mes variables

    GameObject canva;
    GameObject uiCases;
    SO_Challenge myChallenge;


    [SerializeField]
    List<C_Case> listCase;

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
            C_Case myCase = Instantiate(myChallenge.myCase, uiCases.transform);

            listCase.Add(myCase);
        }
    }

    //Set la position de tous les acteurs sur les cases.
    void InitialiseAllPosition()
    {
        ActorPosition();

        //Check si une liste d'Acc existe.
        if (myChallenge.listAcc != null)
        {
            AccPosition();
        }

        //Fonction de spawn "actor".
        void ActorPosition()
        {
            //Check si le GameManager possède les informations pour placer les personnages.
            //Ne possède pas l'info.
            if (GameManager.instance.GetInitialPlayerPosition() == null || GameManager.instance.GetInitialPlayerPosition().Count < GameManager.instance.GetTeam().Count)
            {
                Debug.LogWarning("La liste des positions des personnages sur le GameManager ne possède pas assez d'informations ou ne possède pas du tout d'informations pour placer les personnages.");

                //Check si la scene possède les informations pour placer les personnages.
                //Ne possède pas l'info.
                if (myChallenge.initialplayerPosition == null || myChallenge.initialplayerPosition.Count < GameManager.instance.GetTeam().Count)
                {
                    Debug.LogError("ERROR : Aucune informations de trouvé dans la scene. Impossible de spawn correctement les personnages");

                    //Fait spawn avec des info random.
                    SpawnPlayerRandom(GameManager.instance.GetTeam());
                }
                else //Possède l'info.
                {
                    //Update la position de tout les joueurs via les info de la scene.
                    for (int i = 0; i < GameManager.instance.GetTeam().Count - 1; i++)
                    {
                        SpawnOrUpdatePosition(myChallenge.initialplayerPosition, GameManager.instance.GetTeam());
                    }
                }
            }
            else //Possède l'info.
            {
                //Update la position de tout les joueurs via les info du GameManager.
                for (int i = 0; i < GameManager.instance.GetTeam().Count; i++)
                {
                    //SpawnOrUpdatePosition(GameManager.instance.GetTeam()[i].GetComponent<Proto_Actor>().GetPosition(), GameManager.instance.GetTeam());
                }
            }
        }

        //Fonction de spawn "Accessories".
        void AccPosition()
        {
            //
            if (myChallenge.initialAccPosition != null && myChallenge.initialAccPosition.Count == myChallenge.listAcc.Count)
            {
                //Update la position de tout les joueurs via les info de la scene.
                SpawnOrUpdatePosition(myChallenge.initialAccPosition, myChallenge.listAcc);
                Debug.Log("Lancement du spawn Acc");
            }
            else //
            {
                //Fait spawn avec des info random.
                SpawnPlayerRandom(myChallenge.listAcc);
                Debug.LogWarning("ERROR : Aucune informations de trouvé dans la scene. Impossible de spawn correctement les Acc");
            }
        }
    }

    #endregion

    #region Fonctions
    //Déplace ou fait spawn les acteurs.
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

    //Change la position des acteurs de facon aléatoire.
    void SpawnPlayerRandom(List<GameObject> listActor)
    {
        //Génération d'une liste int vide
        List<int> newListPosition = new List<int>();
        //Créer et ajoute un nombre dans la liste.
        for (int i = 0; i < listActor.Count; i++)
        {
            int newPosition = RandomNumberGenerator.GetInt32(0, myChallenge.nbCase -1);
            newListPosition.Add(newPosition);
        }
        

        SpawnOrUpdatePosition(newListPosition, listActor);
    }

    //Pour faire déplacer les accessoires.
    void UpdateAccessories()
    {
        //Pour tous les acc.
        foreach (var myAcc in myChallenge.listAcc)
        {
            //Update le déplacement des Acc. 
            myAcc.GetComponent<SO_Accessories>().UpdateAcc(listCase);
        }
    }

    //Applique la cata.
    void ApplyCata()
    {

    }
    #endregion
}
