using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public static class EventManager
{
    public static UnityEvent OnGameStarted = new UnityEvent();
    public static UnityEvent OnGameEnd= new UnityEvent();
    public static UnityEvent OnLookAtTouchPosCompleted = new UnityEvent();
    public static UnityEvent OnEnemyDie = new UnityEvent();
    public static UnityEvent OnLevelFailed = new UnityEvent();
    public static UnityEvent OnLevelSuccess = new UnityEvent();
    public static UnityEvent OnTapStart = new UnityEvent();
    public static UnityEvent OnLevelStart = new UnityEvent();
    public static UnityEvent OnFinishLine = new UnityEvent();
    public static UnityEvent OnPlayerDeath = new UnityEvent();
    public static UnityEvent OnTapBar = new UnityEvent();
    public static UnityEvent OnGoldenEnemyDie = new UnityEvent();
}

