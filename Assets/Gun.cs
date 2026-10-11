using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour
{
    [Header("Gun Settings")]
    public float range = 100f;
    public float damage = 10f;

    [Header("Laser Sight Settings")]
    public LineRenderer laserLine;
    public Transform muzzlePoint;

    [Header("Audio & Effects")]
    public AudioSource audioSource;
    public AudioClip shootSound;
    public ParticleSystem muzzleFlash;

    void Start()
    {
        if (muzzlePoint == null)
        {
            muzzlePoint = transform;
        }

        // Auto-get LineRenderer if attached to same object
        if (laserLine == null)
        {
            laserLine = GetComponent<LineRenderer>();
        }
    }

    void Update()
    {
        UpdateLaser();

        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            Shoot();
        }
    }

    void UpdateLaser()
    {
        if (laserLine == null || muzzlePoint == null) return;

        // Position 0 is always the gun barrel tip
        laserLine.SetPosition(0, muzzlePoint.position);

        RaycastHit hit;
        if (Physics.Raycast(muzzlePoint.position, muzzlePoint.forward, out hit, range))
        {
            // Position 1 ends at the exact impact point
            laserLine.SetPosition(1, hit.point);
        }
        else
        {
            // If aiming into open sky, extend laser out to max range
            laserLine.SetPosition(1, muzzlePoint.position + (muzzlePoint.forward * range));
        }
    }

    void Shoot()
    {
        Debug.Log("Shooting");

        if (audioSource != null && shootSound != null)
        {
            audioSource.PlayOneShot(shootSound);
        }

        if (muzzleFlash != null)
        {
            muzzleFlash.Play();
        }

        RaycastHit hit;
        if (Physics.Raycast(muzzlePoint.position, muzzlePoint.forward, out hit, range))
        {
            Debug.Log("Hit: " + hit.transform.name);
        }
    }
}