using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LevelManager : Singleton<LevelManager>
{
    public int levelCount;
    public int LevelIndex
    {
        get
        {
            return PlayerPrefs.GetInt("LastLevel", 0);
        }
        set
        {
            if (value >= levelCount)
                value = 0;

            PlayerPrefs.SetInt("LastLevel", value);
        }
    }
    public void LevelUp()
    {
        LevelIndex += 1;
        PlayerPrefs.SetInt("LastLevel", LevelIndex);
    }
}
