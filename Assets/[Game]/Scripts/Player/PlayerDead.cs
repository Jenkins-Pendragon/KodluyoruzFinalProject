using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerDead : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            print("space key was pressed");
            transform.DORotate(
            new Vector3(0, 0, -80), 1.5f, RotateMode.FastBeyond360);
        }
    }
}
