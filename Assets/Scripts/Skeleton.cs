using UnityEngine;

public class Skeleton : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 30f; // Set to match 3 hits if Gun damage is 10 (3 x 10 = 30 HP)
    private float currentHealth;

    [Header("Movement Settings")]
    public float moveSpeed = 2.0f;
    public Transform targetTransform;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (targetTransform != null)
        {
            // Walk toward the candy bucket target
            transform.position = Vector3.MoveTowards(transform.position, targetTransform.position, moveSpeed * Time.deltaTime);

            // Turn facing direction toward target
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
        // Decrement candy count if reaching the candy bucket
        if (other.CompareTag("StationaryTarget") || other.transform.root.CompareTag("StationaryTarget"))
        {
            if (CandyManager.Instance != null)
            {
                CandyManager.Instance.DecreaseCandy(1);
            }

            // Destroy(gameObject);
            Die();
        }
    }
}