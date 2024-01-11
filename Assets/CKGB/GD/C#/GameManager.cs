using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    int[] niveauTerminé;
    Proto_Actor[] team;

    private void Awake()
    {
        #region Singleton
        if (instance == null)
            instance = this;
        #endregion

        DontDestroyOnLoad(gameObject);
    }

    public void ChangeActionMap(string actionMap)
    {
        GetComponent<PlayerInput>().SwitchCurrentActionMap(actionMap);
    }
}
