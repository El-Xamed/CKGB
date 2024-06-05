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
    }

    public void SetupFirthEvent(UnityAction thisUnityAction)
    {
        currentEvent.AddListener(thisUnityAction);
    }

    //Fonction placé à la fin des animation de transition.
    public void TransiScene()
    {
        Debug.Log("Launch -> " + nextScene.name);
        SceneManager.LoadScene(nextScene.name);
    }

    public void EndTransition()
    {
        GetComponentInParent<GameManager>().EndAnimation(currentEvent);
    }
}
