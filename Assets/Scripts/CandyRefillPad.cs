using UnityEngine;

public class CandyRefillPad : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Object entered pad: " + other.gameObject.name + " with Tag: " + other.tag);
        // Check if the object entering the trigger is the Player
        if (other.CompareTag("Player") || other.transform.root.CompareTag("Player"))
        {
            if (CandyManager.Instance != null)
            {
                CandyManager.Instance.TryRefillCandy(30);
                Debug.Log("Refill attempt ");
            }
        }
    }
}