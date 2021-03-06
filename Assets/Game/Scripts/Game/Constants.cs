﻿using System;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Save constans use in other scripts
/// </summary>
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
    [SerializeField]
    public Values values;

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

    [System.Serializable]
    public class Values
    {
        public float createPlatformDelay;
    }

}
