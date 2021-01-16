using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(OutlineShader))]
public abstract class InteractableBase : MonoBehaviour, IInteractable
{
    public bool IsInteractable { get; set; }
    public bool IsKillable { get; set; }
    private Rigidbody rb;
    public Rigidbody RigidbodyObj { get { return (rb == null) ? rb = GetComponentInChildren<Rigidbody>() : rb; } }
    private Collider col;
    public Collider Collider { get { return (col == null) ? col = GetComponentInChildren<Collider>() : col; } }
    private OutlineShader outline;
    public OutlineShader Outline { get { return (outline == null) ? outline = GetComponentInChildren<OutlineShader>() : outline; } }

    protected readonly float throwForce = 200f;
    private readonly float tweenDelay = 0.3f;    
    private Vector3 offSet;
    protected virtual void Start()
    {
        IsInteractable = true;
        IsKillable = false;
        Outline.enabled = false;
        Outline.Initiliaze(Color.yellow, 8f, OutlineShader.Mode.OutlineVisible);
    }    

    protected virtual void OnDisable() 
    {
        transform.DOKill();
    }

    public virtual void OnInteractStart(Transform parent, Transform destination)
    {        
        IsInteractable = false;
        if (Outline.enabled == false)
        {            
            Outline.enabled = true;
        }
        IsKillable = false;
        offSet = Collider.bounds.center - transform.position;        
        transform.parent = parent;        
        RigidbodyObj.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
        RigidbodyObj.isKinematic = true;
        transform.DOLocalMove(destination.localPosition - offSet, tweenDelay);
        
    }

    public virtual void OnInteractEnd(Transform forceDirection)
    {
        transform.DOKill();
        if (Outline.enabled == true)
        {
            Outline.enabled = false;
        }
        gameObject.transform.parent = null;
        RigidbodyObj.isKinematic = false;
        RigidbodyObj.collisionDetectionMode = CollisionDetectionMode.Continuous;
        RigidbodyObj.AddForce(forceDirection.forward * throwForce, ForceMode.Impulse);  
    }

       
}
