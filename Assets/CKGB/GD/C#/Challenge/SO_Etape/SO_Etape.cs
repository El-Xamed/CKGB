using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Etape", menuName = "ScriptableObjects/Challenge/Etape", order = 3)]
public class SO_Etape : ScriptableObject
{
    #region Mes variables
    public bool useCata = true;
    public List<SO_ActionClass> actions;
    public SO_ActionClass rightAnswer;

    #endregion

}
