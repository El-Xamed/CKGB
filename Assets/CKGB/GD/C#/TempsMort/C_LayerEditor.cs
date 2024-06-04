using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_LayerEditor : MonoBehaviour
{
    C_TempsLibre c;
    public void Start()
    {
        c = FindObjectOfType<C_TempsLibre>();
    }
    public void ModifyLayerMorgan(int layer)
    {
        GameObject.Find("Morgan").GetComponent<C_Actor>().mainchild.transform.parent.GetComponent<Canvas>().sortingOrder = layer;
        Debug.Log(layer);
    }
    public void ModifyLayerNimu(int layer)
    {
        GameObject.Find("Nimu").GetComponent<C_Actor>().mainchild.transform.parent.GetComponent<Canvas>().sortingOrder = layer;
        Debug.Log(layer);
    }
    public void ModifyLayerEsthela(int layer)
    {
        GameObject.Find("Esthela").GetComponent<C_Actor>().mainchild.transform.parent.GetComponent<Canvas>().sortingOrder = layer;
        Debug.Log(layer);
    }
}
