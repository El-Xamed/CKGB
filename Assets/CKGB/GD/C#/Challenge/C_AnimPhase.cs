using UnityEngine;
using UnityEngine.InputSystem;

public class C_AnimPhase : MonoBehaviour
{
    C_Challenge challenge;
    public string sfxTransition;

    // Start is called before the first frame update
    void Start()
    {
        challenge = GetComponentInParent<C_Challenge>();
    }

    
    #region Animation
    public void StartChallenge()
    {
        EnableControl();

        //Pour afficher l'Ui.
        challenge.ShowUiChallenge(true);
    }

    public void EnableControl()
    {
        GetComponentInParent<PlayerInput>().enabled = true;

        //Relance la phase de jeu.
        switch (challenge.GetPhaseDeJeu())
        {
            case C_Challenge.PhaseDeJeu.PlayerTrun:
                challenge.PlayerTurn();
                break;
            case C_Challenge.PhaseDeJeu.ResoTurn:
                Debug.Log("Launch by AniPhase !");
                challenge.ResolutionTurn();
                break;
            case C_Challenge.PhaseDeJeu.CataTurn:
                challenge.CataTurn();
                break;
        }
    }

    public void DisableControl()
    {
        GetComponentInParent<PlayerInput>().enabled = false;

        //SFX
        if (AudioManager.instanceAM)
        {
            AudioManager.instanceAM.Play(sfxTransition);
        }
    }
    #endregion
}
