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

}
