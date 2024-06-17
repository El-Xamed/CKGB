using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class TestSoundAnimation : MonoBehaviour
{


    public void AnimAngerEsthelaNimu()
    {
       AudioManager.instanceAM.Play("Anger");  
    }

    public void AnimAngerMorgan()
    {
        AudioManager.instanceAM.Play("Anger2");
    }

    public void SfxPowerUp()
    {
        AudioManager.instanceAM.Play("SfxPowerUp");
    }

    public void SfxPowerDown()
    {
        AudioManager.instanceAM.Play("SfxPowerDown");
    }

    public void Rainbow()
    {
        AudioManager.instanceAM.Play("Rainbow");
    }

    public void Interrogation()
    {
        AudioManager.instanceAM.Play("Interrogation");
    }

    public void Hearts()
    {
       AudioManager.instanceAM.Play("Hearts");
    }

    public void Deception()
    {
        AudioManager.instanceAM.Play("Deception");
    }

    public void Exclamation()
    {
        AudioManager.instanceAM.Play("Exclamation");
    }

    public void Dots()
    {
        AudioManager.instanceAM.Play("Dots");
    }

    public void BruitDePas()
    {
        AudioManager.instanceAM.Play("BruitDePas");
    }
}

