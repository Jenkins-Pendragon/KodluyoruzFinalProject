using UnityEngine;
using UnityEngine.EventSystems;

public class TapToStop : MonoBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        EventManager.OnTapBar.Invoke();
    }
}
