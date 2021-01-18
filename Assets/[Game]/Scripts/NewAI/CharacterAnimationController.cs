using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



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

    public void Taunt(bool state)
    {
        Animator.SetBool("Taunt", state);
    }

    public void Catch(bool state)
    {
        Animator.SetBool("Catch", state);
    }

    public void Run(bool state) 
    {
        Animator.SetBool("Run", state);
    }

    public void StopShooting() 
    {
        Shoot(false);
    }
}


