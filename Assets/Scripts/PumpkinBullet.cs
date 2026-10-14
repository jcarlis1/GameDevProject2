using UnityEngine;

public class PumpkinBullet : MonoBehaviour
{
    [HideInInspector]
    public float damage = 1f;

    private void OnCollisionEnter(Collision collision)
    {
        // Search hit object or parent for Skeleton script
        Skeleton skeleton = collision.gameObject.GetComponentInParent<Skeleton>();

        if (skeleton != null)
        {
            skeleton.TakeDamage(damage);
            Destroy(gameObject); // Destroy pumpkin on impact with skeleton
        }
    }
}