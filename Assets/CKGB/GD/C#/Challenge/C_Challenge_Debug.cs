using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class C_Challenge_Debug : MonoBehaviour
{
    public void ResetChallenge()
    {
        SceneManager.LoadScene("S_Challenge");
    }

    public void GoWorldMap()
    {
        SceneManager.LoadScene("S_WorldMap");
    }

    public void MuteMusic(InputAction.CallbackContext context)
    {
        if (!context.performed) { return; }

        if (context.performed)
            AudioManager.instance.MuteMusic("MusiqueTuto");
    }

    public void EndGame()
    {
        if (GameObject.Find("Challenge"))
        {
            GameObject.Find("Challenge").GetComponent<C_Challenge>().SetAnimFinish(true);
            GameObject.Find("Challenge").GetComponent<C_Challenge>().EndChallenge();
        }
        else
        {
            Debug.LogWarning("Aucun challenge détecté !!!");
        }
        
    }
}
