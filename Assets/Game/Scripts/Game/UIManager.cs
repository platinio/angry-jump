using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls the ui for the gameplay
/// </summary>
public class UIManager : MonoBehaviour 
{
    #region SINGLETON
    private static UIManager UI_Manager;
    public static UIManager instance
    {
        get
        {
            if (!UI_Manager)
            {
                UI_Manager = FindObjectOfType(typeof(UIManager)) as UIManager;



                if (!UI_Manager)
                    Debug.LogError("You need to have a UIManager script active in the scene");
            }

            return UI_Manager;
        }

    }
    #endregion SINGLETON

    //inspector
    [SerializeField] private Text _score                    = null;
    [SerializeField] private GameObject _gameOverTimer      = null;
    [SerializeField] private Button _useLife                = null;
   
    //private
    private int _currentScore   = 0;

    public void ShowGameOverTimer()
    {
        //show gameOver screen
        _gameOverTimer.SetActive(true);
        //if we have lives enable button
        _useLife.interactable = PlayerPrefs.GetInt("lives") > 0;
    }

    public void HideGameOverTimer()
    {
        //hide ui
        _gameOverTimer.SetActive(false);
    }

    /// <summary>
    /// add a score point
    /// </summary>
    public void AddScore()
    {
        _currentScore++;
        int length = _currentScore.ToString().Length;

        string text = string.Empty;

        if (length == 1)
            text = "Score 000" + _currentScore;
        else if (length == 2)
            text = "Score 00" + _currentScore;
        else if (length == 3)
            text = "Score 0" + _currentScore;
        else
            text = "Score " + _currentScore;


        _score.text = text;

    }

}
