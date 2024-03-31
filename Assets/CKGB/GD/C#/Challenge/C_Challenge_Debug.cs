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

    public void MuteMusic(InputAction.CallbackContext context)
    {
        if (!context.performed) { return; }

        if (context.performed)
            AudioManager.instance.MuteMusic("MusiqueTuto");
    }
}
