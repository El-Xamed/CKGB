using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class D_Diorama : MonoBehaviour
{
    [SerializeField] Animator energyUp;
    [SerializeField] ParticleSystem PSenergy;
    [SerializeField] GameObject spEnergy;
    [SerializeField] GameObject DedEsthela;
    [SerializeField] GameObject esthela;
    [SerializeField] Animator Tetanisation;
    [SerializeField] GameObject JaugeKO;
    [SerializeField] GameObject jaugenormal;
    [SerializeField] GameObject[] choiceButton;


    [SerializeField] bool action;
    [SerializeField] GameObject[] actionsUI;
    // Start is called before the first frame update
    void Start()
    {
        spEnergy.GetComponent<SpriteMask>().enabled=false;
        DedEsthela.SetActive(false);
        JaugeKO.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
    
    }
    public void GoToTempsMort()
    {
        SceneManager.LoadScene("Diorama_TempsMort");
    }
    public void GoToChallenge()
    {
        SceneManager.LoadScene("Diorama_Challenge");
    }
    public void ENERGYUP()
    {
       
        energyUp.SetTrigger("trigger");
        spEnergy.GetComponent<SpriteMask>().enabled = true;
        PSenergy.Play();
        StartCoroutine("hideSpriteMask");

    }
    public IEnumerator hideSpriteMask()
    {
        yield return new WaitForSeconds(1.4f);
        spEnergy.GetComponent<SpriteMask>().enabled = false;
    }
    public IEnumerator hideSpritetetanise()
    {
        yield return new WaitForSeconds(1f);
        DedEsthela.SetActive(false);
    }
    public IEnumerator hideSpriteesthela()
    {
        yield return new WaitForSeconds(0.8f);
        esthela.SetActive(false);
    }

    public void KillReviveEsthela()
    {
        if(Tetanisation.GetBool("ded")!=true)
        {
            DedEsthela.SetActive(true);
            Tetanisation.SetBool("ded",true);
            StartCoroutine(hideSpriteesthela());
            JaugeKO.SetActive(true);
            jaugenormal.SetActive(false);
            actionsUI[6] = JaugeKO;
        }
        else
        {
            Tetanisation.SetBool("ded",false);
            StartCoroutine(hideSpritetetanise());
            esthela.SetActive(true);
            JaugeKO.SetActive(false);
            jaugenormal.SetActive(true);
            actionsUI[6] = jaugenormal;
        }
    }
    public void enterExitActionMenu()
    {
        if(action!=true)
        {
            action = true;
            foreach(GameObject ui in actionsUI)
            ui.SetActive(true);
        }
        else
        {
            action = false;
            foreach (GameObject ui in actionsUI)
                ui.SetActive(false);
        }
    }
    public void switchButtonChoice()
    {
        if (choiceButton[0] == actionsUI[4])
        {
            actionsUI[4] = choiceButton[1];
            choiceButton[0].SetActive(false);
            choiceButton[1].SetActive(true);
        }
        else
        {
            actionsUI[4] = choiceButton[0];
            choiceButton[1].SetActive(false);
            choiceButton[0].SetActive(true);
        }           
    }
}
