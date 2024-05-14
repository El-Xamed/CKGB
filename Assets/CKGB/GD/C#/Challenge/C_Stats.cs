using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class C_Stats : MonoBehaviour
{
    #region data
    [Header("Stats PDP")]
    [SerializeField] Image PDP;
    [SerializeField] GameObject borderPrefab;
    [SerializeField] Image calmJaugePreview;

    [Header("Stats (Text)")]
    [SerializeField] TMP_Text textCalm;
    [SerializeField] TMP_Text textEnergie;

    [Header("Jauge")]
    [SerializeField] Image uiCalm;
    [SerializeField] RectTransform uiBorderJauge;
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
    [SerializeField] RectTransform uiEnergie;
    [SerializeField] GameObject prefabEnergie;
    List<GameObject> listEnergie = new List<GameObject>();

    [Header("Preview")]
    [SerializeField] Animator animatorUiCalmPreview;
    [SerializeField] Animator animatorUiEnergyPreview;
    [SerializeField] Image uiCalmPreview;

    //BESOIN DE REFAIRE L'ULM.
    C_Actor myActor;
    #endregion

    #region Fonctions
    #region Ini
    public void InitUiStats(C_Actor thisActor)
    {
        //Setup le PDP.
        PDP.sprite = thisActor.GetDataActor().challengeSpriteUi;

        //Place les bordures par rapport au nombres de calm que poss�de le personnage.
        SpawnBorderCalm(thisActor.GetComponent<C_Actor>().GetMaxStress());

        UpdateUi(thisActor);

        //Sécu anim.
        animatorUiCalmPreview.SetBool("isPreview", false);
    }

    public void SetActor(C_Actor thisActor)
    {
        myActor = thisActor;
    }

    void SpawnBorderCalm(int nbCalm)
    {
        float calmWidth = 360 / nbCalm;

        //new Quaternion(0, 0, calmWidth * i, 1)

        for (int i = 0; i < nbCalm; i++)
        {
            GameObject newBorder = Instantiate(borderPrefab, PDP.transform.position, Quaternion.Euler(0, 0, calmWidth * i), uiBorderJauge);

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
        //Check si la stats possède bien le nombre max d'energie.
        if (uiEnergie.transform.childCount < myActor.GetMaxEnergy())
        {
            //Créer la pool d'energy.
            for (int i = listEnergie.Count; i < myActor.GetMaxEnergy(); i++)
            {
                //Cr�ation d'un nouvel GameObject.
                GameObject newEnergieGameObject = Instantiate(prefabEnergie, uiEnergie);
                //Nouveau nom.
                newEnergieGameObject.name = "UI_Stats_" + myActor.name + "_Energie_Pastille_ " + (i + 1);

                //Ajoute dans la liste.
                listEnergie.Add(newEnergieGameObject);
            }
        }

        //Desactive les points d'energies.
        foreach (GameObject thisEnergy in listEnergie)
        {
            thisEnergy.SetActive(false);
        }

        //Active les points necessaire.
        for (int i = 0; i < myActor.GetcurrentEnergy(); i++)
        {
            listEnergie[i].SetActive(true);
        }

        /*Old
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
        }*/
        #endregion
    }


    #region Preview
    public void ResetUiPreview()
    {
        //Stop toutes les preview. AJOUTER LE DEV POUR L'ENERGIE.
        animatorUiCalmPreview.SetBool("isPreview", false);
    }

    //Fonction pour afficher une preview d'une stats en particulier. BESOIN SUREMENT DE LE DECALER DANS L'ACTOR DIRECTEMENT.
    public void CheckUiPreview(SO_ActionClass thisActionClass, Interaction_NewInspector.ETypeTarget target)
    {
        foreach (Interaction_NewInspector thisInteraction in thisActionClass.newListInteractions)
        {
            //Check si c'est égale à "actorTarget".
            if (thisInteraction.whatTarget == target)
            {
                foreach (TargetStats_NewInspector thisTargetStats in thisInteraction.listTargetStats)
                {
                    //Check si c'est des stats ou un Mouvement.
                    if (thisTargetStats.whatStatsTarget == TargetStats_NewInspector.ETypeStatsTarget.Stats)
                    {
                        //Inscrit la preview de texte + ui. Avec les info de preview. (C_Challenge)
                        //onPreview += TextPreview;

                        //Check si c'est pour le calm.
                        if (thisTargetStats.whatStats == TargetStats_NewInspector.ETypeStats.Calm)
                        {
                            //Inscrit la preview de calm.
                            C_PreviewAction.onPreview += UiPreviewCalm;
                        }
                        //Check si c'est pour l'energie.
                        else if (thisTargetStats.whatStats == TargetStats_NewInspector.ETypeStats.Energy)
                        {
                            //Inscrit la preview de calm.
                            //C_PreviewAction.onPreview += UiPreviewEnergy;
                        }
                    }
                }
            }
        }
    }

    public void UiPreviewCalm(SO_ActionClass thisActionClass)
    {
        //Lance l'animation de clignotoment.
        animatorUiCalmPreview.SetBool("isPreview", true);

        //Caclul pour la preview du calm actuel + de l'action.
        calmJaugePreview.fillAmount = CalculJauge(myActor.GetCurrentStress() + thisActionClass.GetValue(Interaction_NewInspector.ETypeTarget.Self, TargetStats_NewInspector.ETypeStatsTarget.Stats), myActor.GetMaxStress());
    }

    //PAS FINI ! + BUG INCONNUE, SA JOUE L'ANIMATION MAIS RIEN SE PASSE.
    public void UiPreviewEnergy(SO_ActionClass thisActionClass)
    {
        //Caclul pour la preview de l'energy actuel + de l'action. 
        //Pour la perte d'energy.
        if (thisActionClass.GetValue(Interaction_NewInspector.ETypeTarget.Self, TargetStats_NewInspector.ETypeStatsTarget.Stats) < 0)
        {
            //Commence par le nombre actuel d'energy.
            for (int i = myActor.GetcurrentEnergy() + thisActionClass.GetValue(Interaction_NewInspector.ETypeTarget.Self, TargetStats_NewInspector.ETypeStatsTarget.Stats); i < myActor.GetcurrentEnergy(); i++)
            {
                //Active la preview.
                listEnergie[i].GetComponent<Animator>().SetBool("isPreview", true);
            }
        }
    }
    #endregion
    #endregion
}
