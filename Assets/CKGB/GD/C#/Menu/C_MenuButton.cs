using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class C_MenuButton : MonoBehaviour
{
    [SerializeField] Button firtButton;

    public void SetFirstButton()
    {
        GameObject.Find("EventSystem").GetComponent<EventSystem>().firstSelectedGameObject = firtButton.gameObject;
    }


}
