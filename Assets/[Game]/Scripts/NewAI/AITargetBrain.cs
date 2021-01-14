using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace AICharacterController
{
    public class AITargetBrain : MonoBehaviour, ICharacterBrain
    {
        public float lookRadius = 10f;
        private NavMeshAgent navMeshAgent;
        public NavMeshAgent NavMeshAgent { get { return (navMeshAgent == null) ? navMeshAgent = GetComponent<NavMeshAgent>() : navMeshAgent; } }

        private Rigidbody rigidbody;
        public Rigidbody Rigidbody { get { return (rigidbody == null) ? rigidbody = GetComponent<Rigidbody>() : rigidbody; } }
        public Transform targetPlayer;

        private CharacterAnimationController characterAnimationController;
        public CharacterAnimationController CharacterAnimationController { get { return (characterAnimationController == null) ? characterAnimationController = GetComponent<CharacterAnimationController>() : characterAnimationController; } }

        
        public void Logic()
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

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, lookRadius);
        }
    }
}

