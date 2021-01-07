using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : InteractableBase
{

    public override void OnStart()
    {
        base.OnStart();
    }
    public override void OnInteractStart(Transform parent, Transform destination)
    {
        base.OnInteractStart(parent, destination);
        IsInteractable = false;
    }

    public override void OnInteractEnd(Transform forceDirection)
    {
        base.OnInteractEnd(forceDirection);
        IsInteractable = false;
    }
}
