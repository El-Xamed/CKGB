using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

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

    [Header("Preview")]
    [SerializeField] Image uiCalmPreview;

    #region Ini
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
            GameObject newBorder = Instantiate(borderPrefab, PDP.transform.position, Quaternion.Euler(0, 0, calmWidth * i), uiCalm.transform);

            newBorder.name = "Border " + i;
        }
    }
    #endregion

    float CalculJauge(int currentValue, int maxValue)
    {
        float currentWidth = (float)currentValue / maxValue;

        return currentWidth;
    }

    public void UpdateUi(C_Actor myActor)
    {
        #region Text
        //Update le text.
        textCalm.text = myActor.GetComponent<C_Actor>().GetCurrentStress() + " / " + myActor.GetDataActor().stressMax;
        textEnergie.text = myActor.GetComponent<C_Actor>().GetcurrentEnergy() + " / " + myActor.GetDataActor().energyMax;
        #endregion

        #region Jauge

        //Update le calm.
        uiCalm.fillAmount = CalculJauge(myActor.GetComponent<C_Actor>().GetCurrentStress(), myActor.GetDataActor().stressMax);

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


    #region Preview
    //Fonction pour afficher une preview d'une stats en particulier.
    public void UiPreview(SO_ActionClass thisActionClass, C_Actor thisActor)
    {
        //Check si la liste n'est pas vide
        if (thisActionClass.newListInteractions.Count != 0)
        {
            Debug.Log("La liste n'est pas vide !");

            //Pour "Self".
            SetupUiStatsPreview(Interaction_NewInspector.ETypeTarget.Self, thisActor);

            //Pour "Other".
            SetupUiStatsPreview(Interaction_NewInspector.ETypeTarget.Other, thisActor);
        }

        void SetupUiStatsPreview(Interaction_NewInspector.ETypeTarget target, C_Actor thisActor)
        {
            Debug.Log("Preview pour " + thisActor);

            foreach (Interaction_NewInspector thisInteraction in thisActionClass.newListInteractions)
            {
                //Check si c'est égale à "actorTarget".
                if (thisInteraction.whatTarget == target)
                {
                    //Applique à l'actor SEULEMENT LES STATS les stats.
                    foreach (TargetStats_NewInspector thisTargetStats in thisInteraction.listTargetStats)
                    {
                        //Check si c'est des stats ou un Mouvement.
                        if (thisTargetStats.whatStatsTarget == TargetStats_NewInspector.ETypeStatsTarget.Stats)
                        {
                            int value = 0;

                            if (thisTargetStats.whatCost == TargetStats_NewInspector.ETypeCost.Price)
                            {
                                //Retourne une valeur négative.
                                value = -thisTargetStats.value;
                            }
                            else if (thisTargetStats.whatCost == TargetStats_NewInspector.ETypeCost.Gain)
                            {
                                //Retourne une valeur positive.
                                value = thisTargetStats.value;
                            }

                            //Check si c'est pour le calm ou l'energie.
                            if (thisTargetStats.whatStats == TargetStats_NewInspector.ETypeStats.Calm)
                            {
                                //Applique une preview sur le calm.
                                PreviewCalm(value, thisActor);
                            }
                            else if (thisTargetStats.whatStats == TargetStats_NewInspector.ETypeStats.Energy)
                            {
                                //Applique une preview sur l'enrgie.
                            }
                        }
                    }
                }
            }
        }

        void PreviewCalm(int value, C_Actor thisActor)
        {
            //Update le calm.
            uiCalmPreview.fillAmount = CalculJauge(value, thisActor.GetDataActor().stressMax);

            GetComponent<Animator>().SetBool("isPreview", true);
        }
    }
    #endregion
}
