using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class C_UIButton : MonoBehaviour
{
    private Button button;


    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();

        
    }


    public void OnAnimationFinished()
    {
        button.onClick?.Invoke();

    }
    
}
