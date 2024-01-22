using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Etape", menuName = "ScriptableObjects/Challenge/Etape", order = 3)]
public class SO_Etape : ScriptableObject
{
    #region Mes variables
    public string titre;
    public string énoncé;

    public SO_ActionClass action;
    #endregion

    #region Mes fonctions
    public void CheckAction()
    {

    }

    public void CheckCanNext()
    {

    }

    public void GiveNewAction()
    {

    }
    #endregion
}
