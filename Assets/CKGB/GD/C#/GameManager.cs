using System.Collections.Generic;
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
    GameObject[] ressourceActor = new GameObject[3];

    #region Variables
    public static GameManager instance;

    [Header("Paramètre de dev")]
    //Récupération en variable qui apparait dans l'inspector.
    [SerializeField]
    List<EActorClass> myActor;

    [SerializeField]
    int[] niveauTerminé;

    [SerializeField]
    List<Proto_Actor> team;

    //Variable pour les challenge. DOIT RESTER CACHE C'EST UNE INFORMATION QUI RECUPERE SUR LA WORLDMAP AVANT DE LANCER LE NIVEAU.
    int[] initialPlayerPositionOnThisDestination;
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
                        team.Add(Resources.Load<Proto_Actor>("Actor/" + ressourceActor[0].name));
                    else Debug.LogWarning("Il n'y a pas de redirection pour cette objet.");
                    break;
                case EActorClass.Grandma:
                    if (ressourceActor[1])
                        team.Add(Resources.Load<Proto_Actor>("Actor/" + ressourceActor[1].name));
                    else Debug.LogWarning("Il n'y a pas de redirection pour cette objet.");
                    break;
                case EActorClass.Clown:
                    if (ressourceActor[2])
                        team.Add(Resources.Load<Proto_Actor>("Actor/" + ressourceActor[2].name));
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
    public void SetInitialPlayerPosition(int[] newPosition)
    {
        initialPlayerPositionOnThisDestination = newPosition;
    }
    #endregion

    #region Pour le temps mort/Challenge

    public List<Proto_Actor> GetTeam()
    {
        return team;
    }

    public int[] GetInitialPlayerPosition()
    {
        return initialPlayerPositionOnThisDestination;
    }

    //Update toutes les positions des acteurs pour le challenge.
    public void UpdateInitialPlayerPosition()
    {
        for (int i = 0; i < team.Count -1; i++)
        {
            team[i].SetPosition(initialPlayerPositionOnThisDestination[i]);
        }
    }

    EActorClass eActor;
    public EActorClass GetEnum()
    {
        return eActor;
    }
    #endregion
}
