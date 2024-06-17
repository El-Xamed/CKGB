using UnityEngine.Audio;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public static AudioManager instanceAM;

    void Awake()
    {

        if (instanceAM == null)
            instanceAM = this;
        else
            Destroy(instanceAM.gameObject);


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




    public void Stop(string name)
    {



        Sound p = System.Array.Find(sounds, sound => sound.name == name);
        p.source.clip = p.clip[Random.Range(0, p.clip.Length)];
        if (p == null)
        {
            Debug.LogWarning("Sound:" + name + "not found!");
            return;

        }
        p.source.Stop();
    }
}

[System.Serializable]
    public class Sound
    {
        public string name;

        public AudioClip[] clip;
        public AudioMixerGroup group;
     

        [Range(0f, 1f)]
        public float volume;
        [Range(0f, 2f)]
        public float pitch;

        public bool loop;

        [HideInInspector]
        public AudioSource source;
    }
