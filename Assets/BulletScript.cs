using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BulletScript : MonoBehaviour
{
    GameObject bulletObj;
    [SerializeField]
    public Transform targetEnemy;
    private float waitTime = 5f;
    public float shootSpeed = 5f;
    private Vector3 bulletRotate = new Vector3(-90f, 0f, 0f);
    bool isSpawned = false;
    Prop prop;
    // Start is called before the first frame update
    void Start()
    {
        targetEnemy = GameObject.FindGameObjectWithTag("Player").transform;
        isSpawned = true;
        bulletObj = this.gameObject;
        BulletDT();
    }

    // Update is called once per frame
  

    void BulletDT()
    {
      
            Sequence shootMech = DOTween.Sequence();
            bulletObj.transform.LookAt(targetEnemy);
            shootMech.Append(bulletObj.transform.DOLocalRotate(bulletRotate, 0));
            shootMech.Append(bulletObj.transform.DOMove(targetEnemy.position, shootSpeed));

        
          


            
        


    }

    private void Update()
    {
       // if (Prop.isInteract == true)
        //{
         //   transform.DOKill();
       // }
    }
}
