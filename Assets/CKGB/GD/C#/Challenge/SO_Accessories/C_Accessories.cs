using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class C_Accessories : MonoBehaviour
{
    [SerializeField] public SO_Accessories dataAcc;

    public int currentPosition;

    private void Awake()
    {
        gameObject.name = dataAcc.name;
        currentPosition = dataAcc.initialPosition;

        IniChallenge();
    }

    public void IniChallenge()
    {
        //Sprite
        GetComponent<Image>().sprite = dataAcc.spriteAcc;
        GetComponent<Image>().preserveAspect = true;
        GetComponent<Image>().useSpriteMesh = true;
    }

    private void NewPosition(Transform newPosition)
    {

    }

    public void MoveAcc(List<C_Case> listCase)
    {
        switch (dataAcc.moveType)
        {
            case ETypeMovetype.normal:
                if (currentPosition == listCase.Count - 1)
                {
                    transform.parent = listCase[0].transform;
                    GetComponent<RectTransform>().localPosition = new Vector3(0, GetComponent<RectTransform>().localPosition.y, 0);
                    currentPosition = 0;
                }
                else
                {
                    currentPosition++;
                    transform.parent = listCase[currentPosition].transform;
                    GetComponent<RectTransform>().localPosition = new Vector3(0, GetComponent<RectTransform>().localPosition.y, 0);
                }
                break;
            case ETypeMovetype.inverse:
                if (currentPosition == 0)
                {
                    transform.parent = listCase[listCase.Count - 1].transform;
                    GetComponent<RectTransform>().localPosition = new Vector3(0, GetComponent<RectTransform>().localPosition.y, 0);
                    currentPosition = listCase.Count - 1;
                }
                else
                {
                    currentPosition--;
                    transform.parent = listCase[currentPosition].transform;
                    GetComponent<RectTransform>().localPosition = new Vector3(0, GetComponent<RectTransform>().localPosition.y, 0);
                }
                break;
            case ETypeMovetype.random:
                //Augmente ou réduit le nombre.
                int newInt = Random.Range(0, 1);
                if (newInt == 0) { currentPosition++; }
                else { currentPosition--; }
                transform.parent = listCase[currentPosition].transform;
                GetComponent<RectTransform>().localPosition = new Vector3(0, GetComponent<RectTransform>().localPosition.y, 0);
                break;
        }
    }
}
