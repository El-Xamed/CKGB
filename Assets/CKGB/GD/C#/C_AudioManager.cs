using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class C_AudioManager : MonoBehaviour
{
    #region Variables
    public static C_AudioManager instance;

    public AudioListener AL;
    public AudioSource AS;
    #region Bool
    [SerializeField]
    bool muteSound;

    [SerializeField]
    bool muteMusic;

    [SerializeField]
    bool muteSoundEffect;
    #endregion

    #region Float
    [SerializeField]
    float soundVolum;

    [SerializeField]
    bool musicVolum;

    [SerializeField]
    float soundEffectVolum;
    #endregion

    #endregion

    private void Awake()
    {
        #region Singleton
        if (instance == null)
            instance = this;
        #endregion

        DontDestroyOnLoad(gameObject);

        //Récupère les paramètre enregistré dans les data.
       
    }

    //Fonction pour récupérer les valeurs enregistré dans le "PlayerPref".
    void LoadAudioOnce(AudioClip clip)
    {
        AS.PlayOneShot(clip);
        //Check si un "PlayerPref" existe déjà. Si c'est pas le cas, il le créer avec les paramètre par default.

        //Applique les paramètres qui sont stocké dans le "PlayerPref".
    }
}
