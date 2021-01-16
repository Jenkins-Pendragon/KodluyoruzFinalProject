using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldenEnemy : Enemy
{
    public GameObject ragdoll;

    protected override void Awake(){} //Dont delete.
    protected override void Start()
    {
        base.Start();
        IsInteractable = false;
    }

    private void OnEnable()
    {
        EventManager.OnFinishLine.AddListener(() => IsInteractable = true);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        EventManager.OnFinishLine.AddListener(() => IsInteractable = true);
    }

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
