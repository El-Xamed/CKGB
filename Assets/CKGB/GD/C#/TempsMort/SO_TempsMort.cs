using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using UnityEditor.Animations;

[CreateAssetMenu(fileName = "New TM", menuName = "ScriptableObjects/TempsMort", order = 1)]

public class SO_TempsMort : ScriptableObject
{
    #region DATA


    [Header("Dialogues")]
    [SerializeField] public TextAsset intro;
    [SerializeField] public TextAsset Outro;
    [SerializeField] public TextAsset Observer;

    [Header("Data Temps Mort")]
    [SerializeField] public GameObject TMbackground = null;
    [SerializeField] public GameObject[] Team;
    [SerializeField] public InitialActorPosition[] startPos;

    public AnimatorController introAnimPatern;
    public AnimatorController outroAnimPatern;
    public AnimatorController[] observageAnimPatern;

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
