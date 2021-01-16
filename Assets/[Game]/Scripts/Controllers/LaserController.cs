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
    public LineRenderer LaserLine { get { return (laserLine == null) ? laserLine = GetComponent<LineRenderer>() : laserLine; } }
    private GameObject lastSelection = null;
    private Collider lastSelectionCollider = null;
    private bool canDrawLaser;

    private void OnEnable()
    {
        EventManager.OnLookAtTouchPosCompleted.AddListener(() => canDrawLaser = true);
    }

    private void OnDisable()
    {
        EventManager.OnLookAtTouchPosCompleted.RemoveListener(() => canDrawLaser = true);
    }        
    void Update()
    {
        if (PlayerData.Instance.IsControlable)
        {
            if (Input.GetMouseButtonUp(0) || (lastSelection != null && (PlayerData.Instance.IsPlayerDead)))
            {
                RealaseInteractableObject();
                canDrawLaser = false;
                LaserLine.enabled = false;
            }

            if (canDrawLaser && !PlayerData.Instance.IsPlayerDead)
            {
                DrawLaser();
                if (!LaserLine.enabled) LaserLine.enabled = true;
            }
        }
        
    }

    public void DrawLaser()
    {        
        Vector3 rayOrigin = gunCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;
        LaserLine.SetPosition(0, gunEndPoint.position);
        var pos = rayOrigin + (gunVisual.forward * rayRange);
        if (lastSelectionCollider != null)
        {
            //laserLine.SetPosition(1, lastSelection.transform.position);
            LaserLine.SetPosition(1, lastSelectionCollider.bounds.center);
        }
        else
        {
            if (Physics.Raycast(gunEndPoint.position, gunVisual.forward, out hit, rayRange))
            {
                LaserLine.SetPosition(1, hit.point);
                CheckInteractableObject(hit);
            }
            else
            {
                LaserLine.SetPosition(1, pos);
            }
        }
               
    }  

    private void CheckInteractableObject(RaycastHit hit) 
    {
        if (lastSelection == null)
        {
            GameObject obj = hit.collider.gameObject;
            IInteractable interactable = obj.GetComponent<IInteractable>();
            if (interactable != null && interactable.IsInteractable)
            {
                lastSelection = obj;
                lastSelectionCollider = lastSelection.GetComponent<Collider>();
                interactable.OnInteractStart(gunVisual, destination);
            }
        }
    }

    private void RealaseInteractableObject() 
    {
        if (lastSelection != null)
        {
            lastSelection.GetComponent<IInteractable>().OnInteractEnd(gunCamera.transform);
            lastSelection = null;
            lastSelectionCollider = null;
        }
    }
}
