using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace AICharacterController
{
    public class AIPetrolBrain : CharacterBrainBase
    {
        public float lookRadius = 10f;
        private NavMeshAgent navMeshAgent;
        public NavMeshAgent NavMeshAgent { get { return (navMeshAgent == null) ? navMeshAgent = GetComponent<NavMeshAgent>() : navMeshAgent; } }

        private Rigidbody rigidbody;
        public Rigidbody Rigidbody { get { return (rigidbody == null) ? rigidbody = GetComponent<Rigidbody>() : rigidbody; } }
        public Transform targetPlayer;
        

        private void Start()
        {
            Initialize();
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        private void FixedUpdate()
        {
            Debug.Log(Rigidbody.velocity.magnitude);
        }

        public override void Logic()
        {
         

            
            float distance = Vector3.Distance(targetPlayer.position, transform.position);


            if(distance < lookRadius)
            {
                NavMeshAgent.SetDestination(targetPlayer.position);
            }

        }

        public override float GetCurrentSpeed(float magnitude)
        {
            return base.GetCurrentSpeed(Rigidbody.velocity.magnitude);
        }



        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, lookRadius);
        }
    }
}

