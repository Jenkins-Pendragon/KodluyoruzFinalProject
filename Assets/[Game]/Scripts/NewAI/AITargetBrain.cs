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
        

        private CharacterAnimationController characterAnimationController;
        public CharacterAnimationController CharacterAnimationController { get { return (characterAnimationController == null) ? characterAnimationController = GetComponent<CharacterAnimationController>() : characterAnimationController; } }

        
        public void Logic()
        {            
            float distance = Vector3.Distance(PlayerData.Instance.transform.position, transform.position);
            if(distance < lookRadius)
            {
                if (NavMeshAgent == null || NavMeshAgent.enabled == false)
                    return;
                CharacterAnimationController.Run(true);
                NavMeshAgent.SetDestination(PlayerData.Instance.transform.position);

                if (distance < NavMeshAgent.stoppingDistance)
                {
                    NavMeshAgent.enabled = false;
                    CharacterAnimationController.Punch(true);
                    if(!PlayerData.Instance.IsPlayerDead) EventManager.OnLevelFailed.Invoke();
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

