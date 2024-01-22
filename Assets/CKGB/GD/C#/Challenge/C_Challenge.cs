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

    [Header("Paramètre du challenge")]
    [SerializeField]
    [Tooltip("Information pour faire spawn un nombre de case prédéfinis.")]
    int nbCase;
    [SerializeField]
    [Tooltip("Prefab de case")]
    C_Case prefabCase;
    [Space]
    [Header("Acc")]
    [SerializeField]
    List<GameObject> listAcc;
    [SerializeField]
    List<int> initialAccPosition;
    [SerializeField]
    [Tooltip("Information SECONDAIRE pour faire spawn les personnages et Acc sur la scene")]
    List<int> initialplayerPosition;

    [Header("Info dev")]
    [SerializeField]
    List<C_Case> listCase;

    #endregion

    private void Awake()
    {
        #region Racourcis
        canva = transform.GetChild(0).gameObject;

        uiCases = canva.transform.GetChild(1).gameObject;
        #endregion

        #region Data GameManager
        //Pour récupérer les positions enregistrer dans la worldmap.
        if (GameObject.Find("GameManager") && GameManager.instance.GetInitialPlayerPosition() != null)
        {
            initialplayerPosition = GameManager.instance.GetInitialPlayerPosition();
        }
        else { Debug.LogWarning("Get initial players position on this scene and not by the GameManager."); }
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
        for (int i = 0; i < nbCase; i++)
        {
            C_Case myCase = Instantiate(prefabCase, uiCases.transform);

            listCase.Add(myCase);
        }
    }

    //Set la position de tous les acteurs sur les cases.
    void InitialiseAllPosition()
    {
        //ActorPosition();

        //Check si une liste d'Acc existe.
        if (listAcc != null)
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
                if (initialplayerPosition == null || initialplayerPosition.Count < GameManager.instance.GetTeam().Count)
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
                        SpawnOrUpdatePosition(initialplayerPosition, GameManager.instance.GetTeam());
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
            if (initialAccPosition != null && initialAccPosition.Count == listAcc.Count)
            {
                //Update la position de tout les joueurs via les info de la scene.
                SpawnOrUpdatePosition(initialAccPosition, listAcc);
                Debug.Log("Lancement du spawn Acc");
            }
            else //
            {
                //Fait spawn avec des info random.
                SpawnPlayerRandom(listAcc);
                Debug.LogWarning("ERROR : Aucune informations de trouvé dans la scene. Impossible de spawn correctement les Acc");
            }
        }
    }

    #endregion

    #region Fonctions pour partager les info

    #region Data
    public List<C_Case> GetCasesList()
    {
        return listCase;
    }

    public int GetNbCases()
    {
        return nbCase;
    }

    public List<int> GetInitialPlayersPosition()
    {
        return initialplayerPosition;
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
                Instantiate(listActor[i], listCase[listPosition[i]].transform);
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
            int newPosition = RandomNumberGenerator.GetInt32(0, nbCase -1);
            newListPosition.Add(newPosition);
        }
        

        SpawnOrUpdatePosition(newListPosition, listActor);
    }

    //Pour faire déplacer les accessoires.
    void MoveAccessories()
    {

    }
    #endregion

    #endregion
}
