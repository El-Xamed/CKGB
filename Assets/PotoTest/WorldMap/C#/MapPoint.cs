using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MapPoint : MonoBehaviour
{
    #region variables
    [Header("WayPoints")]
    [SerializeField] MapPoint up, left, right, down;

    [Header("Options")]
    [SerializeField] int levelID = 0;
    [SerializeField] string SceneToLoad;

    [TextArea (1, 2)]
    [SerializeField] string levelName;

    [Header("type")]
    [SerializeField] bool IsLevel;
    [SerializeField] bool IsCorner;

    [Header("Level UI")]
    [SerializeField] Text UiLevelName;
    
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
