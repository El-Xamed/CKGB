using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class C_Transition : MonoBehaviour
{
    public static C_Transition instance;

    [SerializeField] Animator flanel;
    [SerializeField] Animator maskRond;
    [SerializeField] Animator softBlackSwipe;

    void Awake()
    {
        #region Singleton
        if (instance == null)
            instance = this;
        #endregion

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        flanel = GetComponent<Animator>();
        maskRond = GetComponent<Animator>();
        softBlackSwipe = GetComponent<Animator>();
    }

    #region Animation
    public void OpenTransFlannel()
    {
        flanel.SetTrigger("Open");
    }

    public void CloseTransFlannel()
    {
        flanel.SetTrigger("Close");
    }
    #endregion

    #region Event
    //Pour check dans quel scene on est.
    public void CheckStatsGame()
    {
        //Dans la scene du challenge.
        if (SceneManager.GetActiveScene().name == "S_Challenge")
        {
            GameObject.Find("LesEnnuiesCommencent").GetComponent<Animator>().enabled = true;
        }
    }
    #endregion
}
