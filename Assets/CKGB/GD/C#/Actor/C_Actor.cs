using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_Actor : MonoBehaviour
{
    #region
    int position;
   
    [SerializeField]
    public SO_Character dataActor;
    #endregion


    private void Awake()
    {
        gameObject.name = dataActor.name;
    }

    public SO_Character GetDataActor()
    {
        return dataActor;
    }

    public int GetPosition()
    {
        return position;     
    }

    public void SetPosition(int newPosition)
    {
        position = newPosition;
    }
}
