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

        }

        public virtual void Initialize()
        {

        }

        public abstract void Logic();

        public abstract float GetCurrentSpeed();
 
    }
}

