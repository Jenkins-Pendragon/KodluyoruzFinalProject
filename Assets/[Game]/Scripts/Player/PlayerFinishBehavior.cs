using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerFinishBehavior : MonoBehaviour
{    
    public GameObject gunVisual;
    public GameObject goldenEnemy;
    public Vector3 gunRotation;
    public LaserController laserController;
    private bool canDraw;
    private void OnEnable()
    {
        EventManager.OnFinishLine.AddListener(Finish);   
    }


    private void OnDisable()
    {
        EventManager.OnFinishLine.RemoveListener(Finish);
    }

    private void Update()
    {
        if (canDraw)
        {
            laserController.DrawLaser();
            if (!laserController.LaserLine.enabled) laserController.LaserLine.enabled = true;
        }
    }

    private void Finish() 
    {
        PlayerData.Instance.IsControlable = false;
        gunVisual.transform.LookAt(goldenEnemy.transform);
        canDraw = true;
        Sequence player = DOTween.Sequence();          
        player.Append(gunVisual.transform.DORotate(gunRotation, 0.5f));
    }
}
