using UnityEngine.Audio;
using UnityEngine;
using System;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public Sound[] sounds;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        /*
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
    }
    */


    }
    [Serializable]
    public class Sound
    {
        [SerializeField] string febehbekbg;

    }
}
