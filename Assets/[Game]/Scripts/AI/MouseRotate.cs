using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMove : MonoBehaviour
{
    private Rigidbody rigidbody;
    public float sensivity = 3;


    public Rigidbody Rigidbody
    {
        get
        {
            if (rigidbody == null)
            {
                rigidbody = gameObject.GetComponent<Rigidbody>();
            }
            return rigidbody;
        }
    }

    public float MoveSpeed;
    public float jumpSpeed = 5f;
    // Update is called once per frame
    void Update()
    {
        Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        if (input != Vector3.zero)
        {
            Vector3 newMove = input * MoveSpeed * Time.deltaTime;
            newMove.y = Rigidbody.velocity.y;

            Rigidbody.velocity = newMove;

        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Rigidbody.velocity = new Vector3(0, jumpSpeed, 0);

        }
    }
}
