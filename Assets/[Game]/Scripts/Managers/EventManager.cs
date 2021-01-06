using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public static class EventManager
{
    public static PointerEvent OnDrag = new PointerEvent();
    public static PointerEvent OnPointerDown = new PointerEvent();
    public static PointerEvent OnPointerUp = new PointerEvent();
}

public class PointerEvent : UnityEvent<PointerEventData> { }