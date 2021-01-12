using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;

public class Saw : MonoBehaviour
{
    public Material material;
    public LayerMask mask;

    private void OnTriggerEnter(Collider other)
    {
        Collider[] colliders = Physics.OverlapBox(transform.position, new Vector3(1f, 0.1f, 0.1f), transform.rotation, mask);

        foreach (Collider item in colliders)
        {
            SliceChibi sliceChibi = item.GetComponent<SliceChibi>();
            if (sliceChibi != null)
            {
                GameObject obj = sliceChibi.Slice();
                SlicedHull slicedHull = Slice(obj.GetComponent<Collider>().gameObject, material);
                GameObject upperHull = slicedHull.CreateUpperHull(obj.gameObject, material);
                GameObject lowerHull = slicedHull.CreateLowerHull(obj.gameObject, material);
                AddComponents(upperHull);
                AddComponents(lowerHull);
                obj.gameObject.SetActive(false);
            }
            else
            {
                SlicedHull slicedHull = Slice(item.GetComponent<Collider>().gameObject, material);
                GameObject upperHull = slicedHull.CreateUpperHull(item.gameObject, material);
                GameObject lowerHull = slicedHull.CreateLowerHull(item.gameObject, material);
                AddComponents(upperHull);
                AddComponents(lowerHull);
                Destroy(item.gameObject);
            }

        }
    }

    private SlicedHull Slice(GameObject obj, Material material)
    {
        return obj.Slice(transform.position, transform.up, material);
    }

    private void AddComponents(GameObject obj)
    {
        obj.AddComponent<MeshCollider>().convex = true;
        Rigidbody rb = obj.AddComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.AddExplosionForce(100, obj.transform.position, 20);
    }
}
