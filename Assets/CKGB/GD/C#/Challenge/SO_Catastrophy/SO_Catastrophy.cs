using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Catastrophy", menuName = "ScriptableObjects/Challenge/Catastrophy", order = 5)]
public class SO_Catastrophy : ScriptableObject
{
    public int reducStress;
    public int reducEnergie;
    public GameObject vfxCataPrefab;
    public int[] targetCase;

    public string catastrophyLog;
}
