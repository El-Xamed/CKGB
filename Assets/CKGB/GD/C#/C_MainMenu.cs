using System.Collections;
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
    [SerializeField] GameObject playButton;
    bool caMarche = true;

    [SerializeField] GameObject eventSystem;
    [SerializeField] GameObject currentButton;

    private void Start()
    {
        IniSplashScreen();
    }

    private void IniSplashScreen()
    {
        splashScreen.SetActive(true);
        eventSystem.GetComponent<EventSystem>().SetSelectedGameObject(bouttonSplashScreen);
        currentButton = eventSystem.GetComponent<EventSystem>().currentSelectedGameObject;
    }

    public void GoToFirthButton()
    {
        splashScreen.GetComponent<Animator>().SetTrigger("trigger");
        bouttonSplashScreen.GetComponent<Animator>().SetTrigger("Pressed");
        logoJeu.GetComponent<Animator>().SetTrigger("trigger");
        StartCoroutine("firstButton");
       // splashScreen.SetActive(false);

        boutonsGroupe.GetComponent<Animator>().SetBool("onMenuScreen", true);
    }
    IEnumerator firstButton()
    {
        yield return new WaitForSeconds(1.8f);
        eventSystem.GetComponent<EventSystem>().SetSelectedGameObject(playButton);
    }

    public void NewParty()
    {
        if (GameManager.instance)
        {
            GameManager.instance.SetDataLevel(null, firthChallenge);
        }
        GameManager.instance.transform.GetChild(1).gameObject.SetActive(true);
        GameManager.instance.TS_flanel.GetComponent<Animator>().SetTrigger("Open");
        StartCoroutine("loadFirstGame");
    }
    IEnumerator loadFirstGame()
    {
        yield return new WaitForSeconds(0.8f);

        SceneManager.LoadScene("S_Challenge");
    }
    public void OpenSave()
    {

    }
    public void BackOnTrack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            GameManager.instance.pauseBackground.SetActive(false);
            GameManager.instance.pauseMenu.SetActive(false);
            GameManager.instance.optionsMenu.SetActive(false);
            boutonsGroupe.SetActive(true);
            eventSystem.GetComponent<EventSystem>().SetSelectedGameObject(playButton);
        }
    }
        public void OpenOptions()
    {


        if (optionsParent)
        {
            //bouttonSplashScreen.SetActive(false);
            //logoEart.SetActive(false);
            //logoJeu.SetActive(false);
            boutonsGroupe.SetActive(false);
            GameManager.instance.pauseBackground.SetActive(true);
            GameManager.instance.optionsMenu.SetActive(true);
            //optionsParent.SetActive(true);
            Debug.Log("Options");
            eventSystem.GetComponent<EventSystem>().SetSelectedGameObject(GameManager.instance.baseToggle.gameObject);
        }


    }


    public void OpenCredits()
    {

    }

    public void LeaveGame()
    {
        Application.Quit();
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
