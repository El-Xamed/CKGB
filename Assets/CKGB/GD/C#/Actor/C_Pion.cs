using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class C_Pion : MonoBehaviour
{
    [Header("Challenge")]
    [SerializeField] protected int position;
    protected bool inDanger = false;

    public virtual int GetPosition()
    {
        return position;
    }

    public virtual void SetPosition(int newPosition)
    {
        position = newPosition;
    }

    public bool GetInDanger()
    {
        return inDanger;
    }

    public virtual void SetInDanger(bool value)
    {
        inDanger = value;
    }
}
