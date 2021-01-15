using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PowerBar : MonoBehaviour
{
    public GameObject pointer;
    public Transform startPoint;
    public Transform endPoint;
    public bool isActive = false;

    private void OnEnable()
    {
        EventManager.OnLevelSuccess.AddListener(ActivatePowerPoint);
    }

    private void ActivatePowerPoint()
    {
     
        StartCoroutine(PowerPoint());
    }

    IEnumerator PowerPoint()
    {
        while (true)
        {
            pointer.transform.DOMoveY(startPoint.position.y, 1f).OnComplete(() => pointer.transform.DOMoveY(endPoint.position.y, 1f));
            yield return new WaitForSeconds(2f);
        }


        
    }

}
