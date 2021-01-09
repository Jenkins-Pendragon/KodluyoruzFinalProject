using UnityEngine;

public class Barrel : InteractableBase
{
    public float radius = 5.0f;
    public float power = 20.0f;
    public GameObject blastEffectPrefab; // Bunu daha sonra static instance'i olan bir classa atıp ordan çekelim // Örneğin GameData classı olabilir
    
    public override void OnInteractStart(Transform parent, Transform destination)
    {
        base.OnInteractStart(parent, destination);        
    }

    public override void OnInteractEnd(Transform forceDirection)
    {
        base.OnInteractEnd(forceDirection);
        IsKillable = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        IInteractable interactable = collision.gameObject.GetComponent<IInteractable>();
        if (interactable!=null && interactable.IsKillable)
        {
            OnRelation();
        }
        if (IsKillable)
        {
            OnRelation();
        }
    }
    public void OnRelation()
    {
        Instantiate(blastEffectPrefab,transform.position,Quaternion.identity);
        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
        foreach (Collider collider in colliders)
        {
            Rigidbody myRigidbody = collider.GetComponent<Rigidbody>();
            if (myRigidbody != null)
            {
                myRigidbody.AddExplosionForce(power, explosionPos, radius, 3.0f , ForceMode.Impulse);
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
