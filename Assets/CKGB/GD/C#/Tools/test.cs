using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class test : MonoBehaviour
{
    public static event Action<SO_Catastrophy> onTime;
    [SerializeField] SO_Catastrophy cata;
    float oui = 2;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<TMP_Text>().text = cata + "\n";
    }

    // Update is called once per frame
    void Update()
    {
        if (oui > 0)
        {
            oui -= Time.deltaTime;
            if (oui <= 0)
            {
                onTime?.Invoke(cata);
            }
        }
    }
}
