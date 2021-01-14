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
    public Transform enemyBody;
    private float waitTime = 5f;   
    public float shootSpeed = 5f;   

    private void Start()
    {
        StartCoroutine(EnemyShooting());
        
    }

    private void Update()
    {
        enemyBody.LookAt(targetEnemy);
    }

    IEnumerator EnemyShooting()
    {
        while (true)
        {
            if (IsCanFire)
            {                
                var bulletObj = GameObject.Instantiate(Bullet, gundEndPoint.position, Quaternion.identity);
                bulletObj.transform.LookAt(targetEnemy);
                bulletObj.transform.DOMove(targetEnemy.position, shootSpeed);                
                AnimationController.Shoot(true);
            }
            yield return new WaitForSeconds(waitTime);
        }
    }

}


