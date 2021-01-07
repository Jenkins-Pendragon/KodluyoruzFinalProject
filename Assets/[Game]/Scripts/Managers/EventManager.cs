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
}

