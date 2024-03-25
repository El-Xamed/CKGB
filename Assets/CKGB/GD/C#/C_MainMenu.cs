using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class C_MainMenu : MonoBehaviour
{
    [SerializeField] SO_Challenge firthChallenge;

    public void NewParty()
    {
        if (GameManager.instance)
        {
            GameManager.instance.SetDataLevel(null, firthChallenge);
        }

        SceneManager.LoadScene("S_Challenge");
    }

    public void OpenSave()
    {

    }

    public void OpenCredits()
    {

    }

    public void LeaveGame()
    {

    }

    //Focntion à dev plus tard. Sert pour tous les menu.
    public void Back()
    {
        
    }
}
