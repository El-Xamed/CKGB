using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class C_TransitionManager : MonoBehaviour
{
    string nextScene;
    string thisClip;

    //Le "new" permet de ne pas avoir une valeur null.
    UnityEvent currentEvent = new UnityEvent();

    //Fonction public qui permet de setup correctement la transition entre les scene.
    public void SetupNextScene(string thisScene, string cutClip)
    {
        Debug.Log("Setup next scene !");
        nextScene = thisScene;
        thisClip = cutClip;

        //Retire toutes les fonctions stocké dans l'event.
        currentEvent.RemoveAllListeners();

        //Setup automatiquement l'event de transition.
        currentEvent.AddListener(TransiScene);
    }

    public void SetupFirthEvent(UnityAction thisUnityAction)
    {
        //Retire toutes les fonctions stocké dans l'event.
        currentEvent.RemoveAllListeners();

        currentEvent.AddListener(thisUnityAction);
    }

    //Fonction placé à la fin des animation de transition.
    void TransiScene()
    {
        Debug.Log("Launch -> " + nextScene);
        SceneManager.LoadScene(nextScene);
    }

    //Event d'animation.
    public void EndTransition()
    {
        Debug.Log("Fin de transition !");

        //GetComponentInParent<GameManager>().EndAnimation(currentEvent);

        if (AudioManager.instanceAM && !string.IsNullOrEmpty(thisClip))
        {
            Debug.Log(thisClip);
            AudioManager.instanceAM.Stop(thisClip);
        }

        currentEvent.Invoke();
    }
}
