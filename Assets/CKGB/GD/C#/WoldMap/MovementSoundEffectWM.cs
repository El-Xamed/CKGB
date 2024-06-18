using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementSoundEffectWM : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PlayMovementSFX(string sfxName)
    {
        AudioManager.instanceAM.Play(sfxName);
    }
    public void StopMovementSFX(string sfxName)
    {
        AudioManager.instanceAM.Stop(sfxName);
    }
}
