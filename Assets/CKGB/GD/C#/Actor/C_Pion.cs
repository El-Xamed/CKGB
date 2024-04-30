using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class C_Pion : MonoBehaviour
{
    [Header("Challenge")]
    [SerializeField] protected int position;
    protected bool inDanger = false;

    //Pour faire déplacer l'actor dans le challenge.
    //New version.
    /*DEPLACEMENT EN COUR DANS LA CHALLENGE CAR C'EST LUI QUI GERE LE PLATEAU.
    public virtual void MoveActor(int nbMove, Move.ETypeMove whatMove, List<C_Actor> otherActor, List<C_Case> plateau, bool isTp)
    {
        //Check si c'est le mode normal de déplacement ou alors le mode target case.
        if (whatMove == Move.ETypeMove.Right || whatMove == Move.ETypeMove.Left) //Normal move mode.
        {
            //Check si cette valeur doit etre negative ou non pour setup correctement la direction.
            if (whatMove == Move.ETypeMove.Left)
            {
                nbMove = -nbMove;
            }
            
            CheckIfNotExceed(nbMove);
        }
        else //Passe en mode "targetCase". Pour permettre de bien setup le déplacement meme si la valeur est trop élevé par rapport au nombre de case dans la liste.
        {
            //Check si le nombre de déplacement est trop élevé par rapport au nombre de case.
            if (nbMove > plateau.Count -1)
            {
                Debug.LogWarning("La valeur de déplacement et trop élevé par rapport au nombre de cases sur le plateau la valeur sera donc égale à 0.");

                nbMove = 0;
            }
        }

        //Check si un autre membre de l'équipe occupe deja a place. A voir si je le garde.
        foreach (C_Actor thisOtherActor in otherActor)
        {
            //Si dans la list de l'équipe c'est pas égale à l'actor qui joue. Et si "i" est égale à "newPosition" pour décaler seulement l'actor qui occupe la case ou on souhaite ce déplacer.
            if (this != thisOtherActor)
            {
                //Détection de si il y a un autres actor.
                if (nbMove == thisOtherActor.GetPosition())
                {
                    //Check si c'est une Tp ou non.
                    if (isTp)
                    {
                        //Place l'autre actor à la position de notre actor.
                        thisOtherActor.PlaceActorOnBoard(plateau, GetPosition());
                        Debug.Log(TextUtils.GetColorText(name, Color.cyan) + " a échangé sa place avec " + TextUtils.GetColorText(thisOtherActor.name, Color.green) + ".");
                    }
                    else
                    {
                        //Déplace le deuxieme actor. Fonctionne en récurrence. (New version)
                        thisOtherActor.MoveActor(1, whatMove, otherActor, plateau, isTp);

                        //Check si il y a pas encore un autre perso.
                        Debug.Log(TextUtils.GetColorText(name, Color.cyan) + " a prit la place de " + TextUtils.GetColorText(thisOtherActor.name, Color.green) + " et sera déplacer " + GetDirectionOfMovement());
                    }
                }
            }
        }

        //Nouvelle position
        PlaceActorOnBoard(plateau, nbMove);

        //BESOIN DE FAIRE ENCORE DES MODIF DE CALCUL + Faire en sort de faire de la récurence tant que la valeur ne sera pas inférieur au nombre de case ou supérieur à 0 !!!
        //Récurence qui permet de réduire/augmenter la valeur pour placer l'actor dans la scene.
        void CheckIfNotExceed(int value)
        {
            //Detection de si le perso est au bord (à droite).
            if (GetPosition() + nbMove > plateau.Count - 1)
            {
                //Change la valeur du déplacement.
                nbMove = (GetPosition() + nbMove) - (plateau.Count - 1);
            }
            else if (GetPosition() + nbMove < 0)
            {
                //Change la valeur du déplacement.
                nbMove = (plateau.Count - 1) + nbMove;
            }
            else
            {
                return;
            }

            CheckIfNotExceed(value);
        }

        string GetDirectionOfMovement()
        {
            if (nbMove < 0)
            {
                return " à gauche.";
            }
            else if (nbMove > 0)
            {
                return " à droite.";
            }

            return "Direction Inconu.";
        }
    }*/

    /*Old version. UTILISE POUR PLACER LE PERSO SUR LE PLATEAU. A VOIR POUR PLACER CETTE FONCTION DANS LE CHALLENGE !!!!
    public virtual void PlaceActorOnBoard(List<C_Case> plateau, int thisCase)
    {
        Debug.Log(thisCase);

        //Place l'actor et change sa valeur de position.
        //Old
        //transform.position = new Vector3(plateau[newPosition].transform.position.x, 0, plateau[newPosition].transform.position.z);

        //New
        //Supprime la dernière position.
        plateau[position].ResetPion();

        //Place l'actor
        plateau[thisCase].PlacePion(this);
        //Change la valeur
        position = thisCase;
    }*/

    /*Check si dans le challenge l'actor et pas sur une case qui pourrait lui retirer des stats. SUREMENT A SUP
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
    }*/

    public virtual int GetPosition()
    {
        return position;
    }

    public virtual void SetPosition(int newPosition)
    {
        position = newPosition;
    }

    public bool GetInDanger()
    {
        return inDanger;
    }

    public virtual void SetInDanger(bool value)
    {
        inDanger = value;
    }
}
