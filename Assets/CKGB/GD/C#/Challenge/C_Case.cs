using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_Case : MonoBehaviour
{
    #region Mes variables
    bool isBusyByActor;
    bool isInDanger;
    #endregion

    #region Mes fonctions
    public void UpdateIsBusyByActor(bool isBusy)
    {
        isBusyByActor = isBusy;
    }

    public void UpdateIsInDanger(bool danger)
    {
        isInDanger = danger;
    }
    public void ShowDangerZone()
    {

    }
    public void ApplyNewStats()
    {

    }
    #endregion
}
