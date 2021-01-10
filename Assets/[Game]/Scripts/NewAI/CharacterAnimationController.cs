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

        Animator animator;

        Animator Animator { get { return (animator == null) ? animator = GetComponent<Animator>() : animator; } }

        private void Update()
        {
            UpdateAnimations();
        }

        private void UpdateAnimations()
        {
            //Animator.enabled = false;
            Debug.Log("updateanimations");
           Animator.SetFloat("Speed", CharacterBrain.GetCurrentSpeed());
           

        }
    }
}

