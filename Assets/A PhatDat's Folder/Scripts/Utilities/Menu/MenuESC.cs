using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuESC : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public GameObject optionsMenuUI;
    public GameObject LoadingMainMenu;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseMenuUI.activeInHierarchy == true)
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
        pauseMenuUI.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1; // Tiếp tục game
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0; // Tạm dừng game
    }

    public void OpenOptions()
    {
        optionsMenuUI.SetActive(true); // Hiển thị menu tùy chọn
    }

    public void ExitToMenu()
    {
        LoadingMainMenu.SetActive(true);
        SceneManager.LoadScene(0);
        Time.timeScale = 1; // Đảm bảo rằng game tiếp tục khi quay lại menu chính
    }
}
