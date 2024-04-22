using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Catastrophy", menuName = "ScriptableObjects/Challenge/Catastrophy", order = 5)]
public class SO_Catastrophy : ScriptableObject
{
    //BESOIN DE RECUPERER LES ACTIONS POUR SETUP PLUS FACILEMENT MAIS SI JE FAIS CA, DEPLACER LES FONCTIONS DE RECUPERATION DE VALEUR DANS LE SO_ACTIONCLASS.
    public enum EModeAttack { None, Random}

    public EModeAttack modeAttack;
    public bool applyForAll = false;
    public GameObject vfxCataPrefab;
    [HideInInspector] public List<int> targetCase = new List<int>();

    public string catastrophyLog;

    public SO_ActionClass actionClass;

    public override string ToString()
    {
        return TextUtils.GetColorText(catastrophyLog, Color.red);
    }
}
