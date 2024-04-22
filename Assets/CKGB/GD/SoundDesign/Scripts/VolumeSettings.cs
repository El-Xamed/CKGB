using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{

    // permets l acess aux differents slider
    [SerializeField]  AudioMixer myMixer;
    [SerializeField]  Slider musicSlider;
    [SerializeField]  Slider sfxSlider;
    [SerializeField]  Slider voiceSlider;
    [SerializeField]  Slider generalSlider;
    bool Generalmute = true;
    bool MusiqueMute = true;
    bool VoixMute = true;
    bool EffetsSonoresMute = true;

    // recupere les valeurs de audio mixer
    public void Start()
    {

        SetMusicVolume();
        SetGeneralVolume();
        SetSFXVolume();
        SetVoiceVolume();

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
    public void SetVoiceVolume()
    {
        float volume = voiceSlider.value;
        myMixer.SetFloat("voice",Mathf.Log10(volume) * 20);
    }
    public void SetGeneralVolume()
    {
        float volume = generalSlider.value;
        myMixer.SetFloat("general",Mathf.Log10(volume) * 20);
    }

    // fonction des buttons toggle pour mute chacun des sliders
    public void MuteGeneral()
    {
        if (Generalmute)
        {
            
            Generalmute = false;

            Debug.Log("ça mute general");
        }
        else
        {
            
            Generalmute = true;
            Debug.Log("ça mute pas general");
        }

         generalSlider.enabled = Generalmute;

        
    }
    public void MuteMusique()
    {
        if (MusiqueMute)
        {

            MusiqueMute = false;

            Debug.Log("ça mute musique");
        }
        else
        {

            MusiqueMute = true;
            Debug.Log("ça mute pas musique");
        }

        musicSlider.enabled = MusiqueMute;


    }
    public void MuteEffetsSonores()
    {
        if (EffetsSonoresMute)
        {

            EffetsSonoresMute = false;

            Debug.Log("ça mute SFX");
        }
        else
        {

            EffetsSonoresMute = true;
            Debug.Log("ça mute pas SFX");
        }

        sfxSlider.enabled = EffetsSonoresMute;


    }
    public void MuteVoice()
    {
        if (VoixMute)
        {

            VoixMute = false;

            Debug.Log("ça mute voix");
        }
        else
        {

            VoixMute = true;
            Debug.Log("ça mute pas voix");
        }

        voiceSlider.enabled = VoixMute;


    }
}
