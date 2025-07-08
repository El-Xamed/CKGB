using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class C_credits : MonoBehaviour
{
    [SerializeField] GameObject skipButton;

    void Start()
    {
        GameManager.instance?.SetFirtButton(skipButton);
        GameManager.instance?.OpenTransitionFlannel();
    }

    public void GoMainMenuALERT()
    {
        //A SUPP
        //GameManager.instance.GoToMainMenuALERT();

        //Pour changer de scenne.
        SceneManager.LoadScene("S_MainMenu");

        AudioManager.instanceAM.Stop("MusicCredits");
    }
}
