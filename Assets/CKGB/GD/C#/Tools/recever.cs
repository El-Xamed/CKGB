using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class recever : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        test.onTime += TimeOut;
    }

    private void OnDisable()
    {
        test.onTime -= TimeOut;
    }

    void TimeOut(SO_Catastrophy cata)
    {
        Debug.Log(cata.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
