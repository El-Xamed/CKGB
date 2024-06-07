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
    [SerializeField] Image cadenas;
    [SerializeField] GameObject borderPrefab;
    [SerializeField] Image calmJaugePreview;

    [Header("Stats PDP")]
    [SerializeField] Image heart;

    [Header("Stats (Text)")]
    [SerializeField] TMP_Text textCalm;
    [SerializeField] TMP_Text textEnergie;

    [Header("Jauge")]
    [SerializeField] Image uiCalm;
    [SerializeField] RectTransform uiBorderJauge;
    [SerializeField] Sprite uiPvJaugeGreen;
    [SerializeField] Sprite uiPvJaugeOrange;
    [SerializeField] Sprite uiPvJaugeRed;

    [Header("Background")]
    [SerializeField] Image uiPvBackground;
    [SerializeField] Sprite uiPvGreenBackground;
    [SerializeField] Sprite uiPvOrangeBackground;
    [SerializeField] Sprite uiPvRedBackground;

    [Header("Points")]
    [SerializeField] Image eclair;
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

        //Cache l'Ui de mort.
        cadenas.enabled = false;

        //Ini la petite éclair.
        eclair.enabled = false;

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
        textCalm.text = myActor.GetComponent<C_Actor>().GetCurrentStress() + "/" + myActor.GetDataActor().stressMax;
        textEnergie.text = myActor.GetComponent<C_Actor>().GetcurrentEnergy() + "/" + myActor.GetDataActor().energyMax;
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
            heart.color = Color.HSVToRGB(0.233f, 1, 1);
        }
        else if((float)myActor.GetComponent<C_Actor>().GetCurrentStress() < 2 / (float)myActor.GetDataActor().stressMax) //Si c'est pv sont au dessus des 1/3.
        {
            uiCalm.sprite = uiPvJaugeOrange;
            uiPvBackground.sprite = uiPvOrangeBackground;
            heart.color = Color.HSVToRGB(0.133f, 1, 1);
        }
        else if ((float)myActor.GetComponent<C_Actor>().GetCurrentStress() < 1 / (float)myActor.GetDataActor().stressMax) //Si c'est pv sont inf�rieur des 1/3.
        {
            uiCalm.sprite = uiPvJaugeRed;
            uiPvBackground.sprite = uiPvRedBackground;
            heart.color = Color.HSVToRGB(0, 1, 1);
        }

        if (myActor.GetComponent<C_Actor>().GetCurrentStress() == 0)
        {
            IsOut();
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
        if (myActor.GetcurrentEnergy() == 0)
        {
            eclair.enabled = true;
        }
        else
        {
            eclair.enabled = false;
        }

        for (int i = 0; i < myActor.GetcurrentEnergy(); i++)
        {
            listEnergie[i].SetActive(true);
        }
        #endregion
    }

    void IsOut()
    {
        //Cache l'Ui de mort.
        cadenas.enabled = true;
    }

    #region Preview
    public void ResetUiPreview()
    {
        Debug.Log("Reset");

        //Désinscrit les fonction.
        C_PreviewAction.onPreview -= UiPreviewCalm;
        C_PreviewAction.onPreview -= UiPreviewEnergy;

        //Par default desactive les anim de jauge.
        animatorUiCalmPreview.SetBool("isPreview", false);

        //Par default desactive les anim d'energie.
        foreach (var thisEnergy in listEnergie)
        {
            //Desactive la preview.
            thisEnergy.GetComponent<Animator>().SetBool("isPreview", false);
            Debug.Log(thisEnergy.name + " is desactivate");
        }
    }

    //A SUPP.
    /*Fonction pour afficher une preview d'une stats en particulier. BESOIN SUREMENT DE LE DECALER DANS L'ACTOR DIRECTEMENT.
    public void CheckUiPreview(SO_ActionClass thisActionClass, Interaction.ETypeTarget target)
    {
        Debug.Log("CheckUiPreview");

        //Désinscrit les fonction.
        C_PreviewAction.onPreview -= UiPreviewCalm;
        C_PreviewAction.onPreview -= UiPreviewEnergy;

        foreach (Interaction thisInteraction in thisActionClass.listInteraction)
        {
            //Check si c'est égale à "actorTarget".
            if (thisInteraction.whatTarget == target)
            {
                foreach (TargetStats thisTargetStats in thisInteraction.listTargetStats)
                {
                    //Check si c'est des stats ou un Mouvement.
                    if (thisTargetStats.whatStatsTarget == TargetStats.ETypeStatsTarget.Stats)
                    {
                        //Inscrit la preview de texte + ui. Avec les info de preview. (C_Challenge)
                        //onPreview += TextPreview;

                        //Check si c'est pour le calm.
                        if (thisTargetStats.whatStats == TargetStats.ETypeStats.Calm)
                        {
                            //Inscrit la preview de calm.
                            C_PreviewAction.onPreview += UiPreviewCalm;
                            Debug.Log("Add UiPreviewCalm");
                        }

                        //Check si c'est pour l'energie.
                        if (thisTargetStats.whatStats == TargetStats.ETypeStats.Energy)
                        {
                            //Inscrit la preview de calm.
                            C_PreviewAction.onPreview += UiPreviewEnergy;
                            Debug.Log("Add UiPreviewEnergy");
                        }
                    }
                }
            }
        }
    }*/

    public void UiPreviewCalm(SO_ActionClass thisActionClass)
    {
        //Lance l'animation de clignotoment.
        animatorUiCalmPreview.SetBool("isPreview", true);

        //Caclul pour la preview du calm actuel + de l'action.
        calmJaugePreview.fillAmount = CalculJauge(myActor.GetCurrentStress() + thisActionClass.GetValue(Interaction.ETypeTarget.Self, TargetStats.ETypeStatsTarget.Stats), myActor.GetMaxStress());
    }

    //PAS FINI ! + BUG INCONNUE, SA JOUE L'ANIMATION MAIS RIEN SE PASSE.
    public void UiPreviewEnergy(SO_ActionClass thisActionClass)
    {
        //Caclul pour la preview de l'energy actuel + de l'action. 
        //Pour la perte d'energy.
        if (thisActionClass.GetValue(Interaction.ETypeTarget.Self, TargetStats.ETypeStatsTarget.Stats) < 0)
        {
            //Commence par le nombre actuel d'energy.
            for (int i = myActor.GetcurrentEnergy() + thisActionClass.GetValue(Interaction.ETypeTarget.Self, TargetStats.ETypeStatsTarget.Stats); i < myActor.GetcurrentEnergy(); i++)
            {
                //Active la preview.
                listEnergie[i].GetComponent<Animator>().SetBool("isPreview", true);
                Debug.Log(listEnergie[i].name + " is activate");
            }
        }
    }
    #endregion
    #endregion
}
