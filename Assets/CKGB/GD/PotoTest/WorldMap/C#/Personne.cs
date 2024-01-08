using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Personne", menuName = "Personne")]
public class Personne : ScriptableObject
{
    public new string name;
    public int age;
    public float taille;
}
