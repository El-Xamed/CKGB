using UnityEngine;
using UnityEngine.InputSystem;

public class C_AnimPhase : MonoBehaviour
{
    C_Challenge challenge;
    PlayerInput inputs;
    public string sfxTransition;

    // Start is called before the first frame update
    void Start()
    {
        challenge = GetComponentInParent<C_Challenge>();
        inputs = GetComponentInParent<PlayerInput>();
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
        inputs.enabled = true;

        //Relance la phase de jeu.
        switch (challenge.GetPhaseDeJeu())
        {
            case C_Challenge.PhaseDeJeu.PlayerTrun:
                challenge.PlayerTurn();
                break;
            case C_Challenge.PhaseDeJeu.ResoTurn:
                challenge.ResolutionTurn();
                break;
            case C_Challenge.PhaseDeJeu.CataTurn:
                challenge.CataTurn();
                break;
        }
    }

    public void DisableControl()
    {
        inputs.enabled = false;

        //SFX
        if (AudioManager.instanceAM)
        {
            AudioManager.instanceAM.Play(sfxTransition);
        }
    }
    #endregion
}
