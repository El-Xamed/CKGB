using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Accessories", menuName = "ScriptableObjects/Challenge/Accessories", order = 4)]
public class SO_Accessories : ScriptableObject
{
    #region Mes variables
    //Pour check si la case est occupé par un acteur
    bool isBusyByCharacter;

    [Header("Position de l'objet")]
    [SerializeField]
    int initialPosition;
    int currentPosition;

    [Header("Damage")]
    //Pour aplliquer les degat si le joueur se place dessus
    [SerializeField]
    bool canMakeDamage;
    [SerializeField]
    int reducEnergie;
    [SerializeField]
    int reducStress;

    C_Challenge challenge;

    [Header("Movement")]
    //Déplacements
    [SerializeField]
    ETypeMovetype moveType;


    #endregion

    #region Mes fonctions
    //Pour lancer les fonction de l'acc qui sera appelé par le C_Challenge.
    private void UpdateAcc()
    {
        switch (moveType)
        {
            case ETypeMovetype.notMove:
                break;
            case ETypeMovetype.normal:
                break;
            case ETypeMovetype.inverse:
                break;
            case ETypeMovetype.random:
                break;
            default:
                break;
        }
    }

    private void NormalMove()
    {

    }

    private void InverseMove()
    {

    }

    private void RandomMove()
    {

    }

    void Move(C_Case targetCase)
    {
        //gameObject.transform.SetParent(targetCase.transform);
    }
    #endregion
}

public enum ETypeMovetype
{
    notMove, normal, inverse, random
}
