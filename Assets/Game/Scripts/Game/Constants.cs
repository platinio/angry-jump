using System;
using System.Collections.Generic;
using UnityEngine;



public class Constants : MonoBehaviour
{

    #region SINGLETON
    private static Constants constants;
    public static Constants instance
    {
        get
        {
            if (!constants)
            {
                constants = FindObjectOfType(typeof(Constants)) as Constants;



                if (!constants)
                    Debug.LogError("You need to have a Constants script active in the scene");
            }

            return constants;
        }

    }
    #endregion SINGLETON

    [SerializeField]
    public Tags tags;
    [SerializeField]
    public Layers layers;

    [System.Serializable]
    public class Tags
    {
        public string player;
        public string platform;        
    }

    [System.Serializable]
    public class Layers
    {
        public LayerMask Platform; 
    }

}
