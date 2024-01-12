using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class C_Worldmap : MonoBehaviour
{
    [SerializeField]
    int currentPosition;

    [SerializeField]
    C_destination[] destination;

    //Navigation entre les destination
    public void UpdateDestination(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (currentPosition == 0 && context.ReadValue<float>() == -1)
                return;
            if (currentPosition == destination.Length - 1 && context.ReadValue<float>() == 1)
                return;

            //Fonction qui va déplacer les personnages en fonction de la valeur si dessous.
            ChangeDestination(currentPosition += (int)context.ReadValue<float>());
        }
    }

    //Fonction qui lance le niveau en question.
    public void SelectLevel(InputAction.CallbackContext context)
    {
        if (context.performed)
            destination[currentPosition].GoToLevel();
    }

    //Déplace les joueurs.
    private void ChangeDestination(int currentDEstiantion)
    {

    }
}
