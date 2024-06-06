using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class C_TransitionManager : MonoBehaviour
{
    SceneAsset nextScene;

    //Le "new" permet de ne pas avoir une valeur null.
    UnityEvent currentEvent = new UnityEvent();

    //Fonction public qui permet de setup correctement la transition entre les scene.
    public void SetupNextScene(SceneAsset thisScene)
    {
        nextScene = thisScene;

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
        Debug.Log("Launch -> " + nextScene.name);
        SceneManager.LoadScene(nextScene.name);
    }

    //Event d'animation.
    public void EndTransition()
    {
        //GetComponentInParent<GameManager>().EndAnimation(currentEvent);

        currentEvent.Invoke();
    }
}
