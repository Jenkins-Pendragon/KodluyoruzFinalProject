using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollCollision : MonoBehaviour
{
    private Enemy enemy;
    public Enemy Enemy { get { return (enemy == null) ? enemy = GetComponentInParent<Enemy>() : enemy; } }
    private void OnCollisionEnter(Collision other)
    {        
        Enemy.OnRagdollCollision(other);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!Enemy.IsDead && other.gameObject.CompareTag("Water"))
        {
            Enemy.Die();
        }
    }
}
