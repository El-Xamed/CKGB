using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Condition", menuName = "ScriptableObjects/Challenge/Condition", order = 6)]
public class SO_turnContext : ScriptableObject
{
    [SerializeField] SO_ActionClass playedAction;
    //Proto_SO_Character actor;
    [SerializeField] int position;
    [SerializeField] SO_Accessories currentAccessoire;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
