using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Ammo : Prop, IPlayerKillable    
{
    public bool IsDeadly { get; set;}
    private TrailRenderer trailRenderer;
    public TrailRenderer TrailRenderer { get { return (trailRenderer == null) ? trailRenderer = GetComponent<TrailRenderer>() : trailRenderer; } }

    private void Awake()
    {
        IsDeadly = true;
    }

    public override void OnInteractStart(Transform parent, Transform destination)
    {
        base.OnInteractStart(parent, destination);
        TrailRenderer.enabled = false;
    }

    public override void OnInteractEnd(Transform forceDirection)
    {
        base.OnInteractEnd(forceDirection);
        RigidbodyObj.useGravity = true;
    }
    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
        if (IsInteractable)
        {
            Off();
            if (!collision.gameObject.CompareTag("Player")) IsDeadly = false;
        }
    }

    public void Off() 
    {
        transform.DOKill();
        RigidbodyObj.useGravity = true;
        TrailRenderer.enabled = false;
    }
}
