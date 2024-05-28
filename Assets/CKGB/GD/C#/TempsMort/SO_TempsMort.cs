using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;



[CreateAssetMenu(fileName = "New TM", menuName = "ScriptableObjects/TempsMort", order = 1)]

public class SO_TempsMort : ScriptableObject
{
    #region DATA

    [SerializeField] public string TLname;

    [Header("Dialogues")]
    [SerializeField] public TextAsset intro;
    [SerializeField] public TextAsset Outro;
    [SerializeField] public TextAsset Observer;

    [Header("Data Temps Mort")]
    [SerializeField] public GameObject TMbackground = null;
    [SerializeField] public GameObject[] Team;
    [SerializeField] public InitialActorPosition[] startPos;
    [SerializeField] public Vector3[] defautpos;

    public RuntimeAnimatorController introAnimPatern;
    public RuntimeAnimatorController outroAnimPatern;
    public RuntimeAnimatorController observageAnimPatern;

    #endregion
    [System.Serializable]
    public class InitialActorPosition
    {
        public Vector3 position;
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
