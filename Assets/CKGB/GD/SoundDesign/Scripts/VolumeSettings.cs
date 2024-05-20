using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
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
    
    // recupere les valeurs de audio mixer
    public void Start()
    {

        SetMusicVolume();
        SetGeneralVolume();
        SetSFXVolume();
       
    
       
       

    }

    // convertie les valeurs du slider a laudio mixer
    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        myMixer.SetFloat("music",Mathf.Log10(volume) * 20);
    }
    public void SetSFXVolume()
    {
        float volume = sfxSlider.value;
        myMixer.SetFloat("sfx",Mathf.Log10(volume) * 20);
    }
    
    public void SetGeneralVolume()
    {
        float volume = generalSlider.value;
        myMixer.SetFloat("general",Mathf.Log10(volume) * 20);
        myMixer.SetFloat("sfx", Mathf.Log10(volume) * 20);
        myMixer.SetFloat("music", Mathf.Log10(volume) * 20);

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
   
}
