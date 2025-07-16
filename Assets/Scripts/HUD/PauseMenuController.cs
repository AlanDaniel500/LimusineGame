using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuController : MonoBehaviour
{
    public Button playPauseButton;
    public Sprite playIcon;
    public Sprite pauseIcon;
    public Button mainMenuButton;

    private bool isPaused = false;

    private void Start()
    {
        playPauseButton.onClick.AddListener(TogglePause);
        mainMenuButton.onClick.AddListener(ReturnToMainMenu);
        UpdateButtonIcon();
    }

    private void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0f : 1f;
        UpdateButtonIcon();
    }

    private void UpdateButtonIcon()
    {
        Image btnImage = playPauseButton.GetComponent<Image>();
        if (btnImage != null)
        {
            btnImage.sprite = isPaused ? playIcon : pauseIcon;
        }
    }

    private void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuPrincipal");
    }
}
