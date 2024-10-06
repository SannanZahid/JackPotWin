using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConstantsPlayerPref : MonoBehaviour
{

    #region LevelBoardJsonSave

    public static void SetBoardData(string data)
    {
        PlayerPrefs.SetString("Game_BoardData", data);
    }

    public static string GetBoardData()
    {
        return PlayerPrefs.GetString("Game_BoardData");
    }

    #endregion

    #region Sound

    public static void SetSound(int i)
    {
        PlayerPrefs.SetInt("Game_Sound", i);
    }

    public static int GetSound()
    {
        return PlayerPrefs.GetInt("Game_Sound");
    }

    #endregion

    #region GameScoreKeeping

    public static void SetGameLevel(int i)
    {
        PlayerPrefs.SetInt("Game_Level", i);
    }

    public static int GetGameLevel()
    {
        return PlayerPrefs.GetInt("Game_Level");
    }

    public static void SetTotalMatches(int i)
    {
        PlayerPrefs.SetInt("Game_TotalMatches", i);
    }

    public static int GetTotalMatches()
    {
        return PlayerPrefs.GetInt("Game_TotalMatches");
    }

    public static void SetTotalTurns(int i)
    {
        PlayerPrefs.SetInt("Game_TotalTurns", i);
    }

    public static int GetTotalTurns()
    {
        return PlayerPrefs.GetInt("Game_TotalTurns");
    }

    public static void SetTotalCombo(int i)
    {
        PlayerPrefs.SetInt("Game_TotalCombo", i);
    }

    public static int GetTotalCombo()
    {
        return PlayerPrefs.GetInt("Game_TotalCombo");
    }

    #endregion

    #region Set Default Game Playerpref Values

    public static void SetFirstTimeGameOpen()
    {
        if (PlayerPrefs.GetInt("FirstTimeSet").Equals(0))
            PlayerPrefs.SetInt("FirstTimeSet", 1);
    }

    public static int GetFirstTimeGameOpenSet()
    {
        return PlayerPrefs.GetInt("FirstTimeSet");
    }

    public static void SetInitialGameStats()
    {
        if (GetFirstTimeGameOpenSet().Equals(0))
        {
            SetDefaultGameValues();
        }
    }

    public static void SetDefaultGameValues()
    {
        if (GetFirstTimeGameOpenSet().Equals(0))
        {
            SetGameLevel(1);
            SetFirstTimeGameOpen();
        }
    }

    #endregion
}
