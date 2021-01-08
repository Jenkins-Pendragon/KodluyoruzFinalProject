using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Rigidbody))]
public class InteractableBase : MonoBehaviour, IInteractable
{
    public bool IsInteractable { get; set; }
    private Rigidbody rb;
    public Rigidbody RigidbodyObj { get { return (rb == null) ? rb = GetComponentInChildren<Rigidbody>() : rb; } }
  
    private readonly float throwForce = 50f;
    private readonly float tweenDelay = 0.3f;
    private void Start()
    {
        OnStart();
    }

    public virtual void OnStart() 
    {
        IsInteractable = true;        
    }

    public virtual void OnInteractStart(Transform parent, Transform destination)
    {
        if (GetComponent<OutlineShader>() == null)
        {
            transform.gameObject.AddComponent<OutlineShader>().Initiliaze(Color.yellow, 8f, OutlineShader.Mode.OutlineVisible);
        }        
        transform.parent = parent;
        RigidbodyObj.isKinematic = true;
        transform.DOLocalMove(destination.localPosition, tweenDelay);
        
    }

    public virtual void OnInteractEnd(Transform forceDirection)
    {
        transform.DOKill();
        OutlineShader outlineShader = GetComponent<OutlineShader>();
        if (GetComponent<OutlineShader>() != null)
        {
            Destroy(outlineShader);
        }
        gameObject.transform.parent = null;
        RigidbodyObj.isKinematic = false;
        RigidbodyObj.AddForce(forceDirection.forward * throwForce, ForceMode.Impulse);
    }

       
}
