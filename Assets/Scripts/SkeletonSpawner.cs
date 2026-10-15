using UnityEngine;

public class SkeletonSpawner : MonoBehaviour
{
    [Header("Skeleton Prefabs")]
    public GameObject standardSkeletonPrefab; // Standard: 3 HP, 2.0 Speed
    public GameObject darkSkeletonPrefab;     // Dark: 4 HP, 4.0 Speed
    public GameObject redEyeSkeletonPrefab;   // Red-Eye: 5 HP, 6.0 Speed

    [Header("Spawn Settings")]
    public Transform[] spawnPoints;
    public Transform candyTarget;
    public float spawnYOffset = -0.4f;

    [Header("Timing Settings")]
    public float initialDelay = 12.0f;
    public float spawnInterval = 4.0f;

    [Header("Time Thresholds (in Seconds)")]
    public float darkSkeletonUnlockTime = 90f;   // 1.5 minutes
    public float redEyeSkeletonUnlockTime = 210f; // 3.5 minutes

    private float gameTimer = 0f;

    private void Start()
    {
        InvokeRepeating(nameof(SpawnSkeleton), initialDelay, spawnInterval);
    }

    private void Update()
    {
        if (Time.timeScale > 0f)
        {
            gameTimer += Time.deltaTime;
        }
    }

    private void SpawnSkeleton()
    {
        if (spawnPoints.Length == 0) return;

        GameObject selectedPrefab = ChooseSkeletonPrefab();
        if (selectedPrefab == null) return;

        int randomIndex = Random.Range(0, spawnPoints.Length);
        Transform selectedSpawn = spawnPoints[randomIndex];

        // Calculate position with Y-offset applied
        Vector3 spawnPosition = selectedSpawn.position + new Vector3(0, spawnYOffset, 0);

        // Instantiate at adjusted position
        GameObject newSkeleton = Instantiate(selectedPrefab, spawnPosition, selectedSpawn.rotation);
        // GameObject newSkeleton = Instantiate(selectedPrefab, selectedSpawn.position, selectedSpawn.rotation);

        Skeleton skeletonScript = newSkeleton.GetComponent<Skeleton>();
        if (skeletonScript != null)
        {
            // Configure unique HP and Speed per skeleton type
            if (selectedPrefab == redEyeSkeletonPrefab)
            {
                skeletonScript.Initialize(candyTarget, 5f, 6.0f); // 5 Hits, Speed 6
            }
            else if (selectedPrefab == darkSkeletonPrefab)
            {
                skeletonScript.Initialize(candyTarget, 4f, 4.0f); // 4 Hits, Speed 4
            }
            else
            {
                skeletonScript.Initialize(candyTarget, 3f, 2.0f); // 3 Hits, Speed 2
            }
        }

        if (TutorialManager.Instance != null)
        {
            TutorialManager.Instance.ShowSkeletonMessage();
        }
    }

    private GameObject ChooseSkeletonPrefab()
    {
        // 3.5 Min+: Pick randomly among all 3
        if (gameTimer >= redEyeSkeletonUnlockTime && redEyeSkeletonPrefab != null)
        {
            int roll = Random.Range(0, 3);
            if (roll == 0) return standardSkeletonPrefab;
            if (roll == 1) return darkSkeletonPrefab;
            return redEyeSkeletonPrefab;
        }
        // 1.5 Min+: Pick randomly between Standard and Dark
        else if (gameTimer >= darkSkeletonUnlockTime && darkSkeletonPrefab != null)
        {
            int roll = Random.Range(0, 2);
            return (roll == 0) ? standardSkeletonPrefab : darkSkeletonPrefab;
        }
        // Start: Standard only
        else
        {
            return standardSkeletonPrefab;
        }
    }
}