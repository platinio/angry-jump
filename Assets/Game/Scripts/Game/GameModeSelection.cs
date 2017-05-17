using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameModeSelection : MonoBehaviour 
{
    public Button easyModeButton;
    public Button mediumModeButton;
    public Button hardModeButton;
    public Button backButton;

    private UIScreen screen;


    void Start()
    { 
        easyModeButton.onClick.AddListener(delegate { GameSettings.gameMode = GameSettings.GameMode.EASY; });
        mediumModeButton.onClick.AddListener(delegate { GameSettings.gameMode = GameSettings.GameMode.MEDIUM; });
        hardModeButton.onClick.AddListener(delegate { GameSettings.gameMode = GameSettings.GameMode.HARD; });
        
        backButton.onClick.AddListener(delegate { PlatinioUI.instance.MoveToBack(); });

        screen = GetComponent<UIScreen>();
        PlatinioUI.instance.OnAnimationComplete += ShowButtons;
    }

    public void ShowButtons()
    {
        PlatinioUI.instance.OnAnimationComplete -= ShowButtons;
        screen.OnAnimationComplete += TriggerMediumButtonAnim;
        screen.ShowElement("Easy");

    }

   
    public void TriggerMediumButtonAnim()
    {
        screen.OnAnimationComplete += TriggerHardButtonAnim;
        screen.OnAnimationComplete -= TriggerMediumButtonAnim;  
        screen.ShowElement("Medium");           

    }

    public void TriggerHardButtonAnim()
    {
        screen.OnAnimationComplete -= TriggerHardButtonAnim;
        screen.ShowElement("Hard");        
    }

    private void SetButtonReferences()
    {
        easyModeButton.onClick.AddListener(delegate{});
        mediumModeButton.onClick.AddListener(delegate { });
        hardModeButton.onClick.AddListener(delegate { });
    }
	
}
