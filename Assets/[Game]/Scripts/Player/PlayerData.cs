using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public static PlayerData Instance;
    [HideInInspector]
    public bool IsPlayerDead = false;
    [HideInInspector]
    public bool IsControlable = true;
    private void Awake()
    {        
        Instance = this;
    }

    private void OnEnable()
    {
        EventManager.OnLevelFailed.AddListener(() => IsPlayerDead = true);
    }

    private void OnDisable()
    {
        EventManager.OnLevelFailed.RemoveListener(() => IsPlayerDead = true);
    }
}
