using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class C_MainMenu : MonoBehaviour
{
    [SerializeField] GameObject splashScreen;
    [SerializeField] GameObject bouttonSplashScreen;
    [SerializeField] GameObject logoJeu;
    [SerializeField] GameObject logoEart;
    [SerializeField] GameObject boutonsGroupe;
    [SerializeField] GameObject optionsParent;
    [SerializeField] SO_Challenge firthChallenge;
    bool caMarche = true;

    [SerializeField] GameObject eventSystem;

    private void Start()
    {
        IniSplashScreen();
    }

    private void IniSplashScreen()
    {
        splashScreen.SetActive(true);
        eventSystem.GetComponent<EventSystem>().SetSelectedGameObject(bouttonSplashScreen);
    }

    public void GoToFirthButton(Button thisButton)
    {
        eventSystem.GetComponent<EventSystem>().SetSelectedGameObject(thisButton.gameObject);
        splashScreen.SetActive(false);

        boutonsGroupe.GetComponent<Animator>().SetBool("onMenuScreen", true);
    }

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


        if (optionsParent)
        {
            bouttonSplashScreen.SetActive(false);
            logoEart.SetActive(false);
            logoJeu.SetActive(false);
            boutonsGroupe.SetActive(false);
            optionsParent.SetActive(true);
            Debug.Log("ça marche");
        }
        else
        {
            optionsParent.SetActive(false);
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
