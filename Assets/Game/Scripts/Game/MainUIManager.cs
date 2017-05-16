using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainUIManager : MonoBehaviour 
{
    #region SINGLETON
    private static MainUIManager mainUIManager;
    public static MainUIManager instance
    {
        get
        {
            if (!mainUIManager)
            {
                mainUIManager = FindObjectOfType(typeof(MainUIManager)) as MainUIManager;



                if (!mainUIManager)
                    Debug.LogError("You need to have a MainUIManager script active in the scene");
            }

            return mainUIManager;
        }

    }
    #endregion SINGLETON

    public List<>


}
