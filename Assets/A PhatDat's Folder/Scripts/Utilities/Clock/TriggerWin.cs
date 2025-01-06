//using UnityEngine;

//public class TriggerWin : MonoBehaviour
//{
//    public ScreenNoiseEffect screenNoiseEffect;

//    private bool hasWon = false;

//    void Update()
//    {
//        if (hasWon == true)
//        {
//            // Kích hoạt hiệu ứng nhiễu màn hình
//            screenNoiseEffect.StartNoiseEffect();

//            // Bắt đầu hiệu ứng fade
//            Invoke("StartFadeEffect", 1f);

//            // Hiển thị menu chiến thắng
//            Invoke("ShowEndGameMenu", 8f);

//            // Gọi UIManager để hiển thị bảng chiến thắng
//            UIManager.Instance.ShowWinPanel(); 
//        }
//    }

//    private void OnTriggerEnter(Collider other)
//    {
//        if (other.gameObject.tag == "Hour") // Nếu đối tượng chạm có tag là "Hour"
//        {
//            Debug.Log("Chuc mung ban da thang tro choi!");
//            hasWon = true; // Đánh dấu người chơi đã thắng
//        }
//    }

//    void StartFadeEffect()
//    {
//        screenNoiseEffect.StartFadeEffect();
//    }

//    void ShowEndGameMenu()
//    {
//        screenNoiseEffect.ShowEndGameMenu();

//        // Gọi UIManager để hiển thị bảng chiến thắng khi kết thúc hiệu ứng
//        UIManager.Instance.ShowWinPanel();
//    }
//}

using UnityEngine;

public class TriggerWin : MonoBehaviour
{
    // Tham chiếu đến AnalogGlitch
    public Kino.AnalogGlitch analogGlitch;

    private bool hasWon = false;

    void Update()
    {
        if (hasWon)
        {
            // Kích hoạt hiệu ứng Analog Glitch
            StartAnalogGlitch();

            // Gọi UIManager để hiển thị bảng chiến thắng
            UIManager.Instance.ShowWinPanel();

            // Vô hiệu hóa script để tránh lặp lại
            this.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Hour") // Nếu đối tượng chạm có tag là "Hour"
        {
            Debug.Log("Chuc mung ban da thang tro choi!");
            hasWon = true; // Đánh dấu người chơi đã thắng
        }
    }

    // Kích hoạt hiệu ứng Analog Glitch
    void StartAnalogGlitch()
    {
        if (analogGlitch != null)
        {
            analogGlitch.scanLineJitter = 0.5f;
            analogGlitch.verticalJump = 0.4f;
            analogGlitch.horizontalShake = 0.3f;
            analogGlitch.colorDrift = 0.6f;

            // Đặt lại hiệu ứng sau 8 giây
            Invoke("ResetAnalogGlitch", 8f);
        }
    }

    // Đặt lại hiệu ứng Analog Glitch
    void ResetAnalogGlitch()
    {
        if (analogGlitch != null)
        {
            analogGlitch.scanLineJitter = 0f;
            analogGlitch.verticalJump = 0f;
            analogGlitch.horizontalShake = 0f;
            analogGlitch.colorDrift = 0f;
        }
    }
}
