using UnityEngine;

public class Ghost : MonoBehaviour
{
    [Header("Ghost Health Settings")]
    public float maxHealth = 5f;
    private float currentHealth;

    [Header("Movement Settings")]
    public float moveSpeed = 3.0f;
    private Transform endDestination;

    private Animator animator;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        if (animator != null)
        {
            animator.Play("idle_up_down"); // Plays default float/walk animation
        }
    }

    public void Initialize(Transform destination)
    {
        endDestination = destination;
    }

    private void Update()
    {
        if (endDestination != null)
        {
            // Move toward destination point
            transform.position = Vector3.MoveTowards(transform.position, endDestination.position, moveSpeed * Time.deltaTime);

            // Rotate toward destination
            Vector3 lookDirection = endDestination.position - transform.position;
            lookDirection.y = 0;
            if (lookDirection != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(lookDirection);
            }

            // If ghost reaches end point, disappear without rewarding gun upgrade
            if (Vector3.Distance(transform.position, endDestination.position) < 0.2f)
            {
                Destroy(gameObject);
            }
        }
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            DieAndUpgradeGun();
        }
    }

    private void DieAndUpgradeGun()
    {
        // Increase Gun Damage by +1
        if (Gun.Instance != null)
        {
            Gun.Instance.IncreaseDamage(1f);
        }

        Destroy(gameObject);
    }
}