using UnityEngine;
using UnityEngine.InputSystem; // Added for New Input System
using TMPro;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance;

    [Header("UI References")]
    public GameObject popupPanel;
    public TextMeshProUGUI popupText;

    private bool hasShownKidMsg = false;
    private bool hasShownSkeletonMsg = false;
    private bool hasShownRefillMsg = false;

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
    }

    private void Update()
    {
        // Check input using the New Input System
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