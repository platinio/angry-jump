using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerSelection : MonoBehaviour 
{
    //inspector
    [SerializeField] private Button _backButton             = null;
    [SerializeField] private Button _choosePlayerButton     = null;

    //private
    private CharacterSelection _characterSelection   = null;
    
    void Start()
    {
        _characterSelection = GameObject.FindObjectOfType<CharacterSelection>();

        _backButton.onClick.AddListener(delegate { PlatinioUI.instance.MoveToBack(); });
        _choosePlayerButton.onClick.AddListener(delegate 
        {
            GameSettings.characterSelected = (CharacterSelected)_characterSelection.CurrentSelection;
            PlatinioUI.instance.MoveToNext(); 
        });
    }

   
	
}
