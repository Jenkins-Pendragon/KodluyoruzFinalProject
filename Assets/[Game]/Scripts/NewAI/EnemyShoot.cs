using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace AICharacterController
{
    public class EnemyShoot : MonoBehaviour
    {
        private CharacterAnimationController characterAnimationController;
        public CharacterAnimationController CharacterAnimationController { get { return (characterAnimationController == null) ? characterAnimationController = GetComponentInParent<CharacterAnimationController>() : characterAnimationController; } }


        [SerializeField] private GameObject Bullet;
        public Transform targetEnemy;
        private float waitTime = 5f;
        private float waitTime2;

        public float shootSpeed = 5f;
        private Vector3 bulletRotate = new Vector3(-90f, 0f, 0f);

        private void Start()
        {
            
                StartCoroutine(EnemyShooting());
            
            
        }







        IEnumerator EnemyShooting()
        {
            while (true)
            {

           
            
                Sequence shootMech = DOTween.Sequence();
                var bulletObj = GameObject.Instantiate(Bullet, transform.position, Quaternion.identity);
                bulletObj.transform.LookAt(targetEnemy);
                shootMech.Append(bulletObj.transform.DOLocalRotate(bulletRotate, 0));
                shootMech.Append(bulletObj.transform.DOMove(targetEnemy.position, shootSpeed));
                CharacterAnimationController.Animator.SetTrigger("Shoot");
                CharacterAnimationController.Animator.SetTrigger("TargetAim");
                
              



                

                yield return new WaitForSeconds(waitTime);




            }

        }

        

    }

}
