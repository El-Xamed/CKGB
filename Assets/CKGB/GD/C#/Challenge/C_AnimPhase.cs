using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class C_AnimPhase : MonoBehaviour
{
    C_Challenge challenge;

    // Start is called before the first frame update
    void Start()
    {
        challenge = GetComponentInParent<C_Challenge>();
    }

    #region Animation
    public void StartChallenge()
    {
        challenge.PlayerTurn();
    }

    public void EnableControl()
    {
        GetComponentInParent<PlayerInput>().enabled = true;
    }

    public void DisableControl()
    {
        GetComponentInParent<PlayerInput>().enabled = false;
    }
    #endregion
}
