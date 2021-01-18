using UnityEngine;
using DG.Tweening;

public class Barrel : InteractableBase, IExplodable
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
            Explode();
        }
        if (IsKillable)
        {
            Explode();
        }
    }    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(gameObject.transform.position, radius);
    }

    public void Explode()
    {
        Instantiate(blastEffectPrefab, transform.position, Quaternion.identity);
        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
        foreach (Collider collider in colliders)
        {
            
            Rigidbody rb = collider.GetComponent<Rigidbody>();
            IDamageable damageable = collider.GetComponent<IDamageable>();
            Ammo ammo = collider.GetComponent<Ammo>();
            if (damageable != null)
            {
                damageable.Die();
            }
            if (rb != null)
            {
                rb.AddExplosionForce(power, explosionPos, radius, 3.0f, ForceMode.Impulse);
            }

            if (ammo != null)
            {
                ammo.Off();
            }
        }
        this.gameObject.SetActive(false);
    }
}
