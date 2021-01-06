using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class InteractableBase : MonoBehaviour, IInteractable
{
    public bool IsInteractable { get;}
    private Rigidbody rb;
    private readonly float throwForce = 100f;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public virtual void OnInteractStart(Transform parent, Transform destination)
    {
        transform.parent = parent;
        rb.isKinematic = true;
        transform.position = destination.position;
    }

    public virtual void OnInteractEnd(Transform forceDirection)
    {
        this.gameObject.transform.parent = null;
        rb.isKinematic = false;        
        rb.AddForce(forceDirection.forward * throwForce, ForceMode.Impulse);
    }

       
}
