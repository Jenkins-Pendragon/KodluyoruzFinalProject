using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollController : MonoBehaviour
{
    public List<Rigidbody> rigidbodies;
    public List<Collider> colliders;
    public Rigidbody mainRigidbody;
    public Collider mainCollider;

    
    public Animator AnimatorRagdoll;
    private Enemy enemy;
    public Enemy EnemyScript { get { return (enemy == null) ? enemy = GetComponentInChildren<Enemy>() : enemy; } }
   
    
    public void ActivateRagdoll() 
    {
        AnimatorRagdoll.enabled = false;
        SetRigidbodies(false);
        SetColliders(true);
        EnemyScript.IsRagdoll = true;        
    }

    public void DisableRagdoll() 
    {
        AnimatorRagdoll.enabled = true;
        SetRigidbodies(true);
        SetColliders(false);
        EnemyScript.IsRagdoll = false;
    }

    public void ForceRagdoll(Vector3 direction) 
    {
        foreach (Rigidbody rigidbody in rigidbodies)
        {
            rigidbody.AddForce(direction * 25f, ForceMode.Impulse);
        }
    }

    private void SetRigidbodies(bool state)
    {
        foreach (Rigidbody rigidbody in rigidbodies)
        {
            rigidbody.isKinematic = state;            
        }
        if (!state)
        {
            mainRigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
        }
        else
        {
            mainRigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
        }
        mainRigidbody.isKinematic = !state;

    }

    private void SetColliders(bool state)
    {
        foreach (var item in colliders)
        {
            item.enabled = state;
        }
        mainCollider.enabled = !state;
    }
}
