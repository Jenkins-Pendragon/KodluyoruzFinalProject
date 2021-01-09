//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//namespace AICharacterController
//{
//    public abstract class CharacterControllerBase : MonoBehaviour, ICharacterController
//    {
//        private Character character;

//        protected Character Character { get { return (character == null) ? character = GetComponent<Character>() : character; } }


//        public virtual void Dispose()
//        {
//            Debug.Log("Controller Disposed " + gameObject.GetType());
//        }

//        public virtual void Initialize()
//        {
//            Debug.Log("Controller Intialized " + gameObject.GetType());
//        }

//        public abstract void Move(Vector3 direction);

//        public virtual void Rotate(Vector3 direction)
//        {
//            if (direction == Vector3.zero)
//            {
//                return;
//            }

//            float distanceToTargetDir = Vector3.Distance(transform.TransformPoint(Vector3.forward), direction);
//            direction = Camera.main.transform.TransformDirection(direction);

//            direction.y = 0;
//            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), (3 * Mathf.Clamp(distanceToTargetDir, 0f, 2f)) * Time.deltaTime); ;
//        }

//        public abstract float GetCurrentSpeed();
//    }
//}

