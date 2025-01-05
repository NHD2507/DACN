using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // Đối tượng singleton duy nhất của UIManager
    public static UIManager Instance { get; private set; }

    // Các màn hình UI
    [Header("UI Elements")]
    public GameObject toggleUI;
    public GameObject losePanelUI;
    public GameObject winPanelUI;

    private void Awake()
    {
        // Đảm bảo rằng chỉ có một instance duy nhất của UIManager
        if (Instance == null)
        {
            Instance = this; // Gán instance nếu chưa tồn tại
            DontDestroyOnLoad(gameObject); // Giữ UIManager khi chuyển cảnh
        }
        else
        {
            Destroy(gameObject); // Nếu instance đã tồn tại, phá huỷ đối tượng mới
        }
    }

    void Start()
    {
        //Gọi hàm tắt tất cả UI khi mới vào game
        SetUnactiveUI();
    }

    public void showToggleUI()
    {
        if (toggleUI == null) return;
        toggleUI.SetActive(true);
    }

    public void hideToggleUI()
    {
        if (toggleUI == null) return;
        toggleUI.SetActive(false);
    }


    private void SetUnactiveUI ()
    {
        toggleUI.SetActive(false);
        losePanelUI.SetActive(false);
        winPanelUI.SetActive(false);
    }    
}
