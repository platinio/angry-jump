using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
public static class GameSettings 
{

    public static CharacterSelected characterSelected
    {
        get 
        {
            return _characterSelected;
        }

        set
        {
            _characterSelected = value;
            switch (_characterSelected)
            {
                case CharacterSelected.NORMAN:
                    characterName = "Norman";
                    break;
                case CharacterSelected.INDIANAGUY:
                    characterName = "IndianaGuy";
                    break;
                case CharacterSelected.SOMBI:
                    characterName = "Sombi";
                    break;
                case CharacterSelected.BOB:
                    characterName = "Bob";
                    break;
                case CharacterSelected.LUCY:
                    characterName = "Lucy";
                    break;
            }
        }
    }
    public static GameMode gameMode;
    public static string characterName = "IndianaGuy";

    private static CharacterSelected _characterSelected;
    

}
