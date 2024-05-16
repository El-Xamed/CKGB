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
        EnableControl();

        //Pour afficher l'Ui.
        challenge.ShowUiChallenge(true);
    }

    public void EnableControl()
    {
        GetComponentInParent<PlayerInput>().enabled = true;
        challenge.SetAnimFinish(true);

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
                challenge.CataTrun();
                break;
        }
    }

    public void DisableControl()
    {
        GetComponentInParent<PlayerInput>().enabled = false;
    }
    #endregion
}
