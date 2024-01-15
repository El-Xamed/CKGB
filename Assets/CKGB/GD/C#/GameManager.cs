using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    //Pour le dev.
    [Header("Paramètre de dev")]
    [Tooltip("Ceci est un paramètre de dev (Paul) ce dernier à pour objectif de rediriger correctement les object en question pour la création de l'équipe.")]
    [SerializeField]
    GameObject[] ressourceActor = new GameObject[3];

    public enum EActorClass
    {
        Koolkid, Grandma, Clown
    }

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

        //Récupération automatique des personnages dans le tableau.
        //Pour tous les SO_personnage qui possède les noms qui sont dans une liste.
        //Pour tous les acteurs qui possède le componnent "Actor".
        /*foreach (Proto_Actor actor in Resources.FindObjectsOfTypeAll(typeof(Proto_Actor)) as Proto_Actor[])
        {
            team.Add(actor);
        }*/
    }

    #region Mes Fonctions
    public void ChangeActionMap(string actionMap)
    {
        GetComponent<PlayerInput>().SwitchCurrentActionMap(actionMap);
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
    #endregion
}
