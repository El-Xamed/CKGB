using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_Follower : MonoBehaviour
{
    #region
    [SerializeField]
    public GameObject actor;

    [SerializeField] public float moveSpeed = 1f;

    [SerializeField] public Transform destination;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        initiateTheMapCharacterProtocol();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
              
    }
    
    private void initiateTheMapCharacterProtocol()
    {
        GetComponent<SpriteRenderer>().sprite = actor.GetComponent<C_Actor>().dataActor.MapTmSprite;
    }
}
