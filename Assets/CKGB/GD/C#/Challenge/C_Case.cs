using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_Case : MonoBehaviour
{
    #region Mes variables
    GameObject vfxCata;
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

    public void DestroyVfxCata()
    {
        if (vfxCata != null)
        {
            Destroy(vfxCata);
        }
    }
    #endregion
}
