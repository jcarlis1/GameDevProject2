using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour
{
    [Header("Gun Settings")]
    public float damage = 1f;
    public float initialVelocity = 35f; // Speed of the bullet/arc (higher = flatter line)

    [Header("Curved Laser Settings")]
    public LineRenderer laserLine;
    public Transform muzzlePoint;
    public int resolution = 30;          // Number of points in the curved line
    public float timeStep = 0.05f;       // Time between each calculated point

    [Header("Audio & Effects")]
    public AudioSource audioSource;
    public AudioClip shootSound;
    public ParticleSystem muzzleFlash;

    [Header("Bullet Prefab")]
    public GameObject pumpkinPrefab; // Drag your Pumpkin Prefab here in the Inspector
    public float shootForce = 35f;   // Match this with initialVelocity for accurate arc


    void Start()
    {
        if (muzzlePoint == null)
        {
            muzzlePoint = transform;
        }

        if (laserLine == null)
        {
            laserLine = GetComponent<LineRenderer>();
        }
    }

    void Update()
    {
        UpdateCurvedLaser();

        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            Shoot();
        }
    }

    void UpdateCurvedLaser()
    {
        if (laserLine == null || muzzlePoint == null) return;

        // Initialize points array for the Line Renderer
        laserLine.positionCount = resolution;

        Vector3 currentPosition = muzzlePoint.position;
        Vector3 currentVelocity = muzzlePoint.forward * initialVelocity;
        Vector3 gravity = Physics.gravity; // Uses Unity's default gravity (-9.81 on Y axis)

        laserLine.SetPosition(0, currentPosition);

        for (int i = 1; i < resolution; i++)
        {
            // Calculate next position using projectile physics formula: P = P0 + V*t + 0.5*g*t^2
            Vector3 nextPosition = currentPosition + currentVelocity * timeStep + 0.5f * gravity * (timeStep * timeStep);
            
            // Apply gravity to velocity over time
            currentVelocity += gravity * timeStep;

            // Check for collision along the segment between currentPosition and nextPosition
            Vector3 direction = nextPosition - currentPosition;
            float distance = direction.magnitude;

            if (Physics.Raycast(currentPosition, direction.normalized, out RaycastHit hit, distance))
            {
                // Stop the line at the hit point and trim extra points
                laserLine.positionCount = i + 1;
                laserLine.SetPosition(i, hit.point);
                return;
            }

            laserLine.SetPosition(i, nextPosition);
            currentPosition = nextPosition;
        }
    }


    void Shoot()
    {
        if (audioSource != null && shootSound != null)
        {
            audioSource.PlayOneShot(shootSound);
        }

        if (muzzleFlash != null)
        {
            muzzleFlash.Play();
        }

        if (pumpkinPrefab != null && muzzlePoint != null)
        {
            // Spawn pumpkin at muzzle location
            GameObject pumpkin = Instantiate(pumpkinPrefab, muzzlePoint.position, muzzlePoint.rotation);
            
            // Pass the Gun's damage value to the Pumpkin script
            PumpkinBullet bulletScript = pumpkin.GetComponent<PumpkinBullet>();
            if (bulletScript != null)
            {
                bulletScript.damage = damage; // Uses "public float damage = 10f;" from Gun.cs
            }
            
            // Push the pumpkin forward with physical force
            Rigidbody rb = pumpkin.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(muzzlePoint.forward * shootForce, ForceMode.VelocityChange);
            }

            // Optional: Destroy pumpkin after 5 seconds to keep the game clean
            Destroy(pumpkin, 5f);
        }
    }
}