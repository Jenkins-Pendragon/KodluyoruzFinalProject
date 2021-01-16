using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TapToStart : MonoBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        if (GameManager.Instance.IsGameStarted && !GameManager.Instance.IsLevelStarted)
        {
            EventManager.OnLevelStart.Invoke();
            GameManager.Instance.IsLevelStarted = true;
        }
    }
}
