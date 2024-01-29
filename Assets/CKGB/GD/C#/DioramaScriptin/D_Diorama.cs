using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class D_Diorama : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void GoToTempsMort()
    {
        SceneManager.LoadScene("Diorama_TempsMort");
    }
    public void GoToChallenge()
    {
        SceneManager.LoadScene("Diorama_Challenge");
    }
}
