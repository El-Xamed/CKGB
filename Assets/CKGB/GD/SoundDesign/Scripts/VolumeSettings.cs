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

    // recupere les valeurs de audio mixer
    private void Start()
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
        myMixer.SetFloat("music",volume);
    }

    public void SetSFXVolume()
    {
        float volume = sfxSlider.value;
        myMixer.SetFloat("sfx",volume);
    }
    public void SetVoiceVolume()
    {
        float volume = voiceSlider.value;
        myMixer.SetFloat("voice",volume);
    }

    public void SetGeneralVolume()
    {
        float volume = generalSlider.value;
        myMixer.SetFloat("general",volume);
    }
}
