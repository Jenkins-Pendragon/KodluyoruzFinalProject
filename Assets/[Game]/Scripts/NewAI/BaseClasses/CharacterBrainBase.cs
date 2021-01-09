using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AICharacterController
{
    public abstract class CharacterBrainBase : MonoBehaviour, ICharacterBrain
    {
        // private ICharacterController characterController;
       // public ICharacterController CharacterController { get { return (characterController == null) ? characterController = GetComponent<ICharacterController>() : characterController; } }

        public virtual void Dispose()
        {
            Debug.Log("Brain Disposed " + gameObject.GetType());
        }

        public virtual void Initialize()
        {
            Debug.Log("Brain Intialized " + gameObject.GetType());
        }

        public abstract void Logic();

        public virtual float GetCurrentSpeed(float magnitude)
        {
            return magnitude;
        }

        public float GetCurrentSpeed()
        {
            throw new System.NotImplementedException();
        }
    }
}

