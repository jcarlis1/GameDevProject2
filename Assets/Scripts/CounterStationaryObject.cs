using UnityEngine;
using TMPro; // Include if using TextMeshPro for the counter UI

public class CounterStationaryObject : MonoBehaviour
{
    public int counter = 10;
    public TMP_Text counterText; // Drag your UI text here

    void Start()
    {
        UpdateUI();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Only trigger if the colliding object has the "Walker" tag
        if (collision.gameObject.CompareTag("Kid"))
        {
            counter--;
            UpdateUI();

            // Optional: Destroy or destroy/repool the walking object after it hits
            // Destroy(collision.gameObject);
        }
    }

    void UpdateUI()
    {
        if (counterText != null)
        {
            counterText.text = "Count: " + counter;
        }
    }
}