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

    [Header("Jauge")]
    [SerializeField] Image uiCalm;
    [SerializeField] Sprite uiPvJaugeGreen;
    [SerializeField] Sprite uiPvJaugeOrange;
    [SerializeField] Sprite uiPvJaugeRed;
    [SerializeField] Sprite uiPvJaugeTetanise;

    [Header("Background")]
    [SerializeField] Image uiPvBackground;
    [SerializeField] Sprite uiPvGreenBackground;
    [SerializeField] Sprite uiPvOrangeBackground;
    [SerializeField] Sprite uiPvRedBackground;

    [Header("Points")]
    [SerializeField] GameObject uiEnergie;
    [SerializeField] Sprite spriteEnergie;
    List<GameObject> listEnergie = new List<GameObject>();
    public void InitUiStats(C_Actor thisActor)
    {
        //Setup le PDP.
        PDP.sprite = thisActor.GetDataActor().challengeSpriteUi;

        //Place les bordures par rapport au nombres de calm que poss�de le personnage.
        SpawnBorderCalm(thisActor.GetComponent<C_Actor>().GetMaxStress());

        UpdateUi(thisActor);
    }

    void SpawnBorderCalm(int nbCalm)
    {
        float calmWidth = 360 / nbCalm;

        //new Quaternion(0, 0, calmWidth * i, 1)

        for (int i = 0; i < nbCalm; i++)
        {
            GameObject newBorder = Instantiate(borderPrefab, uiCalm.transform.position, Quaternion.Euler(0, 0, calmWidth * i), uiCalm.transform);

            newBorder.name = "Border " + i;
        }
    }

    public void UpdateUi(C_Actor myActor)
    {
        #region Text
        //Update le text.
        textCalm.text = myActor.GetComponent<C_Actor>().GetCurrentStress() + " / " + myActor.GetDataActor().stressMax;
        textEnergie.text = myActor.GetComponent<C_Actor>().GetcurrentEnergy() + " / " + myActor.GetDataActor().energyMax;
        #endregion

        #region Jauge
        //Calcul.
        float calmWidth = (float)myActor.GetComponent<C_Actor>().GetCurrentStress() / (float)myActor.GetDataActor().stressMax;

        //Update le calm.
        uiCalm.fillAmount = calmWidth;
        //Check l'�tat de l'actor
        //Si c'est pv sont au dessus des 2/3.
        if ((float)myActor.GetComponent<C_Actor>().GetCurrentStress() > 2 / (float)myActor.GetDataActor().stressMax)
        {
            uiCalm.sprite = uiPvJaugeGreen;
            uiPvBackground.sprite = uiPvGreenBackground;
        }
        else if((float)myActor.GetComponent<C_Actor>().GetCurrentStress() < 2 / (float)myActor.GetDataActor().stressMax) //Si c'est pv sont au dessus des 1/3.
        {
            uiCalm.sprite = uiPvJaugeOrange;
            uiPvBackground.sprite = uiPvOrangeBackground;
        }
        else if ((float)myActor.GetComponent<C_Actor>().GetCurrentStress() < 1 / (float)myActor.GetDataActor().stressMax) //Si c'est pv sont inf�rieur des 1/3.
        {
            uiCalm.sprite = uiPvJaugeRed;
            uiPvBackground.sprite = uiPvRedBackground;
        }
        #endregion

        #region Energie
        //Update l'energie.
        //Check si le nombre est bon
        if (listEnergie.Count -1 < myActor.GetComponent<C_Actor>().GetcurrentEnergy())
        {
            //Regarde combien de points il manque.
            for (int i = listEnergie.Count; i < myActor.GetComponent<C_Actor>().GetcurrentEnergy(); i++)
            {
                //Cr�ation d'un nouvel GameObject.
                GameObject newEnergieGameObject = new GameObject();
                newEnergieGameObject.name = "UI_Stats_" + myActor.name + "_Energie_Pastille_ " + (i + 1);
                newEnergieGameObject.AddComponent<Image>();
                newEnergieGameObject.GetComponent<Image>().sprite = spriteEnergie;
                newEnergieGameObject.transform.parent = uiEnergie.transform;
                newEnergieGameObject.transform.localScale = Vector3.one;
                newEnergieGameObject.transform.localPosition = Vector3.zero;

                //Ajoute dans la liste.
                listEnergie.Add(newEnergieGameObject);
            }
        }
        else if (listEnergie.Count > myActor.GetComponent<C_Actor>().GetcurrentEnergy())
        {
            //Regarde si il y a pas trop de points d'energie.
            for (int i = listEnergie.Count; i > myActor.GetComponent<C_Actor>().GetcurrentEnergy(); i--)
            {
                //Detruit les points en trop.
                Debug.Log(i);
                Destroy(listEnergie[i -1]);
            }
        }
        #endregion
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
            Debug.Log("Prix de stress trop �lev� !");
        }
        */
    }

    public void DesactivedAllPreview()
    {
        GetComponent<Animator>().SetBool("isPreview", false);
    }
}
