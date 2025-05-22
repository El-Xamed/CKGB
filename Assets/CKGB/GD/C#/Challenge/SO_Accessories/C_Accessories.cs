using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class C_Accessories : C_Pion
{
    [SerializeField] SO_Accessories dataAcc;

    [SerializeField] bool canTakeDamage;

    Image img;
    private void Awake()
    {
        gameObject.name = dataAcc.name;

        IniChallenge();
    }

    #region Fonction
    public void IniChallenge()
    {
        img = GetComponentInChildren<Image>();
        //Sprite
        img.sprite = dataAcc.spriteAcc;
        img.preserveAspect = true;
        img.useSpriteMesh = true;
    }
    #endregion

    #region Partage de données
    public SO_Accessories GetDataAcc()
    {
        return dataAcc;
    }

    #endregion
}
