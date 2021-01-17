using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldenEnemyRagdoll : MonoBehaviour
{
    #region Properties
    private bool IsCheckCollison;
    private ScorePlatform lastScorePlatform = null;

    private GoldenEnemy enemy;
    public GoldenEnemy GoldenEnemy { get { return (enemy == null) ? enemy = GetComponentInParent<GoldenEnemy>() : enemy; } }

    private Rigidbody rb;
    public Rigidbody Rigidbody { get { return (rb == null) ? rb = GetComponent<Rigidbody>() : rb; } }
    #endregion
        
    private void OnEnable()
    {
        EventManager.OnGoldenEnemyDie.AddListener(() => IsCheckCollison = true);
    }

    private void OnDisable()
    {
        EventManager.OnGoldenEnemyDie.RemoveListener(() => IsCheckCollison = true);
    }

    private void OnCollisionEnter(Collision other)
    {
        GoldenEnemy.OnRagdollCollision(other);

        ScorePlatform scorePlatform = other.gameObject.GetComponent<ScorePlatform>();
        if (scorePlatform != null)
        {
            lastScorePlatform = scorePlatform;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (GoldenEnemy.IsDead && other.gameObject.CompareTag("Water"))
        {
            if(IsCheckCollison) CalculateScoreMultiplier();
        }
    }
    private void Update()
    {
        if (IsCheckCollison)
        {            
            float velocity = Rigidbody.velocity.z;
            if (velocity <= 0.1)
            {
                CalculateScoreMultiplier();
            }
        }
    }    

    private void CalculateScoreMultiplier() 
    {
        IsCheckCollison = false;
        FollowCamera.Instance.enabled = false;
        if (lastScorePlatform != null)
        {
            Announcer.Instance.multiplier = lastScorePlatform.multiplier;            
        }
        else
        {
            Announcer.Instance.multiplier = 1f;
        }
        Debug.Log("X:" + Announcer.Instance.multiplier);
        EventManager.OnCalculateScoreMultiplier.Invoke();
    }
}
