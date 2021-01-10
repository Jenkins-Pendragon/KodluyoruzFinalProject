using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Cannon : MonoBehaviour
{
    public bool canFire;
    public Ease myEase;
    [SerializeField] GameObject cannonBall;
    public IEnumerator StartFire()
    {
        while (canFire)
        {
            yield return new WaitForSeconds(2f);
            Debug.Log("fire!");
            transform.DOLookAt(PlayerTransfomStreamer.Instance.transform.position, 0.5f).OnComplete(() =>
            {
                GameObject clone = Instantiate(cannonBall, transform.position, Quaternion.identity);
                clone.transform.DOJump(PlayerTransfomStreamer.Instance.transform.position+Vector3.forward * 5, 6, 1, 1.5f).SetEase(myEase);
            });
        }
    }
}
