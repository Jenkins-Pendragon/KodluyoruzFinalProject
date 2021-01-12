using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;

public class Blade : MonoBehaviour
{
    public Material cross;

    void Update()
    {
            //Box ray detection
            Collider[] colliders = Physics.OverlapBox(transform.position,
                           new Vector3(0.5f, 0.005f, 0.5f),
                           transform.rotation,
                           LayerMask.GetMask("Sliceable"));
            //Cut each detected
            foreach (Collider c in colliders)
            {
                if (c.gameObject != this.gameObject)
                {
                    Destroy(c.gameObject);                  
                }
                SlicedHull hull = c.gameObject.Slice(transform.position, transform.up);
                print(hull);
                if (hull != null)
                {
                    GameObject lower = hull.CreateLowerHull(c.gameObject, cross);
                    GameObject upper = hull.CreateUpperHull(c.gameObject, cross);
                    GameObject[] objs = new GameObject[] { lower, upper };

                    foreach (GameObject o in objs)
                    {
                        o.AddComponent<Rigidbody>();
                        o.AddComponent<MeshCollider>().convex = true;
                        o.layer = LayerMask.NameToLayer("Sliceable");
                    }
                }
            }
    }

}
