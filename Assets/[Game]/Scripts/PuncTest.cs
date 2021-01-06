using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuncTest : MonoBehaviour
{
    [SerializeField] Animator animc;
    private void Start()
    {
        animc = GetComponent<Animator>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("aaaaaa");
        animc.SetBool("Running", false);
        animc.SetBool("Punching", true);
    }
}
