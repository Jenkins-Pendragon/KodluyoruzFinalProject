//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;


//namespace AICharacterController
//{
//    public class CharacterController : CharacterControllerBase
//    {

//        private Rigidbody rigidbody;
//        public Rigidbody Rigidbody { get { return (rigidbody == null) ? rigidbody = GetComponent<Rigidbody>() : rigidbody; } }

//        public override void Move(Vector3 direction)
//        {
//            if (!Character.IsControlable)
//                return;
//            direction.Normalize();
//            Rotate(direction);
//            if (Rigidbody.velocity.magnitude > Character.CharacterData.MovementData.MaxSpeed)
//                return;

//            Rigidbody.AddForce(direction * Character.CharacterData.MovementData.MovmeentForce * Time.fixedDeltaTime, ForceMode.VelocityChange);
//        }


//        public override float GetCurrentSpeed()
//        {
//            return Rigidbody.velocity.magnitude;
//        }


//    }
//}