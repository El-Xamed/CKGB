using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    #region variables
    [SerializeField] MapPoint startPoint;
    [SerializeField] float moveSpeed = 3f;

    MapPoint[] allMapPoints;
    MapPoint previousPoint, currentPoint, nextPoint;

    //movement variables part under
    float x, y;
    bool canMove = true;
    int direction;
    Vector2 movement;
    #endregion

    #region methodes
    private void Awake()
    {
        allMapPoints = FindObjectsOfType<MapPoint>();
    }
    void Start()
    {
        canMove = false;
        SetPlayerPosition();
    }
    // Update is called once per frame
    void Update()
    {
       if (canMove)
        {
            transform.position = Vector3.MoveTowards(transform.position, currentPoint.transform.position, moveSpeed * Time.deltaTime);
        }
       if(Vector3.Distance(transform.position, currentPoint.transform.position)<0.1f)
        {
            CheckMapPoint();
        }
    }
    private void FixedUpdate()
    {
        GetMovement();
    }
    void AutoMove()
    {
        if(currentPoint.up != null && currentPoint.up != previousPoint)
        {
            SetNextPoint(currentPoint.up);
            direction = 1;
        }
        if (currentPoint.right != null && currentPoint.right != previousPoint)
        {
            SetNextPoint(currentPoint.right);
            direction = 2;
        }
        if (currentPoint.left != null && currentPoint.left != previousPoint)
        {
            SetNextPoint(currentPoint.left);
            direction = 2;
        }
        if (currentPoint.down != null && currentPoint.down != previousPoint)
        {
            SetNextPoint(currentPoint.down);
            direction = 4;
        }
    }
    void CheckInput()
    {
        if(y > 0.5f)
        {
            if (currentPoint.up != null)
            {
                SetNextPoint(currentPoint.up);
                direction = 1;
            }
        }
        if (x > 0.5f)
        {
            if (currentPoint.right != null)
            {
                SetNextPoint(currentPoint.right);
                direction = 2;
            }
        }
        if (y < -0.5f)
        {
            if (currentPoint.left != null)
            {
                SetNextPoint(currentPoint.left);
                direction = 2;
            }
        }
        if (x < -0.5f)
        {
            if (currentPoint.down != null)
            {
                SetNextPoint(currentPoint.down);
                direction = 4;
            }
        }
    }
    void SetNextPoint(MapPoint next)
    {
        previousPoint = currentPoint;
        currentPoint = nextPoint;
    }
    void GetMovement()
    {
        
    }
    void SetPlayerPosition()
    {
       
            transform.position = startPoint.transform.position;
            currentPoint = startPoint;
            previousPoint = currentPoint;
            canMove = true;
        
    }
    void CheckMapPoint()
    {
      if(currentPoint.IsCorner)
        {
            AutoMove();
        }
      else
        {
            if(direction != 0)
            {
                direction = 0;
            }
            CheckInput();
            SelectLevel();
        }
    }
    void SelectLevel()
    {

    }
    #endregion
}
