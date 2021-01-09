using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prop : InteractableBase
{
    public override void OnInteractStart(Transform parent, Transform destination)
    {
        base.OnInteractStart(parent, destination);
    }

    public override void OnInteractEnd(Transform forceDirection)
    {
        base.OnInteractEnd(forceDirection);
        IsInteractable = true;
        IsKillable = true; //Set false after
    }
}
