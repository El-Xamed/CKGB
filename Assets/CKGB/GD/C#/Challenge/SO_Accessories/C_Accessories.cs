using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class C_Accessories : C_Pion
{
    [SerializeField] SO_Accessories dataAcc;

    private void Awake()
    {
        gameObject.name = dataAcc.name;

        IniChallenge();
    }

    public void IniChallenge()
    {
        //Sprite
        GetComponent<Image>().sprite = dataAcc.spriteAcc;
        GetComponent<Image>().preserveAspect = true;
        GetComponent<Image>().useSpriteMesh = true;
    }

    public SO_Accessories GetDataAcc()
    {
        return dataAcc;
    }

    public void SetDataAcc(SO_Accessories thisAcc)
    {
        dataAcc = thisAcc;
    }
}
