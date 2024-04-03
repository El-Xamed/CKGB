using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class C_CaseManager : MonoBehaviour
{
    [SerializeField] float largeur;
    [SerializeField] int nbCases;

    //
    [SerializeField] SpriteRenderer line;
    [SerializeField] SpriteRenderer border;
    [SerializeField] SpriteRenderer circle;

    [SerializeField] Transform test;

    // Start is called before the first frame update
    void Start()
    {
        Build();

        PlaceActorOnCase(test, 0);
    }

    void Build()
    {
        Vector3 start = transform.position - Vector3.right * largeur / 2;
        float caseWidth = largeur / nbCases;

        for (int i = 0; i < nbCases; i++)
        {
            SpriteRenderer currentLine = Instantiate(line, start + Vector3.right * (i + 0.5f) * caseWidth, Quaternion.identity, transform);

            currentLine.size =  new Vector2 (caseWidth, currentLine.size.y);

            Instantiate(circle, start + Vector3.right * (i + 0.5f) * caseWidth, Quaternion.identity, transform);
            if(border != null)
            Instantiate(border, start + Vector3.right * i * caseWidth, Quaternion.identity, transform);
        }

        Instantiate(border, start + Vector3.right * largeur, Quaternion.identity , transform);
    }

    public void PlaceActorOnCase(Transform actorTransform, int caseIndex)
    {
        Vector3 start = transform.position - Vector3.right * largeur / 2;
        float caseWidth = largeur / nbCases;

        //
        actorTransform.position = start + Vector3.right * (caseIndex + 0.5f) * caseWidth;
    }

    private void OnDrawGizmosSelected()
    {
        //
        Gizmos.color = Color.blue;
        Gizmos.DrawCube(transform.position, new Vector3(largeur, 0.2f, 0.2f));
        
    }
}
