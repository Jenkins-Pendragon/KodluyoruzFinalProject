using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerFinishBehavior : MonoBehaviour
{
    public Transform finishPoint;
    public GameObject gunVisual;
    public GameObject goldenEnemy;
    public Vector3 gunRotation;
    public LaserController laserController;
    private bool canDraw;
    private void OnEnable()
    {
        EventManager.OnLastPlatform.AddListener(Finish);   
    }


    private void OnDisable()
    {
        EventManager.OnLastPlatform.RemoveListener(Finish);
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
        Vector3 pos = finishPoint.position;
        pos.y = transform.position.y;

        Sequence player = DOTween.Sequence();
        player.Append(transform.DOMove(pos, 1f).OnComplete(() =>
        {            
            gunVisual.transform.LookAt(goldenEnemy.transform);
            canDraw = true;                     
        }));
        player.Append(gunVisual.transform.DORotate(gunRotation, 0.5f));
    }
}
