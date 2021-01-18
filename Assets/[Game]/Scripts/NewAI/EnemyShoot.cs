using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using AICharacterController;


public class EnemyShoot : Enemy, IShootable
{   
    public bool IsCanFire { get; set;}

    [SerializeField] private GameObject Bullet;    
    public Transform gundEndPoint;
    public Transform enemyBody;
    public Transform enemyRotation;
    private float waitTime = 5f;   
    public float shootSpeed = 5f;

    protected override void Start()
    {
        base.Start();
        StartCoroutine(EnemyShooting());
        enemyBody.LookAt(PlayerData.Instance.transform);
        canRun = false;
    }    

    IEnumerator EnemyShooting()
    {
        while (true)
        {
            if (IsCanFire)
            {
                CharacterAnimationController.Shoot(true);
                yield return new WaitForSeconds(waitTime);
            }
            yield return null;
        }
    }

    public override void OnCollisionEnter(Collision collision)
    {
        if (!onGround && collision.gameObject.CompareTag("Ground"))
        {
            if (NavMeshAgent != null && !NavMeshAgent.enabled && canRun && !IsCanFire)
            {
                ResetRotation();
                NavMeshAgent.enabled = true;
            }
        }
        base.OnCollisionEnter(collision);
        
    }

    public override void OnInteractStart(Transform parent, Transform destination)
    {
        base.OnInteractStart(parent, destination);
        CharacterAnimationController.Shoot(false);
    }

    public void ShootBullet() 
    {        
        var bulletObj = Instantiate(Bullet, gundEndPoint.position, Quaternion.identity);
        bulletObj.transform.LookAt(PlayerData.Instance.transform);
        bulletObj.transform.DOMove(PlayerData.Instance.transform.position, shootSpeed);         
    }

    public void ResetRotation() 
    {
        enemyRotation.transform.localEulerAngles = new Vector3(0, 180, 0);
        enemyBody.transform.localEulerAngles = new Vector3(0, 0, 0);
    }

}


