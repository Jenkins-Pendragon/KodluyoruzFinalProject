using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AICharacterController
{
    public class CharacterAnimationController : MonoBehaviour
    {   
        private Animator animator;

        public Animator Animator { get { return (animator == null) ? animator = GetComponent<Animator>() : animator; } }


        public void Shoot(bool state)
        {
            Animator.SetBool("Shoot", state);
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

    }
}

