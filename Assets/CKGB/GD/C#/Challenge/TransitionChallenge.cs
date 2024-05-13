using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TransitionChallenge : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Animator>().SetTrigger("Open");
    }

    public void LunchIntro()
    {
        GetComponentInParent<C_Challenge>().StartIntroChallenge();
    }

    public void DesactiveInput()
    {
        GetComponentInParent<PlayerInput>().enabled = false;
    }
}
