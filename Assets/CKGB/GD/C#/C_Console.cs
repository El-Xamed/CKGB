using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class C_Console : MonoBehaviour
{
    bool isOpen = false;
    [SerializeField] GameObject console;
    GameObject lastButton;

    [SerializeField] List<DataChallenge> listDataChallenge;

    private void Start()
    {
        console.SetActive(isOpen);
    }

    public void OpenConsole(InputAction.CallbackContext context)
    {
        if (!context.performed) { return; }

        if (context.performed)
        {
            OpenConsole();
        }
    }

    void OpenConsole()
    {
        if (isOpen)
        {
            isOpen = false;
            EventSystem.current.SetSelectedGameObject(lastButton);
        }
        else
        {
            isOpen = true;
            lastButton = EventSystem.current.currentSelectedGameObject;
            EventSystem.current.SetSelectedGameObject(console);
            StartCoroutine(SelectInputField());
        }

        console.SetActive(isOpen);
    }

    public void ConfirmCommand()
    {
        switch (EventSystem.current.currentSelectedGameObject.GetComponent<TMP_InputField>().text)
        {
            case "launch_lvl_tuto":
                MoveAllAcctorInGameManager();
                GameManager.instance.currentC = listDataChallenge[0].GetDataChallenge();
                GameManager.instance.ExitDialogueMode();
                SceneManager.LoadScene("S_Challenge");
                break;
            case "launch_lvl_1":
                MoveAllAcctorInGameManager();
                GameManager.instance.currentC = listDataChallenge[1].GetDataChallenge();
                GameManager.instance.ExitDialogueMode();
                SceneManager.LoadScene("S_Challenge");
                break;
            case "launch_lvl_2":
                MoveAllAcctorInGameManager();
                GameManager.instance.currentC = listDataChallenge[2].GetDataChallenge();
                GameManager.instance.ExitDialogueMode();
                SceneManager.LoadScene("S_Challenge");
                break;
            case "launch_lvl_3":
                MoveAllAcctorInGameManager();
                GameManager.instance.currentC = listDataChallenge[3].GetDataChallenge();
                GameManager.instance.ExitDialogueMode();
                SceneManager.LoadScene("S_Challenge");
                break;
            case "reset_challenge":
                GameManager.instance.ExitDialogueMode();
                SceneManager.LoadScene("S_Challenge");
                break;
            case "launch_Tuto":
                Debug.Log("Tuto pas encore pret");
                //GameObject.Find("Interface").GetComponent<C_Challenge>().LaunchTuto();
                break;
            case "endChallenge":
                GameObject.Find("Challenge").GetComponent<C_Challenge>().EndChallenge();
                break;
        }

        OpenConsole();
    }

    IEnumerator SelectInputField()
    {
        yield return new WaitForEndOfFrame();
        EventSystem.current.currentSelectedGameObject.GetComponent<TMP_InputField>().ActivateInputField();
    }

    void MoveAllAcctorInGameManager()
    {
        foreach (var thisActor in GameManager.instance.GetTeam())
                {
                    thisActor.GetComponent<Animator>().SetBool("isInDanger", false);
                    thisActor.transform.parent = GameManager.instance.transform;
                    thisActor.GetComponent<C_Actor>().GetImageActor().enabled = false;
                }
    }

    public void LoadScene(InputAction.CallbackContext context)
    {
        if (!context.performed) { return; }

        if (context.performed)
        {
            
        }
    }
}

[Serializable]
public class DataChallenge
{
    [SerializeField] SO_Challenge data;

    public SO_Challenge GetDataChallenge()
    {
        return data;
    }
}
