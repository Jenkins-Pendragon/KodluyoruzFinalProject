﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public void Awake()
    {
        Instance = this;
        EventManager.OnGameStart.Invoke();
    }
    
}
