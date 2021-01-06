using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour
{
    public Camera gunCamera;
    public Transform gunEndPoint;
    public Transform gunVisual;
    public Transform destination;
    public float rayRange = 100f;
    private LineRenderer laserLine;
    private GameObject lastSelection = null;
    
    
    void Start()
    {        
        laserLine = GetComponent<LineRenderer>();
    }

    
    void Update()
    {        
        if (Input.GetMouseButtonDown(0))
        {            
            DrawLaser();            
            laserLine.enabled = true;
        }

        else if (Input.GetMouseButtonUp(0))
        {
            RealaseInteractableObject();
            laserLine.enabled = false;            
        }

        else if (Input.GetMouseButton(0))
        {
            DrawLaser();
        }
    }

    private void DrawLaser()
    {
        
        Vector3 rayOrigin = gunCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;
        laserLine.SetPosition(0, gunEndPoint.position);
        var pos = rayOrigin + (gunCamera.transform.forward * rayRange);
        if (Physics.Raycast(gunEndPoint.position, gunVisual.forward, out hit, rayRange))
        {
            laserLine.SetPosition(1, hit.point);
            CheckInteractableObject(hit);
        }
        else
        {            
            laserLine.SetPosition(1, pos);                      
        }
        gunVisual.LookAt(pos);
    }  

    private void CheckInteractableObject(RaycastHit hit) 
    {
        if (lastSelection == null)
        {
            GameObject obj = hit.collider.gameObject;
            IInteractable interactable = lastSelection.GetComponent<IInteractable>();
            if (interactable != null && interactable.IsInteractable)
            {
                lastSelection = obj;
                interactable.OnInteractStart(gunCamera.transform, destination);
            }
        }
    }

    private void RealaseInteractableObject() 
    {
        if (lastSelection != null)
        {
            lastSelection.GetComponent<IInteractable>().OnInteractEnd(gunCamera.transform);
            lastSelection = null;
        }
    }
}
