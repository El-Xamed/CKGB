using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_TempsMort : MonoBehaviour
{
    #region Variables
    //Voir commment récupérer les info stocké dans le scipt "C_destination".
    C_destination destination;

    #region Animation
    Animation[] papoter;
    Animation[] observer;
    Animation[] revasser;
    #endregion

    Proto_Actor actorActif;
    #endregion

    private void Start()
    {
        //Lance le dialogue.
        StartIntroduction();
    }

    #region Mes fonctions
    void NextActor()
    {

    }

    void StartPapoter()
    {
        
    }

    void StartObserver()
    {

    }

    void StartRevasser()
    {

    }

    void StartIntroduction()
    {
        C_DialogueManager.instance.LetsTalk(0);
    }

    public void StartChallenge()
    {
        
    }

    #endregion
}
