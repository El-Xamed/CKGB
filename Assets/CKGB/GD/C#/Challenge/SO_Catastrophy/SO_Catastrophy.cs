using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Accessories", menuName = "ScriptableObjects/Challenge/Catastrophy", order = 5)]
public class SO_Catastrophy : ScriptableObject
{
    public bool canMakeDamage;
    public int reducStress;
    public int reducEnergie;

    public int targetCase;

    public string catastrophyLog;

    public void initialteCatastrophy()
    {

    }

    void ApplyCatastrophy()
    {

    }
}
