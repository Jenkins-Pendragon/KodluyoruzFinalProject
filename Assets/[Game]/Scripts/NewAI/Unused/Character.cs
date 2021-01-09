using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AICharacterController
{
    /// <summary>
    /// Responsible for Communication
    /// Character State
    /// </summary>
    public class Character : MonoBehaviour
    {
   

        #region Events
        [HideInInspector]
        public UnityEvent OnCharacterRevive = new UnityEvent();
        [HideInInspector]
        public UnityEvent OnCharacterDie = new UnityEvent();
        [HideInInspector]
        public UnityEvent OnCharacterJump = new UnityEvent();
        #endregion

        private bool isControlable = true;
        public bool IsControlable { get { return isControlable; } set { isControlable = value; } }

        private bool isDead;
        public bool IsDead { get { return isDead; } set { isDead = value; } }
    }
}

