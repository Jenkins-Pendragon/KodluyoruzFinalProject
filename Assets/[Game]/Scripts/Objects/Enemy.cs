using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using AICharacterController;


public class Enemy : InteractableBase, IDamageable
{
    #region Properties
    private CharacterAnimationController characterAnimationController;
    public CharacterAnimationController CharacterAnimationController { get { return (characterAnimationController == null) ? characterAnimationController = GetComponent<CharacterAnimationController>() : characterAnimationController; } }
    private NavMeshAgent navMeshAgent;
    public NavMeshAgent NavMeshAgent { get { return (navMeshAgent == null) ? navMeshAgent = GetComponent<NavMeshAgent>() : navMeshAgent; } }
    private RagdollController ragdollController;
    public RagdollController RagdollController { get { return (ragdollController == null) ? ragdollController = GetComponent<RagdollController>() : ragdollController; } }
    #endregion


    public Material deathMat;
    public SkinnedMeshRenderer skinnedMeshRenderer; 
    public Collider ragdollCollider;
    public bool IsDead { get; protected set; }
    public bool IsRagdoll { get; set; }
    protected override void Start()
    {
        base.Start();
        RagdollController.DisableRagdoll();
    }

    public void Die()
    {
        if (!IsRagdoll)
        {
            RagdollController.ActivateRagdoll();
        }
        IsDead = true;
        IsInteractable = false;
        IsKillable = false;            
        skinnedMeshRenderer.sharedMaterial = deathMat;
        NavMeshAgent.enabled = false;
        ragdollCollider.enabled = false;

        EventManager.OnEnemyDie.Invoke();

    }

    public void OnRagdollCollision(Collision other)
    {
        if (IsKillable && !IsDead)
        {
            IExplodable explodable = other.gameObject.GetComponent<IExplodable>();
            IDamageable damageable = other.gameObject.GetComponent<IDamageable>();
            if (explodable != null)
            {
                explodable.Explode();
            }

            if (damageable != null)
            {
                damageable.Die();
            }

            Die();
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (!IsDead)
        {
            IInteractable interactable = collision.gameObject.GetComponent<IInteractable>();
            if (interactable != null && interactable.IsKillable)
            {
                Die();
            }
        }
    }

    public override void OnInteractStart(Transform parent, Transform destination)
    {
        base.OnInteractStart(parent, destination);
        IsInteractable = false;
        NavMeshAgent.enabled = false;
        CharacterAnimationController.Punch(false);
        CharacterAnimationController.Catch(true); ;
    }

    public override void OnInteractEnd(Transform forceDirection)
    {
        base.OnInteractEnd(forceDirection);
        CharacterAnimationController.Animator.enabled = false;        
        IsInteractable = true;
        IsKillable = true;
        RagdollController.ActivateRagdoll();
        RagdollController.ForceRagdoll(forceDirection.forward);
    }


}

