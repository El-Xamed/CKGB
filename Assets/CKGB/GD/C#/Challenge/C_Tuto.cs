using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_Tuto : MonoBehaviour
{
    public void NextTuto(int tutoIndex)
    {
        GetComponent<Animator>().SetTrigger("NextTutoEtape" + tutoIndex);
    }

    public void EndTuto()
    {
        GetComponentInParent<C_Challenge>().GetInterface().SetCurrentInterface(C_Interface.Interface.Neutre);
    }

     //J'aimerais que cette fonction fasse l'animation du menu de trait qui s'ouvre.
    public void OpenTraits()
    {
        GetComponentInParent<C_Challenge>().GetInterface().GoTraits();
    }

    //J'aimerais que cette fonction fasse l'animation du menu d'action qui s'ouvre.
    public void OpenActions()
    {
        GetComponentInParent<C_Challenge>().GetInterface().GoAction();
    }

    //J'aimerais que cette fonction fasse l'animation du menu de trait qui se ferme.
    public void CloseInterface()
    {
        GetComponentInParent<C_Challenge>().GetInterface().GoBack();
    }

    //J'aimerais que cette fonction fasse l'animation de la jauge de calme qui descend jusqu'au rouge.
     public void CalmDown()
    {
        //GetComponentInParent<C_Challenge>().GetInterface().SetCurrentInterface();
    }

    //J'aimerais que cette fonction fasse l'animation de la jauge de calme qui remonte full.
     public void CalmUp()
    {
        //GetComponentInParent<C_Challenge>().GetInterface().SetCurrentInterface();
    }
}
