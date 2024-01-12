using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_AudioManager : MonoBehaviour
{
    #region Variables
    public static C_AudioManager instance;

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

        //R�cup�re les param�tre enregistr� dans les data.
        LoadAudio();
    }

    //Fonction pour r�cup�rer les valeurs enregistr� dans le "PlayerPref".
    void LoadAudio()
    {
        //Check si un "PlayerPref" existe d�j�. Si c'est pas le cas, il le cr�er avec les param�tre par default.

        //Applique les param�tres qui sont stock� dans le "PlayerPref".
    }
}
