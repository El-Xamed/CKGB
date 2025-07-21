using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pyjama : MonoBehaviour
{
    [Header("Default")]
    [SerializeField] Sprite defaultSprite;
    [SerializeField] Sprite cataSprite;

    [Header("Step")]
    [SerializeField] Sprite stepDefaultSprite;
    [SerializeField] Sprite stepCataSprite;

    C_Actor actor;
    private void Awake()
    {
        actor = GetComponent<C_Actor>();
    }
    public void Activate()
    {
        actor.GetDataActor().challengeSprite = defaultSprite;
        actor.GetDataActor().challengeSpriteOnCata = cataSprite;
    }

    public void Step()
    {
        actor.GetDataActor().challengeSprite = stepDefaultSprite;
        actor.GetDataActor().challengeSpriteOnCata = stepCataSprite;
    }
}
