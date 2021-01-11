using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public float lookRadius = 10f;

    Animator enemyAnim;
    Transform target;
    NavMeshAgent agent;
    public Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        enemyAnim = GetComponentInChildren<Animator>();        
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.enabled)
        {


            float distance = Vector3.Distance(target.position, transform.position);

            if (distance <= lookRadius)
            {
                agent.SetDestination(target.position);
                enemyAnim.SetBool("Run", true);


                if (distance <= agent.stoppingDistance)
                {
                    rb.isKinematic = true;
                    enemyAnim.SetBool("Run", false);
                    enemyAnim.SetBool("Punch", true);
                    FaceTarget();

                }
            }
        }
    }
    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
}
