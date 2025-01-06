//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class TriggerLose : MonoBehaviour
//{
//    public CutScreenEnd csE;
//    public bool _hasLose;

//    private void Start()
//    {
//        _hasLose = false;
//    }
//    void Update()
//    {
//        if (_hasLose == true)
//        {
//            // Xử lý khi người chơi thắng game
//            // ...

//            // Kích hoạt hiệu ứng nhiễu màn hình và hiển thị menu end game
//            csE.ShowCutScreen();
//        }
//    }

//    private void OnTriggerEnter(Collider other)
//    {
//        if (other.gameObject.tag == "Hour")
//        {
//            Debug.Log("Chuc mung ban da thua! Ga vcl!");
//            _hasLose = true;
//        }
//    }
//}

using System.Collections;
using UnityEngine;

public class TriggerLose : MonoBehaviour
{
    // Tham chiếu đến hiệu ứng glitch
    public Kino.AnalogGlitch glitchEffect;

    // Biến trạng thái thua
    public bool _hasLose;

    private void Start()
    {
        _hasLose = false; // Khởi tạo trạng thái thua là false
    }

    void Update()
    {
        if (_hasLose == true)
        {
            // Kích hoạt hiệu ứng glitch
            StartGlitchEffect();

            // Tắt glitch sau 2 giây
            Invoke("StopGlitchEffect", 2f);

            // Phát video cắt cảnh sau 3 giây
            Invoke("PlayCutScene", 3f);

            // Hiển thị LosePanel sau 8 giây
            Invoke("ShowLosePanel", 8f);

            // Vô hiệu hóa script sau khi chạy
            this.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Kích hoạt khi va chạm với đối tượng có tag "Hour"
        if (other.gameObject.tag == "Hour")
        {
            Debug.Log("Chuc mung ban da thua!");
            _hasLose = true; // Đánh dấu người chơi đã thua
        }
    }

    // Kích hoạt hiệu ứng nhiễu màn hình
    void StartGlitchEffect()
    {
        glitchEffect.scanLineJitter = 0.5f;
        glitchEffect.verticalJump = 0.3f;
        glitchEffect.horizontalShake = 0.4f;
        glitchEffect.colorDrift = 0.5f;
    }

    // Tắt hiệu ứng nhiễu màn hình
    void StopGlitchEffect()
    {
        glitchEffect.scanLineJitter = 0f;
        glitchEffect.verticalJump = 0f;
        glitchEffect.horizontalShake = 0f;
        glitchEffect.colorDrift = 0f;
    }

    // Hiển thị bảng thua từ UIManager
    void ShowLosePanel()
    {
        UIManager.Instance.ShowLosePanel(); // Gọi hàm từ UIManager
    }
}
