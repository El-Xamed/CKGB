using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEngine.UI;

public class C_Worldmap : MonoBehaviour
{
    #region variables
    [SerializeField]C_destination startPoint;
    [SerializeField] float moveSpeed = 1f;

    [SerializeField] C_destination[] allMapPoints;
    C_destination currentPoint;
    [SerializeField] C_destination Left, Right, Up, Down;

    bool canMove = true;

    [SerializeField]
    GameObject actor;

    [SerializeField]
    SceneAsset destination;
    #endregion

    #region methodes
    private void Awake()
    {
        allMapPoints = FindObjectsOfType<C_destination>();
    }
    /*void Start()
    {
        //sets the initial position
        transform.position = startPoint.transform.position;
        currentPoint = startPoint;

        //sets up the destinations
        Left = currentPoint.left;
        Right = currentPoint.right;
        Up = currentPoint.up;
        Down = currentPoint.down;
    }*/

    //Fonction qui lance le niveau en question.
    public void SelectLevel(InputAction.CallbackContext context)
    {
        if (context.performed && currentPoint.Islocked == false)
        {
            GameManager.instance.ChangeActionMap("TempsMort");

            //Lance la scene selectionné.
            SceneManager.LoadScene(currentPoint.destination.name);
            AddActorInTeam();
            Debug.Log("Load Scene...");
        }
    }

    //Déplace les joueurs.

    public void moveUp(InputAction.CallbackContext context)
    {
        Debug.Log("Up");
        if (context.performed && Up != null && Up.Islocked == false)
        {
            Debug.Log("Up");
            transform.position = Vector2.Lerp(transform.position, Up.transform.position, moveSpeed);
            if (transform.position == Up.transform.position)
            {
                currentPoint = Up;
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
            }

        }
    }
    public void moveLeft(InputAction.CallbackContext context)
    {
        if (context.performed && Left != null && Left.Islocked == false)
        {
            transform.position = Vector2.Lerp(transform.position, Left.transform.position, moveSpeed);
            if (transform.position == Left.transform.position)
            {
                currentPoint = Left;
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
            }
            Debug.Log("Left");
        }
    }
    public void moveRight(InputAction.CallbackContext context)
    {
        if (context.performed && Right != null && Right.Islocked == false)
        {
            transform.position = Vector2.Lerp(transform.position, Right.transform.position, moveSpeed);
            if (transform.position == Right.transform.position)
            {
                currentPoint = Right;
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
            }

            Debug.Log("Right");
        }
    }
    public void moveDown(InputAction.CallbackContext context)
    {
        if (context.performed && Down != null && Down.Islocked == false)
        {
            transform.position = Vector2.Lerp(transform.position, Down.transform.position, moveSpeed);
            if (transform.position == Down.transform.position)
            {
                currentPoint = Down;
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
            }
            Debug.Log("Down");
        }
    }
    void AddActorInTeam()
    {

    }
    #endregion
}