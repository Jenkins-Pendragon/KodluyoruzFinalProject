using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FinalCamFollow : MonoBehaviour
{
    [SerializeField] GameObject target;
    void Start()
    {
        Tweener tweener = transform.DOMove(target.transform.position, 10).SetSpeedBased();
        tweener.OnUpdate(() => tweener.ChangeEndValue(target.transform.position + Vector3.back*5 +Vector3.up*2, true));
    }

}
