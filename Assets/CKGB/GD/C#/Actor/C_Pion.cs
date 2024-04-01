using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class C_Pion : MonoBehaviour
{
    [SerializeField] protected int position;
    protected bool inDanger = false;

    protected SO_Catastrophy currentCata = null;

    //Pour faire déplacer l'actor dans le challenge. PEUT ETRE AUSSI UTILISE DANS LE TM MAIS C'EST PAS SETUP POUR ET C'EST PAS IMPORTANT.
    public virtual void MoveActor(List<C_Case> plateau, int newPosition)
    {
        //Detection de si le perso est au bord. (TRES UTILE QUAND UN PERSONNAGE SE FAIT POUSSER)
        if (newPosition < 0)
        {
            //Déplace le perso à droite du pleteau.
            transform.position = new Vector3(plateau[plateau.Count - 1].transform.position.x, transform.position.y, transform.position.z);
            GetComponent<RectTransform>().localPosition = new Vector3(plateau[plateau.Count - 1].transform.position.x, transform.position.y, transform.position.z);
            position = plateau.Count - 1;
        }
        else if (newPosition > plateau.Count - 1)
        {
            //Déplace le perso à gauche du plateau.
            transform.position = new Vector3(plateau[0].transform.position.x, transform.position.y, transform.position.z);
            position = 0;
        }
        else
        {
            //Déplace le perso.
            transform.position = new Vector3(plateau[newPosition].transform.position.x, transform.position.y, transform.position.z);
            position = newPosition;
        }

        //Recentre le perso.
        //Centrage sur la case et position sur Y.
        transform.localPosition = new Vector3();
        //GetComponent<RectTransform>().localPosition = new Vector3(0, transform.localPosition.y, transform.localPosition.z);

        //Check si l'objet est un actor
        if (GetComponent<C_Actor>() && currentCata != null)
        {
            //Check apres chaque déplacement si il est sur une case dangereuse.
            CheckIsInDanger(currentCata);
        }


    }

    //Check si dans le challenge l'actor et pas sur une case qui pourrait lui retirer des stats. FONCTIONNE QUE SUR LES ACTOR !!!!
    public void CheckIsInDanger(SO_Catastrophy listDangerCases)
    {
        foreach (var thisCase in listDangerCases.targetCase)
        {
            if (thisCase == position)
            {
                inDanger = true;
                transform.GetChild(2).GetComponent<Image>().sprite = GetComponent<C_Actor>().GetDataActor().challengeSpriteOnCata;
                transform.GetChild(5).gameObject.SetActive(true);
            }
            else
            {
                inDanger = false;
                transform.GetChild(2).GetComponent<Image>().sprite = GetComponent<C_Actor>().GetDataActor().challengeSprite;
                transform.GetChild(5).gameObject.SetActive(false);
            }
        }

        GetComponent<Animator>().SetBool("isInDanger", inDanger);
    }

    public virtual int GetPosition()
    {
        return position;
    }
}
