using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class C_Stats : MonoBehaviour
{
    [Header("Stats PDP")]
    [SerializeField] Image PDP;
    [Header("Stats (Text)")]
    [SerializeField] TMP_Text textCalm;
    [SerializeField] TMP_Text textEnergie;

    [Header("Jauge/Points")]
    [SerializeField] GameObject uiCalm;
    [SerializeField] GameObject uiEnergie;

    public void UpdateUi(C_Actor myActor)
    {
        //Setup le PDP.
        PDP.sprite = myActor.dataActor.challengeSpriteUi;

        //Setup le text.
        textCalm.text = myActor.GetComponent<C_Actor>().GetCurrentStress() + " / " + myActor.dataActor.stressMax;
        textEnergie.text = myActor.GetComponent<C_Actor>().GetcurrentEnergy() + " / " + myActor.dataActor.energyMax;

        //Setup le calm.

        //Setup l'energie.

    }
}
