using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;


public class C_MainMenu : MonoBehaviour
{
    [SerializeField] GameObject Boutton_SplashScreen;
    [SerializeField] GameObject Logo_Jeu;
    [SerializeField] GameObject Logo_Eart;
    [SerializeField] GameObject Boutons_Groupe;
    [SerializeField] GameObject OptionsParent;
    [SerializeField] SO_Challenge firthChallenge;
    bool caMarche = true;

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

    public void OpenOptions()
    {


        if (OptionsParent)
        {
            Boutton_SplashScreen.SetActive(false);
            Logo_Eart.SetActive(false);
            Logo_Jeu.SetActive(false);
            Boutons_Groupe.SetActive(false);
            OptionsParent.SetActive(true);
            Debug.Log("ça marche");
        }
        else
        {
            OptionsParent.SetActive(false);
            Debug.Log("ça marche pas");
        }

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
    public void Naviguate(InputAction.CallbackContext context)
    {
        
        if (!context.performed) { return; }

        if (context.performed )
        {
            if (caMarche == true)
            {
                AudioManager.instance.PlayOnce(AudioManager.instance.sounds[0].clip[0]);
                caMarche = false;
            }
            
            
            
        }
        else if (!context.canceled) { caMarche = true; return; }
        
        
    }
}
