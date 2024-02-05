using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class C_Accessories : MonoBehaviour
{
    [SerializeField] public SO_Accessories dataAcc;

    public int currentPosition;

    private void Awake()
    {
        gameObject.name = dataAcc.name;
        currentPosition = dataAcc.initialPosition;

        IniChallenge();
    }

    public void IniChallenge()
    {
        //Sprite
        GetComponent<Image>().sprite = dataAcc.spriteAcc;
        GetComponent<Image>().preserveAspect = true;
        GetComponent<Image>().useSpriteMesh = true;
    }

    private void NewPosition(Transform newPosition)
    {

    }
}
