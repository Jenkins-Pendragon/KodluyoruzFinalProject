using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScorePlatform : MonoBehaviour
{
    public float multiplier;
    public Transform[] confettiPositions;

    public void OnCollisionEnter(Collision collision)
    {
        GoldenEnemyRagdoll enemyRagdoll = collision.gameObject.GetComponent<GoldenEnemyRagdoll>();
        if (enemyRagdoll != null)
        {
            for (int i = 0; i < confettiPositions.Length; i++)
            {
                Confetti.Instance.PlayConfetti(confettiPositions[i]);
            }
        }        
    }

}
