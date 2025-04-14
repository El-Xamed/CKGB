using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_Tuto : MonoBehaviour
{
    C_Challenge challenge;
    Animator anim;

    private void Start()
    {
        challenge = GetComponentInParent<C_Challenge>();
        anim = GetComponent<Animator>();
    }
    public void LaunchTuto(int tutoIndex)
    {
        anim.SetTrigger("LaunchTutoEtape" + tutoIndex);
    }

    public void NextTuto(int tutoIndex)
    {
        anim.SetTrigger("NextTutoEtape" + tutoIndex);
    }

    public void EndTuto()
    {
        challenge.GetInterface().EndInterfaceAnimationClose();
        challenge.GetInterface().SetCurrentInterface(C_Interface.Interface.Neutre);
    }

    #region anim interface
    //J'aimerais que cette fonction fasse l'animation du menu de trait qui s'ouvre.
    public void OpenTraits()
    {
        challenge.GetInterface().GoTraits();

        //Sup le premier bouton.
        challenge.GetEventSystem().SetSelectedGameObject(null);
    }

    //J'aimerais que cette fonction fasse l'animation du menu d'action qui s'ouvre.
    public void OpenActions()
    {
        challenge.GetInterface().GoAction();
        //Sup le premier bouton.
        challenge.GetEventSystem().SetSelectedGameObject(null);
    }

    //J'aimerais que cette fonction fasse l'animation du menu de trait qui se ferme.
    public void CloseInterface()
    {
        challenge.GetInterface().GoBack();
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
