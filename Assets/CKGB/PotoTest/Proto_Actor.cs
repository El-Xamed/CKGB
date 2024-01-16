using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Proto_Actor : MonoBehaviour
{
    [SerializeField]
    int position;
    [SerializeField]
    Proto_SO_Character dataActor;


    private void Awake()
    {
        gameObject.name = dataActor.name;
    }

    public Proto_SO_Character GetDataActor()
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
