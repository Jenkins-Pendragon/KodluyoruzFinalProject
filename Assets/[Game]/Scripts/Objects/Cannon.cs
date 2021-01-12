using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Cannon : MonoBehaviour, IShootable
{    
    public Ease myEase;
    public bool IsCanFire { get; set;}
    [SerializeField] GameObject cannonBall;

    
    public IEnumerator StartFire()
    {
        while (IsCanFire)
        {            
            transform.DOLookAt(PlayerTransfomStreamer.Instance.transform.position, 0.5f).OnComplete(() =>
            {
                GameObject clone = Instantiate(cannonBall, transform.position, Quaternion.identity);
                clone.transform.DOJump(PlayerTransfomStreamer.Instance.transform.position+Vector3.forward * 5, 6, 1, 1.5f).SetEase(myEase);
            });
            yield return new WaitForSeconds(2f);           
        }

        StopAllCoroutines();
    }

    public void Shoot()
    {
        StartCoroutine(StartFire());
    }    
}
