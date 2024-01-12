using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField]
    int[] niveauTermin�;

    [SerializeField]
    List<Proto_Actor> team;

    private void Awake()
    {
        #region Singleton
        if (instance == null)
            instance = this;
        #endregion

        DontDestroyOnLoad(gameObject);

        //R�cup�ration automatique des personnages dans le tableau.
        //Pour tous les SO_personnage qui poss�de les noms qui sont dans une liste.
        foreach (var character in team)
            //Pour tous les acteurs qui poss�de le componnent "Actor".
            foreach (Proto_Actor actor in Resources.FindObjectsOfTypeAll(typeof(Proto_Actor)) as Proto_Actor[])
            {
                team.Add(character);
            }
    }

    public void ChangeActionMap(string actionMap)
    {
        GetComponent<PlayerInput>().SwitchCurrentActionMap(actionMap);
    }
}
