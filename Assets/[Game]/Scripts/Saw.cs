using UnityEngine;
using EzySlice;
using DG.Tweening;

public class Saw : Prop
{
    public Material material;
    public LayerMask mask;
    private float propMass = 4f;    

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.gameObject.layer);
        //Debug.Log(mask.value);
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
                    Debug.LogWarning("Kesme Hatası Chibi");
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
                Slice(slicedHull, other.gameObject, true);                
            }
        }        
    }    

    private void Slice(SlicedHull slicedHull, GameObject obj, bool interactable = false) 
    {
        GameObject upperHull = slicedHull.CreateUpperHull(obj, material);
        GameObject lowerHull = slicedHull.CreateLowerHull(obj, material);
        AddComponents(upperHull, interactable);
        AddComponents(lowerHull, interactable);
        Destroy(obj);
    }

    private SlicedHull Slice(GameObject obj, Material material)
    {
        return obj.Slice(transform.position, transform.up, material);
    }

    private void AddComponents(GameObject obj, bool interactable)
    {
        obj.AddComponent<MeshCollider>().convex = true;
        //obj.AddComponent<BoxCollider>();
        Rigidbody rb = obj.AddComponent<Rigidbody>();
        rb.mass = propMass;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.AddExplosionForce(300, obj.transform.position, 20);
        //obj.layer = 10;
        obj.layer = 12;

        if (interactable)
        {
            obj.AddComponent<Prop>();
        }
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
