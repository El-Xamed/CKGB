using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;


public class C_MainMenu : MonoBehaviour
{
    [SerializeField] string firthScene = "S_challenge";

    [SerializeField] GameObject splashScreen;
    [SerializeField] GameObject bouttonSplashScreen;
    [SerializeField] GameObject logoJeu;
    [SerializeField] GameObject logoEart;
    [SerializeField] GameObject boutonsGroupe;
    [SerializeField] GameObject optionsParent;
    [SerializeField] GameObject playButton;
    [SerializeField] GameObject optionsButton;
    [SerializeField] GameObject ChapterImage;

    [SerializeField] GameObject eventSystem;
    [SerializeField] GameObject currentButton;

    private void Start()
    {
        IniSplashScreen();
        AudioManager.instance.Play("MusiqueSplashScreen");
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
        AudioManager.instance.PlayOnce("SfxSplashScreen");
    }
    IEnumerator firstButton()
    {
        yield return new WaitForSeconds(1.8f);
        eventSystem.GetComponent<EventSystem>().SetSelectedGameObject(playButton);
        splashScreen.SetActive(false);
    }
    public void ShowChapterPicture()
    {
        ChapterImage.SetActive(true);
        eventSystem.GetComponent<EventSystem>().SetSelectedGameObject(ChapterImage.transform.GetChild(0).GetChild(0).gameObject);

    }
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
        GameManager.instance.TS_flanel.GetComponent<C_TransitionManager>().SetupNextScene(firthScene);

        //Transition.
        GameManager.instance.TS_flanel.GetComponent<Animator>().SetTrigger("Close");
       
        //Stop les sons.
        AudioManager.instance.Stop("MusiqueSplashScreen");
        AudioManager.instance.PlayOnce("SfxSonDeConfirmation");
        AudioManager.instance.Play("MusiqueTuto");

    }

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
            boutonsGroupe.GetComponent<Animator>().SetBool("onMenuScreen", true);
            optionsButton.GetComponent<Animator>().SetTrigger("unselected");
            eventSystem.GetComponent<EventSystem>().SetSelectedGameObject(optionsButton);
            
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
            logoJeu.SetActive(false);
            GameManager.instance.pauseBackground.SetActive(true);
            GameManager.instance.PauseParent.GetComponent<Animator>().SetTrigger("trigger");
            GameManager.instance.optionsMenu.SetActive(true);
            //optionsParent.SetActive(true);
            Debug.Log("Options");
            eventSystem.GetComponent<EventSystem>().SetSelectedGameObject(GameManager.instance.baseToggle.gameObject);
            AudioManager.instance.PlayOnce("SfxSonDeConfirmation");
        }
    }
    public void updateCurrentButton()
    {
        //currentButton.GetComponent<Animator>().SetTrigger("unselected");
        currentButton = eventSystem.GetComponent<EventSystem>().currentSelectedGameObject;
        // currentButton.GetComponent<Animator>().SetTrigger("Selected");
        AudioManager.instance.PlayOnce("SfxHover");

    }
    public void Naviguate(InputAction.CallbackContext context)
    {
        if (!context.performed) { return; }

        if (context.performed && context.ReadValue<Vector2>()!=Vector2.zero)
        {
            updateCurrentButton();
            
        }
        
    }

    public void OpenCredits()
    {
        AudioManager.instance.PlayOnce("SfxSonDeConfirmation");
    }

    public void LeaveGame()
    {
        Application.Quit();
        AudioManager.instance.PlayOnce("SfxSonDeConfirmation");
    }

    //Focntion à dev plus tard. Sert pour tous les menu.
    public void Back()
    {
        
    }
    /*public void Naviguate(InputAction.CallbackContext context)
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
        
        
    }*/
}
