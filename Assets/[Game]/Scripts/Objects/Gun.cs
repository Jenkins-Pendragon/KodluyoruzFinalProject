using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Gun : Prop
{
    public EnemyShoot enemyShoot;
    public Enemy enemy;
    private Collider col;
    public Collider Collider { get { return (col == null) ? col = GetComponentInChildren<Collider>() : col; } }
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
        if (enemy.NavMeshAgent != null && !enemy.IsDead) 
        { 
            enemy.NavMeshAgent.enabled = true; 
        }
    }

    public override void OnInteractEnd(Transform forceDirection)
    {        
        base.OnInteractEnd(forceDirection);
        Collider.isTrigger = false;
    }
}
