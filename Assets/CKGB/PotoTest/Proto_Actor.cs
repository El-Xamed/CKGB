using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Proto_Actor : MonoBehaviour
{
    #region
    int position;
    public new string name;
    public Sprite challengeSprite;
    public Sprite MapTmSprite;
    public int stressMax;
    int currentStress;
    public int energyMax;
    int currentEnergy;
    public int nbtraitpointMax;
    int nbtraitpoint;
    [SerializeField]
    Proto_SO_Character dataActor;
    #endregion


    private void Awake()
    {
        gameObject.name = dataActor.name;
    }

    public Proto_SO_Character GetDataActor()
    {
        return dataActor;
    }

    public int GetPosition()
    {
        return position;     
    }

    public void SetPosition(int newPosition)
    {
        position = newPosition;
    }
}
