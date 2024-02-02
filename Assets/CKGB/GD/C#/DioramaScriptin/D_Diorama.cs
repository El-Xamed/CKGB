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
    [SerializeField] bool action;
    [SerializeField] GameObject[] actionsUI;
    // Start is called before the first frame update
    void Start()
    {
        spEnergy.GetComponent<SpriteMask>().enabled=false;
        DedEsthela.SetActive(false);
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
        yield return new WaitForSeconds(1f);
        esthela.SetActive(false);
    }

    public void KillReviveEsthela()
    {
        if(Tetanisation.GetBool("ded")!=true)
        {
            DedEsthela.SetActive(true);
            Tetanisation.SetBool("ded",true);
            hideSpriteesthela();
        }
        else
        {
            Tetanisation.SetBool("ded", false);
            hideSpritetetanise();
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
}
