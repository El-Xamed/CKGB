using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class T_TempsMort : MonoBehaviour
{
    #region Variables

    [SerializeField]
    Image[] charactersLittleResume;
    [SerializeField]
    Image[] charactersCompleteResume;
    [SerializeField]
    Button[] charactersButton;
    [SerializeField]
    Button[] actions;

    [SerializeField]
    List<StartPosition> listCharactersPositions;

    [SerializeField]
    GameObject[] characters;

    

    #region Animation
    Animation[] papoter;
    Animation[] observer;
    Animation[] revasser;
    #endregion

    GameObject actorActif;
    #endregion

    private void Awake()
    {
       
    }

    private void Start()
    {
        initiatePosition();
        //Pour setup les perso
        //InitialisationTempsMort();

        //Lance le dialogue.
        //StartIntroduction();
    }

    private void InitialisationTempsMort()
    {
        if (GameObject.Find("GameManager"))
        {
            //Place les personnage selon la liste des positions.
            foreach (var myPosition in listCharactersPositions)
            {
                //Regarde l'enum de l'objet.
                switch (myPosition.GetEnum())
                {
                    case EActorClass.Koolkid:
                        //Place le personnage sur cette position avec .
                        SetTransform(myPosition.GetPosition().transform, GameManager.instance.GetRessournce()[0]);
                        Debug.Log("Koolkid");
                        break;
                    case EActorClass.Grandma:
                        SetTransform(myPosition.GetPosition().transform, GameManager.instance.GetRessournce()[1]);
                        Debug.Log("Grandma");
                        break;
                    case EActorClass.Clown:
                        SetTransform(myPosition.GetPosition().transform, GameManager.instance.GetRessournce()[3]);
                        Debug.Log("Clown");
                        break;
                }
            }
        }
        else
        {
            Debug.Log("Pas de GameManager de détecté.");
        }
       
        void SetTransform(Transform position, GameObject actor)
        {
            //Pour chaque perso dans l'équipe.
            foreach (var myActor in GameManager.instance.GetTeam())
            {
                //Regarde si dans la liste d'acteur du GameManager est égale au GameObject des ressources, et que la resource n'est pas null. 
                if (myActor.GetComponent<C_Actor>().GetDataActor().name == actor.GetComponent<C_Actor>().GetDataActor().name && actor != null)
                {
                    /*
                    //check si il existe dans la scene pour le placer ou alors il le fait spawn à la bonne position.
                    if (GameObject.Find(myActor.GetDataActor().name))
                    {
                        Debug.Log("Deja spawn");

                        GameObject.Find(myActor.GetDataActor().name).transform.SetParent(position);
                    }
                    else
                    {
                        Debug.Log("Spawn");

                        Instantiate(myActor, position);
                    }
                    */
                }
            }
        }
    }

    #region Mes fonctions

    void initiatePosition()
    {
     for(int i=0;i<characters.Length;i++)
        {
            Instantiate(characters[i], listCharactersPositions[i].InitialPosition.gameObject.transform);
        }
    }
    //retour au choix de personnage et disparition des boutons de choix
    void NextActor()
    {

    }
    //fait apparaitre les boutons de choix
    void CharacterTurn(GameObject actualCharacter)
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
    #region buttons

  

    #endregion
}

[Serializable]
public class StartPosition
{
    [SerializeField]
    EActorClass actor;

    [SerializeField]
    public GameObject InitialPosition;
    
    public EActorClass GetEnum()
    {
        return actor;
    }

    public GameObject GetPosition()
    {
        return InitialPosition;
    }
}
