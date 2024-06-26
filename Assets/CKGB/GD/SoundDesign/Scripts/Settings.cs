using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class Settings : MonoBehaviour
{

    // permets l acess aux differents slider
    [SerializeField]  AudioMixer myMixer;
    [SerializeField]  Slider musicSlider;
    [SerializeField]  Slider sfxSlider;
    [SerializeField]  Slider generalSlider;
    [SerializeField] Toggle musicToggle;
    [SerializeField] Toggle sfxToggle;
    [SerializeField] Toggle generalToggle;
    bool Generalmute = true;
    bool MusiqueMute = true;
    bool EffetsSonoresMute = true;

    [SerializeField] private TMP_Dropdown resolutionDropDown;

    private Resolution[] resolutions;
    private List<Resolution> filteredResolutions;

    private float currentrefreshRate;
    private int currentResolutionindex = 0;
    public GameObject currentButton;
    public EventSystem Es;

    // recupere les valeurs de audio mixer
    public void Start()
    {
        Es = FindObjectOfType<EventSystem>();
        SetMusicVolume();
        SetGeneralVolume();
        SetSFXVolume();
        resolutions = Screen.resolutions;
        filteredResolutions = new List<Resolution>();

        resolutionDropDown.ClearOptions();
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

        resolutionDropDown.AddOptions(options);
        resolutionDropDown.value = currentResolutionindex;
        resolutionDropDown.RefreshShownValue();
    }

    // convertie les valeurs du slider a laudio mixer
    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        myMixer.SetFloat("music",Mathf.Log10(volume) * 20);
        if(volume<=0)
        {
            myMixer.SetFloat("music", 0);
        }
    }
    public void SetSFXVolume()
    {
        float volume = sfxSlider.value;
        myMixer.SetFloat("sfx",Mathf.Log10(volume) * 20);
        if (volume <= 0)
        {
            myMixer.SetFloat("sfx",0);
        }
    }
    
    public void SetGeneralVolume()
    {
        float volume = generalSlider.value;
        myMixer.SetFloat("general",Mathf.Log10(volume) * 20);
        myMixer.SetFloat("sfx", Mathf.Log10(volume) * 20);
        myMixer.SetFloat("music", Mathf.Log10(volume) * 20);
        if (volume <= 0)
        {
            myMixer.SetFloat("music", 0);
            myMixer.SetFloat("sfx", 0);
            myMixer.SetFloat("general", 0);
        }

    }

    // fonction des buttons toggle pour mute chacun des sliders
    public void MuteGeneral(bool NewValue)
    {

        Generalmute = NewValue;
        if (Generalmute)
        {
            myMixer.SetFloat("general", -80);
            




            Debug.Log("ça mute general");
        }
        else
        {
            float volume = generalSlider.value;
            myMixer.SetFloat("general", Mathf.Log10(volume) * 20);

        }

         generalSlider.enabled = !NewValue;

        
    }
    public void MuteMusique()
    {
        if (MusiqueMute&&musicToggle.GetComponent<Toggle>().isOn==false&& generalToggle.GetComponent<Toggle>().isOn == false)
        {

        }
        else
        {

           
        }

        musicSlider.enabled = MusiqueMute;



    }
    public void MuteEffetsSonores()
    {
        if (EffetsSonoresMute && sfxToggle.GetComponent<Toggle>().isOn == false && generalToggle.GetComponent<Toggle>().isOn == false)
        {

           
        }
        else
        {

          
        }

        sfxSlider.enabled = EffetsSonoresMute;


    }
    public void TogleFullScreen()
    {
        Screen.fullScreen = !Screen.fullScreen;
        Debug.Log("fullScreenChanged");
    }
    public void setResolution(int resolutionIndex)
    {
        Resolution resolution = filteredResolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, true);

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
            if (currentButton.transform.GetChild(0).name == "fleche")
            {
                currentButton.transform.GetChild(0).gameObject.SetActive(false);
            }
            if (currentButton.transform.GetChild(2).name == "fleche")
            {
                currentButton.transform.GetChild(2).gameObject.SetActive(false);
            }
            updateCurrentButton();
            if (currentButton.transform.GetChild(0).name == "fleche")
            {
                currentButton.transform.GetChild(0).gameObject.SetActive(true);
            }
            if (currentButton.transform.GetChild(2).name == "fleche")
            {
                currentButton.transform.GetChild(2).gameObject.SetActive(true);
            }
            AudioManager.instanceAM.Play("Selection");
        }

    }
    void updateCurrentButton()
    {
        currentButton = Es.currentSelectedGameObject;
    }
}
