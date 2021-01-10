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
    private void OnTriggerEnter(Collider other)
    {
        Cannon cannon = other.GetComponent<Cannon>();
        if (cannon!=null)
        {
            cannon.canFire = true;
            StartCoroutine(cannon.StartFire());
        }
    }
    private void OnTriggerExit(Collider other)
    {
        Cannon cannon = other.GetComponent<Cannon>();
        if (cannon != null)
        {
            cannon.canFire = false;
            StopCoroutine(cannon.StartFire());
        }
    }
}
