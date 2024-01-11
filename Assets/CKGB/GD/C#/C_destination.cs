using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build.Content;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class C_destination : MonoBehaviour
{
    [SerializeField]
    GameObject actor;

    [SerializeField]
    SceneAsset destination;

    bool finished;
    bool isBusyByTeam;
    //Variable de scene

    public void GoToLevel()
    {
        //Rajoute dans l'�quipe l'acteur en question.
        AddActorInTeam();

        //Change l'actionMap dans le GameManager.
        GameManager.instance.ChangeActionMap("TempsMort");

        //Lance la scene selectionn�.
        SceneManager.LoadScene(destination.name);

        Debug.Log("Load Scene...");
    }

    //Pour ajouter l'acteur dans l'�quipe
    void AddActorInTeam()
    {

    }
}
