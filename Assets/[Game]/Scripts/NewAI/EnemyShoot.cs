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
        public float waitTime;

        public float shootSpeed = 5f;
        private Vector3 bulletRotate = new Vector3(-90f, 0f, 0f);

        private void Update()
        {
            if(Time.time > waitTime)
            StartCoroutine(EnemyShooting());
        }



        IEnumerator EnemyShooting()
        {

            Sequence shootMech = DOTween.Sequence();
            
            var bulletObj = GameObject.Instantiate(Bullet, transform.position, Quaternion.identity);
            shootMech.Append(bulletObj.transform.DOLocalRotate(bulletRotate, 0));
            shootMech.Append(bulletObj.transform.DOMove(targetEnemy.position, shootSpeed));
           
            
            
            CharacterAnimationController.Shoot(true);
            CharacterAnimationController.Shoot(false);

            waitTime += 5;
            yield return new WaitForSeconds(waitTime);
        }

    }

}
