using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class C_Console : MonoBehaviour
{
    bool isOpen = false;
    [SerializeField] GameObject console;
    GameObject lastButton;

    private void Start()
    {
        console.SetActive(isOpen);
    }

    public void OpenConsole(InputAction.CallbackContext context)
    {
        if (!context.performed) { return; }

        if (context.performed)
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
                Debug.Log(GetComponentInChildren<TMP_InputField>().gameObject.name);
                EventSystem.current.SetSelectedGameObject(GetComponentInChildren<TMP_InputField>().gameObject);
            }

            console.SetActive(isOpen);
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
