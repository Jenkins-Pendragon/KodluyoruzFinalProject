using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy2 : InteractableBase, IDamageable
{
    NavMeshAgent agent;
    public Animator enemyAnim;
    public Material deathMat;
    public SkinnedMeshRenderer skinnedMeshRenderer;    
    private Vector3 direction = Vector3.forward;
    private RagdollController ragdoll;
    public bool IsDead { get; protected set; }
    public bool IsRagdoll { get; set; }
    protected override void Start()
    {
        base.Start();
        agent = GetComponentInParent<NavMeshAgent>();
        //enemyAnim = GetComponentInChildren<Animator>();
        ragdoll = GetComponent<RagdollController>();
        ragdoll.DisableRagdoll();
    }

    public void Die()
    {
        if (!IsRagdoll)
        {
            ragdoll.ActivateRagdoll();
        }
        IsInteractable = false;
        IsKillable = false;
        enemyAnim.enabled = false;
        skinnedMeshRenderer.sharedMaterial = deathMat;
        agent.enabled = false;
    }
    

    public void Kill()
    {
        
    }

    public void OnRagdollCollision(Collision other) 
    {
        Debug.Log(other.gameObject.name);        

        if (IsKillable)
        {
            IExplodable explodable = other.gameObject.GetComponent<IExplodable>();
            IDamageable damageable = other.gameObject.GetComponent<IDamageable>();
            if (explodable!=null)
            {
                explodable.Explode();
            }

            if (damageable !=null)
            {
                damageable.Die();
            }

            Die();
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        IInteractable interactable = collision.gameObject.GetComponent<IInteractable>();
        if (interactable != null && interactable.IsKillable)
        {
            Die();
        }
    }

    public override void OnInteractStart(Transform parent, Transform destination)
    {
        base.OnInteractStart(parent, destination);
        IsInteractable = false;
        agent.enabled = false;
        enemyAnim.SetBool("Run", false);
        enemyAnim.SetBool("Punch", false);
        enemyAnim.SetBool("Catch", true);
    }

    public override void OnInteractEnd(Transform forceDirection)
    {        
        base.OnInteractEnd(forceDirection);
        direction = forceDirection.forward;
        IsInteractable = true;
        IsKillable = true;
        ragdoll.ActivateRagdoll();
        ragdoll.ForceRagdoll(direction);
    }

    
}
