using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class C_credits : MonoBehaviour
{
    [SerializeField] GameObject skipButton;
    // Start is called before the first frame update
    void Start()
    {
        EventSystem Es = FindObjectOfType<EventSystem>();
        Es.SetSelectedGameObject(skipButton);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void GoToMainMenuALERT()
    {
        GameManager.instance.GoToMainMenuALERT();
    }
}
