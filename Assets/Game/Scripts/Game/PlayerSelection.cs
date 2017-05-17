using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerSelection : MonoBehaviour 
{
    public Button backButton;
    public Button choosePlayerButton;

    private CharacterSelection characterSelection;
    
    void Start()
    {
        characterSelection = GameObject.FindObjectOfType<CharacterSelection>();

        backButton.onClick.AddListener(delegate { PlatinioUI.instance.MoveToBack(); });
        choosePlayerButton.onClick.AddListener(delegate 
        {
            GameSettings.characterSelected = (GameSettings.CharacterSelected)characterSelection.currentSelection;
            PlatinioUI.instance.MoveToNext(); 
        });
    }

   
	
}
