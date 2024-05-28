using UnityEngine.Audio;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
using UnityEngine.UI;
using System.Collections.Generic;





public class AudioManager : MonoBehaviour
{
    
    public AudioSource source;
    public static AudioManager instance;
    public Sound[] sounds;
    public AudioMixerGroup SoundMixer;
    public AudioMixerGroup MusicMixer;

    
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
       
        
        foreach (Sound s in sounds)
        {
            if (s.loop == false && s.group == SoundMixer)
            {
                s.source = source;
            }
            else
            {
                s.source = gameObject.AddComponent<AudioSource>();
            }

            s.source.clip = null;
            s.source.outputAudioMixerGroup = s.group;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }

    }

    
    private void Start()
    {
      
    } 
    public void PlayOnce(AudioClip mp3name)
    {
        source.PlayOneShot(mp3name);
    }
    public void PlayOnce(string mp3name)
    {
        Sound p = System.Array.Find(sounds, sound => sound.name == mp3name);
        AudioClip clip = p.clip[Random.Range(0, p.clip.Length)];
        if (p == null)
        {
            Debug.LogWarning("Sound:" + mp3name + "not found!");
            return;


        }
        source.PlayOneShot(clip, p.volume);
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

    public void Stop (string name)
    {
        AudioManager.instance.Stop(name);
    }

    public void MuteMusic(string name)
    {
        Sound p = System.Array.Find(sounds, sound => sound.name == name);

        p.source.volume = 0;

        /*
        if (p.source.volume == 0)
        {
            p.source.volume = 1;
            return;
        }
        else
        {
            p.source.volume = 0;
            return;
        }*/
    }

   /* public void PlaySfx()
    {
        AudioSource.PlayOneShot();
    }*/
    
    
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
   


   
}