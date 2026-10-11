using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour
{
    [Header("Gun Settings")]
    public float range = 100f;
    public float damage = 10f;

    [Header("Crosshair Settings")]
    public Transform crosshairTransform;
    public Transform muzzlePoint;
    public float defaultDistance = 10f;
    public float crosshairSize = 0.05f; // Editable scale multiplier in Inspector

    [Header("Audio & Effects")]
    public AudioSource audioSource;
    public AudioClip shootSound;
    public ParticleSystem muzzleFlash;

    private Camera mainCam;

    void Start()
    {
        FindMainCamera();

        if (muzzlePoint == null)
        {
            muzzlePoint = transform;
        }
    }

    void Update()
    {
        UpdateGunCrosshair();

        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            Shoot();
        }
    }

    void FindMainCamera()
    {
        mainCam = Camera.main;
        // Fallback if MainCamera tag is missing on camera object
        if (mainCam == null)
        {
            mainCam = FindFirstObjectByType<Camera>();
        }
    }

    void UpdateGunCrosshair()
    {
        if (crosshairTransform == null) return;
        if (mainCam == null) FindMainCamera();
        if (mainCam == null) return;

        RaycastHit hit;
        Vector3 targetPoint;
        float distance;

        // Cast ray from muzzle
        if (Physics.Raycast(muzzlePoint.position, muzzlePoint.forward, out hit, range))
        {
            distance = hit.distance;
            // Slightly pull towards camera to stop surface clipping
            targetPoint = hit.point - (mainCam.transform.forward * 0.05f);
        }
        else
        {
            distance = defaultDistance;
            targetPoint = muzzlePoint.position + (muzzlePoint.forward * defaultDistance);
        }

        crosshairTransform.position = targetPoint;

        // Face the camera directly
        crosshairTransform.rotation = mainCam.transform.rotation;

        // Scale based on distance, preventing it from shrinking to 0
        float finalScale = Mathf.Max(0.01f, distance * crosshairSize);
        crosshairTransform.localScale = new Vector3(finalScale, finalScale, finalScale);
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