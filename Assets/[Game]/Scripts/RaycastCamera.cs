using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RaycastCamera : MonoBehaviour
{
    public float mouseSensivity = 100f;    
    private float xRotation = 0f;
    private float yRotation = 0f;
    //private float mouseX = 0f;
    //private float mouseY = 0f;   
    public Joystick joystick;
    public Transform destination;
    private void Start()
    {
        Cursor.visible = false;
    }

    void Update()
    {        
        SetCameraRot();
    }    

    public void SetCameraRot() 
    {        
        float mouseY = Mathf.Abs(joystick.Vertical) * mouseSensivity * Time.deltaTime*Input.GetAxis("Mouse Y");
        float mouseX = Mathf.Abs(joystick.Horizontal) * mouseSensivity * Time.deltaTime * Input.GetAxis("Mouse X");
        

        xRotation -= mouseY;
        yRotation += mouseX;
        xRotation = Mathf.Clamp(xRotation, -25f, 25f);
        yRotation = Mathf.Clamp(yRotation, -18f, 18f);
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }    

    public void OnPointerDown(PointerEventData eventData)
    {
        Vector3 mousePos = eventData.position;
        mousePos.z = Mathf.Abs(transform.position.z - destination.position.z);
        transform.LookAt(Camera.main.ScreenToWorldPoint(eventData.position));
        xRotation = transform.eulerAngles.x;
        yRotation = transform.eulerAngles.y;
    }

    public void OnDrag(PointerEventData eventData)
    {
        SetCameraRot();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }
}
