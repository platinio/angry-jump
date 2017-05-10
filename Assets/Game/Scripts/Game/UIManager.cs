using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public Text score;
    public Text xRot;
    public Text yRot;
    public Text zRot;

    private int currentScore;


    void Update()
    {


        zRot.text = "Z: " + Input.acceleration.z * 360.0f;
        xRot.text = "X: " + Input.acceleration.x * 360.0f;
        yRot.text = "Y: " + Input.acceleration.y * 360.0f;
    }

    public void AddScore()
    {
        currentScore++;
        int length = currentScore.ToString().Length;

        string text = string.Empty;

        if (length == 1)
            text = "Score 000" + currentScore;
        else if (length == 2)
            text = "Score 00" + currentScore;
        else if (length == 3)
            text = "Score 0" + currentScore;
        else
            text = "Score " + currentScore;


        score.text = text;

    }

}
