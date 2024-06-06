using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_Tuto : MonoBehaviour
{
    public void LaunchTuto()
    {
        GetComponent<Animator>().SetTrigger("next");
    }

    public void EndTuto()
    {
        GetComponentInParent<C_Challenge>().GetInterface().SetCurrentInterface(C_Interface.Interface.Neutre);
    }
}
