using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour 
{

    private UIScreen thisScreen;

    void Start()
    {
        thisScreen = GetComponent<UIScreen>();
        //thisScreen.ShowElement("GameOver");
    }
}
