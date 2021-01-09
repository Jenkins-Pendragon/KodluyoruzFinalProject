using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AICharacterController
{
    public class CharacterInputController : MonoBehaviour
    {
        ICharacterBrain characterBrain;
        ICharacterBrain CharacterBrain { get { return (characterBrain == null) ? characterBrain = GetComponent<ICharacterBrain>() : characterBrain; } }


        private void FixedUpdate()
        {
            CharacterBrain.Logic();
        }
    }
}

