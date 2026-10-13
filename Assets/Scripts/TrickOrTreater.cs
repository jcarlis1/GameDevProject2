using UnityEngine;

public class TrickOrTreater : MonoBehaviour
{
    public enum MovementState { WalkingToTarget, RunningBack }
    public MovementState currentState = MovementState.WalkingToTarget;
    private Animator anim;

    [Header("Speeds")]
    public float walkSpeed = 2.5f;
    public float runSpeed = 4.5f;

    [Header("Waypoints")]
    public Transform targetTransform;
    public Vector3 retreatPosition;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (currentState == MovementState.WalkingToTarget)
        {
            if (targetTransform != null)
            {
                MoveTowards(targetTransform.position, walkSpeed);
                // Set visual animation to Walk (e.g. 0.5f for walk speed)
                if (anim != null)
                {
                    anim.SetFloat("Blend", 0.5f);
                }
            }
        }
        else if (currentState == MovementState.RunningBack)
        {
            MoveTowards(retreatPosition, runSpeed);

            if (anim != null)
            {
                anim.SetFloat("Blend", 1.0f);
            }

            // Destroy the kid object once it reaches the retreat location
            if (Vector3.Distance(transform.position, retreatPosition) < 0.5f)
            {
                Destroy(gameObject);
            }
        }
    }

    private void MoveTowards(Vector3 destination, float speed)
    {
        // Keep height flat on Y-axis while turning to face destination
        Vector3 targetPoint = new Vector3(destination.x, transform.position.y, destination.z);
        Vector3 direction = (targetPoint - transform.position).normalized;

        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
        }

        transform.position = Vector3.MoveTowards(transform.position, targetPoint, speed * Time.deltaTime);
    }

    // Triggered if stationary target object's collider has "Is Trigger" checked
    private void OnTriggerEnter(Collider other)
    {
        // if (currentState == MovementState.WalkingToTarget && other.CompareTag("StationaryTarget"))
        // {
        //     currentState = MovementState.RunningBack;
        // }
        if (currentState == MovementState.WalkingToTarget && 
            (other.CompareTag("StationaryTarget") || other.transform.root.CompareTag("StationaryTarget")))
        {
            // Decrease candy count when reaching the target
            if (CandyManager.Instance != null)
            {
                CandyManager.Instance.DecreaseCandy(1);
            }

            currentState = MovementState.RunningBack;
        }
    }

    // Triggered if stationary target object's collider is solid (Is Trigger unchecked)
    private void OnCollisionEnter(Collision collision)
    {
        if (currentState == MovementState.WalkingToTarget && collision.gameObject.CompareTag("StationaryTarget"))
        {
            currentState = MovementState.RunningBack;
        }
    }
   

}