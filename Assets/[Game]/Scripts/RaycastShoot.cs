using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastShoot : MonoBehaviour
{
    public Camera raycastCam;
    public Transform gunEndPoint;
    public Transform gunVisual;
    public Transform destination;    
    private LineRenderer laserLine;
    private float rayRange = 100f;
    
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
            laserLine.enabled = false;
        }

        else if (Input.GetMouseButton(0))
        {
            DrawLaser();
        }
    }

    private void DrawLaser()
    {
        
        Vector3 rayOrigin = raycastCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;
        laserLine.SetPosition(0, gunEndPoint.position);
        if (Physics.Raycast(rayOrigin, raycastCam.transform.forward, out hit, rayRange))
        {
            laserLine.SetPosition(1, hit.point);
            gunVisual.LookAt(hit.point);
        }
        else
        {
            var pos = rayOrigin + (raycastCam.transform.forward * rayRange);
            laserLine.SetPosition(1, pos);
            gunVisual.LookAt(pos);
        }
    }    
}
