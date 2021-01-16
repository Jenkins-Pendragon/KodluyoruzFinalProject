
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public static FollowCamera Instance;    
    [HideInInspector]
    public GameObject targetObject;
    private Vector3 offset;
    private bool followStart = false;
    private void Awake()
    {
        Instance = this;        
    }    
    public void CameraFollow()
    {
        offset = transform.position - targetObject.transform.position;
        followStart = true;
    }
    private void LateUpdate()
    {
        if (followStart)
        {
            //Vector3 wantedPos = targetObject.transform.position + offset - Vector3.forward*4f + Vector3.up*2.0f + Vector3.left * 2;

            //transform.localEulerAngles = new Vector3(0, 10, 0);

            Vector3 wantedPos = targetObject.transform.position + offset + Vector3.up*3f - Vector3.forward*2f;

            
            transform.position = wantedPos;
        }
    }
}

