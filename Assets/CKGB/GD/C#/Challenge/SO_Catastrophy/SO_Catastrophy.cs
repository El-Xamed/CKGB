using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[CreateAssetMenu(fileName = "New Catastrophy", menuName = "ScriptableObjects/Challenge/Catastrophy", order = 5)]
public class SO_Catastrophy : ScriptableObject
{
    //BESOIN DE RECUPERER LES ACTIONS POUR SETUP PLUS FACILEMENT MAIS SI JE FAIS CA, DEPLACER LES FONCTIONS DE RECUPERATION DE VALEUR DANS LE SO_ACTIONCLASS.
    public enum EModeAttack { None, Random}

    public EModeAttack modeAttack;
    public GameObject vfxCataPrefab;
    [HideInInspector] public List<int> targetCase = new List<int>();

    public string catastrophyLog;

    public SO_ActionClass actionClass;

    //Fonction pour faire spawn les cata.
    public void InitialiseCata(List<C_Case> plateau, List<C_Actor> team)
    {
        //Supprime toutes les catasur le plateau.
        foreach (var thisCase in plateau)
        {
            thisCase.DestroyVfxCata();
        }

        Debug.Log("Les cases ont été sup.");

        //Initialise la cata (Random avec 1 valeur).
        if (modeAttack == SO_Catastrophy.EModeAttack.Random)
        {
            //Augmente ou réduit le nombre.
            int newInt = Random.Range(0, plateau.Count);

            //Vide la liste.
            targetCase.Clear();

            //Ajoute la valeur aléatoire.
            targetCase.Add(newInt);
        }

        //Affiche la prochaine cata.
        foreach (var thisCase in targetCase)
        {
            //Check si le VFX est bien renseigné.
            if (vfxCataPrefab == null)
            {
                Debug.LogWarning("Il y a pas de cata dans cette cata !");
                return;
            }

            plateau[thisCase].ShowDangerZone(vfxCataPrefab);
            Debug.Log("nouvelle Cata sur la case " + thisCase);
        }

        /*//Ajout des zones de danger des acc
        foreach (var thisAcc in listAcc)
        {
            if (thisAcc.GetDataAcc().canMakeDamage)
            {
                listCase[thisAcc.GetPosition()].ShowDangerZone(myChallenge.listCatastrophy[0].vfxCataPrefab);
            }
        }*/

        //Check si les actor sont en danger
        foreach (var thisActor in team)
        {
            thisActor.CheckIsInDanger(this);
        }
    }

    //Fonction de feedback pour montrer qu'un actor est dessus.

    //Fonction pour appliquer la cata.
    public void ApplyCatastrophy(List<C_Case> plateau, List<C_Actor> team)
    {
        //Pour tous les nombre dans la liste dela cata.
        foreach (var thisCase in targetCase)
        {
            //Check si la case possède un vfx.
            if (plateau[thisCase].GetVfxCata() != null)
            {
                //VFX de la cata qui s'applique.
                plateau[thisCase].GetComponentInChildren<Animator>().SetTrigger("cata_Kaboom");
            }

            //Pour tous les actor.
            foreach (C_Actor thisActor in team)
            {
                if (thisCase == thisActor.GetPosition())
                {
                    Debug.Log("La case " + thisCase + " est attaqué !");

                    //Applique des conséquence grace au finction de actionClass.
                    actionClass.SetStatsTarget(Interaction.ETypeTarget.Self, team, thisActor, plateau);

                    thisActor.CheckIsOut();
                }
            }
        }
    }

    public override string ToString()
    {
        return TextUtils.GetColorText(catastrophyLog, Color.red);
    }
}
