using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{

    // permets l acess aux differents slider
    [SerializeField] AudioMixer myMixer;

    [SerializeField] GameObject backGround;

    //Pour vérifier si le jeu et en pause ou alors dans le menu afin de retirer le fond correctement.
    bool onPause;

    #region Slider
    [SerializeField] Slider generalSlider;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;
    #endregion

    #region Resolution
    Resolution[] resolutions;
    List<Resolution> filteredResolutions;

    float currentrefreshRate;
    int currentResolutionindex = 0;
    #endregion

    // recupere les valeurs de audio mixer
    void Start()
    {
        backGround.SetActive(false);

        #region Volume
        //SetMusicVolume();
        //SetGeneralVolume();
        //SetSFXVolume();
        #endregion

        #region Resolution
        resolutions = Screen.resolutions;
        filteredResolutions = new List<Resolution>();

        currentrefreshRate = Screen.currentResolution.refreshRate;

        Debug.Log("refresh rate : " + currentrefreshRate);
        for(int i = 0;i<resolutions.Length;i++)
        {
            if(resolutions[i].refreshRate==currentrefreshRate)
            {
                filteredResolutions.Add(resolutions[i]);
            }
        }

        List<string> options = new List<string>();
        for(int i = 0;i<filteredResolutions.Count;i++)
        {
            string resolutionsOption = filteredResolutions[i].width + "x" + filteredResolutions[i].height + " " + filteredResolutions[i].refreshRate + "Hz";
            options.Add(resolutionsOption);
            if (filteredResolutions[i].width == Screen.width && filteredResolutions[i].height == Screen.height)
            {
                currentResolutionindex = i;
            }
        }
        #endregion
    }

    #region Mes Fonctions
    //Pour ouvrir les paramètres
    public void OpenOptions()
    {
        //Active le trigger qui est dans le parent de cet object car le component est mal placé.
        backGround.GetComponentInParent<Animator>().SetTrigger("trigger");
        backGround.SetActive(true);
    }

    // convertie les valeurs du slider a laudio mixer
    #region Set Volume Slider
    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        myMixer.SetFloat("music",volume);
    }
    public void SetSFXVolume()
    {
        float volume = sfxSlider.value;
        myMixer.SetFloat("sfx",volume);
    }
    
    public void SetGeneralVolume()
    {
        float volume = generalSlider.value;
        myMixer.SetFloat("general", volume);
        myMixer.SetFloat("sfx", volume);
        myMixer.SetFloat("music", volume);
    }
    #endregion

    public void TogleFullScreen()
    {
        Screen.fullScreen = !Screen.fullScreen;
        Debug.Log("fullScreenChanged");
    }

    public void GoBack()
    {
        if (onPause)
        {
            //Ferme seulement les paramètres. (Utilisé in game)
            gameObject.SetActive(false);
        }
        else
        {
            //Si en plus il est dans le menu. (Pour faire fonctionner correctement dans le menu ou juste le fermer si il est ouvert via une commande)
            if (FindObjectOfType<C_MainMenu>() != null)
            {
                FindObjectOfType<C_MainMenu>().OpenMenuGroup();

                //Set le premier bouton du Menu.
                GetComponentInParent<GameManager>().SetFirtButton(FindObjectOfType<C_MainMenu>().GetFirtBoutonMenu());
            }

            //Ferme les paramètres + le fond. (Utilisé dans le menu principal)
            gameObject.SetActive(false);
            backGround.SetActive(false);

            onPause = false;
        }
    }
    #endregion

    #region Partage de données
    public void SetBoolOnPause(bool value)
    {
        onPause = value;
    }
    #endregion
}
