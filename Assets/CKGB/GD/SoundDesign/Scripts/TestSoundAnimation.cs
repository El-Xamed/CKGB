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

    public void musique()
    {
        AudioManager.instance.PlayOnce("SfxHover");  
    }

}

