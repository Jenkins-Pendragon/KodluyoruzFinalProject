using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Gun : Prop
{
    public EnemyShoot enemyShoot;   
    
    protected override void Start()
    {
        base.Start();
        RigidbodyObj.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
        RigidbodyObj.isKinematic = true;
    }

    public override void OnInteractStart(Transform parent, Transform destination)
    {        
        base.OnInteractStart(parent, destination);        
        enemyShoot.IsCanFire = false;
        enemyShoot.canRun = true;        
        if (enemyShoot.NavMeshAgent != null && !enemyShoot.IsDead) 
        {
            enemyShoot.ResetRotation();
            enemyShoot.NavMeshAgent.enabled = true; 
        }
    }

    public override void OnInteractEnd(Transform forceDirection)
    {        
        base.OnInteractEnd(forceDirection);
        Collider.isTrigger = false;
    }
}
