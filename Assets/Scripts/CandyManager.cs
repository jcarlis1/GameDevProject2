using UnityEngine;
using TMPro;

public class CandyManager : MonoBehaviour
{
    public static CandyManager Instance;

    [Header("UI References")]
    public TextMeshProUGUI candyText;
    public TextMeshProUGUI cooldownText;

    [Header("Candy Settings")]
    public int startingCandy = 30;
    private int currentCandy;

    [Header("Cooldown Settings")]
    public float cooldownDuration = 30f;
    private float cooldownTimer = 0f;

    private void Awake()
    {
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
        UpdateCandyUI();
    }

    private void Update()
    {
        // Handle cooldown countdown timer
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;

            if (cooldownTimer <= 0)
            {
                cooldownTimer = 0;
            }

            UpdateCooldownUI();
        }
    }

    public void DecreaseCandy(int amount = 1)
    {
        currentCandy -= amount;
        if (currentCandy < 0) currentCandy = 0;

        UpdateCandyUI();
    }

    public void TryRefillCandy(int refillAmount = 30)
    {
        // Block refill if still on cooldown
        if (cooldownTimer > 0) return;

        currentCandy = refillAmount;
        cooldownTimer = cooldownDuration;

        UpdateCandyUI();
        UpdateCooldownUI();
        
    }

    private void UpdateCandyUI()
    {
        if (candyText != null)
        {
            candyText.text = "Candy Count: " + currentCandy;
        }
    }

    private void UpdateCooldownUI()
    {
        if (cooldownText != null)
        {
            if (cooldownTimer > 0)
            {
                // Displays ceiling integer value (e.g. 30s, 29s...)
                cooldownText.text = "Refill Pad: " + Mathf.CeilToInt(cooldownTimer) + "s";
            }
            else
            {
                cooldownText.text = "Refill Pad: Ready!";
            }
        }
    }
}