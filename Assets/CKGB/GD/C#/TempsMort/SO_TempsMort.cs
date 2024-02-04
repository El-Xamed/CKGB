using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New TM", menuName = "ScriptableObjects/TempsMort", order = 1)]

public class SO_TempsMort : ScriptableObject
{
    #region DATA

    [SerializeField] public Sprite TMbackground;
    [SerializeField] public GameObject[] Team;

    #endregion
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
