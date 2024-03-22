using UnityEngine.Audio;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
using UnityEngine.UI;



public class AudioManager : MonoBehaviour
{
    // reference audio mixer et les sliders sfx, music et audio dans le canvas
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider audioSlider;

    public AudioSource source;
    public static AudioManager instance;
    public Sound[] sounds;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        DontDestroyOnLoad(gameObject);
        
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = null;
            s.source.outputAudioMixerGroup = s.group;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }

    }

    // recupere les valeurs du mixer
    private void Start()
    {
        instance = this;
        Play("MusiqueTuto");

       /* UpdateSlider();*/

    } 

        public void Play(string name)
        {



        Sound p = System.Array.Find(sounds, sound => sound.name == name);
        p.source.clip = p.clip[Random.Range(0, p.clip.Length)];
        if (p == null)
        {
            Debug.LogWarning("Sound:" + name + "not found!");
            return;

        }
        p.source.Play();
        Debug.Log("re,h");
        }
    
    
    [System.Serializable]
    public class Sound
    {
        public string name;

        public AudioClip[] clip;
        public AudioMixerGroup group;
     

        [Range(0f, 1f)]
        public float volume;
        [Range(0f, 1f)]
        public float pitch;

        public bool loop;

        [HideInInspector]
        public AudioSource source;
    }
    /*
    //convertie les valeurs logarithmes de audio mixer et les valeurs lineaires du slider
    public void SetMusicVolume(float value)
    {
        mixer.SetFloat(name"MusicVolume", value);
        Debug.Log("NO VOLUME ???");
    }

    public void SetSFXVolume(float value)
    {
        mixer.SetFloat("SfxVolume", value);
        Debug.Log("NO SFX ???");
    }

    public void SetAudioVolume(float value)
    {
        mixer.SetFloat("AudioVolume, value");
        Debug.Log("NO AUDIO ???");

    }

    //reinitialise les valeurs en fonction des valeur du mixeur
    private void UpdateSlider()
    {
        mixer.GetFloat("MusicVolume", out float value);
        musicSlider.value = value;

        mixer.GetFloat("SFXVolume", out float volume);
        sfxSlider.value = volume;

        mixer.GetFloat("AudioVolume",out float pitch);
        sfxSlider.value = pitch;
    }*/
}
