using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.PlayerLoop;

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
    [SerializeField]
    [Tooltip("Information SECONDAIRE pour faire spawn les personnages sur la scene")]
    int[] initialplayerPosition;

    [Header("Info dev")]
    [SerializeField]
    int etape;
    [SerializeField]
    int nbEtape;
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
        if (GameManager.instance.GetInitialPlayerPosition() != null)
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
        InitialiseAllActorPosition();
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
    void InitialiseAllActorPosition()
    {
        //Check si le GameManager possède les informations pour placer les personnages.
        //Ne possède pas l'info.
        if (GameManager.instance.GetInitialPlayerPosition() == null || GameManager.instance.GetInitialPlayerPosition().Length < GameManager.instance.GetTeam().Count -1)
        {
            Debug.LogWarning("La liste des positions des personnages sur le GameManager ne possède pas assez d'informations ou ne possède pas du tout d'informations pour placer les personnages.");

            //Check si la scene possède les informations pour placer les personnages.
            //Ne possède pas l'info.
            if (initialplayerPosition == null || initialplayerPosition.Length < GameManager.instance.GetTeam().Count - 1)
            {
                Debug.LogError("ERROR : Aucune informations de trouvé dans la scene. Impossible de spawn correctement les personnages");

                //Fait spawn avec des info random.
                SpawnPlayerRandom();
            }
            else //Possède l'info.
            {
                //Update la position de tout les joueurs via les info de la scene.
                for (int i = 0; i < GameManager.instance.GetTeam().Count - 1; i++)
                {
                    SpawnOrUpdatePosition(initialplayerPosition[i]);
                }
            }
        }
        else //Possède l'info.
        {
            //Update la position de tout les joueurs via les info du GameManager.
            for (int i = 0; i < GameManager.instance.GetTeam().Count - 1; i++)
            {
                SpawnOrUpdatePosition(GameManager.instance.GetTeam()[i].GetPosition());
            }
        }

        //Change la position des acteurs de facon aléatoire.
        void SpawnPlayerRandom()
        {
            SpawnOrUpdatePosition(RandomNumberGenerator.GetInt32(0, nbCase));
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

    public int[] GetInitialPlayersPosition()
    {
        return initialplayerPosition;
    }
    #endregion

    #region Fonction
    //Déplace ou fait spawn les acteurs.
    public void SpawnOrUpdatePosition(int position)
    {
        foreach (var actorPosition in GameManager.instance.GetTeam())
        {
            //Donne toutes les position initial au acteurs.

            //Check si le GameObject en question exist.
            if (GameObject.Find(actorPosition.GetDataActor().name))
            {
                Debug.Log("Exists");

                GameObject.Find(actorPosition.GetDataActor().name).transform.SetParent(listCase[position].transform);
            }
            else
            {
                Debug.Log("Doesn't exist");

                Instantiate(actorPosition, listCase[position].transform);
            }
        }
    }
    #endregion

    #endregion
}
