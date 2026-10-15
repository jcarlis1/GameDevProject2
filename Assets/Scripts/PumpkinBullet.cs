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

    private void OnTriggerEnter(Collider other)
    {
        // 1. Check for Ghost
        Ghost ghost = other.GetComponentInParent<Ghost>();
        if (ghost != null)
        {
            float damage = (Gun.Instance != null) ? Gun.Instance.damage : 1f;
            ghost.TakeDamage(damage);
            Destroy(gameObject); // Destroy the pumpkin bullet on hit
            return;
        }

        // 2. Check for Skeleton
        Skeleton skeleton = other.GetComponentInParent<Skeleton>();
        if (skeleton != null)
        {
            float damage = (Gun.Instance != null) ? Gun.Instance.damage : 1f;
            skeleton.TakeDamage(damage);
            Destroy(gameObject); // Destroy the pumpkin bullet on hit
            return;
        }
    }
}