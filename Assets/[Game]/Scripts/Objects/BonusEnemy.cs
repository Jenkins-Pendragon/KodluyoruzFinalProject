using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusEnemy : Enemy
{   
    protected override void Start()
    {
        base.Start();
        IsInteractable = false;
    }
    

    public override void Die()
    {        
        if (!IsRagdoll)
        {
            RagdollController.ActivateRagdoll();
        }
        IsDead = true;
        IsInteractable = false;
        IsKillable = false;
        if (NavMeshAgent != null) NavMeshAgent.enabled = false;
        skinnedMeshRenderer.sharedMaterial = deathMat;
        //ragdollCollider.enabled = false;

        EventManager.OnEnemyDie.Invoke();        
    }
}
