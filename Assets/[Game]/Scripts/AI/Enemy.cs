using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : InteractableBase, Damageable 
{
    NavMeshAgent agent;
    Animator enemyAnim;
    public override void OnStart()
    {
        base.OnStart();
        agent = GetComponent<NavMeshAgent>();
        enemyAnim = GetComponent<Animator>();

    }

    public void Damage(Animator anim)
    {
        
       
        anim.SetBool("Punching", true);
        // punch animation and kill character
    }

   

    public void Dead()
    {
        
    }

    public override void OnInteractStart(Transform parent, Transform destination)
    {
        base.OnInteractStart(parent, destination);
        IsInteractable = false;
        agent.enabled = false;
        enemyAnim.SetBool("Running", false);
        enemyAnim.SetBool("Punching", false);
        enemyAnim.SetBool("Catching", true);


    }

    public override void OnInteractEnd(Transform forceDirection)
    {
        enemyAnim.enabled = false;
        base.OnInteractEnd(forceDirection);
        IsInteractable = true;
        

    }






}
