using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
using UnityEngine.InputSystem;

public enum EActorClass
{
    Koolkid, Grandma, Clown
}

public class GameManager : MonoBehaviour
{
    //Pour le dev.
    [Header("Paramètre de dev")]
    [Tooltip("Ceci est un paramètre de dev (Paul) ce dernier à pour objectif de rediriger correctement les object en question pour la création de l'équipe.")]
    [SerializeField]
    List<GameObject> ressourceActor = new List<GameObject>();

    #region Variables
    public static GameManager instance;

    [Header("Paramètre de dev")]
    //Récupération en variable qui apparait dans l'inspector.
    [SerializeField]
    List<EActorClass> myActor;

    [SerializeField]
    int[] niveauTerminé;

    [SerializeField]
    List<GameObject> team;

    //Variable pour les challenge. DOIT RESTER CACHE C'EST UNE INFORMATION QUI RECUPERE SUR LA WORLDMAP AVANT DE LANCER LE NIVEAU.
    List<int> initialPlayerPositionOnThisDestination;

    //Information qu'il récupère pour le Temps mort / Challenge.
    SO_TempsMort currentTM = null;
    SO_Challenge currentC = null;
    #endregion

    private void Awake()
    {
        #region Singleton
        if (instance == null)
            instance = this;
        #endregion

        DontDestroyOnLoad(gameObject);

        SetUpTeam();
    }

    #region Mes Fonctions
    void SetUpTeam()
    {
        foreach (EActorClass thisActor in myActor)
        {
            //Définition des acteurs dans une nouvelle list par l'enum.
            switch (thisActor)
            {
                //Récupération automatique dans le dossier Resources.
                case EActorClass.Koolkid:
                    if (ressourceActor[0])
                    {
                        GameObject newActor = Instantiate(ressourceActor[0], transform);
                        //newActor.GetComponent<C_Actor>().SetDataActor(ScriptableObject.Instantiate(ressourceActor[0].GetComponent<C_Actor>().GetDataActor()));
                        team.Add(newActor);
                        Debug.Log(newActor.GetComponent<C_Actor>().GetDataActor());
                    }
                    else Debug.LogWarning("Il n'y a pas de redirection pour cette objet.");
                    break;
                case EActorClass.Clown:
                    if (ressourceActor[1])
                    {
                        GameObject newActor = Instantiate(ressourceActor[1], transform);
                        //newActor.GetComponent<C_Actor>().SetDataActor(ScriptableObject.Instantiate(ressourceActor[1].GetComponent<C_Actor>().GetDataActor()));
                        team.Add(newActor);
                        Debug.Log(newActor.GetComponent<C_Actor>().GetDataActor());
                    }
                    else Debug.LogWarning("Il n'y a pas de redirection pour cette objet.");
                    break;
                case EActorClass.Grandma:
                    if (ressourceActor[2])
                    {
                        GameObject newActor = Instantiate(ressourceActor[2], transform);
                        //newActor.GetComponent<C_Actor>().SetDataActor(ScriptableObject.Instantiate(ressourceActor[2].GetComponent<C_Actor>().GetDataActor()));
                        team.Add(newActor);
                        Debug.Log(newActor.GetComponent<C_Actor>().GetDataActor());
                    }
                    else Debug.LogWarning("Il n'y a pas de redirection pour cette objet.");
                    break;
            }
        }
    }

    public void ChangeActionMap(string actionMap)
    {
        GetComponent<PlayerInput>().SwitchCurrentActionMap(actionMap);
    }
    #endregion

    #region Pour la WorldMap
    public void SetInitialPlayerPosition(List<int> newPosition)
    {
        initialPlayerPositionOnThisDestination = newPosition;
    }

    public void SetDataLevel(SO_TempsMort dataTM,SO_Challenge dataC)
    {
        currentTM = dataTM;
        currentC = dataC;

        Debug.Log(currentTM);
        Debug.Log(currentC);
    }

    public SO_TempsMort GetDataTempsMort()
    {
        return currentTM;
    }

    public SO_Challenge GetDataChallenge()
    {
        return currentC;
    }
    #endregion

    #region Pour le temps mort/Challenge

    public List<GameObject> GetTeam()
    {
        return team;
    }

    public List<GameObject> GetRessournce()
    {
        return ressourceActor;
    }

    public List<int> GetInitialPlayerPosition()
    {
        return initialPlayerPositionOnThisDestination;
    }

    //Update toutes les positions des acteurs pour le challenge.
    public void UpdateInitialPlayerPosition()
    {
        for (int i = 0; i < team.Count -1; i++)
        {
            team[i].GetComponent<C_Actor>().SetPosition(initialPlayerPositionOnThisDestination[i]);
        }
    }
    #endregion
}
