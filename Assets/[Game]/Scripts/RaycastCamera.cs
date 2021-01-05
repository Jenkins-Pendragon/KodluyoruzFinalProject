using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RaycastCamera : MonoBehaviour
{
    public float mouseSensivity = 100f;    
    private float xRotation = 0f;
    private float yRotation = 0f;
    private float mouseX = 0f;
    private float mouseY = 0f;
    private float offset = 0.1f;
    public CustomJoystick joystick;   
    private void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        joystick.GetComponent<CustomJoystick>();
    }

    void Update()
    {
        Debug.Log(joystick.isDragging);
        
        mouseY = joystick.Vertical * mouseSensivity * Time.deltaTime;
        mouseX = joystick.Horizontal * mouseSensivity * Time.deltaTime;

        xRotation -= mouseY;
        yRotation += mouseX;
        xRotation = Mathf.Clamp(xRotation, -25f, 25f);
        yRotation = Mathf.Clamp(yRotation, -18f, 18f);
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
        
    }
}
