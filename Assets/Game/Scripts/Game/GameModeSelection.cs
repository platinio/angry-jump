using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI screen to choose game mode selection
/// </summary>
public class GameModeSelection : MonoBehaviour 
{
    //inspector
    private Button _easyModeButton     = null;
    private Button _mediumModeButton   = null;
    private Button _hardModeButton     = null;
    private Button _backButton         = null;
    private Slider _loadBar            = null;

    //private
    private UIScreen            _screen             = null;
    private LoadLevelManager    _loadLevelManager   = null;
    private bool                _endAnimation       = false;

    void Start()
    {
        //get buttons
        _easyModeButton     = transform.FindChild("Easy").GetComponent<Button>();
        _mediumModeButton   = transform.FindChild("Medium").GetComponent<Button>();
        _hardModeButton     = transform.FindChild("Hard").GetComponent<Button>();
        _backButton         = transform.FindChild("BackButton").GetComponent<Button>();
        _loadBar            = transform.FindChild("LoadBar").GetComponent<Slider>();

        _loadLevelManager = GameObject.FindObjectOfType<LoadLevelManager>() as LoadLevelManager;

        _loadLevelManager.elementsToDeactivated = new List<GameObject>();
        //set elements
        _loadLevelManager.elementsToDeactivated.Add(_easyModeButton.gameObject);
        _loadLevelManager.elementsToDeactivated.Add(_mediumModeButton.gameObject);
        _loadLevelManager.elementsToDeactivated.Add(_hardModeButton.gameObject);
        _loadLevelManager.elementsToDeactivated.Add(_backButton.gameObject);

        _loadLevelManager.loadBar = _loadBar;

        _easyModeButton.onClick.AddListener(delegate 
        {
            if (!_endAnimation)
            {
                _endAnimation = !_endAnimation;
                PlatinioUI.instance.OnAnimationComplete -= TriggerMediumButtonAnim;
                PlatinioUI.instance.OnAnimationComplete -= TriggerHardButtonAnim;
            }



            GameSettings.gameMode = GameMode.EASY;
            _loadLevelManager.LoadLevel("Game");
 
        });
        _mediumModeButton.onClick.AddListener(delegate 
        {
            if (!_endAnimation)
            {
                _endAnimation = !_endAnimation;
                PlatinioUI.instance.OnAnimationComplete -= TriggerHardButtonAnim;
            }
            GameSettings.gameMode = GameMode.MEDIUM;
            _loadLevelManager.LoadLevel("Game");
        });
        _hardModeButton.onClick.AddListener(delegate 
        {
            if (!_endAnimation)
                _endAnimation = !_endAnimation;
            GameSettings.gameMode = GameMode.HARD;
            _loadLevelManager.LoadLevel("Game");
        });
        
        _backButton.onClick.AddListener(delegate 
        {
            PlatinioUI.instance.MoveToBack(); 

        });

        _screen = GetComponent<UIScreen>();
        PlatinioUI.instance.OnAnimationComplete += ShowButtons;
    }

    public void ShowButtons()
    {
        PlatinioUI.instance.OnAnimationComplete -= ShowButtons;
        _screen.OnAnimationComplete += TriggerMediumButtonAnim;
        _screen.ShowElement("Easy");

    }

   
    public void TriggerMediumButtonAnim()
    {
        if (_endAnimation)
            return;

        _screen.OnAnimationComplete += TriggerHardButtonAnim;
        _screen.OnAnimationComplete -= TriggerMediumButtonAnim;
        _screen.ShowElement("Medium");           

    }

    public void TriggerHardButtonAnim()
    {
        if (_endAnimation)
            return;

       _screen.OnAnimationComplete -= TriggerHardButtonAnim;
       _screen.ShowElement("Hard");        
    }

    private void SetButtonReferences()
    {
        _easyModeButton.onClick.AddListener(delegate{});
        _mediumModeButton.onClick.AddListener(delegate { });
        _hardModeButton.onClick.AddListener(delegate { });
    }
	
}
