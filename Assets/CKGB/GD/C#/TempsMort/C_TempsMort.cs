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
        foreach (var myPosition in listPositions)
        {
            switch (myPosition.GetEnum())
            {
                //Récupération automatique dans le dossier Resources.
                case EActorClass.Koolkid:
                    test(myPosition.GetPosition().transform);
                    Debug.Log("Koolkid");
                    break;
                case EActorClass.Grandma:
                    test(myPosition.GetPosition().transform);
                    Debug.Log("Grandma");
                    break;
                case EActorClass.Clown:
                    test(myPosition.GetPosition().transform);
                    Debug.Log("Clown");
                    break;
            }
        }

        void test(Transform position)
        {
            foreach (var myActor in GameManager.instance.GetTeam())
            {
                for (int i = 0; i < GameManager.instance.GetRessournce().Length -1; i++)
                {
                    if (myActor.GetComponent<Proto_Actor>().GetDataActor().name == GameManager.instance.GetRessournce()[i].GetComponent<Proto_Actor>().GetDataActor().name)
                    {
                        if (GameObject.Find(myActor.GetDataActor().name))
                        {
                            Debug.Log("Exists");

                            GameObject.Find(myActor.GetDataActor().name).transform.SetParent(position);
                        }
                        else
                        {
                            Debug.Log("Doesn't exist");

                            Instantiate(myActor, position);
                        }
                    }
                }
                
            }
        }
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
    [SerializeField]
    EActorClass actor;

    [SerializeField]
    GameObject InitialPosition;
    
    public EActorClass GetEnum()
    {
        return actor;
    }

    public GameObject GetPosition()
    {
        return InitialPosition;
    }
}
