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
        AudioManager.instance.PlayOnce("Anger");  
    }

    public void AnimAngerMorgan()
    {
        AudioManager.instance.PlayOnce("Anger2");
    }
}

