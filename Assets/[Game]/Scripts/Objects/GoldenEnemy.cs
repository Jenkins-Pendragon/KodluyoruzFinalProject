using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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
        #region InteractableBase
        transform.DOKill();
        if (Outline.enabled == true)
        {
            Outline.enabled = false;
        }
        gameObject.transform.parent = null;
        RigidbodyObj.isKinematic = false;
        RigidbodyObj.collisionDetectionMode = CollisionDetectionMode.Continuous;
        RigidbodyObj.AddForce(forceDirection.forward * throwForce, ForceMode.Impulse);
        #endregion

        CharacterAnimationController.Animator.enabled = false;        
        IsKillable = true;
        RagdollController.ActivateRagdoll();
        Vector3 dir = forceDirection.forward;
        dir.x = 0;
        RagdollController.ForceRagdoll(dir, PowerBar.ForceSpeed);
        UIController.Instance.powerBar.SetActive(false);
        FollowCamera.Instance.CameraFollow();

    }
}
