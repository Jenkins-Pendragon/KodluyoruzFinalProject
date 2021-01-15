using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerDeath : MonoBehaviour
{
    private void OnEnable()
    {
        EventManager.OnLevelFailed.AddListener(Death);
    }

    private void OnDisable()
    {
        EventManager.OnLevelFailed.RemoveListener(Death);
    }

    private void Death() 
    {
        transform.DORotate(new Vector3(0, 0, -30), 1.5f, RotateMode.FastBeyond360);
    }
}
