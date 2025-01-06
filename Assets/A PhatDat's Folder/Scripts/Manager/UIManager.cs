using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // Đối tượng singleton duy nhất của UIManager
    public static UIManager Instance { get; private set; }

    // Các màn hình UI
    [Header("UI Elements")]
    public GameObject toggleUI;         // UI để hiển thị/toggle khi người chơi tương tác
    public GameObject losePanelUI;      // Màn hình UI hiển thị khi người chơi thua
    public GameObject winPanelUI;       // Màn hình UI hiển thị khi người chơi thắng

    private void Awake()
    {
        // Đảm bảo rằng chỉ có một instance duy nhất của UIManager
        if (Instance == null)
        {
            Instance = this;  // Gán instance nếu chưa tồn tại
        }
        else
        {
            Destroy(gameObject);  // Nếu instance đã tồn tại, phá huỷ đối tượng mới
        }
    }

    // Phương thức Start() được gọi khi trò chơi bắt đầu
    void Start()
    {
        // Gọi hàm tắt tất cả UI khi mới vào game
        SetUnactiveUI();
    }

    // Phương thức để hiển thị UI toggle
    public void showToggleUI()
    {
        if (toggleUI == null) return;  // Nếu toggleUI chưa được gán, không làm gì
        toggleUI.SetActive(true);  // Hiển thị UI toggle
    }

    // Phương thức để ẩn UI toggle
    public void hideToggleUI()
    {
        if (toggleUI == null) return;  // Nếu toggleUI chưa được gán, không làm gì
        toggleUI.SetActive(false);  // Ẩn UI toggle
    }

    // Phương thức để hiển thị màn hình thắng
    public void ShowWinPanel()
    {
        winPanelUI.SetActive(true);  // Hiển thị màn hình thắng
    }

    // Phương thức để hiển thị màn hình thua
    public void ShowLosePanel()
    {
        losePanelUI.SetActive(true);  // Hiển thị màn hình thua
    }

    // Phương thức để tắt tất cả UI
    private void SetUnactiveUI()
    {
        toggleUI.SetActive(false);   // Ẩn UI toggle
        losePanelUI.SetActive(false); // Ẩn màn hình thua
        winPanelUI.SetActive(false);  // Ẩn màn hình thắng
    }

    // Phương thức để quay lại menu chính
    public void OnClickBackToMenu()
    {
        Loader.Load(Loader.Scene.MainMenuScene);  // Tải lại menu chính
    }

    // Phương thức để bắt đầu lại trò chơi
    public void OnClickRestart()
    {
        Loader.Load(Loader.Scene.SampleScene);  // Tải lại màn chơi hiện tại
    }
}
