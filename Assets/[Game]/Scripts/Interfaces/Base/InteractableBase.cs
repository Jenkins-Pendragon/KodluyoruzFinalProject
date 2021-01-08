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
    private OutlineShader outline;
    public OutlineShader Outline { get { return (outline == null) ? outline = GetComponentInChildren<OutlineShader>() : outline; } }

    private readonly float throwForce = 50f;
    private readonly float tweenDelay = 0.3f;
    private void Start()
    {
        OnStart();
    }

    public virtual void OnStart() 
    {
        IsInteractable = true;
        IsKillable = false;
        Outline.enabled = false;
        Outline.Initiliaze(Color.yellow, 8f, OutlineShader.Mode.OutlineVisible);
    }

    public virtual void OnInteractStart(Transform parent, Transform destination)
    {
        IsInteractable = false;
        if (Outline.enabled == false)
        {            
            Outline.enabled = true;
        }        
        transform.parent = parent;
        RigidbodyObj.isKinematic = true;
        transform.DOLocalMove(destination.localPosition, tweenDelay);
        
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
        RigidbodyObj.AddForce(forceDirection.forward * throwForce, ForceMode.Impulse);
    }

       
}
