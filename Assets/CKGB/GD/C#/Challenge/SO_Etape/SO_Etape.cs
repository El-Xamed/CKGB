using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Etape", menuName = "ScriptableObjects/Etape", order = 3)]
public class SO_Etape : ScriptableObject
{
    #region Mes variables
    public string titre;
    public string �nonc�;

    public SO_Choix choix;
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