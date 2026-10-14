using UnityEngine;

public class SkeletonSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject skeletonPrefab;
    public Transform[] spawnPoints;
    public Transform candyTarget;

    [Header("Timing Settings")]
    public float initialDelay = 12.0f;
    public float spawnInterval = 4.0f;

    private void Start()
    {
        InvokeRepeating(nameof(SpawnSkeleton), initialDelay, spawnInterval);
    }

    private void SpawnSkeleton()
    {
        if (spawnPoints.Length == 0 || skeletonPrefab == null) return;

        // Pick a random spawn point from array
        int randomIndex = Random.Range(0, spawnPoints.Length);
        Transform selectedSpawn = spawnPoints[randomIndex];

        GameObject newSkeleton = Instantiate(skeletonPrefab, selectedSpawn.position, selectedSpawn.rotation);

        Skeleton skeletonScript = newSkeleton.GetComponent<Skeleton>();
        if (skeletonScript != null)
        {
            skeletonScript.targetTransform = candyTarget;
        }
        
        if (TutorialManager.Instance != null)
        {
            TutorialManager.Instance.ShowSkeletonMessage();
        }
    }
}