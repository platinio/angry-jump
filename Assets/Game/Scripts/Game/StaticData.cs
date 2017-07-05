using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Data what must live all the game execution
/// </summary>
public static class StaticData 
{
    private static int _numberOfDies = 0;

    public static int numberOfDies { get { return _numberOfDies; } set { _numberOfDies = value; } }
    
}
