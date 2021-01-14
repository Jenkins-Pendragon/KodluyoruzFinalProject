using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace AICharacterController
{
    public class AITargetBrain : CharacterBrainBase
    {
        public float lookRadius = 10f;
        private NavMeshAgent navMeshAgent;
        public NavMeshAgent NavMeshAgent { get { return (navMeshAgent == null) ? navMeshAgent = GetComponent<NavMeshAgent>() : navMeshAgent; } }

        private Rigidbody rigidbody;
        public Rigidbody Rigidbody { get { return (rigidbody == null) ? rigidbody = GetComponent<Rigidbody>() : rigidbody; } }
        public Transform targetPlayer;

        private CharacterAnimationController characterAnimationController;
        public CharacterAnimationController CharacterAnimationController { get { return (characterAnimationController == null) ? characterAnimationController = GetComponent<CharacterAnimationController>() : characterAnimationController; } }

        


        private void Start()
        {
            Initialize();
        }

        public override void Initialize()
        {
            base.Initialize();
        }


        public override void Logic()
        {
         

            
            float distance = Vector3.Distance(targetPlayer.position, transform.position);


            if(distance < lookRadius)
            {
                if (NavMeshAgent == null || NavMeshAgent.enabled == false)
                    return;

                CharacterAnimationController.Run(true);
                NavMeshAgent.SetDestination(targetPlayer.position);

                if (distance < NavMeshAgent.stoppingDistance)
                {
                    CharacterAnimationController.Punch(true);
                    
                }
            }

        }

        public override float GetCurrentSpeed()
        {
            return Rigidbody.velocity.magnitude;
        }



        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, lookRadius);
        }
    }
}

