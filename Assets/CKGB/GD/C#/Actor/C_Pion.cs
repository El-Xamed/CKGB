using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_Pion : MonoBehaviour
{
    [SerializeField] protected int position;

    //Pour faire d�placer l'actor dans le challenge. PEUT ETRE AUSSI UTILISE DANS LE TM MAIS C'EST PAS SETUP POUR ET C'EST PAS IMPORTANT.
    public virtual void MoveActor(List<C_Case> plateau, int newPosition)
    {
        //Detection de si le perso est au bord. (TRES UTILE QUAND UN PERSONNAGE SE FAIT POUSSER)
        if (newPosition < 0)
        {
            //D�place le perso � droite du pleteau.
            transform.position = new Vector3(plateau[plateau.Count - 1].transform.position.x, transform.position.y, 0);
            position = plateau.Count - 1;
        }
        else if (newPosition > plateau.Count - 1)
        {
            //D�place le perso � gauche du plateau.
            transform.position = new Vector3(plateau[0].transform.position.x, transform.position.y, 0);
            position = 0;
        }
        else
        {
            //D�place le perso.
            transform.position = new Vector3(plateau[newPosition].transform.position.x, transform.position.y, 0);
            position = newPosition;
        }

        //Recentre le perso.
        //GetComponent<RectTransform>().localPosition = new Vector3(0, transform.localPosition.y, transform.localPosition.z);

        //Check apr�s chaque d�placement si il est sur une case dangereuse.
    }

    public virtual int GetPosition()
    {
        return position;
    }
}
