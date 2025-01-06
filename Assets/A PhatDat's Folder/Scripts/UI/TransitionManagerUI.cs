using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class TransitionManagerUI : MonoBehaviour
{
    public CinemachineVirtualCamera currentCamera;

    private void Awake()
    {
        UnlockCursor();
    }

    private void Start()
    {
        // Chỉ tăng priority của camera nếu camera không phải là camera hiện tại
        if (currentCamera != null)
            currentCamera.Priority++;
    }

    public void UpdateCamera(CinemachineVirtualCamera target)
    {
        // Kiểm tra nếu camera mới khác camera hiện tại để tránh thay đổi không cần thiết
        if (currentCamera != target)
        {
            // Giảm priority của camera cũ
            if (currentCamera != null)
                currentCamera.Priority--;

            // Cập nhật camera hiện tại và tăng priority
            currentCamera = target;
            currentCamera.Priority++;

            Debug.Log($"Camera switched to: {currentCamera.name}");

            // Force Cinemachine Brain cập nhật
            CinemachineBrain brain = Camera.main.GetComponent<CinemachineBrain>();
            if (brain != null)
            {
                brain.enabled = false;
                brain.enabled = true;
            }
        }
    }

    public void Game()
    {
        // Load scene SingleplayerScene
        //Loader.Load(Loader.Scene.GameScene);
        Loader.Load(Loader.Scene.SampleScene);
    }

    public void Exit()
    {
        Application.Quit();
    }

    private void UnlockCursor()
    {
        // Mở khóa con trỏ khi cần thiết
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
