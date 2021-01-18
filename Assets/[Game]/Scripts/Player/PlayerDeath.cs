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
        DOTween.KillAll();
    }

    private void Death() 
    {
        Vector3 playerRot = transform.eulerAngles;
        transform.DORotate(playerRot+Vector3.right*20f, 1.5f, RotateMode.FastBeyond360);
        EventManager.OnPlayerDeath.Invoke();
    }
}
