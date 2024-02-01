using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class C_Accessories : MonoBehaviour
{
    [SerializeField] public SO_Accessories dataAcc;

    private void Awake()
    {
        gameObject.name = dataAcc.name;
    }

    public void IniChallenge()
    {
        //Sprite
        GetComponent<Image>().sprite = dataAcc.spriteAcc;
        GetComponent<Image>().preserveAspect = true;
        GetComponent<Image>().useSpriteMesh = true;
    }
}
