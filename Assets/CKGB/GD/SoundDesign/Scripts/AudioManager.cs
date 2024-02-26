using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AudioManager : MonoBehaviour
{
    #region Variable
    public static AudioManager instance;
    #endregion

    private void Awake()
    {
        #region Singleton
        if (instance == null)
            instance = this;
        #endregion

        DontDestroyOnLoad(gameObject);
    }

    public static void Main()
   {

        IDictionary<string, string> openWith =
            new Dictionary<string, string>();

        openWith.Add("fire", "fire2");
        openWith.Add("resolution", "resolusion2");
        openWith.Add("water", "water2");
        openWith.Add("birds", "birds2");
        openWith.Add("fire", "fire2");
   }
}
