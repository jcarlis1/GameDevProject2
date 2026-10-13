using System.Collections;
using UnityEngine;

public class KidSpawner : MonoBehaviour
{
    [Header("Kid Prefabs")]
    public GameObject[] kidPrefabs; // Assign your 4 kid prefabs here in the Inspector

    [Header("Target & Path Settings")]
    public Transform stationaryTarget;
    public float sideOffsetAmount = 2.0f; // Distance to side for retreat

    [Header("Wave Spawning Settings")]
    public int minKidsPerGroup = 1;
    public int maxKidsPerGroup = 5;

    [Header("Timing Settings")]
    public float minDelayBetweenGroups = 4.0f;  // Seconds to wait between groups
    public float maxDelayBetweenGroups = 8.0f;
    public float timeBetweenKidsInGroup = 0.3f; // Small stagger so kids don't overlap

    void Start()
    {
        StartCoroutine(SpawnGroupRoutine());
    }

    IEnumerator SpawnGroupRoutine()
    {
        while (true)
        {
            // Pick a random number of kids for this group
            int kidsInGroup = Random.Range(minKidsPerGroup, maxKidsPerGroup + 1);

            // Spawn each kid in the group
            for (int i = 0; i < kidsInGroup; i++)
            {
                SpawnSingleKid();

                if (timeBetweenKidsInGroup > 0f)
                {
                    yield return new WaitForSeconds(timeBetweenKidsInGroup);
                }
            }

            // Wait before sending the next group
            float nextGroupDelay = Random.Range(minDelayBetweenGroups, maxDelayBetweenGroups);
            yield return new WaitForSeconds(nextGroupDelay);
        }
    }

    void SpawnSingleKid()
    {
        if (kidPrefabs == null || kidPrefabs.Length == 0 || stationaryTarget == null) return;

        // 1. Pick a random prefab from your array
        int randomPrefabIndex = Random.Range(0, kidPrefabs.Length);
        GameObject selectedPrefab = kidPrefabs[randomPrefabIndex];

        // 2. Instantiate the randomly selected kid prefab
        GameObject newKid = Instantiate(selectedPrefab, transform.position, Quaternion.identity);

        // 3. Calculate retreat destination (offset to left or right)
        Vector3 directionToTarget = (stationaryTarget.position - transform.position).normalized;
        Vector3 sideDirection = Vector3.Cross(directionToTarget, Vector3.up);
        
        float leftOrRight = Random.value > 0.5f ? 1f : -1f; 
        Vector3 offsetRetreatPoint = transform.position + (sideDirection * sideOffsetAmount * leftOrRight);

        // 4. Pass references to the kid's movement script
        TrickOrTreater behavior = newKid.GetComponent<TrickOrTreater>();
        if (behavior != null)
        {
            behavior.targetTransform = stationaryTarget;
            behavior.retreatPosition = offsetRetreatPoint;
        }
    }
}