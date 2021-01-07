using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : InteractableBase
{
    public override void OnInteractStart(Transform parent, Transform destination)
    {
        base.OnInteractStart(parent, destination);
        IsInteractable = false;
    }

    public override void OnInteractEnd(Transform forceDirection)
    {
        base.OnInteractEnd(forceDirection);
        IsInteractable = true;
    }
}
