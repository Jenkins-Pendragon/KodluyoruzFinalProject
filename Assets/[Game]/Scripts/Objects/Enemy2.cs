using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy2 : InteractableBase, IDamageable
{
    NavMeshAgent agent;
    Animator enemyAnim;

    protected override void Start()
    {
        base.Start();
        agent = GetComponent<NavMeshAgent>();
        enemyAnim = GetComponentInChildren<Animator>();
    }

    public void Die()
    {
        IsInteractable = false;
        enemyAnim.enabled = false;
    }

    public void Kill()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Hitttt");
        IInteractable interactable = collision.gameObject.GetComponent<IInteractable>();
        if (interactable != null && interactable.IsKillable)
        {
            Die();
        }
        if (IsKillable)
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
        IsInteractable = true;
        IsKillable = true;
    }

    
}
