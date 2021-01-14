using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Ammo : Prop, IPlayerKillable    
{
    public bool IsDeadly { get; set;}

    private void Awake()
    {
        IsDeadly = true;
    }
    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
        if (IsInteractable)
        {            
            transform.DOKill();
            RigidbodyObj.useGravity = true;
            IsDeadly = false;
        }
    }
}
