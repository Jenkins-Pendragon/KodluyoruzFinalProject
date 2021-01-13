using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Cannon : MonoBehaviour, IShootable
{    
    public Ease myEase;
    public bool IsCanFire { get; set;}
    public GameObject cannonBall;
    public Transform firePoint;

    private void Start()
    {
        StartCoroutine(StartFire());
    }

    public IEnumerator StartFire()
    {
        while (true)
        {
            if (IsCanFire)
            {
                firePoint.DOLookAt(PlayerTransfomStreamer.Instance.transform.position, 0.5f).OnComplete(() =>
                {
                    GameObject clone = Instantiate(cannonBall, firePoint.position, Quaternion.identity);
                    clone.transform.DOJump(PlayerTransfomStreamer.Instance.transform.position, 6, 1, 5f).SetEase(myEase);
                });
            }            
            yield return new WaitForSeconds(5f);           
        }        
    }       
}
