using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{

    // permets l acess aux differents slider
    [SerializeField] private AudioMixer myMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider voiceSlider;
    [SerializeField] private Slider generalSlider;

    private void Start()
    {

        SetMusicVolume();
        SetGeneralVolume();
        SetSFXVolume();
        SetVoiceVolume();

    }


    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        myMixer.SetFloat("music", Mathf.Log10(volume)*20);
    }

    public void SetSFXVolume()
    {
        float volume = sfxSlider.value;
        myMixer.SetFloat("sfx", Mathf.Log10(volume) * 20);
    }
    public void SetVoiceVolume()
    {
        float volume = voiceSlider.value;
        myMixer.SetFloat("voice", Mathf.Log10(volume) * 20);
    }

    public void SetGeneralVolume()
    {
        float volume = generalSlider.value;
        myMixer.SetFloat("general", Mathf.Log10(volume) * 20);
    }
}
