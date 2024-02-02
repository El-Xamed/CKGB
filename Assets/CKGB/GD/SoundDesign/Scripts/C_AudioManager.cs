using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


[System.Serializable]

public class C_AudioManager : MonoBehaviour
{
    
    [SerializeField] Dictionary<soundName, AudioClip[]> clips;


    public string name;
    public AudioClip clip;
    public soundName soundId;
    [Serializable]
    
    

    public enum soundName
    {
        music, ambiance, sfx
    }
   

    //Fonction pour récupérer les valeurs enregistré dans le "PlayerPref".
    void LoadAudioOnce(AudioClip clip)
    {
        //AS.PlayOneShot(clip);
        //Check si un "PlayerPref" existe déjà. Si c'est pas le cas, il le créer avec les paramètre par default.

        //Applique les paramètres qui sont stocké dans le "PlayerPref".
    }
}
