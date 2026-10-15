using UnityEngine;

public class Skeleton : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 3f;
    private float currentHealth;

    [Header("Movement Settings")]
    public float moveSpeed = 2.0f;
    public Transform targetTransform;

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
            animator.Play("Walk");
        }
    }

    // Called by SkeletonSpawner upon creation
    public void Initialize(Transform target, float health, float speed)
    {
        targetTransform = target;
        maxHealth = health;
        currentHealth = health;
        moveSpeed = speed;
    }

    private void Update()
    {
        if (targetTransform != null)
        {
            // Move toward target at assigned speed
            transform.position = Vector3.MoveTowards(transform.position, targetTransform.position, moveSpeed * Time.deltaTime);

            // Look toward target
            Vector3 lookDirection = targetTransform.position - transform.position;
            lookDirection.y = 0;
            if (lookDirection != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(lookDirection);
            }
        }
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("StationaryTarget") || other.transform.root.CompareTag("StationaryTarget"))
        {
            if (CandyManager.Instance != null)
            {
                CandyManager.Instance.DecreaseCandy(1);
            }

            Destroy(gameObject);
        }
    }
}