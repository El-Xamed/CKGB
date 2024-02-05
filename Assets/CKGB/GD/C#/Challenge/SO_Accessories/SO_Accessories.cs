using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Accessories", menuName = "ScriptableObjects/Challenge/Accessories", order = 4)]
public class SO_Accessories : ScriptableObject
{
    #region Mes variables
    public new string name;
    public Sprite spriteAcc;
    bool isBusyByCharacter;

    [Header("Position de l'objet")]
    public int initialPosition;

    
    public enum ETypeAttack { None, All}
    [Header("Damage")]
    public ETypeAttack typeAttack;

    public string damageLogs;
    public bool canMakeDamage;
    public int reducEnergie;
    public int reducStress;
    public int leftRange;
    public int rightRange;
    #endregion
}
