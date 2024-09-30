using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_Tuto : MonoBehaviour
{
    public void LaunchTuto(int tutoIndex)
    {
        GetComponent<Animator>().SetTrigger("LaunchTutoEtape" + tutoIndex);
    }

    public void NextTuto(int tutoIndex)
    {
        GetComponent<Animator>().SetTrigger("NextTutoEtape" + tutoIndex);
    }

    public void EndTuto()
    {
        GetComponentInParent<C_Challenge>().GetInterface().SetCurrentInterface(C_Interface.Interface.Neutre);
    }

    #region anim interface
    //J'aimerais que cette fonction fasse l'animation du menu de trait qui s'ouvre.
    public void OpenTraits()
    {
        GetComponentInParent<C_Challenge>().GetInterface().GoTraits();

        //Sup le premier bouton.
        GetComponentInParent<C_Challenge>().GetEventSystem().SetSelectedGameObject(null);
    }

    //J'aimerais que cette fonction fasse l'animation du menu d'action qui s'ouvre.
    public void OpenActions()
    {
        GetComponentInParent<C_Challenge>().GetInterface().GoAction();
        //Sup le premier bouton.
        GetComponentInParent<C_Challenge>().GetEventSystem().SetSelectedGameObject(null);
    }

    //J'aimerais que cette fonction fasse l'animation du menu de trait qui se ferme.
    public void CloseInterface()
    {
        GetComponentInParent<C_Challenge>().GetInterface().GoBack();
    }
    #endregion

    #region anim Ui stats
    public void PlayAnimDeathUiStats()
    {
        foreach (C_Actor thisActor in GetComponentInParent<C_Challenge>().GetTeam())
        {
            //Joue sur tous les actor l'nim de "t'étanisation" sur l'ui des tats.
            thisActor.GetUiStats().GetComponent<Animator>().SetBool("isOut", true);
        }
    }

    public void StopAnimDeathUiStats()
    {
        foreach (C_Actor thisActor in GetComponentInParent<C_Challenge>().GetTeam())
        {
            //Joue sur tous les actor l'nim de "t'étanisation" sur l'ui des tats.
            thisActor.GetUiStats().GetComponent<Animator>().SetBool("isOut", false);
        }
    }
    #endregion

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
