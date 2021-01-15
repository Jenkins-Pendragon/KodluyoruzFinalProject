using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PowerBar : MonoBehaviour
{
    public GameObject pointer;
    public bool isActive = false;
    

    private void OnEnable()
    {
        EventManager.OnLevelSuccess.AddListener(() => isActive = true);
        EventManager.OnLevelSuccess.AddListener(ActivatePowerPoint);
    }

    private void OnDisable()
    {
        EventManager.OnLevelSuccess.RemoveListener(() => isActive = true);
        EventManager.OnLevelSuccess.RemoveListener(ActivatePowerPoint);
    }

    private void ActivatePowerPoint()
    {
     
        StartCoroutine(PowerPoint());
    }

    IEnumerator PowerPoint()
    {
        while (true)
        {

            pointer.transform.DORotate(new Vector3(0f, 0f, 40f), 0.5f).OnComplete(() => pointer.transform.DORotate(new Vector3(0f, 0f, -40f), 0.5f));
           
        }


        
    }

}
