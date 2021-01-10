using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy2 : InteractableBase, IDamageable
{
    NavMeshAgent agent;
    Animator enemyAnim;
    public Material deathMat;
    public SkinnedMeshRenderer skinnedMeshRenderer;
    private Collider[] colliderList;
    private Rigidbody[] rigidbodyList;
    private Collider mainCollider;
    private Vector3 direction = Vector3.forward;
    protected override void Start()
    {
        base.Start();
        agent = GetComponent<NavMeshAgent>();
        enemyAnim = GetComponentInChildren<Animator>();
        colliderList = GetComponentsInChildren<Collider>(true);
        rigidbodyList = GetComponentsInChildren<Rigidbody>();
        mainCollider = GetComponent<Collider>();
        SetRigidbody(true);
        //SetCollider(false);
        //RagDoll(false);
    }

    public void Die()
    {
        IsInteractable = false;
        IsKillable = false;
        enemyAnim.enabled = false;
        skinnedMeshRenderer.sharedMaterial = deathMat;
        SetRigidbody(false);
        //SetCollider(true);
        /*
        foreach (var item in rigidbodyList)
        {
            item.AddForce(direction * 5f, ForceMode.Impulse);
            //item.velocity = velocity/12f;
        }
        */
        //RagDoll(true);
    }

    private void SetRigidbody(bool state) 
    {
        foreach (var item in rigidbodyList)
        {
            item.isKinematic = state;            
            item.AddForce(direction * 50f, ForceMode.Impulse);
        }
        RigidbodyObj.isKinematic = !state;
    }

    private void SetCollider(bool state)
    {
        foreach (var item in colliderList)
        {
            item.enabled = state;
        }
        mainCollider.enabled = !state;
    }

    public void Kill()
    {
        
    }

    private void RagDoll(bool isRagdoll) 
    {
        foreach (var item in colliderList)
            item.enabled = !isRagdoll;
        mainCollider.enabled = !isRagdoll;
        enemyAnim.enabled = !isRagdoll;

    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        IInteractable interactable = other.gameObject.GetComponent<IInteractable>();
        if (interactable != null && interactable.IsKillable)
        {
            Die();
        }
        if (IsKillable)
        {
            Die();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log(collision.gameObject.name);
        IInteractable interactable = collision.gameObject.GetComponent<IInteractable>();
        if (interactable != null && interactable.IsKillable)
        {
            //Die();
        }
        if (IsKillable)
        {
            //Die();
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
        
    }

    
}
