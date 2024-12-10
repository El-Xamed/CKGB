using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class C_Stats : MonoBehaviour
{
    #region data
    //Etat de la stats
    EStateStats stateActor;
    enum EStateStats { Eleve, Moyen, Bas}

    [Header("Stats PDP")]
    [SerializeField] Image PDP;
    [SerializeField] Image cadenas;
    [SerializeField] GameObject borderPrefab;
    [SerializeField] Image calmJaugePreview1;
    [SerializeField] Image calmJaugePreview2;

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

    [Header("Points d'énergie")]
    [SerializeField] Image eclair;
    [SerializeField] List<RectTransform> uiSpawnEnergie;
    [SerializeField] GameObject prefabEnergie;
    List<GameObject> listEnergie = new List<GameObject>();

    [Header("Preview")]
    [SerializeField] Animator animatorUiCalmPreview;
    [SerializeField] Image previewBarre;

    //Couleur
    Color32 colorStatsActor;

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

        //Place les point d'énergies.
        SpawnEnergy(thisActor.GetComponent<C_Actor>().GetMaxEnergy());

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

    void SpawnEnergy(int nbEnergy)
    {
        //Cache toutes les branches.
        foreach (var thisEnergy in uiSpawnEnergie)
        {
            thisEnergy.gameObject.SetActive(false);
        }

        //Fait spawn les nombre max d'energie de l'actor.
        for (int i = 0; i < nbEnergy; i++)
        {
            //Active le fond.
            uiSpawnEnergie[i].gameObject.SetActive(true);

            //création d'une nouvelle bille.
            GameObject newEnergieGameObject = Instantiate(prefabEnergie, uiSpawnEnergie[i]);

            //Nouveau nom.
            newEnergieGameObject.name = "UI_Stats_" + myActor.name + "_Energie_Pastille_ " + (i + 1);

            listEnergie.Add(newEnergieGameObject);
        }
    }
    #endregion

    float CalculJauge(float currentValue, float maxValue)
    {
        float currentWidth = (float)currentValue / maxValue;

        return currentWidth;
    }

    void CheckStatsOfActor(float currentPv, float maxPv)
    {
        if (currentPv > 2f / 3f * maxPv)
        {
            stateActor = EStateStats.Eleve;
            colorStatsActor = Color.HSVToRGB(0.233f, 1, 1);
        }
        else if (currentPv < 2f / 3f * maxPv && currentPv > 1f / 3f * maxPv) //Si c'est pv sont au dessus des 1/3.
        {
            stateActor = EStateStats.Moyen;
            colorStatsActor = Color.HSVToRGB(0.133f, 1, 1);
        }
        else if (currentPv < 1f / 3f * maxPv) //Si c'est pv sont inf�rieur des 1/3.
        {
            stateActor = EStateStats.Bas;
            colorStatsActor = Color.HSVToRGB(0, 1, 1);
        }
    }

    [ContextMenu("Check etat des pv")]
    void UpdateUi()
    {
        UpdateUi(myActor);
    }

    public void UpdateUi(C_Actor myActor)
    {
        CheckStatsOfActor(myActor.GetCurrentStress(), myActor.GetDataActor().stressMax);

        #region Text
        textCalm.text = TextUtils.GetColorText(myActor.GetComponent<C_Actor>().GetCurrentStress().ToString(), colorStatsActor) + "/" + myActor.GetDataActor().stressMax;
        textEnergie.text = myActor.GetComponent<C_Actor>().GetcurrentEnergy() + "/" + myActor.GetDataActor().energyMax;
        #endregion

        #region Jauge

        //Update le calm.
        uiCalm.fillAmount = CalculJauge(myActor.GetComponent<C_Actor>().GetCurrentStress(), myActor.GetDataActor().stressMax);

        #region Couleur de la jauge et autres
        //Check l'�tat de l'actor
        //Si c'est pv sont au dessus des 2/3.
        if (stateActor == EStateStats.Eleve)
        {
            uiCalm.sprite = uiPvJaugeGreen;
            uiPvBackground.sprite = uiPvGreenBackground;
        }
        else if(stateActor == EStateStats.Moyen) //Si c'est pv sont au dessus des 1/3.
        {
            uiCalm.sprite = uiPvJaugeOrange;
            uiPvBackground.sprite = uiPvOrangeBackground;
        }
        else if (stateActor == EStateStats.Bas) //Si c'est pv sont inf�rieur des 1/3.
        {
            uiCalm.sprite = uiPvJaugeRed;
            uiPvBackground.sprite = uiPvRedBackground;
        }

        //Set la couleur du coeur.
        heart.color = colorStatsActor;
        previewBarre.color = colorStatsActor;

        #endregion
        #endregion

        #region Energie
        //Update l'energie.

        //Desactive les points d'energies.
        foreach (GameObject thisEnergy in listEnergie)
        {
            thisEnergy.SetActive(false);
        }

        //Check si il reste encore de l'energie.
        if (myActor.GetcurrentEnergy() == 0)
        {
            eclair.enabled = true;
        }
        else
        {
            eclair.enabled = false;
        }

        //Active les points necessaire.
        for (int i = 0; i < myActor.GetcurrentEnergy(); i++)
        {
            listEnergie[i].SetActive(true);
        }
        #endregion
    }

    #region Preview
    public void ResetUiPreview()
    {
        Debug.Log("Reset");

        //Par default desactive les anim de jauge.
        animatorUiCalmPreview.SetBool("isPreview", false);

        //Par default desactive les anim d'energie.
        foreach (var thisEnergy in listEnergie)
        {
            //Desactive la preview.
            thisEnergy.GetComponent<Animator>().SetBool("isPreview", false);
            //Debug.Log(thisEnergy.name + " is desactivate");
        }

        //Désinscrit les fonction.
        C_PreviewAction.onPreview -= UiPreviewCalm;
        C_PreviewAction.onPreview -= UiPreviewEnergy;
    }

    Interaction.ETypeTarget target;

    public void SetTarget(Interaction.ETypeTarget thisTarget)
    {
        target = thisTarget;
    }

    public void UiPreviewCalm(SO_ActionClass thisActionClass)
    {
        //Lance l'animation de clignotoment.
        animatorUiCalmPreview.SetBool("isPreview", true);

        //Calcul si c'est un gain ou une perte.
        if (thisActionClass.GetValue(target, TargetStats.ETypeStatsTarget.Stats) > 0)
        {
            //Caclul pour la preview du calm actuel + de l'action (Gain).
            calmJaugePreview1.fillAmount = CalculJauge(myActor.GetCurrentStress() + thisActionClass.GetValue(target, TargetStats.ETypeStatsTarget.Stats), myActor.GetMaxStress());
            calmJaugePreview2.fillAmount = CalculJauge(myActor.GetCurrentStress(), myActor.GetMaxStress());
        }
        else if (thisActionClass.GetValue(target, TargetStats.ETypeStatsTarget.Stats) < 0)
        {
            //Caclul pour la preview du calm actuel + de l'action (Perte).
            calmJaugePreview1.fillAmount = CalculJauge(myActor.GetCurrentStress(), myActor.GetMaxStress());
            calmJaugePreview2.fillAmount = CalculJauge(myActor.GetCurrentStress() + thisActionClass.GetValue(target, TargetStats.ETypeStatsTarget.Stats), myActor.GetMaxStress());
        }
    }

    //PAS FINI ! + BUG INCONNUE, SA JOUE L'ANIMATION MAIS RIEN SE PASSE.
    public void UiPreviewEnergy(SO_ActionClass thisActionClass)
    {
        Debug.Log("Preview Energy de " + name);

        //Caclul pour la preview de l'energy actuel + de l'action. 
        //Pour la perte d'energy.
        if (thisActionClass.GetValue(target, TargetStats.ETypeStatsTarget.Stats) < 0)
        {
            //Check si l'actor possède l'energie.
            if (myActor.GetcurrentEnergy() > 0)
            {
                //Commence par le nombre actuel d'energy.
                for (int i = myActor.GetcurrentEnergy() + thisActionClass.GetValue(target, TargetStats.ETypeStatsTarget.Stats); i < myActor.GetcurrentEnergy(); i++)
                {
                    //Active la preview.
                    listEnergie[i].GetComponent<Animator>().SetBool("isPreview", true);
                    Debug.Log(listEnergie[i].name + " is activate");
                }
            }
        }
    }
    #endregion

    #endregion
}
