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
   

    //Fonction pour r�cup�rer les valeurs enregistr� dans le "PlayerPref".
    void LoadAudioOnce(AudioClip clip)
    {
        //AS.PlayOneShot(clip);
        //Check si un "PlayerPref" existe d�j�. Si c'est pas le cas, il le cr�er avec les param�tre par default.

        //Applique les param�tres qui sont stock� dans le "PlayerPref".
    }
}
