using UnityEngine;

public class GhostSpawner : MonoBehaviour
{
    [Header("Ghost Prefab")]
    public GameObject ghostPrefab;

    [Header("Path Settings")]
    public Transform ghostSpawnPoint;
    public Transform ghostEndPoint;

    [Header("Spawn Times (in Seconds)")]
    public float firstGhostTime = 120f;  // 2 minutes
    public float secondGhostTime = 240f; // 4 minutes

    private float gameTimer = 0f;
    private bool spawnedFirstGhost = false;
    private bool spawnedSecondGhost = false;

    private void Update()
    {
        // Don't advance timer when paused in popups
        if (Time.timeScale <= 0f) return;

        gameTimer += Time.deltaTime;

        // Spawn 1st Ghost at 2 Minutes (120s)
        if (!spawnedFirstGhost && gameTimer >= firstGhostTime)
        {
            spawnedFirstGhost = true;
            SpawnGhost();
        }

        // Spawn 2nd Ghost at 4 Minutes (240s)
        if (!spawnedSecondGhost && gameTimer >= secondGhostTime)
        {
            spawnedSecondGhost = true;
            SpawnGhost();
        }
    }

    private void SpawnGhost()
    {
        if (ghostPrefab == null || ghostSpawnPoint == null || ghostEndPoint == null) return;

        // Spawn Ghost at start point
        GameObject newGhost = Instantiate(ghostPrefab, ghostSpawnPoint.position, ghostSpawnPoint.rotation);

        // Configure Ghost destination
        Ghost ghostScript = newGhost.GetComponent<Ghost>();
        if (ghostScript != null)
        {
            ghostScript.Initialize(ghostEndPoint);
        }

        // Show tutorial popup on first ghost spawn
        if (TutorialManager.Instance != null)
        {
            TutorialManager.Instance.ShowGhostMessage();
        }
    }
}