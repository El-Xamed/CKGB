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
    [SerializeField] GameObject borderPrefab;
    [SerializeField] Image calmJaugePreview;

    [Header("Stats (Text)")]
    [SerializeField] TMP_Text textCalm;
    [SerializeField] TMP_Text textEnergie;

    [Header("Jauge/Points")]
    [SerializeField] GameObject uiCalm;
    [SerializeField] GameObject uiEnergie;

    public void InitUiStats(C_Actor thisActor)
    {
        //Setup le PDP.
        PDP.sprite = thisActor.GetDataActor().challengeSpriteUi;

        //Place les bordures par rapport au nombres de calm que possède le personnage.
        SpawnBorderCalm(thisActor.GetComponent<C_Actor>().getMaxStress());

    }

    void SpawnBorderCalm(int nbCalm)
    {
        float calmWidth = 360 / nbCalm;

        //new Quaternion(0, 0, calmWidth * i, 1)

        for (int i = 0; i < nbCalm; i++)
        {
            GameObject newBorder = Instantiate(borderPrefab, uiCalm.transform.position, Quaternion.Euler(0, 0, calmWidth * i), transform);

            newBorder.name = "Border " + i;
        }
    }

    public void UpdateUi(C_Actor myActor)
    {
        //Setup le text.
        textCalm.text = myActor.GetComponent<C_Actor>().GetCurrentStress() + " / " + myActor.GetDataActor().stressMax;
        textEnergie.text = myActor.GetComponent<C_Actor>().GetcurrentEnergy() + " / " + myActor.GetDataActor().energyMax;

        //Setup le calm.

        //Setup l'energie.

    }

    public void ActiveAllPreviewUI(List<C_Actor> otherActor, C_Actor thisActor, int range, C_ActionButton thisActionButon)
    {
        ActiveSelfPreviewUi(thisActor, thisActionButon);
    }

    //MODIFER LA FONCTION POUR QU'IL RECUPERE LES BONNE INFO (QUE SE SOIT "SELF" OU "OTHER").
    public void ActiveSelfPreviewUi(C_Actor thisActor, C_ActionButton thisActionButon)
    {
        /*
        calmJaugePreview.gameObject.SetActive(true);

        if (thisActor.GetCurrentStress() >= thisActionButon.GetSelfPriceCalm())
        {
            calmJaugePreview.fillAmount = ((float)thisActor.GetCurrentStress() - (float)thisActionButon.GetSelfPriceCalm()) / (float)thisActor.getMaxStress();
            GetComponent<Animator>().SetBool("isPreview", true);
        }
        else
        {
            Debug.Log("Prix de stress trop élevé !");
        }
        */
    }

    public void DesactivedAllPreview()
    {
        GetComponent<Animator>().SetBool("isPreview", false);
    }
}
