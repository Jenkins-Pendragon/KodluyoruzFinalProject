﻿using UnityEngine;
using EzySlice;
using DG.Tweening;

public class Saw : Prop
{
    public Material material;
    public LayerMask mask;
    private Collider col;
    public Collider Collider { get { return (col == null) ? col = GetComponentInChildren<Collider>() : col; } }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.layer);
        Debug.Log(mask.value);
        if (!IsKillable) return;

        if (other.gameObject.layer == 10)
        {            
            SliceChibi sliceChibi = other.GetComponent<SliceChibi>();
            
            if (sliceChibi != null)
            {
                GameObject obj = sliceChibi.Slice();   
                SlicedHull slicedHull = Slice(obj.GetComponent<Collider>().gameObject, material);
                if (slicedHull == null)
                {                    
                    Destroy(obj.gameObject);
                    other.gameObject.SetActive(true);
                    Debug.Log("Kesme Hatası Chibi");
                }
                else
                {
                    Slice(slicedHull, obj);
                }
                IDamageable damageable = other.GetComponent<IDamageable>();
                if (damageable != null) damageable.Die();
            }
            else
            {
                SlicedHull slicedHull = Slice(other.GetComponent<Collider>().gameObject, material);
                if (slicedHull == null) return;
                Slice(slicedHull, other.gameObject);                
            }
        }        
    }    

    private void Slice(SlicedHull slicedHull, GameObject obj) 
    {
        GameObject upperHull = slicedHull.CreateUpperHull(obj, material);
        GameObject lowerHull = slicedHull.CreateLowerHull(obj, material);
        AddComponents(upperHull);
        AddComponents(lowerHull);
        Destroy(obj);
    }

    private SlicedHull Slice(GameObject obj, Material material)
    {
        return obj.Slice(transform.position, transform.up, material);
    }

    private void AddComponents(GameObject obj)
    {
        obj.AddComponent<MeshCollider>().convex = true;
        Rigidbody rb = obj.AddComponent<Rigidbody>();
        rb.mass = 10f;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.AddExplosionForce(100, obj.transform.position, 5);
    }

       

    public override void OnInteractStart(Transform parent, Transform destination)
    {
        Collider.isTrigger = false;
        transform.DOKill();
        base.OnInteractStart(parent, destination);        
    }

    public override void OnInteractEnd(Transform forceDirection)
    {
        Collider.isTrigger = true;
        base.OnInteractEnd(forceDirection);
        IsInteractable = true;
        IsKillable = true;        
    }

}
