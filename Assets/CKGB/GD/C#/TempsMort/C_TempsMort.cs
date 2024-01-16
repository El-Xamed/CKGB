using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class C_TempsMort : MonoBehaviour
{
    #region Variables
    [SerializeField]
    List<StartPosition> listPositions;

    #region Animation
    Animation[] papoter;
    Animation[] observer;
    Animation[] revasser;
    #endregion

    Proto_Actor actorActif;
    #endregion

    private void Awake()
    {
        
    }

    private void Start()
    {
        //Pour setup les perso
        InitialisationTempsMort();

        //Lance le dialogue.
        //StartIntroduction();
    }

    private void InitialisationTempsMort()
    {
        
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

[Serializable]
public class StartPosition
{
    public enum EActorClass
    {
        Koolkid, Grandma, Clown
    }

    [SerializeField]
    //EActorClass actor = new EActorClass(GameManager.instance.GetEnum());

    

    [SerializeField]
    GameObject InitialPosition;
    
    public GameObject GetPosition()
    {
        return InitialPosition;
    }
}
