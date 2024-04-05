using UnityEngine;
using UnityEngine.UI;

public class C_Case : MonoBehaviour
{
    #region Mes variables
    GameObject vfxCata;
    C_Pion myPion;

    [SerializeField] Sprite addNumberSprite;
    [SerializeField] bool addNumber = true;
    #endregion

    #region Mes fonctions
    public void ShowDangerZone(GameObject newVfxCata)
    {
        //Si une cata à deja spaw.
        if (vfxCata != null) { return; }
        vfxCata = Instantiate(newVfxCata, transform);
    }

    public void PlacePion(C_Pion thisPion)
    {
        //Place l'actor et change sa valeur de position.
        thisPion.transform.position = new Vector3(transform.position.x, 0, transform.position.z);
    }

    public void ResetPion()
    {
        myPion = null;
    }

    //Check si dans le challenge l'actor et pas sur une case qui pourrait lui retirer des stats. FONCTIONNE QUE SUR LES ACTOR A LE DEPLACER DANS LE SCRIPT "C_ACTOR" EN OVERRIDE (prendre exeple sur le check in danger) !!!!
    public void CheckIsInDanger()
    {
        if (myPion.GetComponent<C_Actor>())
        {
            if (vfxCata != null)
            {
                myPion.SetInDanger(true);
                transform.GetChild(2).GetComponent<Image>().sprite = GetComponent<C_Actor>().GetDataActor().challengeSpriteOnCata;
                transform.GetChild(5).gameObject.SetActive(true);
            }
            else
            {
                myPion.SetInDanger(false);
                transform.GetChild(2).GetComponent<Image>().sprite = GetComponent<C_Actor>().GetDataActor().challengeSprite;
                transform.GetChild(5).gameObject.SetActive(false);
            }

            GetComponent<Animator>().SetBool("isInDanger", myPion.GetInDanger());
        }
    }
    #endregion

    #region Data partagé
    public bool AddNumber(int number)
    {
        if (addNumber)
        {
            GetComponent<Image>().sprite = addNumberSprite;

            transform.GetChild(0).gameObject.SetActive(true);

            return true; 
        }

        transform.GetChild(0).gameObject.SetActive(false);

        return false;
    }   

    public GameObject GetVfxCata()
    {
        return vfxCata;
    }

    public void DestroyVfxCata()
    {
        if (vfxCata != null)
        {
            Destroy(vfxCata);
            vfxCata = null;
        }
    }
    #endregion
}
