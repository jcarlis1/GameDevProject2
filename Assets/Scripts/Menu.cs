using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    // Hooked up to the Play button's OnClick event in the Inspector.
    public void OnPlayButton ()
    {
        SceneManager.LoadScene("GameScene");
    }
    // Hooked up to the Quit button's OnClick event.
    public void OnQuitButton ()
    {
        Application.Quit();
    }
}
