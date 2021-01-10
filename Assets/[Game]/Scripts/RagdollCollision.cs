using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollCollision : MonoBehaviour
{
    private Enemy2 enemy;
    public Enemy2 Enemy { get { return (enemy == null) ? enemy = GetComponentInParent<Enemy2>() : enemy; } }
    private void OnTriggerEnter(Collider other)
    {
        Enemy.OnRagdollCollision(other);
    }
}
