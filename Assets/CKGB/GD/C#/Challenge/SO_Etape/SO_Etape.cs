using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Etape", menuName = "ScriptableObjects/Challenge/Etape", order = 3)]
public class SO_Etape : ScriptableObject
{
    #region Mes variables
    public string titre;
    public string énoncé;
    bool[] conditions;

    Test_challenge TC;
    public SO_ActionClass[] actions;
    [SerializeField] public SO_ActionClass rightAnswer;

    #endregion
    private void Awake()
    {
        
     
    }
    #region Mes fonctions
    public void CheckAction()
    {

    }

    public void CheckCanNext()
    {

    }

   
    #endregion
}
