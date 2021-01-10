using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AICharacterController
{
    public class CharacterAnimationController : MonoBehaviour
    {

        ICharacterBrain characterBrain;
        ICharacterBrain CharacterBrain { get { return (characterBrain == null) ? characterBrain = GetComponentInParent<ICharacterBrain>() : characterBrain; } }

        private Animator animator;

        public Animator Animator { get { return (animator == null) ? animator = GetComponent<Animator>() : animator; } }



        private void Update()
        {
            UpdateAnimations();
        }

        public void Punch()
        {
            Animator.SetBool("Punch", true);
        }

        public void noPunch()
        {
            Animator.SetBool("Punch", false);
        }

        public void noRun()
        {
            Animator.SetFloat("Speed", 0);
        }

        public void Catch()
        {
            Animator.SetBool("Catch", true);
        }

      

        private void UpdateAnimations()
        {
            //Animator.enabled = false;
            
           Animator.SetFloat("Speed", CharacterBrain.GetCurrentSpeed());
           

        }
    }
}

