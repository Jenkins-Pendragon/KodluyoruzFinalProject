using UnityEngine;

public class Barrel : MonoBehaviour
{
    public float radius = 5.0f;
    public float power = 20.0f;
    public GameObject blastEffectPrefab;

    private void OnMouseDown()
    {
        OnRelation();
    }
    public void OnRelation()
    {
        Instantiate(blastEffectPrefab);
        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
        foreach (Collider collider in colliders)
        {
            Rigidbody myRigidbody = collider.GetComponent<Rigidbody>();
            if (myRigidbody != null)
            {
                myRigidbody.AddExplosionForce(power, explosionPos, radius, 3.0f, ForceMode.Impulse);
            }
        }
        this.gameObject.SetActive(false);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(gameObject.transform.position, radius);
    }
}
