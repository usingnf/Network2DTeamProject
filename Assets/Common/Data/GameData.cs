using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public const int COUNTDOWN = 3;

    public const string PLAYER_READY = "Ready";
    public const string PLAYER_LOAD = "Load";
    public const string PLAYER_CLEAR = "Clear";
    public const int Stage = 0;
    public const string PLAYER_IN = "Clear";



    public static Color GetColor(int playerNumber)
    {
        switch ( playerNumber )
        {
            case 0 : return Color.red;
            case 1 : return Color.blue;
            case 2 : return Color.green;
            case 3 : return Color.yellow;
            
            default : return Color.black;
        }
    }
}
