using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Etape", menuName = "ScriptableObjects/Challenge/Etape", order = 3)]
public class SO_Etape : ScriptableObject
{
    #region Mes variables
    public bool useCata = true;
    public List<SO_ActionClass> actions;
    public SO_ActionClass rightAnswer;

    #endregion

    #region Mes fonctions
    public void CheckAction()
    {

    }

    public void CheckCanNext()
    {

    }

    public void CheckIfWeCanWinThisEtape()
    {
        //Check si la bonne réponse et dans la list.  CHANGER LE SYST7ME POUR QUE LE DEV DOIT JUSTE RENTRER UNE VALEUR POUR D2F2NIR AUTOMATIQUEMENT QUEL SERA LA BONNE REPONSE ?
        foreach (var myAction in actions)
        {
            int nombreErreurs = 0;

            if (myAction != null && myAction == rightAnswer)
            {
                Debug.Log("La réponse fait partis de la liste.");
                return;
            }
            else
            {
                nombreErreurs++;
                if (nombreErreurs == actions.Count - 1)
                {
                    Debug.LogError("ERREUR : La bonne réponse n'est pas dans la liste des actions, veuillez rensegner la bonne réponse par un élément de la liste");
                }
            }
        }
    }

    #endregion
}
