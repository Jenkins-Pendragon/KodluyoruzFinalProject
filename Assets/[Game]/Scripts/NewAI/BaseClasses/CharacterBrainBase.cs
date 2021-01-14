using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AICharacterController
{
    public abstract class CharacterBrainBase : MonoBehaviour, ICharacterBrain
    {    
        public abstract void Logic();
    }
}

