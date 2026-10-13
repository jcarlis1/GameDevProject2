using UnityEngine;
using TMPro; // Required for TextMeshPro

public class CandyManager : MonoBehaviour
{
    public static CandyManager Instance;

    [Header("UI Reference")]
    public TextMeshProUGUI candyText;

    [Header("Candy Settings")]
    public int startingCandy = 20;
    private int currentCandy;

    private void Awake()
    {
        // Simple Singleton pattern so kids can easily call CandyManager.Instance
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        currentCandy = startingCandy;
        UpdateUI();
    }

    public void DecreaseCandy(int amount = 1)
    {
        currentCandy -= amount;
        if (currentCandy < 0) currentCandy = 0;

        UpdateUI();
    }

    private void UpdateUI()
    {
        if (candyText != null)
        {
            candyText.text = "Candy Count: " + currentCandy;
        }
    }
}