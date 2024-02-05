using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AudioManager : MonoBehaviour
{
    [Header("--------- Audio Source ---------")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;
    [SerializeField] AudioSource ambianceSource;

    [Header("--------- Audio Clip ---------")]
    public AudioClip up;
    public AudioClip down;
    public AudioClip hover;
    public AudioClip confirmation;
    public AudioClip retourEnArriere;
    public AudioClip option;
    public AudioClip tetaniser;
    public AudioClip logs;
    public AudioClip danger;
    public AudioClip musicTempsMort;
    public AudioClip musicChallenge;
    public AudioClip oiseaux;
    public AudioClip vent;
    public AudioClip ruisseau;
    public AudioClip marche;

    public static AudioManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        musicSource.clip = musicTempsMort;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
}
