using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Accessories", menuName = "ScriptableObjects/Challenge/Accessories", order = 4)]
public class SO_Accessories : ScriptableObject
{
    #region Mes variables
    //Pour check si la case est occupé par un acteur
    public GameObject gameObject;
    public new string name;
    public Sprite spriteAcc;
    bool isBusyByCharacter;

    [Header("Position de l'objet")]
    public int initialPosition;
    [HideInInspector] public int currentPosition;

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
    public void UpdateAcc(List<C_Case> listCase)
    {
        Transform newPosition = listCase[currentPosition].transform;

        switch (moveType)
        {
            case ETypeMovetype.notMove:
                return;
            case ETypeMovetype.normal:
                currentPosition++;
                break;
            case ETypeMovetype.inverse:
                currentPosition--;
                break;
            case ETypeMovetype.random:
                //Augmente ou réduit le nombre.
                int newInt = Random.Range(0, 1);
                if (newInt == 0) { currentPosition++; }
                else { currentPosition--; }
                break;
        }

        NewPosition(newPosition);
    }

    private void NewPosition(Transform newPosition)
    {
        gameObject.transform.SetParent(newPosition);
    }
    #endregion
}

public enum ETypeMovetype
{
    notMove, normal, inverse, random
}
