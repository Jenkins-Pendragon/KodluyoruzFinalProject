using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTransfomStreamer : MonoBehaviour
{
    public static PlayerTransfomStreamer Instance;
    private void Awake()
    {
        Instance = this;
    }   
}
