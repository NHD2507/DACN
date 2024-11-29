using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuESC : NetworkBehaviour
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
        Time.timeScale = 1; // Tạm dừng game
    }

    public void OpenOptions()
    {
        optionsMenuUI.SetActive(true); // Hiển thị menu tùy chọn
    }

    public void ExitToMenu()
    {
        // Dừng game và ngắt kết nối nếu là host
        if (NetworkManager.Singleton.IsHost)
        {
            NetworkManager.Singleton.Shutdown();
        }

        LoadingMainMenu.SetActive(true);
        SceneManager.LoadScene(0); // Quay về scene chính
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Kiểm tra nếu quay về scene chính (scene 0) và ngắt kết nối host
        if (scene.buildIndex == 0 && NetworkManager.Singleton.IsHost)
        {
            NetworkManager.Singleton.Shutdown();
        }
    }

    private void OnDestroy()
    {
        // Đảm bảo ngắt kết nối khi đối tượng này bị hủy
        if (NetworkManager.Singleton.IsHost)
        {
            NetworkManager.Singleton.Shutdown();
        }
    }

}
