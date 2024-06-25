using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class C_Accessories : C_Pion
{
    [SerializeField] SO_Accessories dataAcc;

    [SerializeField] bool canTakeDamage;
    private void Awake()
    {
        gameObject.name = dataAcc.name;

        IniChallenge();
    }

    #region Fonction
    public void IniChallenge()
    {
        //Sprite
        GetComponent<Image>().sprite = dataAcc.spriteAcc;
        GetComponent<Image>().preserveAspect = true;
        GetComponent<Image>().useSpriteMesh = true;
    }
    #endregion

    #region Partage de données
    public SO_Accessories GetDataAcc()
    {
        return dataAcc;
    }

    #endregion
}
