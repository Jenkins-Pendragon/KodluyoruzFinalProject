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
    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
        if (IsInteractable)
        {            
            transform.DOKill();
            RigidbodyObj.useGravity = true;
            TrailRenderer.enabled = false;
            if (!collision.gameObject.CompareTag("Player")) IsDeadly = false;
        }
    }
}
