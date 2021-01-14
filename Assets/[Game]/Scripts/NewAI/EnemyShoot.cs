using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using AICharacterController;


public class EnemyShoot : MonoBehaviour, IShootable
{
    private CharacterAnimationController characterAnimationController;
    public CharacterAnimationController AnimationController { get { return (characterAnimationController == null) ? characterAnimationController = GetComponentInParent<CharacterAnimationController>() : characterAnimationController; } }

    public bool IsCanFire { get; set;}

    [SerializeField] private GameObject Bullet;
    public Transform targetEnemy;
    public Transform gundEndPoint;
    private float waitTime = 5f;   

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
            if (IsCanFire)
            {
                //Sequence shootMech = DOTween.Sequence();
                var bulletObj = GameObject.Instantiate(Bullet, gundEndPoint.position, Quaternion.identity);
                bulletObj.transform.LookAt(targetEnemy);
                //shootMech.Append(bulletObj.transform.DOLocalRotate(bulletRotate, 0));
                //shootMech.Append(bulletObj.transform.DOMove(targetEnemy.position, shootSpeed));
                bulletObj.transform.DOLocalRotate(bulletRotate, 0);
                bulletObj.transform.DOMove(targetEnemy.position, shootSpeed);

                //CharacterAnimationController.Animator.SetTrigger("Shoot");
                //CharacterAnimationController.Animator.SetTrigger("TargetAim");
                AnimationController.Shoot(true);
                yield return new WaitForSeconds(waitTime);
            }  
        }
    }

}


