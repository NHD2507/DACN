using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting; // Thư viện Visual Scripting của Unity
using UnityEngine; // Thư viện chính của Unity cho việc lập trình trò chơi

public class Breaker : MonoBehaviour
{
    // Biến công khai để kiểm tra trạng thái nguồn điện
    public bool powerSource;

    // Âm thanh
    public AudioSource audioSource; // Nguồn phát âm thanh
    public AudioClip sound1; // Đoạn âm thanh để phát khi công tắc được bật/tắt

    // Biến riêng tư để kiểm tra người chơi có trong vùng hay không
    private bool PlayerInZone;

    // Biến tĩnh để xác định mục tiêu hiện tại (có thể dùng cho nhiều công tắc)
    public static Breaker currentTarget;

    // Hàm Start được gọi khi khởi động
    void Start()
    {
        PlayerInZone = false; // Đặt trạng thái mặc định là người chơi không ở trong vùng
        powerSource = true;   // Đặt trạng thái mặc định nguồn điện là bật
    }

    // Hàm Update được gọi mỗi frame
    void Update()
    {
        // Nếu người chơi trong vùng và nhấn phím 'E'
        if (PlayerInZone && Input.GetKeyDown(KeyCode.E))
        {
            // Đảo trạng thái của nguồn điện (bật/tắt)
            powerSource = !powerSource;

            // Thiết lập và phát âm thanh
            audioSource.clip = sound1;
            audioSource.Play();
        }
    }

    // Hàm để bật/tắt công tắc (có thể được gọi từ nơi khác trong code)
    public void ToggleBreaker()
    {
        // Nếu người chơi đang trong vùng
        if (PlayerInZone == true)
        {
            // Đảo trạng thái nguồn điện
            powerSource = !powerSource;

            // Phát âm thanh
            audioSource.clip = sound1;
            audioSource.Play();
        }
    }

    // Hàm xử lý khi người chơi đi vào vùng kích hoạt
    private void OnTriggerEnter(Collider other)
    {
        // Nếu đối tượng va chạm có tag là "Reach"
        if (other.gameObject.tag == "Reach")
        {
            PlayerInZone = true; // Đánh dấu người chơi trong vùng
            currentTarget = this; // Đặt mục tiêu hiện tại là công tắc này
        }
    }

    // Hàm xử lý khi người chơi rời khỏi vùng kích hoạt
    private void OnTriggerExit(Collider other)
    {
        // Nếu đối tượng rời khỏi vùng có tag là "Reach"
        if (other.gameObject.tag == "Reach")
        {
            PlayerInZone = false; // Đánh dấu người chơi đã rời khỏi vùng

            // Kiểm tra nếu mục tiêu hiện tại là công tắc này thì đặt về null
            if (currentTarget == this)
            {
                currentTarget = null;
            }
        }
    }
}
