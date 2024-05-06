using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using TMPro;

public class C_Worldmap : MonoBehaviour
{
    public static C_Worldmap instance;
    #region variables
    [SerializeField]public C_destination startPoint;
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] float moveSpeed2 = 0.9f;

    [SerializeField] List<C_destination> allMapPoints=new List<C_destination>();
    [SerializeField] public C_destination currentPoint;
    [SerializeField] C_destination Leftlevel, Rightlevel, Uplevel, Downlevel;
    [SerializeField] Transform[] leftpath, rightpath, uppath, downpath;

    bool canMove = true;

    [SerializeField]
    GameObject actor;
    [SerializeField] GameObject setup1;
    [SerializeField] GameObject setup2;
    [SerializeField] GameObject currentSetup;

    [SerializeField] GameObject Bulle;
    [SerializeField] TMP_Text text;


   
    #endregion

    #region methodes
    private void Awake()
    {


    }
    void Start()
    {
        if(GameManager.instance!=null)
        {
            GameManager.instance.W = this;
        }
        if (GameObject.Find("lvl1")!=null)
        {
            allMapPoints.Add(GameObject.Find("lvl1").GetComponent<C_destination>());
        }
        if (GameObject.Find("lvl2") != null)
        {
            allMapPoints.Add(GameObject.Find("lvl2").GetComponent<C_destination>());
        }
        if (GameObject.Find("lvl3") != null)
        {
            allMapPoints.Add(GameObject.Find("lvl3").GetComponent<C_destination>());
        }
       

        if (GameObject.Find("lvl1") != null&&GameManager.instance.WorldstartPoint==0)
        {
            startPoint = allMapPoints[0];
        }

            for (int i = 0; i < allMapPoints.Count; i++)
            {
                GameManager.instance.levels.Add(allMapPoints[i]);
            }
            initiateTheMapCharacterProtocol();
            //sets the initial position

        startPoint = allMapPoints[GameManager.instance.WorldstartPoint];
        currentPoint = startPoint;
            transform.position = currentPoint.transform.position;
        switch (currentPoint.name)
        {
            case "lvl1":
                currentPoint.IsDone = true;currentPoint.Islocked = true;
                
                currentPoint.right.GetComponent<C_destination>().Islocked = false;
                currentPoint.up.GetComponent<C_destination>().Islocked = false;
                currentPoint.GetComponent<C_destination>().flag.SetActive(true);
                break;
            case "lvl2":
                currentPoint.IsDone = true; currentPoint.Islocked = true;

                currentPoint.right.GetComponent<C_destination>().Islocked = false;
                currentPoint.GetComponent<C_destination>().flag.SetActive(true);
                break;           
            case "lvl3":
                currentPoint.IsDone = true; currentPoint.Islocked = true;
                currentPoint.GetComponent<C_destination>().flag.SetActive(true);
                break;
            default:
                break;
        }



        //sets up the destinations
        Leftlevel = currentPoint.left;
            Rightlevel = currentPoint.right;
            Uplevel = currentPoint.up;
            Downlevel = currentPoint.down;

            leftpath = currentPoint.leftPath;
            rightpath = currentPoint.rightPath;
            uppath = currentPoint.upPath;
            downpath = currentPoint.downPath;
        
        
    }
    private void FixedUpdate()
    {

    }
    public void SetGameManagerWorldData()
    {
        startPoint = currentPoint;
        for(int i=0;i<allMapPoints.Count;i++)
        {
            if(startPoint==allMapPoints[i])
            {
                GameManager.instance.WorldstartPoint = i;
                GameManager.instance.WorldcurrentPoint = i;
            }
        }
       
    }
    //Fonction qui lance le niveau en question.
    public void SelectLevel(InputAction.CallbackContext context)
    {
        if (context.performed && currentPoint.Islocked == false&&currentPoint.IsCorner!=true)
        {
            //GameManager.instance.ChangeActionMap("TempsMort");
            SetGameManagerWorldData();

            currentPoint.GetComponent<C_destination>().IsDone = true;
                switch (currentPoint.name)
                {
                    case "lvl1":
                    currentPoint.right.GetComponent<C_destination>().Islocked = false;
                    currentPoint.up.GetComponent<C_destination>().Islocked = false;

                    currentPoint.right.GetComponent<C_destination>().leveltext = currentPoint.right.GetComponent<C_destination>().leveltextprovenance.text;
                    currentPoint.right.GetComponent<C_destination>().levelUI.GetComponent<Image>().color = Color.white;

                    break;

                    case "lvl2":
                    currentPoint.right.GetComponent<C_destination>().Islocked = false;
                    currentPoint.down.GetComponent<C_destination>().Islocked = true;
                    currentPoint.left.GetComponent<C_destination>().Islocked = true;

                    currentPoint.right.GetComponent<C_destination>().leveltext = currentPoint.right.GetComponent<C_destination>().leveltextprovenance.text;
                    currentPoint.right.GetComponent<C_destination>().levelUI.GetComponent<Image>().color = Color.white;

                    break;
                    case "lvl3":

                    currentPoint.left.GetComponent<C_destination>().Islocked = true;
                   

                    break;
                    default:
                        break;
                }
            

            currentPoint.GetComponent<C_destination>().IsDone = true;
            currentPoint.GetComponent<C_destination>().Islocked = true;
            currentPoint.GetComponent<C_destination>().flag.SetActive(true);
            currentPoint.GetComponent<C_destination>().levelUI.GetComponent<levelUI>().Tampon.SetActive(true);

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
        if (context.started && Uplevel != null && Uplevel.Islocked == false&&canMove==true)
        {
            canMove = false;
            StartCoroutine(UpdatepointPosition(uppath));
            currentSetup.GetComponent<Animator>().SetBool("move", true);
            //transform.position = Vector2.Lerp(transform.position, Up.transform.position, moveSpeed);
            currentPoint = Uplevel;
                updateDestinations();            
            Debug.Log("Uplevel");
            canMove = true;          
        }
    }
    public void moveLeft(InputAction.CallbackContext context)
    {
        if (context.started && Leftlevel != null && Leftlevel.Islocked == false&&canMove==true)
        {
            canMove = false;
            StartCoroutine(UpdatepointPosition(leftpath));
            currentSetup.GetComponent<Animator>().SetBool("move", true);
            //transform.position = Vector2.Lerp(transform.position, Left.transform.position, moveSpeed);
            currentPoint = Leftlevel;
                updateDestinations();           
            Debug.Log("Leftlevel");
            canMove = true;          
        }
    }
    public void moveRight(InputAction.CallbackContext context)
    {
        if (context.started && Rightlevel != null && Rightlevel.Islocked == false&&canMove==true)
        {
            canMove = false;
            StartCoroutine(UpdatepointPosition(rightpath));
            currentSetup.GetComponent<Animator>().SetBool("move", true);
            //transform.position = Vector2.Lerp(transform.position, Right.transform.position, moveSpeed);
            currentPoint = Rightlevel;
                updateDestinations();
            Debug.Log("Rightlevel");
            canMove = true;      
        }
    }
    public void moveDown(InputAction.CallbackContext context)
    {
        if (context.started && Downlevel != null && Downlevel.Islocked == false&&canMove==true)
        {
            canMove = false;
            StartCoroutine(UpdatepointPosition(downpath));
            currentSetup.GetComponent<Animator>().SetBool("move", true);
            //transform.position = Vector2.Lerp(transform.position, Down.transform.position, moveSpeed);           
                currentPoint = Downlevel;
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
        currentSetup.GetComponent<Animator>().SetBool("move", false);
        if (currentPoint.left != null)
            Leftlevel = currentPoint.left;
        else Leftlevel = null;
        if (currentPoint.right != null)
            Rightlevel = currentPoint.right;
        else Rightlevel = null;
        if (currentPoint.up != null)
            Uplevel = currentPoint.up;
        else Uplevel = null;
        if (currentPoint.down != null)
            Downlevel = currentPoint.down;
        else Downlevel = null;

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
        if (allMapPoints[1].GetComponent<C_destination>().IsDone)
        {
            setup2.SetActive(true);
            setup1.SetActive(false);
            currentSetup = setup2;
        }
        else
        {
            setup1.SetActive(true);
            setup2.SetActive(false);
            currentSetup = setup1;
        }
            
    }

    #endregion
}