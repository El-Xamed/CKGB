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
    bool CheckApplyAccDamageInReso(List<C_Actor> team)
    {
        //Check si la position des actor est sur la meme case que l'acc.
        foreach (var thisActor in team)
        {
            if (thisActor.GetPosition() == position)
            {
                //thisActor.SetCurrentStatsPrice(GetDataAcc().reducStress, GetDataAcc().reducEnergie);

                thisActor.CheckIsOut();

                return true;
            }
        }

        return false;
    }

    void ApplyAccDamage(List<C_Actor> team)
    {
        //Check si il attaque.
        if (!GetDataAcc().canMakeDamage) { return; }

        //Check si la position des actor est sur la meme case que l'acc.
        foreach (var thisActor in team)
        {
            if (thisActor.GetPosition() == position)
            {
                //thisActor.SetCurrentStatsPrice(GetDataAcc().reducStress, GetDataAcc().reducEnergie);

                thisActor.CheckIsOut();
            }
        }
    }
    /*
    //Fonction qui attaque tous les joueurs.
    public void ApplyDamageForAll(SO_Catastrophy thisCata)
    {
        //check si il se prend la cata
        foreach (var thisCase in thisCata.targetCase)
        {
            if (thisCase == position)
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
