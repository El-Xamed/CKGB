using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ListDeParsonne : MonoBehaviour
{
    [SerializeField] List<Personne> maListe1;
    [SerializeField] List<PhysicsScene> maListe2;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(maListe1[0].name);
        Debug.Log(maListe1[1].name);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
