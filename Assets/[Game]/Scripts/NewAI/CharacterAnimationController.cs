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


        public void Shoot(bool state)
        {
            Animator.SetBool("Shoot", state);
        }

        private void Update()
        {
           // UpdateAnimations();
        }

        public void Punch(bool state)
        {
            Animator.SetBool("Punch", state);
        }

        public void Catch(bool state)
        {
            Animator.SetBool("Catch", state);
        }

        public void Run(bool state) 
        {
            Animator.SetBool("Run", state);
        }

      

        private void UpdateAnimations()
        {
            //Animator.enabled = false;
            
           Animator.SetFloat("Speed", CharacterBrain.GetCurrentSpeed());
           

        }
    }
}

