using UnityEngine;

public class Exploder
{
    public void Explode(Vector3 position, float explosionRadius, float explosionForce)
    {
        Collider[] colliders = Physics.OverlapSphere(position, explosionRadius);

        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent(out Rigidbody rigidbody))
            {
                rigidbody.AddExplosionForce(explosionForce, position, explosionRadius);
            }
        }
    }
}
