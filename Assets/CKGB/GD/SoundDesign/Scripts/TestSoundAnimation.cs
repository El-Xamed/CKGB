using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class TestSoundAnimation : MonoBehaviour
{
    public AudioManager A;
    public void Start()
    {
      if(AudioManager.instance)
        {
            A = AudioManager.instance;
        }
    }

    public void AnimAngerEsthelaNimu()
    {
        A.PlayOnce("Anger");  
    }

    public void AnimAngerMorgan()
    {
        A.PlayOnce("Anger2");
    }

    public void SfxPowerUp()
    {
        A.PlayOnce("SfxPowerUp");
    }

    public void SfxPowerDown()
    {
        A.PlayOnce("SfxPowerDown");
    }

    public void Rainbow()
    {
        A.PlayOnce("Rainbow");
    }

    public void Interrogation()
    {
        A.PlayOnce("Interrogation");
    }

    public void Hearts()
    {
        A.PlayOnce("Hearts");
    }

    public void Deception()
    {
        A.PlayOnce("Deception");
    }

    public void Exclamation()
    {
        A.PlayOnce("Exclamation");
    }

    public void Dots()
    {
        A.PlayOnce("Dots");
    }


}

