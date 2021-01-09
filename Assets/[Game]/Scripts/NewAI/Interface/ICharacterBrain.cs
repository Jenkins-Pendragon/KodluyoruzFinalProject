using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AICharacterController
{
    public interface ICharacterBrain
    {
        float GetCurrentSpeed();

        void Initialize();
        void Logic();
        void Dispose();
    }
}

