using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerSelection : MonoBehaviour 
{
    public Button backButton;
    
    void Start()
    {
        backButton.onClick.AddListener(delegate { PlatinioUI.instance.MoveToBack(); });
    }
	
}
