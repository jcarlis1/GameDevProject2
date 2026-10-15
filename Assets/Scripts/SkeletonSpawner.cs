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

    [Header("Timing Settings")]
    public float initialDelay = 12.0f; // Seconds to wait before the very first skeleton spawns

    [Header("Spawn Intervals (Seconds Between Spawns)")]
    public float initialSpawnInterval = 4.0f; // Phase 1: Start (0 - 1.5 min)
    public float darkSpawnInterval = 2.5f;    // Phase 2: Dark Unlocked (1.5 - 3.5 min)
    public float redEyeSpawnInterval = 1.2f;  // Phase 3: Red Eye Unlocked (3.5 min+)

    [Header("Time Thresholds (in Seconds)")]
    public float darkSkeletonUnlockTime = 90f;   // 1.5 minutes
    public float redEyeSkeletonUnlockTime = 210f; // 3.5 minutes

    private float gameTimer = 0f;
    private float spawnTimer = 0f;
    private bool isSpawningStarted = false;

    private void Update()
    {
        // Don't update timers while the game is paused in a tutorial popup
        if (Time.timeScale <= 0f) return;

        gameTimer += Time.deltaTime;

        // Handle initial game start delay
        if (!isSpawningStarted)
        {
            if (gameTimer >= initialDelay)
            {
                isSpawningStarted = true;
                SpawnSkeleton(); // Spawn the first skeleton immediately after initial delay
            }
            return;
        }

        // Increment spawn countdown timer
        spawnTimer += Time.deltaTime;

        // Check if it's time to spawn the next skeleton
        float currentInterval = GetCurrentSpawnInterval();
        if (spawnTimer >= currentInterval)
        {
            spawnTimer = 0f;
            SpawnSkeleton();
        }
    }

    private float GetCurrentSpawnInterval()
    {
        if (gameTimer >= redEyeSkeletonUnlockTime)
        {
            return redEyeSpawnInterval; // Fastest spawn rate (Phase 3)
        }
        else if (gameTimer >= darkSkeletonUnlockTime)
        {
            return darkSpawnInterval;   // Medium spawn rate (Phase 2)
        }
        else
        {
            return initialSpawnInterval; // Standard spawn rate (Phase 1)
        }
    }

    private void SpawnSkeleton()
    {
        if (spawnPoints.Length == 0) return;

        GameObject selectedPrefab = ChooseSkeletonPrefab();
        if (selectedPrefab == null) return;

        int randomIndex = Random.Range(0, spawnPoints.Length);
        Transform selectedSpawn = spawnPoints[randomIndex];

        GameObject newSkeleton = Instantiate(selectedPrefab, selectedSpawn.position, selectedSpawn.rotation);

        Skeleton skeletonScript = newSkeleton.GetComponent<Skeleton>();
        if (skeletonScript != null)
        {
            if (selectedPrefab == redEyeSkeletonPrefab)
            {
                skeletonScript.Initialize(candyTarget, 5f, 6.0f);
            }
            else if (selectedPrefab == darkSkeletonPrefab)
            {
                skeletonScript.Initialize(candyTarget, 4f, 4.0f);
            }
            else
            {
                skeletonScript.Initialize(candyTarget, 3f, 2.0f);
            }
        }

        // Trigger specific tutorial message on first spawn of each skeleton type
        if (TutorialManager.Instance != null)
        {
            if (selectedPrefab == redEyeSkeletonPrefab)
            {
                TutorialManager.Instance.ShowRedEyeSkeletonMessage();
            }
            else if (selectedPrefab == darkSkeletonPrefab)
            {
                TutorialManager.Instance.ShowDarkSkeletonMessage();
            }
            else
            {
                TutorialManager.Instance.ShowSkeletonMessage();
            }
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