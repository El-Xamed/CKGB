using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class C_MainMenu : MonoBehaviour
{
    [SerializeField] string firthScene = "S_challenge";

    [SerializeField] GameObject splashScreen;
    [SerializeField] GameObject bouttonSplashScreen;
    [SerializeField] GameObject logoJeu;
    [SerializeField] GameObject logoEart;
    [SerializeField] GameObject boutonsGroupe;
    [SerializeField] GameObject playButton;
    [SerializeField] GameObject optionsButton;
    [SerializeField] GameObject ChapterImage;

    [SerializeField] EventSystem eventSystem;
    GameObject currentButton;

    private void Start()
    {
        IniSplashScreen();
    }

    #region Mes fonctions
    private void IniSplashScreen()
    {
        GameManager.instance.SetFirtButton(bouttonSplashScreen);
        AudioManager.instanceAM.Play("MenuMusic");
        splashScreen.SetActive(true);
    }

    public void GoToFirthButton()
    {
        splashScreen.GetComponent<Animator>().SetTrigger("trigger");
        bouttonSplashScreen.GetComponent<Animator>().SetTrigger("Pressed");
        logoJeu.GetComponent<Animator>().SetTrigger("trigger");
        StartCoroutine("firstButton");
       // splashScreen.SetActive(false);

       // boutonsGroupe.GetComponent<Animator>().SetBool("onMenuScreen", true);

        //SFX
        if (AudioManager.instanceAM)
        {
            AudioManager.instanceAM.Play("NewGameButton");
        }
    }
    IEnumerator firstButton()
    {
        yield return new WaitForSeconds(1.8f);
        GameManager.instance.SetFirtButton(playButton);
        splashScreen.SetActive(false);
    }
    public void ShowChapterPicture()
    {
        AudioManager.instanceAM.Play("NewGameButton");
        ChapterImage.SetActive(true);

        GameManager.instance.SetFirtButton(ChapterImage.transform.GetChild(0).GetChild(0).gameObject);
    }

    #region Boutons
    public void NewParty(SO_Challenge firthChallenge)
    {
        //Setup le niveau.
        if (GameManager.instance)
        {
            GameManager.instance.SetDataLevel(null, firthChallenge);
        }

        //???
        GameManager.instance.transform.GetChild(1).gameObject.SetActive(true);

        //Setup dans quelle scene on souhaite aller.
        GameManager.instance.TS_flanel.GetComponent<C_TransitionManager>().SetupNextScene(firthScene, "NewGameButton");

        //Transition.
        GameManager.instance.CloseTransitionFlannel();

        //Stop les sons.
        AudioManager.instanceAM.Stop("MenuMusic");

        GameManager.instance.SetFirtButton(null);
    }

    /*A VOIR POUR LE SUPP
    public void BackOnTrack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            GameManager.instance.pauseBackground.SetActive(false);
            GameManager.instance.pauseMenu.SetActive(false);
            GameManager.instance.optionsMenu.SetActive(false);
            boutonsGroupe.SetActive(true);
            logoJeu.SetActive(true);
            logoJeu.GetComponent<Animator>().SetTrigger("trigger");
            //boutonsGroupe.GetComponent<Animator>().SetBool("onMenuScreen", true);
            optionsButton.GetComponent<Animator>().SetTrigger("unselected");
            eventSystem.GetComponent<EventSystem>().SetSelectedGameObject(optionsButton);
            
        }
    }*/

    public void OpenOptions()
    {
        //Cache le menu.
        boutonsGroupe.SetActive(false);
        logoJeu.SetActive(false);

        GameManager.instance.OpenOptions();

        //SFX.
        AudioManager.instanceAM.Play("ClickButton");
    }

    public void updateCurrentButton()
    {
        //currentButton.GetComponent<Animator>().SetTrigger("unselected");
        currentButton = eventSystem.GetComponent<EventSystem>().currentSelectedGameObject;
        // currentButton.GetComponent<Animator>().SetTrigger("Selected");
    }

    public void Naviguate(InputAction.CallbackContext context)
    {
        if (!context.performed) { return; }

        if (context.performed)
        {
            /*
            AudioManager.instanceAM.Play("Selection"); 
            updateCurrentButton();
            */
        }
        if (context.started)
        {
         
            updateCurrentButton();
         
            AudioManager.instanceAM.Play("Selection");
        }

    }

    public void OpenCredits()
    {
        AudioManager.instanceAM.Stop("MenuMusic");
        AudioManager.instanceAM.Play("ClickButton");
        AudioManager.instanceAM.Play("MusicCredits");
        SceneManager.LoadScene(4);
    }

    public void LeaveGame()
    {
        //AudioManager.instanceAM.Play("ClickButton");
        AudioManager.instanceAM.Play("ClickButton");
        Application.Quit();
        
    }
    #endregion

    #endregion

    #region Data partagées
    public void OpenMenuGroup()
    {
        boutonsGroupe.SetActive(true);
        logoJeu.SetActive(true);
    }

    public GameObject GetFirtBoutonMenu()
    {
        return playButton;
    }
    #endregion
}
