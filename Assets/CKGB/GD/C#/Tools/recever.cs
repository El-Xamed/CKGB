using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class recever : MonoBehaviour
{
    private void OnEnable()
    {
        test.onTime += TimeOut;
        test.monTest += MonTest;
    }

    private void OnDisable()
    {
        test.onTime -= TimeOut;
        test.monTest -= MonTest;
    }

    void TimeOut(SO_Catastrophy cata)
    {
        GetComponentInChildren<TMP_Text>().text = cata + "\n";
        Debug.Log(cata.ToString());
    }

    void MonTest()
    {
        Debug.Log("Ta gueule merci");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
