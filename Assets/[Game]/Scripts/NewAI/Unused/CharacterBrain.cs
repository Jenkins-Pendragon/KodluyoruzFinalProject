using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AICharacterController
{
    public class CharacterBrain : CharacterBrainBase
    {
        public override void Logic()
        {
           // CharacterController.Move(new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")));
        }
    }
}

