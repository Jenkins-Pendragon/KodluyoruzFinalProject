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
        IShootable cannon = other.GetComponent<IShootable>();
        if (cannon!=null && !cannon.IsCanFire)
        {
            cannon.IsCanFire = true;
            cannon.Shoot();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        IShootable cannon = other.GetComponent<IShootable>();
        if (cannon != null)
        {
            cannon.IsCanFire = false;
            cannon.Shoot();
        }
    }
}
