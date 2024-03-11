using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;

[CreateAssetMenu(fileName = "New TM", menuName = "ScriptableObjects/TempsMort", order = 1)]

public class SO_TempsMort : ScriptableObject
{
    #region DATA
    [Header("Data Temps Mort")]
    [SerializeField] public Sprite TMbackground;
    [SerializeField] public GameObject[] Team;
    [SerializeField] public InitialActorPosition[] startPos;

    [Header("Dialogues")]
    [SerializeField] Story intro;
    [SerializeField] Story Outro;



    #endregion
    [System.Serializable]
    public class InitialActorPosition
    {
        public int position;
        public C_Actor perso;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
