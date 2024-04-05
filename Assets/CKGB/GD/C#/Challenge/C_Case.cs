using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;

public class C_Case : MonoBehaviour
{
    #region Mes variables
    GameObject vfxCata;

    [SerializeField] Sprite addNumberSprite;
    [SerializeField] bool addNumber = true;
    #endregion

    #region Mes fonctions
    public void ShowDangerZone(GameObject newVfxCata)
    {
        //Si une cata à deja spaw.
        if (vfxCata != null) { return; }
        vfxCata = Instantiate(newVfxCata, transform);
    }
    #endregion

    #region Data partagé
    public bool AddNumber(int number)
    {
        if (addNumber)
        {
            GetComponent<Image>().sprite = addNumberSprite;

            transform.GetChild(0).gameObject.SetActive(true);

            return true; 
        }

        transform.GetChild(0).gameObject.SetActive(false);

        return false;
    }   

    public GameObject GetVfxCata()
    {
        return vfxCata;
    }

    public void DestroyVfxCata()
    {
        if (vfxCata != null)
        {
            Destroy(vfxCata);
            vfxCata = null;
        }
    }
    #endregion
}
