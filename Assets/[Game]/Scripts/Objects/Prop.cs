using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Prop : InteractableBase
{
    public override void OnInteractStart(Transform parent, Transform destination)
    {
        transform.DOKill();
        base.OnInteractStart(parent, destination);
    }

    public override void OnInteractEnd(Transform forceDirection)
    {
        base.OnInteractEnd(forceDirection);
        IsInteractable = true;
        IsKillable = true; //Set false after
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (IsKillable)
        {
            //IsKillable = false;
        }
    }
}
