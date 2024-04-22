using System.Collections.Generic;
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
    [SerializeField] Toggle musicToggle;
    [SerializeField] Toggle sfxToggle;
    [SerializeField] Toggle voiceToggle;
    [SerializeField] Toggle generalToggle;
    bool Generalmute = true;
    bool MusiqueMute = true;
    bool VoixMute = true;
    bool EffetsSonoresMute = true;
    [SerializeField]AudioSource general;
    [SerializeField] List<AudioSource> voix = new List<AudioSource>();
    [SerializeField] List<AudioSource> sfx = new List<AudioSource>();
    [SerializeField] List<AudioSource> music = new List<AudioSource>();

    // recupere les valeurs de audio mixer
    public void Start()
    {

        SetMusicVolume();
        SetGeneralVolume();
        SetSFXVolume();
        SetVoiceVolume();
    
        AudioSource[] audios = FindObjectsOfType<AudioSource>();
        for(int i =0;i<audios.Length;i++)
        {
            Debug.Log(audios[i].outputAudioMixerGroup.name);
            
            if (audios[i].outputAudioMixerGroup.name == "Music")
            {
                music.Add(audios[i]);
            }
            if (audios[i].outputAudioMixerGroup.name == "SFX")
            {
                sfx.Add(audios[i]);
            }
            if (audios[i].outputAudioMixerGroup.name == "Voice")
            {
                voix.Add(audios[i]);
            }
        }
       

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
        myMixer.SetFloat("sfx", Mathf.Log10(volume) * 20);
        myMixer.SetFloat("voice", Mathf.Log10(volume) * 20);
        myMixer.SetFloat("music", Mathf.Log10(volume) * 20);

    }

    // fonction des buttons toggle pour mute chacun des sliders
    public void MuteGeneral()
    {
        if (Generalmute)
        {
            
            Generalmute = false;
            for(int i = 0;i<music.Count;i++)
            {
                music[i].volume = 0;
            }
            for (int i = 0; i < sfx.Count; i++)
            {
                sfx[i].volume= 0;
            }
            for (int i = 0; i < voix.Count; i++)
            {
                voix[i].volume = 0;
            }

            
            
            Debug.Log("ça mute general");
        }
        else
        {
            
            Generalmute = true;
            for (int i = 0; i < music.Count; i++)
            {
                if(musicToggle.isOn==false)
                {
                    music[i].volume = musicSlider.value;
                }
               
            }
            for (int i = 0; i < sfx.Count; i++)
            {
                if (sfxToggle.isOn == false)
                {
                    sfx[i].volume = sfxSlider.value;
                }
            }
            for (int i = 0; i < voix.Count; i++)
            {
                if (voiceToggle.isOn == false)
                {
                    voix[i].volume = voiceSlider.value;
                }
            }
            Debug.Log("ça mute pas general");
        }

         generalSlider.enabled = Generalmute;

        
    }
    public void MuteMusique()
    {
        if (MusiqueMute&&musicToggle.GetComponent<Toggle>().isOn==false&& generalToggle.GetComponent<Toggle>().isOn == false)
        {

            MusiqueMute = false;
            for (int i = 0; i < music.Count; i++)
            {
                music[i].volume = musicSlider.value;
            }
            Debug.Log("ça mute musique");
        }
        else
        {

            MusiqueMute = true;
            for (int i = 0; i < music.Count; i++)
            {
                music[i].volume = 0;
            }
            Debug.Log("ça mute pas musique");
        }

        musicSlider.enabled = MusiqueMute;



    }
    public void MuteEffetsSonores()
    {
        if (EffetsSonoresMute && sfxToggle.GetComponent<Toggle>().isOn == false && generalToggle.GetComponent<Toggle>().isOn == false)
        {

            EffetsSonoresMute = false;
            for (int i = 0; i < sfx.Count; i++)
            {
                sfx[i].volume = sfxSlider.value;
            }
            Debug.Log("ça mute SFX");
        }
        else
        {

            EffetsSonoresMute = true;
            for (int i = 0; i < sfx.Count; i++)
            {
                sfx[i].volume = 0;
            }
            Debug.Log("ça mute pas SFX");
        }

        sfxSlider.enabled = EffetsSonoresMute;


    }
    public void MuteVoice()
    {
        if (VoixMute && voiceToggle.GetComponent<Toggle>().isOn == false && generalToggle.GetComponent<Toggle>().isOn == false)
        {

            VoixMute = false;
            for (int i = 0; i < voix.Count; i++)
            {
                voix[i].volume = voiceSlider.value;
            }
            Debug.Log("ça mute voix");
        }
        else
        {

            VoixMute = true;
            for (int i = 0; i < voix.Count; i++)
            {
                voix[i].volume = 0;
            }
            Debug.Log("ça mute pas voix");
        }

        voiceSlider.enabled = VoixMute;


    }
}
