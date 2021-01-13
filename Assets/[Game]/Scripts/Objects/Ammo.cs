using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Ammo : Prop
    
{
    private void OnCollisionEnter(Collision collision)
    {        
        if (IsInteractable)
        {
            transform.DOKill();
            RigidbodyObj.useGravity = true;
        }
    }  
}
