using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldenEnemy : Enemy
{
    public GameObject ragdoll;

    public override void OnInteractStart(Transform parent, Transform destination)
    {
        base.OnInteractStart(parent, destination);
        FollowCamera.Instance.targetObject = this.ragdoll;
    }
    public override void OnInteractEnd(Transform forceDirection)
    {
        base.OnInteractEnd(forceDirection);
        FollowCamera.Instance.CameraFollow();

    }
}
