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

    public Slider loadBar;

    private UIScreen screen;
    private LoadLevelManager loadLevelManager;
    private bool endAnimation;

    void Start()
    {

        loadLevelManager = GameObject.FindObjectOfType<LoadLevelManager>() as LoadLevelManager;

        loadLevelManager.elementsToDeactivated = new List<GameObject>();
        //set elements
        loadLevelManager.elementsToDeactivated.Add(easyModeButton.gameObject);
        loadLevelManager.elementsToDeactivated.Add(mediumModeButton.gameObject);
        loadLevelManager.elementsToDeactivated.Add(hardModeButton.gameObject);
        loadLevelManager.elementsToDeactivated.Add(backButton.gameObject);

        loadLevelManager.loadBar = loadBar;

        easyModeButton.onClick.AddListener(delegate 
        {
            if (!endAnimation)
            {
                endAnimation = !endAnimation;
                PlatinioUI.instance.OnAnimationComplete -= TriggerMediumButtonAnim;
                PlatinioUI.instance.OnAnimationComplete -= TriggerHardButtonAnim;
            }

                          
                
            GameSettings.gameMode = GameSettings.GameMode.EASY;
            loadLevelManager.LoadLevel("Game");
 
        });
        mediumModeButton.onClick.AddListener(delegate 
        {
            if (!endAnimation)
            {
                endAnimation = !endAnimation;
                PlatinioUI.instance.OnAnimationComplete -= TriggerHardButtonAnim;
            }
            GameSettings.gameMode = GameSettings.GameMode.MEDIUM;
            loadLevelManager.LoadLevel("Game");
        });
        hardModeButton.onClick.AddListener(delegate 
        {
            if (!endAnimation)
                endAnimation = !endAnimation;
            GameSettings.gameMode = GameSettings.GameMode.HARD;
            loadLevelManager.LoadLevel("Game");
        });
        
        backButton.onClick.AddListener(delegate 
        {
            PlatinioUI.instance.MoveToBack(); 

        });

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
        if (endAnimation)
            return;

        screen.OnAnimationComplete += TriggerHardButtonAnim;
        screen.OnAnimationComplete -= TriggerMediumButtonAnim;
        screen.ShowElement("Medium");           

    }

    public void TriggerHardButtonAnim()
    {
        if (endAnimation)
            return;

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
