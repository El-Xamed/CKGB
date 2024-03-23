using UnityEngine.Audio;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
using UnityEngine.UI;



public class AudioManager : MonoBehaviour
{
    

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

    
    private void Start()
    {
        instance = this;
        Play("MusiqueTuto");

      

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
   
}