using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameSettings 
{
    public enum CharacterSelected
    {
        NORMAN,
        INDIANAGUY,
        SOMBI,
        BOB,
        LUCY,
    }
    public enum GameMode
    {
        EASY,
        MEDIUM,
        HARD,
    
    }

    public static CharacterSelected characterSelected;
    public static GameMode gameMode;

}
