using UnityEngine;

public class ExplodingPumpkin : MonoBehaviour
{
    [Header("Explosion Effects")]
    public GameObject explosionEffectPrefab; // Particle system prefab
    public AudioClip explosionSound;          // Optional explosion sound
    [Range(0f, 1f)] public float soundVolume = 1.0f;

    private void OnCollisionEnter(Collision collision)
    {
        // 1. Spawn explosion particle effect at the point of impact
        if (explosionEffectPrefab != null)
        {
            // Position effect at impact point (or pumpkin position) facing the surface normal
            ContactPoint contact = collision.contacts[0];
            GameObject effect = Instantiate(explosionEffectPrefab, contact.point, Quaternion.LookRotation(contact.normal));

            // Clean up the particle effect after 2 seconds
            Destroy(effect, 2f);
        }

        // 2. Play sound at the impact location (PlayClipAtPoint ensures audio doesn't cut off when pumpkin is destroyed)
        if (explosionSound != null)
        {
            AudioSource.PlayClipAtPoint(explosionSound, transform.position, soundVolume);
        }

        // 3. Destroy the pumpkin object
        Destroy(gameObject);
    }
}