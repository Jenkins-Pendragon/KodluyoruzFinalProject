using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


namespace AICharacterController
{
    public class Enemy : InteractableBase
    {
        NavMeshAgent agent;
        private CharacterAnimationController characterAnimationController;
        public CharacterAnimationController CharacterAnimationController { get { return (characterAnimationController == null) ? characterAnimationController = GetComponent<CharacterAnimationController>() : characterAnimationController; } }

   


        protected override void Start()
        {
            base.Start();
            agent = GetComponent<NavMeshAgent>();

        }


        public override void OnInteractStart(Transform parent, Transform destination)
        {
            base.OnInteractStart(parent, destination);
            IsInteractable = false;
            //agent.isStopped = true;
            agent.enabled = false;
            CharacterAnimationController.Punch(false);
            CharacterAnimationController.Catch(true);
            


        }

        public override void OnInteractEnd(Transform forceDirection)
        {
            
            base.OnInteractEnd(forceDirection);
            CharacterAnimationController.Animator.enabled = false;
            IsInteractable = true;
            


        }






    }
}

