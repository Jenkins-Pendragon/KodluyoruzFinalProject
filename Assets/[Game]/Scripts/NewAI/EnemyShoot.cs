using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    [SerializeField] private Transform Bullet;


    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            EnemyShooting();
        }
    }



    private void EnemyShooting()
    {
        Instantiate(Bullet, transform.position, Quaternion.identity);
    }

}
