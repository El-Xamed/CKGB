using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Proto_Actor : MonoBehaviour
{
    [SerializeField]
    Proto_SO_Character dataActor;

    private void Awake()
    {
        gameObject.name = dataActor.name;
    }
}
