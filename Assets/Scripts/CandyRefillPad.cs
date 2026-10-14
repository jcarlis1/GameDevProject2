using UnityEngine;

public class CandyRefillPad : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger is the Player
        if (other.CompareTag("Player") || other.transform.root.CompareTag("Player"))
        {
            if (CandyManager.Instance != null)
            {
                CandyManager.Instance.TryRefillCandy(30);
            }
        }
    }
}