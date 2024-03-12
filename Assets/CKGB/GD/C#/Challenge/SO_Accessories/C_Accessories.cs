using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class C_Accessories : MonoBehaviour
{
    [SerializeField] public SO_Accessories dataAcc;

    public int position;

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

    public void MoveActor(List<C_Case> plateau, int newPosition)
    {
        //Detection de si le perso est au bord. (TRES UTILE QUAND UN PERSONNAGE SE FAIT POUSSER)
        if (newPosition < 0)
        {
            //Déplace le perso à droite du pleteau.
            transform.parent = plateau[plateau.Count - 1].transform;
            position = plateau.Count - 1;
        }
        else if (newPosition > plateau.Count - 1)
        {
            //Déplace le perso à gauche du plateau.
            transform.parent = plateau[0].transform;
            position = 0;
        }
        else
        {
            //Déplace le perso.
            transform.parent = plateau[newPosition].transform;
            position = newPosition;
        }
    }

    public int GetPosition() { return  position; }
}
