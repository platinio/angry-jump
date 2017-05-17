using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerSelection : MonoBehaviour 
{
    public Button backButton;
    public Button choosePlayerButton;
    
    void Start()
    {
        backButton.onClick.AddListener(delegate { PlatinioUI.instance.MoveToBack(); });
        choosePlayerButton.onClick.AddListener(delegate { PlatinioUI.instance.MoveToNext(); });
    }
	
}
