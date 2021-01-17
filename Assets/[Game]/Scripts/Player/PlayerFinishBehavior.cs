using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerFinishBehavior : MonoBehaviour
{    
    public GameObject gunVisual;    
    public Vector3 gunRotation;
    public LaserController laserController;    
    private bool canDraw;
    private bool isFinish;
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
        if (isFinish)
        {
            if (canDraw)
            {
                laserController.DrawLaser(LayerMask.GetMask("GoldenEnemy"));
                if (!laserController.LaserLine.enabled) laserController.LaserLine.enabled = true;
            }
            if (Input.GetMouseButtonDown(0))
            {
                isFinish = false;
                EventManager.OnTapBar.Invoke();
                Realase();
                canDraw = false;
            }
        }
    }

    private void Finish() 
    {        
        PlayerData.Instance.IsControlable = false;
        Realase();
        gunVisual.transform.LookAt(GoldenEnemy.Instance.transform);
        canDraw = true;
        isFinish = true;
        Sequence player = DOTween.Sequence();          
        player.Append(gunVisual.transform.DORotate(gunRotation, 0.5f));
    }

    private void Realase() 
    {        
        laserController.RealaseInteractableObject();
        laserController.LaserLine.enabled = false;
    }
}
