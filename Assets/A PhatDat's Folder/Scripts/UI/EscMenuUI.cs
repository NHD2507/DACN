using UnityEngine;
using UnityEngine.SceneManagement;

public class EscMenuUI : MonoBehaviour
{
    public GameObject pauseMenuPanelUI;
    public GameObject settingPanelUI;
    public GameObject StaminaBar;
    private bool isPaused = false;

    void Start()
    {
        pauseMenuPanelUI.SetActive(false);
        settingPanelUI.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused && settingPanelUI.activeSelf)
            {
                BackToPauseMenu();
            }
            else if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuPanelUI.SetActive(false);
        settingPanelUI.SetActive(false);
        if (StaminaBar != null) StaminaBar.SetActive(true);
        Time.timeScale = 1f;  // Ensure the game resumes correctly
        LockCursor();
        isPaused = false;
    }

    public void Pause()
    {
        pauseMenuPanelUI.SetActive(true);
        settingPanelUI.SetActive(false);
        if (StaminaBar != null) StaminaBar.SetActive(false);
        Time.timeScale = 0f;  // Pause the game when the menu is active
        UnlockCursor();
        isPaused = true;
    }

    public void OpenSettings()
    {
        pauseMenuPanelUI.SetActive(false);
        settingPanelUI.SetActive(true);
    }

    public void BackToPauseMenu()
    {
        settingPanelUI.SetActive(false);
        pauseMenuPanelUI.SetActive(true);
    }

    public void OnClickBackToMenu()
    {
        Debug.Log("Đã quay lại menu chính.");
        Time.timeScale = 1f;  // Ensure time scale is reset when returning to the main menu
        UnlockCursor();  // Make sure cursor is unlocked when returning to menu
        LoadMainMenuScene();
    }

    private void LoadMainMenuScene()
    {
        SceneManager.LoadScene("MainMenuScene"); // Đảm bảo bạn có scene "MainMenuScene" trong dự án
    }

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
