using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : Singleton<GameManager>, IPointerDownHandler
{
    private bool isGameStarted;
    public bool IsGameStarted { get { return isGameStarted; } private set { isGameStarted = value; } }
    private bool isLevelStarted;
    public bool IsLevelStarted { get { return isLevelStarted; } set { isLevelStarted = value; } }


    private void OnEnable()
    {
        EventManager.OnLevelFailed.AddListener(EndGame);
        EventManager.OnLevelSuccess.AddListener(EndGame);
    }

    private void OnDisable()
    {
        EventManager.OnLevelFailed.RemoveListener(EndGame);
        EventManager.OnLevelSuccess.RemoveListener(EndGame);
    }

    private void Awake()
    {
        IsGameStarted = false;
        IsLevelStarted = false;
    }

    public void StartGame()
    {
        if (IsGameStarted)
            return;

        IsGameStarted = true;
        EventManager.OnGameStarted.Invoke();
    }

    public void EndGame()
    {
        if (!IsGameStarted)
            return;

        IsGameStarted = false;
        IsLevelStarted = false;
        EventManager.OnGameEnd.Invoke();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (IsGameStarted && !IsLevelStarted)
        {
            EventManager.OnLevelStart.Invoke();
            IsLevelStarted = true;
        }
    }
}
