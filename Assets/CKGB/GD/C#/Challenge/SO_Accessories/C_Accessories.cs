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

    #region Fonction
    public void IniChallenge()
    {
        //Sprite
        GetComponent<Image>().sprite = dataAcc.spriteAcc;
        GetComponent<Image>().preserveAspect = true;
        GetComponent<Image>().useSpriteMesh = true;
    }
    /*
    bool CheckApplyAccDamageInReso(C_Accessories thisAcc)
    {
        //Check si la position des actor est sur la meme case que l'acc.
        foreach (var thisActor in myTeam)
        {
            if (thisActor.GetPosition() == thisAcc.GetPosition())
            {
                thisActor.SetCurrentStatsPrice(thisAcc.GetDataAcc().reducStress, thisAcc.GetDataAcc().reducEnergie);

                thisActor.CheckIsOut();

                //Ecrit dans les logs le résultat de l'action.
                uiLogs.text = thisAcc.GetDataAcc().damageLogs;

                //Check si le jeu est fini "GameOver".
                CheckGameOver();

                return true;
            }
        }

        return false;
    }

    void ApplyAccDamage()
    {
        //Check si il attaque.
        if (!thisAcc.GetDataAcc().canMakeDamage) { return; }

        foreach (var thisCase in myChallenge.listCatastrophy[0].targetCase)
        {
            if (thisCase == thisAcc.GetPosition())
            {
                if (thisAcc.GetDataAcc().typeAttack == SO_Accessories.ETypeAttack.All)
                {
                    //Check si la position des actor est sur la meme case que l'acc.
                    foreach (var thisActor in myTeam)
                    {
                        thisActor.SetCurrentStatsPrice(thisAcc.GetDataAcc().reducStress, thisAcc.GetDataAcc().reducEnergie);

                        thisActor.CheckIsOut();
                    }

                    //Ecrit dans les logs le résultat de l'action.
                    uiLogs.text = "La cata à frappé le lézard ! Tous le monde perd -2 de calm !";
                }
            }
        }


        //Check si la position des actor est sur la meme case que l'acc.
        foreach (var thisActor in myTeam)
        {
            if (thisActor.GetPosition() == thisAcc.GetPosition())
            {
                thisActor.SetCurrentStatsPrice(thisAcc.GetDataAcc().reducStress, thisAcc.GetDataAcc().reducEnergie);

                thisActor.CheckIsOut();

                //Ecrit dans les logs le résultat de l'action.
                uiLogs.text = thisAcc.GetDataAcc().damageLogs;
            }
        }



        //Check si le jeu est fini "GameOver".
        CheckGameOver();
    }
    */
    #endregion

    #region Partage de données
    public SO_Accessories GetDataAcc()
    {
        return dataAcc;
    }

    public void SetDataAcc(SO_Accessories thisAcc)
    {
        dataAcc = thisAcc;
    }
    #endregion
}
