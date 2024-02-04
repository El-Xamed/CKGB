using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;


public class C_TempsMort : MonoBehaviour
{
    #region Variables

    [Header("Characters pages")]
    [SerializeField]
    GameObject[] charactersLittleResume;
    [SerializeField]
    GameObject[] charactersCompleteResume1;
    [SerializeField]
    GameObject[] charactersCompleteResume2;

    [SerializeField]
    TMP_Text[] littleCharactersSpecs;
    [SerializeField]
    TMP_Text[] CompleteCharactersSpecs1;
    [SerializeField]
    TMP_Text[] CompleteCharactersSpecs2;

    [Header("Buttons Lists")]
    [SerializeField]
    GameObject[] charactersButton;
    [SerializeField]
    GameObject[] actions;
    [SerializeField]
    GameObject ChallengeButton;

    [Header("Characters and TM")]
    [SerializeField]
    List<StartPosition> listCharactersPositions;

    [SerializeField]
    GameObject[] characters;
    [SerializeField]
    SO_TempsMort TM;

    [Header("Eventsystem and Selected Gameobjects")]
    [SerializeField] EventSystem Es;
    [SerializeField] GameObject currentButton;

    [SerializeField] GameObject actorActif;

    [SerializeField]
    bool[] characterHasPlayed;

    #endregion
    // Start is called before the first frame update
    void Start()
    {
        #region HideUI
        foreach (GameObject button in actions)
        {
            if (button != null)
                button.SetActive(false);
        }
        foreach (GameObject fiche in charactersCompleteResume1)
        {
            if (fiche != null)
                fiche.SetActive(false);
        }
        foreach (GameObject fiche in charactersCompleteResume2)
        {
            if (fiche != null)
                fiche.SetActive(false);
        }
        ChallengeButton.SetActive(false);
        #endregion

        GameManager.instance.ChangeActionMap("TempsMort");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
