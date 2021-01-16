using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GunCameraController : MonoBehaviour
{
    public float mouseSensivity = 100f;    
    private float xRotation = 0f;
    private float yRotation = 0f;
    private readonly float rayRange = 50f;
    public Joystick joystick;
    public Transform destination;    

    
    private void Start()
    {
        joystick = UIController.Instance.joystick;
        //Cursor.visible = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
           if (PlayerData.Instance.IsPlayerDead) return;            
           LookAtTouchPos();            
        }        

        else if (Input.GetMouseButton(0))
        {
            if (PlayerData.Instance.IsPlayerDead) return;            
            ChangeCameraRotation();
        }        
    }    

    public void ChangeCameraRotation() 
    {        
        float mouseY = Mathf.Abs(joystick.Vertical) * mouseSensivity * Time.deltaTime*Input.GetAxis("Mouse Y");
        float mouseX = Mathf.Abs(joystick.Horizontal) * mouseSensivity * Time.deltaTime * Input.GetAxis("Mouse X");    
        xRotation -= mouseY;
        yRotation += mouseX;
        xRotation = Mathf.Clamp(xRotation, -45f, 15f);
        yRotation = Mathf.Clamp(yRotation, -40f, 30f);
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }    

    public void LookAtTouchPos()
    {
        Vector3 mousePos = Input.mousePosition;
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            transform.LookAt(hit.point);
        }
        else
        {
            mousePos.z = Mathf.Abs(transform.position.z - destination.position.z* rayRange);
            transform.LookAt(Camera.main.ScreenToWorldPoint(mousePos));            
        }
        xRotation = WrapAngle(transform.localEulerAngles.x);
        yRotation = WrapAngle(transform.localEulerAngles.y);
        EventManager.OnLookAtTouchPosCompleted.Invoke();
    }

    public  float WrapAngle(float angle)
    {
        angle %= 360;
        if (angle > 180)
            return angle - 360;

        return angle;
    }
}
