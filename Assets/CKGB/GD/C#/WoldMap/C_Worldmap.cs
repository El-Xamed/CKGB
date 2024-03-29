using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEngine.UI;
using System;

public class C_Worldmap : MonoBehaviour
{
    public static C_Worldmap instance;
    #region variables
    [SerializeField]C_destination startPoint;
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] float moveSpeed2 = 0.9f;

    [SerializeField] C_destination[] allMapPoints;
    [SerializeField] C_destination currentPoint;
    [SerializeField] C_destination Left, Right, Up, Down;
    [SerializeField] Transform[] leftpath, rightpath, uppath, downpath;

    bool canMove = true;

    [SerializeField]
    GameObject actor;




    [SerializeField] Text UIlevel;
    #endregion

    #region methodes
    private void Awake()
    {
        #region Singleton
        if (instance == null)
            instance = this;
        #endregion
        
    }
    void Start()
    {
        if(SceneManager.GetSceneByName("S_Worldmap")==SceneManager.GetActiveScene())
        {
            allMapPoints = FindObjectsOfType<C_destination>();
            for (int i = 0; i < allMapPoints.Length; i++)
            {
                GameManager.instance.levels.Add(allMapPoints[i]);
            }
            initiateTheMapCharacterProtocol();
            //sets the initial position
            currentPoint = startPoint;
            transform.position = startPoint.transform.position;




            //sets up the destinations
            Left = currentPoint.left;
            Right = currentPoint.right;
            Up = currentPoint.up;
            Down = currentPoint.down;

            leftpath = currentPoint.leftPath;
            rightpath = currentPoint.rightPath;
            uppath = currentPoint.upPath;
            downpath = currentPoint.downPath;
        }
        
    }
    private void FixedUpdate()
    {

    }

    //Fonction qui lance le niveau en question.
    public void SelectLevel(InputAction.CallbackContext context)
    {
        if (context.performed && currentPoint.Islocked == false&&currentPoint.IsCorner!=true)
        {
            //GameManager.instance.ChangeActionMap("TempsMort");


                switch (currentPoint.name)
                {
                    case "lvl1":
                    currentPoint.right.right.GetComponent<C_destination>().Islocked = false;
                    currentPoint.right.up.GetComponent<C_destination>().Islocked = false;
                    currentPoint.right.right.GetComponent<SpriteRenderer>().color = Color.yellow;
                    currentPoint.right.up.GetComponent<SpriteRenderer>().color = Color.yellow;
                    break;

                    case "lvl2a":
                    currentPoint.right.GetComponent<C_destination>().Islocked = false;
                    currentPoint.down.GetComponent<C_destination>().Islocked = true;
                    currentPoint.down.right.GetComponent<C_destination>().Islocked = true;
                    break;
                    case "lvl2b":
                    currentPoint.up.GetComponent<C_destination>().Islocked = false;
                    currentPoint.left.GetComponent<C_destination>().Islocked = true;
                    currentPoint.left.up.GetComponent<C_destination>().Islocked = true;
                    break;
                    case "lvl3":
                    currentPoint.left.GetComponent<C_destination>().Islocked = true;
                    currentPoint.down.GetComponent<C_destination>().Islocked = true;
                    break;
                    default:
                        break;
                }
            

            currentPoint.GetComponent<C_destination>().IsDone = true;
            currentPoint.GetComponent<C_destination>().Islocked = true;
            currentPoint.GetComponent<SpriteRenderer>().color = Color.green;
            //Set Les data du TM et C dans le GameManager.
            GameManager.instance.SetDataLevel(currentPoint.GetDataTempsMort(), currentPoint.GetDataChallenge());

            AddActorInTeam();
            Debug.Log("Load Scene...");
            SceneManager.LoadScene("S_TempsLibre");
            //Lance la scene avec les info qu'il récupère.
        }
    }

    //Déplace les joueurs.

    public void moveUp(InputAction.CallbackContext context)
    {
        if (context.started && Up != null && Up.Islocked == false&&canMove==true)
        {
            canMove = false;
            StartCoroutine(UpdatepointPosition(uppath));
            //transform.position = Vector2.Lerp(transform.position, Up.transform.position, moveSpeed);
            currentPoint = Up;
                updateDestinations();            
            Debug.Log("Up");
            canMove = true;          
        }
    }
    public void moveLeft(InputAction.CallbackContext context)
    {
        if (context.started && Left != null && Left.Islocked == false&&canMove==true)
        {
            canMove = false;
            StartCoroutine(UpdatepointPosition(leftpath));
            //transform.position = Vector2.Lerp(transform.position, Left.transform.position, moveSpeed);
            currentPoint = Left;
                updateDestinations();           
            Debug.Log("Left");
            canMove = true;          
        }
    }
    public void moveRight(InputAction.CallbackContext context)
    {
        if (context.started && Right != null && Right.Islocked == false&&canMove==true)
        {
            canMove = false;
            StartCoroutine(UpdatepointPosition(rightpath));
            //transform.position = Vector2.Lerp(transform.position, Right.transform.position, moveSpeed);
            currentPoint = Right;
                updateDestinations();
            Debug.Log("Right");
            canMove = true;      
        }
    }
    public void moveDown(InputAction.CallbackContext context)
    {
        if (context.started && Down != null && Down.Islocked == false&&canMove==true)
        {
            canMove = false;
            StartCoroutine(UpdatepointPosition(downpath));
            //transform.position = Vector2.Lerp(transform.position, Down.transform.position, moveSpeed);           
                currentPoint = Down;
                updateDestinations();            
            Debug.Log("Down");
            canMove = true;         
        }
    }
    void AddActorInTeam()
    {

    }
    void updateDestinations()
    {
        if (currentPoint.left != null)
            Left = currentPoint.left;
        else Left = null;
        if (currentPoint.right != null)
            Right = currentPoint.right;
        else Right = null;
        if (currentPoint.up != null)
            Up = currentPoint.up;
        else Up = null;
        if (currentPoint.down != null)
            Down = currentPoint.down;
        else Down = null;

        if (currentPoint.leftPath != null)
            leftpath = currentPoint.leftPath;
        else leftpath = null;
        if (currentPoint.rightPath != null)
            rightpath = currentPoint.rightPath;
        else rightpath = null;
        if (currentPoint.upPath != null)
            uppath = currentPoint.upPath;
        else uppath = null;
        if (currentPoint.downPath != null)
            downpath = currentPoint.downPath;
        else downpath = null;
        UIlevel.text = currentPoint.leveltext;
    }
    IEnumerator UpdatepointPosition(Transform[]point)
    {
        for(int i = -1; i<point.Length-1;i++)
        {
            if(i==-1)
            {
                yield return MoveToNextPoint(transform, point[i + 1],point);
                //yield return FollowerMoveToNextPoint(Follower.transform, point[i + 1], point);
            }
            else
                yield return MoveToNextPoint(point[i],point[i+1],  point);
                //yield return FollowerMoveToNextPoint(point[i], point[i + 1], point);
        } 
    }
    private IEnumerator MoveToNextPoint(Transform transform1, Transform transform2, Transform[]list)
    {
        float ellapsed = 0;
        float distance = (transform2.position - transform1.position).magnitude;
        
        float maxTime = distance / (moveSpeed*list.Length);
       
        Vector3 a=transform1.position;
        Vector3 b=transform2.position;
       
        while(ellapsed<maxTime)
        {
            ellapsed += Time.deltaTime; 
            transform.position = Vector3.Lerp(a, b, ellapsed / maxTime);

          
           
            yield return null;
        }

        transform.position = transform2.position;
        //Follower.transform.position = transform2.position;
    }
     /*IEnumerator FollowTheBoss(Vector3 a,Vector3 b,float c,Transform nextpos)
    {
        yield return new WaitForSeconds(0.7f);
    }*/
    private void initiateTheMapCharacterProtocol()
    {
        GetComponent<SpriteRenderer>().sprite = actor.GetComponent<C_Actor>().GetDataActor().MapTmSprite;
    }

    #endregion
}