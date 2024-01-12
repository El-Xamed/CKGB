using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField]
    int[] niveauTerminé;

    [SerializeField]
    List<Proto_Actor> team;

    private void Awake()
    {
        #region Singleton
        if (instance == null)
            instance = this;
        #endregion

        DontDestroyOnLoad(gameObject);

        //Récupération automatique des personnages dans le tableau.
        //Pour tous les SO_personnage qui possède les noms qui sont dans une liste.
        //Pour tous les acteurs qui possède le componnent "Actor".
        foreach (Proto_Actor actor in Resources.FindObjectsOfTypeAll(typeof(Proto_Actor)) as Proto_Actor[])
        {
            team.Add(actor);
        }
    }

    public void ChangeActionMap(string actionMap)
    {
        GetComponent<PlayerInput>().SwitchCurrentActionMap(actionMap);
    }

    public List<Proto_Actor> GetTeam()
    {
        return team;
    }
}
