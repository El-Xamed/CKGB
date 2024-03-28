using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class C_Case : MonoBehaviour
{
    #region Mes variables
    [SerializeField] GameObject vfxCata;
    #endregion

    #region Mes fonctions
    public void ShowDangerZone(GameObject newVfxCata)
    {
        //Si une cata à deja spaw.
        if (vfxCata != null) { return; }
        vfxCata = Instantiate(newVfxCata, transform);
    }

    public void ApplyNewStats()
    {

    }


    #endregion

    #region Data partagé

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
