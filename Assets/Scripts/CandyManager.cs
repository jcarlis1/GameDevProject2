using UnityEngine;
using UnityEngine.SceneManagement; // Required for loading scenes
using System.Collections; // Required for Coroutines
using TMPro;

public class CandyManager : MonoBehaviour
{
    public static CandyManager Instance;

    [Header("UI References")]
    public TextMeshProUGUI candyText;
    public TextMeshProUGUI cooldownText;
    public GameObject gameOverPanel; // Displays "Game Over" message during delay

    [Header("Candy Settings")]
    public int startingCandy = 30;
    private int currentCandy;

    [Header("Cooldown Settings")]
    public float cooldownDuration = 30f;
    private float cooldownTimer = 0f;

    [Header("Scene Transition Settings")]
    public float autoReturnDelay = 3.0f; // Time in seconds to show Game Over screen
    public string startMenuSceneName = "Menu"; // Type exact scene name of your Start Menu

    private bool isGameOver = false;

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
        // Always reset time scale to 1 when entering the scene
        Time.timeScale = 1f;

        currentCandy = startingCandy;
        UpdateCandyUI();

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
    }

    private void Update()
    {
        if (isGameOver) return;

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
        if (isGameOver) return;

        currentCandy -= amount;

        // Check for refill pad tutorial trigger
        if (currentCandy <= 15 && TutorialManager.Instance != null)
        {
            TutorialManager.Instance.ShowRefillMessage();
        }

        if (currentCandy <= 0)
        {
            currentCandy = 0;
            TriggerGameOver();
        }

        UpdateCandyUI();
    }

    public void TryRefillCandy(int refillAmount = 30)
    {
        if (isGameOver) return;
        if (cooldownTimer > 0) return;

        currentCandy = refillAmount;
        cooldownTimer = cooldownDuration;

        UpdateCandyUI();
        UpdateCooldownUI();
    }

    private void TriggerGameOver()
    {
        isGameOver = true;

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        // Freeze in-game movement & physics
        Time.timeScale = 0f;

        // Start auto-redirect timer
        StartCoroutine(AutoReturnToStartMenu());
    }

    private IEnumerator AutoReturnToStartMenu()
    {
        // WaitForSecondsRealtime continues counting even when Time.timeScale is 0
        yield return new WaitForSecondsRealtime(autoReturnDelay);

        // Reset game time speed before returning to main menu
        Time.timeScale = 1f;

        // Unlock mouse cursor for menu navigation
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Load Start Menu scene
        SceneManager.LoadScene(startMenuSceneName);
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
                cooldownText.text = "Refill Pad: " + Mathf.CeilToInt(cooldownTimer) + "s";
            }
            else
            {
                cooldownText.text = "Refill Pad: Ready!";
            }
        }
    }
}