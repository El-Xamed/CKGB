using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_Case : MonoBehaviour
{
    #region Mes variables
    C_Actor myActor;

    GameObject vfxCata;
    #endregion

    #region Mes fonctions
    public void ShowDangerZone(GameObject vfxCata)
    {
        vfxCata = Instantiate(vfxCata, transform);
    }

    public void ApplyNewStats()
    {

    }
    #endregion

    #region Data partagé
    public C_Actor GetIsBusy()
    {
        return myActor;
    }

    public void DestroyVfxCata()
    {
        if (vfxCata != null)
        {
            Destroy(vfxCata);
        }
    }
    #endregion
}
