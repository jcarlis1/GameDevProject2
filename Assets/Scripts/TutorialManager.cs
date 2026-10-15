using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement; // Added for SceneManager.LoadScene
using TMPro;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance;

    [Header("Tutorial UI References")]
    public GameObject popupPanel;
    public TextMeshProUGUI popupText;

    [Header("5.5 Min Victory UI References")]
    public GameObject victoryPanel;
    public string menuSceneName = "Menu";

    [Header("Timer Settings")]
    public float victoryTimerDuration = 330f; // 5.5 minutes (5.5 * 60 seconds)
    private float totalGameTimer = 0f;

    private bool hasShownKidMsg = false;
    private bool hasShownSkeletonMsg = false;
    private bool hasShownRefillMsg = false;
    private bool hasShownVictoryMsg = false;

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
        if (popupPanel != null)
        {
            popupPanel.SetActive(false);
        }

        if (victoryPanel != null)
        {
            victoryPanel.SetActive(false);
        }
    }

    private void Update()
    {
        // Increment game timer while gameplay is unpaused
        if (!hasShownVictoryMsg && Time.timeScale > 0f)
        {
            totalGameTimer += Time.deltaTime;

            if (totalGameTimer >= victoryTimerDuration)
            {
                ShowVictoryMessage();
            }
        }

        // Dismiss standard tutorial popups using Space / Enter
        if (popupPanel != null && popupPanel.activeSelf && Keyboard.current != null)
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame || 
                Keyboard.current.enterKey.wasPressedThisFrame || 
                Keyboard.current.numpadEnterKey.wasPressedThisFrame)
            {
                ResumeGame();
            }
        }
    }

    public void ShowKidMessage()
    {
        if (hasShownKidMsg) return;
        hasShownKidMsg = true;

        DisplayPopup("Trick-or-treaters are arriving! Kids will walk down the path to the bucket to take a candy.");
    }

    public void ShowSkeletonMessage()
    {
        if (hasShownSkeletonMsg) return;
        hasShownSkeletonMsg = true;

        DisplayPopup("Skeletons started spawning! Shoot them with your pumpkin launcher before they reach your candy target!");
    }

    public void ShowRefillMessage()
    {
        if (hasShownRefillMsg) return;
        hasShownRefillMsg = true;

        DisplayPopup("Your candy is running low! Walk over the glowing Orange Mat at the front door of your house to Refill off your bucket.");
    }

    private void ShowVictoryMessage()
    {
        hasShownVictoryMsg = true;

        if (victoryPanel != null)
        {
            victoryPanel.SetActive(true);
        }

        // Freeze time & unlock mouse cursor for menu selection
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Called by Continue Button on Victory Panel
    public void ContinuePlaying()
    {
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(false);
        }

        // Resume gameplay
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Called by Quit Button on Victory Panel
    public void QuitToMenu()
    {
        Time.timeScale = 1f; // Reset time scale before loading scene
        SceneManager.LoadScene(menuSceneName);
    }

    private void DisplayPopup(string message)
    {
        if (popupPanel == null || popupText == null) return;

        popupText.text = message;
        popupPanel.SetActive(true);

        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ResumeGame()
    {
        if (popupPanel != null)
        {
            popupPanel.SetActive(false);
        }

        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}